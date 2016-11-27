// Project by Bauss
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Net;
using System.Net.Sockets;
using CandyConquer.Drivers;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Drivers.Repositories.Safe;
using CandyConquer.Security.Api;
using CandyConquer.Compiler.Api;
using CandyConquer.Debugging;

namespace CandyConquer.ApiServer
{
	/// <summary>
	/// An Api Server implementation.
	/// </summary>
	public class Server<T>
	{
		/// <summary>
		/// Gets the associated socket.
		/// </summary>
		public Socket Socket { get; private set; }
		/// <summary>
		/// Gets a value indicating whether the socket has been opened up for connections or not.
		/// </summary>
		public bool IsOpen { get { return Socket.IsBound; } }
		/// <summary>
		/// Gets or sets a value indicating whether disconnections should be handled or not.
		/// </summary>
		public bool ShouldHandleDisconnect { get; set; }
		/// <summary>
		/// Gets or sets the connection event.
		/// </summary>
		public Action<Client<T>> OnConnect { get; set; }
		/// <summary>
		/// Gets or sets the disconnection event.
		/// </summary>
		public Action<T> OnDisconnect { get; set; }
		
		/// <summary>
		/// The compiled method for handling controllers.
		/// </summary>
		private MethodInfo _controllerCall;
		
		/// <summary>
		/// Creates a new api server.
		/// </summary>
		public Server()
		{
			Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		}
		
		/// <summary>
		/// Starts the server.
		/// </summary>
		/// <param name="ip">The ip address.</param>
		/// <param name="port">The port.</param>
		public void Start(string ip, int port)
		{
			if (IsOpen)
			{
				throw new NetworkException("This socket has already been started.");
			}

			Socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
			Socket.Listen(100);
			Socket.BeginAccept(Handle_Accept, null);
		}
		
		/// <summary>
		/// Stops the server.
		/// </summary>
		public void Stop()
		{
			if (!IsOpen)
			{
				throw new NetworkException("Attempted to close an already closed socket.");
			}
			
			ThrowSafe.Execute(() => Socket.Shutdown(SocketShutdown.Both));
			ThrowSafe.Execute(() => Socket.Close());
		}
		
		/// <summary>
		/// The callback for BeginAccept.
		/// </summary>
		/// <param name="asyncResult">The asynchronous result.</param>
		private void Handle_Accept(IAsyncResult asyncResult)
		{
			try
			{
				Client<T> client = Socket.EndAccept(asyncResult);
				if (client.Socket.Connected)
				{
					client.ShouldHandleDisconnect = ShouldHandleDisconnect;
					if (OnConnect != null)
					{
						client.OnDisconnect = OnDisconnect;
						client.ControllerCall = _controllerCall;
						
						OnConnect.BeginInvoke(client, null, null);
					}
				}
				
				Socket.BeginAccept(Handle_Accept, null);
			}
			catch (Exception e)
			{
				#if DEBUG
				#if TRACE
				ErrorLogger.Log(StackTracing.GetCurrentMethod().Name, e);
				#else
				ErrorLogger.Log(Messages.Errors.FATAL_NETWORK_ERROR_TITLE, e);
				#endif
				if (e is SocketException)
				{
					Stop();
				}
				#else
				if (e is SocketException)
				{
					Stop();
					Global.Message(Messages.Errors.FATAL_NETWORK_ERROR_MSG);
				}
				Global.Message(e.ToString());
				#endif
			}
		}
		
		/// <summary>
		/// Initiailizes the controllers.
		/// </summary>
		/// <param name="controllers">IEnumerable of types that should be filtered into controllers.</param>
		public void InitializeControllers(IEnumerable<Type> controllers, string apiClient)
		{
			var controllerCalls = controllers
				.Where(type => type.GetCustomTypeAttribute<ApiControllerAttribute>() != null)
				.Select(controller =>
				        {
				        	var methods = controller.GetMethods()
				        		.Where(method => method.GetCustomTypeAttribute<ApiCallAttribute>() != null).ToList();
				        	if (methods.Count > 0)
				        	{
				        		return methods
				        			.Select(method =>
				        			        {
				        			        	var callAttribute = method.GetCustomTypeAttribute<ApiCallAttribute>();
				        			        	return new ApiControllerMethodInfo
				        			        	{
				        			        		Call = string.Join(".", controller.FullName, method.Name),
				        			        		CallAttribute = callAttribute
				        			        	};
				        			        });
				        	}
				        	return null;
				        }).Where(methods => methods != null).ToList();
			
			if (controllerCalls.Count > 0)
			{
				var apiCalls = new List<ApiControllerMethodInfo>();
				foreach (var controllerCall in controllerCalls)
				{
					apiCalls.AddRange(controllerCall);
				}
				
				_controllerCall = ApiControllerCompiler.CompileControllers(apiCalls, apiClient);
			}
		}
	}
}
