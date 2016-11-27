// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Helpers.Server
{
	/// <summary>
	/// Handler for initialization of the world server.
	/// </summary>
	public static class Initialization
	{
		/// <summary>
		/// Initializes the world server.
		/// </summary>
		public static void Initialize()
		{
			Console.WriteLine(Drivers.Messages.LOADING_LOCALIZATION);
			Data.Localization.Language.Initialize();
			
			Console.WriteLine(Drivers.Messages.LOADING_STATS_SETTINGS);
			Collections.StatCollection.Load();
			
			Console.WriteLine(Drivers.Messages.LOADING_DMAPS);
			CandyConquer.Database.Dal.Maps.SetLoadedEvent(Collections.MapCollection.LoadMaps);
			CandyConquer.Database.Dal.Maps.LoadDMaps();
			
			Console.WriteLine(Drivers.Messages.LOADING_DEFAULT_COORDINATES);
			Collections.MapCollection.LoadDefaultCoordinates();
			
			Console.WriteLine(Drivers.Messages.LOADING_NPC_SCRIPTS);
			Collections.NPCScriptCollection.Load();
			Console.WriteLine(Drivers.Messages.LOADING_NPCS);
			Collections.NPCCollection.Load();
			
			Console.WriteLine(Drivers.Messages.LOADING_MOBS);
			Collections.MonsterCollection.Load();
			Console.WriteLine(Drivers.Messages.LOADING_MOB_SPAWNS);
			Collections.MonsterCollection.LoadSpawns();
			Console.WriteLine(Drivers.Messages.LOADING_GUARDS);
			Collections.MonsterCollection.LoadGuards();
			
			Console.WriteLine(Drivers.Messages.LOADING_SHOPS);
			Collections.ShopCollection.LoadShops();
			Console.WriteLine(Drivers.Messages.LOADING_CPSHOPS);
			Collections.ShopCollection.LoadCpShops();
			
			Console.WriteLine(Drivers.Messages.LOADING_PORTALS);
			Collections.PortalCollection.Load();
			
			Console.WriteLine(Drivers.Messages.LOADING_WEATHERS);
			Collections.WeatherCollection.Load();
			
			Console.WriteLine(Drivers.Messages.LOADING_SPELL_INFOS);
			Collections.SpellInfoCollection.Load();
			
			Console.WriteLine(Drivers.Messages.LOADING_NOBILITY_BOARD);
			Collections.NobilityBoard.Load();
			
			Console.WriteLine(Drivers.Messages.LOADING_ARENA_QUALIFIERS);
			Collections.ArenaQualifierCollection.Load();
			
			Console.WriteLine(Drivers.Messages.LOADING_ITEM_ADDITIONS);
			Collections.ItemCollection.LoadAdditions();
			Console.WriteLine(Drivers.Messages.LOADING_ITEM_SCRIPTS);
			Collections.ItemScriptCollection.Load();
			Console.WriteLine(Drivers.Messages.LOADING_ITEMS);
			Collections.ItemCollection.Load();
			
			// Guilds are depending on items, which is why they have to be loaded here.
			Console.WriteLine(Drivers.Messages.LOADING_GUILDS);
			Collections.GuildCollection.Load();
			
			Console.WriteLine(Drivers.Messages.LOADING_TOURNAMENT_SCRIPTS);
			Collections.TournamentScriptCollection.Load();
			
			Console.WriteLine(Drivers.Messages.LOADING_COMMAND_SCRIPTS);
			Collections.CommandScriptCollection.Load();
			
			Console.WriteLine(Drivers.Messages.LOADING_THREADS);
			Controllers.Threads.ThreadController.StartThreads();

			Console.WriteLine(Drivers.Messages.FINISHED_LOADING);
		}
	}
}
