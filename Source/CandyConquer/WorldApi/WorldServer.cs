// Project by Bauss
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using CandyConquer.Drivers;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.ApiServer;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi
{
	/// <summary>
	/// The internal server class for the world server.
	/// </summary>
	internal class WorldServer
	{
		/// <summary>
		/// The first api server for the world server.
		/// </summary>
		internal static Server<Models.Entities.Player> WorldSocket { get; private set; }
		
		/// <summary>
		/// Initializes the controllers.
		/// </summary>
		private static void InitializeControllers()
		{
			var controllerTypes = Assembly
				.GetExecutingAssembly()
				.GetTypes();
			string apiClient = typeof(Models.Entities.Player).FullName;
			
			WorldSocket.InitializeControllers(controllerTypes, apiClient);
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
			
			WorldSocket = new Server<Models.Entities.Player>
			{
				ShouldHandleDisconnect = true,
				OnConnect = Controllers.Network.ConnectionController.HandleNewConnection,
				OnDisconnect = Controllers.Network.ConnectionController.HandleDisconnection
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
			
			try
			{
				Helpers.Server.Initialization.Initialize();
			}
			catch (Exception e)
			{
				Console.WriteLine(Messages.Errors.FAILED_TO_INITIALIZE_WORLD_SERVER);
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
			Console.Title = string.Format("[{0}]{1} - Game Server",
			                              Drivers.Settings.WorldSettings.Server,
			                              Drivers.Settings.GlobalSettings.ServerName);
			Console.WriteLine(Console.Title);
			
			WorldSocket.Start(Drivers.Settings.WorldSettings.IPAddress, Drivers.Settings.WorldSettings.Port);
			
			Console.WriteLine(Drivers.Messages.WORLD_SERVER_STARTED);

			while (true)
			{
				Console.ReadLine();
			}
		}
	}
}