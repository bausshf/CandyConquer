// Project by Bauss
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace CandyConquer.Compiler.Internal
{
	/// <summary>
	/// A provider for compiling methods.
	/// </summary>
	internal static class CompilerProvider
	{
		/// <summary>
		/// The template for compiling.
		/// </summary>
		private const string CODE_TEMPLATE = @"using System;
@Namespaces

namespace CompiledNamespace
{
	public class CompiledClass
	{
		public static @ReturnType CompiledMethod(@Parameters)
		{
			@Code
		}
	}
}";
		
		/// <summary>
		/// Compiles a piece of code based on a compiler helper.
		/// </summary>
		/// <param name="helper">The compiler helper.</param>
		/// <returns>The result of the compilation.</returns>
		internal static CompilerResults Compile<TCodeProvider>(CompilerHelper helper, string customCode = null)
			where TCodeProvider : CodeDomProvider, new()
		{
			var code = !string.IsNullOrWhiteSpace(customCode) ? customCode :
				CODE_TEMPLATE
				.Replace("@Namespaces",
				         string.Join("\r\n", helper.Namespaces
				                     .Select(ns => string.Format("using {0};", ns))
				                    )
				        )
				.Replace("@ReturnType", helper.ReturnType)
				.Replace("@Parameters", string.Join(", ", helper.Parameters))
				.Replace("@Code", helper.Code);
			
			var compilerParameters = new CompilerParameters
			{
				GenerateInMemory = true
			};
			
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblies)
			{
				compilerParameters.ReferencedAssemblies.Add(assembly.Location);
			}
			
			return new TCodeProvider().CompileAssemblyFromSource(compilerParameters, code);
		}
		
		/// <summary>
		/// Attempts to find a compiled method within an assembly.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <returns>The compiled method.</returns>
		internal static MethodInfo FindMethod(Assembly assembly)
		{
			var method = GetAllMethods(assembly, "CompiledNamespace")
				.Where(m => m.IsStatic && m.Name == "CompiledMethod").FirstOrDefault();
			
			if (method == null)
			{
				throw new CompilerException("Could not find the compiled method.");
			}
			
			return method;
		}
		
		/// <summary>
		/// Attempts to find a compiled method within an assembly.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <param name="nameSpace">The namespace to look in.</param>
		/// <returns>An array of all the methods</returns>
		internal static MethodInfo[] GetAllMethods(Assembly assembly, string nameSpace)
		{
			var parentClass = assembly.GetTypes().Where(type => type.IsClass &&
			                                            type.Namespace == nameSpace &&
			                                            type.Name == "CompiledClass").FirstOrDefault();
			if (parentClass == null)
			{
				throw new CompilerException("Could not find the compiled class.");
			}
			
			return parentClass.GetMethods();
		}
	}
}
