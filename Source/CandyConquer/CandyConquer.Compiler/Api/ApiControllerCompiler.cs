// Project by Bauss
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;
using CandyConquer.Security.Api;
using CandyConquer.Compiler.Internal;
using CandyConquer.Drivers.Repositories.Api.Attributes;

namespace CandyConquer.Compiler.Api
{
	/// <summary>
	/// A compiler for api controllers.
	/// </summary>
	public static class ApiControllerCompiler
	{
		/// <summary>
		/// The switch template for the api calls.
		/// </summary>
		private const string SwitchTemplate = @"switch (packetId)
{
	@Cases
	
	default:
		@packetLog
		return true;
}";
				/// <summary>
		/// The switch template for the api calls.
		/// </summary>
		private const string SubCaseSwitchTemplate = @"case @packetId:
{
	uint subPacketId;
	var subPacket = @controllerCall(client.Data,packet, out subPacketId);
	if (subPacketId == uint.MaxValue)
	{
		return true;
	}
	if (subPacketId == uint.MaxValue - 1)
	{
		return false;
	}
	
	switch (subPacketId)
	{
		@Cases
	
		default:
			@packetLog
			return true;
	}
	
	return true;
}";
		/// <summary>
		/// The case template for an api call.
		/// </summary>
		private const string CaseTemplate = @"
	case @packetId:
		@callTypeCheck
		return @controllerCall(client.Data,packet);";
		
		/// <summary>
		/// The case template for a sub api call.
		/// </summary>
		private const string SubCaseTemplate = @"
	case @packetId:
		@callTypeCheck
		return @controllerCall(client.Data,subPacket);";
		
		/// <summary>
		/// Compiles controllers into a method.
		/// </summary>
		/// <param name="controllers">The controllers to compile.</param>
		/// <returns>A compiled method of the controllers.</returns>
		public static MethodInfo CompileControllers(IEnumerable<ApiControllerMethodInfo> controllers, string apiClient)
		{
			var helper = new CompilerHelper();
			helper.ReturnType = typeof(bool).Name;
			helper.Parameters.AddRange(new[]
			                           {
			                           	string.Format("Client<{0}> client", apiClient.Replace("+", ".")),
			                           	"CallSecurity callType",
			                           	"ushort packetId",
			                           	"SocketPacket packet"
			                           });
			helper.Namespaces.AddRange(new[]
			                           {
			                           	"CandyConquer.Security.Api",
			                           	"CandyConquer.ApiServer",
			                           });
			
			var subIdentities = new Dictionary<ushort, List<ApiControllerMethodInfo>>();
			var mainIdentities = new Dictionary<ushort, ApiControllerMethodInfo>();
			
			var cases = controllers.Where(method => method.CallAttribute != null)
				.Select(method =>
				        {
				        	if (method.CallAttribute.SubIdentity != uint.MaxValue)
				        	{
				        		if (!subIdentities.ContainsKey(method.CallAttribute.Identity))
				        		{
				        			subIdentities.Add(method.CallAttribute.Identity, new List<ApiControllerMethodInfo>());
				        		}
				        		
				        		subIdentities[method.CallAttribute.Identity].Add(method);
				        		return null;
				        	}
				        	
				        	if (method.CallAttribute.TypeReturner)
				        	{
				        		mainIdentities.Add(method.CallAttribute.Identity, method);
				        		return null;
				        	}
				        	
				        	string callTypeCheck = string.Empty;
				        	switch (method.CallAttribute.CallSecurity)
				        	{
				        		case CallSecurity.Idle:
				        			callTypeCheck = "if (callType != CallSecurity.Idle) return true;";
				        			break;
				        			
				        		case CallSecurity.NonIdle:
				        			callTypeCheck = "if (callType != CallSecurity.NonIdle) return true;";
				        			break;
				        			
				        		case CallSecurity.Once:
				        			callTypeCheck = "if (client.AddOrHasSingleCall(packetId)) return true;";
				        			break;
				        	}
				        	
				        	return CaseTemplate
				        		.Replace("@packetId", method.CallAttribute.Identity.ToString())
				        		.Replace("@callTypeCheck", callTypeCheck)
				        		.Replace("@controllerCall", method.Call.Replace("+", "."));
				        }).Where(method => method != null).ToList();
			
			foreach (var mainCall in mainIdentities)
			{
				var subCases = subIdentities[mainCall.Key]
					.Select(method =>
					        {
					        	string callTypeCheck = string.Empty;
					        	switch (method.CallAttribute.CallSecurity)
					        	{
					        		case CallSecurity.Idle:
					        			callTypeCheck = "if (callType != CallSecurity.Idle) return true;";
					        			break;
					        			
					        		case CallSecurity.NonIdle:
					        			callTypeCheck = "if (callType != CallSecurity.NonIdle) return true;";
					        			break;
					        			
					        		case CallSecurity.Once:
					        			callTypeCheck = "if (client.AddOrHasSingleCall(packetId)) return true;";
					        			break;
					        	}
					        	
					        	return SubCaseTemplate
					        		.Replace("@packetId", method.CallAttribute.SubIdentity.ToString())
					        		.Replace("@callTypeCheck", callTypeCheck)
					        		.Replace("@controllerCall", method.Call.Replace("+", "."));
					        });
				
				var subSwitch = SubCaseSwitchTemplate
					.Replace("@controllerCall", mainCall.Value.Call.Replace("+", "."))
					.Replace("@packetId", mainCall.Key.ToString())
					.Replace("@Cases", string.Join("\r\n", subCases));
				
				#if LOCAL
				subSwitch = subSwitch.Replace("@packetLog", "client.LogPacket(packet, true, true, subPacket.SubTypeObject);");
				#else
				subSwitch = subSwitch.Replace("@packetLog", string.Empty);
				#endif
				
				cases.Add(subSwitch);
			}
			
			helper.Code = SwitchTemplate
				.Replace("@Cases", string.Join("\r\n", cases));
			
			#if LOCAL
			helper.Code = helper.Code.Replace("@packetLog", "client.LogPacket(packet, true);");
			#else
			helper.Code = helper.Code.Replace("@packetLog", "client.LogPacket(packet, false);");
			#endif
			
			var compilerResults = CompilerProvider.Compile<CSharpCodeProvider>(helper);
			if (compilerResults.Errors != null && compilerResults.Errors.Count > 0)
			{
				foreach (var error in compilerResults.Errors)
				{
					Console.WriteLine(error);
				}
				
				throw new CompilerException("Failed to compile.");
			}
			
			return CompilerProvider.FindMethod(compilerResults.CompiledAssembly);
		}
	}
}
