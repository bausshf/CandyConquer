// Project by Bauss
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using CandyConquer.Drivers;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.ApiServer;
using CandyConquer.Security.Api;

namespace CandyConquer.AuthApi
{
	/// <summary>
	/// The internal server class for the auth server.
	/// </summary>
	internal class AuthServer
	{
		/// <summary>
		/// The first api server for the auth server.
		/// </summary>
		internal static Server<Models.Client.AuthClient> AuthSocket1 { get; private set; }
		/// <summary>
		/// The second api server for the auth server.
		/// </summary>
		internal static Server<Models.Client.AuthClient> AuthSocket2 { get; private set; }
		
		/// <summary>
		/// Initializes the controllers.
		/// </summary>
		private static void InitializeControllers()
		{
			var controllerTypes = Assembly
				.GetExecutingAssembly()
				.GetTypes();
			string apiClient = typeof(Models.Client.AuthClient).FullName;
			
			AuthSocket1.InitializeControllers(controllerTypes, apiClient);
			AuthSocket2.InitializeControllers(controllerTypes, apiClient);
		}
		
		/// <summary>
		/// The entry point for the auth server.
		/// </summary>
		/// <param name="args">Command-line arguments.</param>
		private static void Main(string[] args)
		{
			#if DEBUG
			args = new string[] { "-instant" };
			#endif
			
			Console.Title = "CandyConquer";
			
			try
			{
				Drivers.Global.Initialize();
			}
			catch (Exception e)
			{
				Console.WriteLine(Messages.Errors.FAILED_TO_INITIALIZE_GLOBAL_DRIVER);
				Console.WriteLine(e);
				Console.ReadLine();
				return;
			}
			
			try
			{
				Repositories.Server.ServerList.Load();
			}
			catch (Exception e)
			{
				Console.WriteLine(Messages.Errors.FAILED_TO_LOAD_WORLD_SERVER_LIST);
				Console.WriteLine(e);
				Console.ReadLine();
				return;
			}
			
			if (args == null || args.Length == 0 || args[0] != "-instant")
			{
				Console.WriteLine(Messages.NO_INSTANT_LOAD_MESSAGE);
				Console.ReadLine();
			}
			
			Console.Clear();
			Console.Title = string.Format("{0} - Auth Server", Drivers.Settings.GlobalSettings.ServerName);
			Console.WriteLine(Console.Title);
			
			AuthSocket1 = new Server<Models.Client.AuthClient>
			{
				ShouldHandleDisconnect = false,
				OnConnect = Controllers.Network.ConnectionController.HandleNewConnection
				#if DEBUG
				, OnDisconnect = Controllers.Network.ConnectionController.HandleDisconnection
				#endif
			};
			
			AuthSocket2 = new Server<Models.Client.AuthClient>
			{
				ShouldHandleDisconnect = false,
				OnConnect = Controllers.Network.ConnectionController.HandleNewConnection
				#if DEBUG
				, OnDisconnect = Controllers.Network.ConnectionController.HandleDisconnection
				#endif
			};
			
			try
			{
				InitializeControllers();
			}
			catch (Exception e)
			{
				Console.WriteLine(Messages.Errors.FAILED_TO_INITIALIZE_CONTROLLERS);
				Console.WriteLine(e);
				Console.ReadLine();
				return;
			}
			
			AuthSocket1.Start(Drivers.Settings.AuthSettings.IPAddress, 9958);
			AuthSocket2.Start(Drivers.Settings.AuthSettings.IPAddress, 9959);
			
			Console.WriteLine(Drivers.Messages.AUTH_SERVER_STARTED);

			while (true)
			{
				Console.ReadLine();
			}
		}
	}
}