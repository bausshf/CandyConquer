// Project by Bauss
using System;

namespace CandyConquer.Drivers
{
	/// <summary>
	/// A collection of all messages.
	/// </summary>
	public static class Messages
	{
		#region Network Messages
		public const string SOCKET_CONNECT =
			"'{0}' has connected to the server.";
		public const string SOCKET_DISCONNECT_MESSAGE =
			"'{0}' has disconnected from the server Reason: {1}";
		#endregion
		
		#region Loading Messages
		public const string LOADING_DATABASE_SETTINGS =
			"Loading database settings.";
		public const string LOADING_GLOBAL_SETTINGS =
			"Loading global settings.";
		public const string LOADING_AUTH_SETTINGS =
			"Loading auth settings.";
		public const string LOADING_WORLD_SETTINGS =
			"Loading world settings.";
		public const string AUTH_SERVER_STARTED =
			"The auth server has been started.";
		public const string WORLD_SERVER_STARTED =
			"The world server has been started.";
		public const string NO_INSTANT_LOAD_MESSAGE =
			"Press ENTER to start the server.";
		public const string LOADING_LOCALIZATION =
			"Loading localizations.";
		public const string LOADING_STATS_SETTINGS =
			"Loading stats settings.";
		public const string LOADING_DMAPS =
			"Loading dmaps.";
		public const string LOADING_MAPS =
			"Loading maps.";
		public const string LOADING_DEFAULT_COORDINATES =
			"Loading default coordinates.";
		public const string LOADING_ITEMS =
			"Loading items.";
		public const string LOADING_ITEM_ADDITIONS =
			"Loading item additions.";
		public const string LOADING_NPC_SCRIPTS =
			"Loading npc scripts.";
		public const string LOADING_NPCS =
			"Loading npcs.";
		public const string LOADING_MOBS =
			"Loading monsters.";
		public const string LOADING_MOB_SPAWNS =
			"Loading monster spawns.";
		public const string LOADING_GUARDS =
			"Loading guards.";
		public const string LOADING_ITEM_SCRIPTS =
			"Loading item scripts.";
		public const string LOADING_SHOPS =
			"Loading shops.";
		public const string LOADING_CPSHOPS =
			"Loading cp shops.";
		public const string LOADING_GUILDS =
			"Loading guilds.";
		public const string LOADING_PORTALS =
			"Loading portals.";
		public const string LOADING_WEATHERS =
			"Loading weathers.";
		public const string LOADING_SPELL_INFOS =
			"Loading spell infos.";
		public const string LOADING_NOBILITY_BOARD =
			"Loading the nobility board.";
		public const string LOADING_ARENA_QUALIFIERS =
			"Loading the arena qualifiers.";
		public const string LOADING_TOURNAMENT_SCRIPTS =
			"Loading tournament scripts.";
		public const string LOADING_COMMAND_SCRIPTS =
			"Loading command scripts.";
		public const string LOADING_THREADS =
			"Loading threads.";
		public const string FINISHED_LOADING =
			"Finished loading ...";
		#endregion
		
		#region Ban Messages
		public const string MOVE_OTHER_PLAYER =
			"Attempted to move another player.";
		public const string INVALID_CLIENT_ID =
			"Invalid client id.";
		public const string INVALID_JOB =
			"Invalid job.";
		public const string INVALID_MODEL =
			"Invalid model.";
		public const string AUTHENTICATION_BYPASS =
			"Authentication bypass.";
		public const string SEND_MESSAGE_FROM_SOMEONE_ELSE =
			"Attempted to send a message from someone else.";
		public const string BYPASS_LOGIN_ENTER_MAP =
			"Tried to bypass login: EnterMap.";
		public const string JUMP_OTHER_PLAYER =
			"Attempted to jump another player.";
		public const string INVALID_DROP_ITEM =
			"Attempted to drop an item that can't be dropped. Note: Possible modified client.";
		public const string SPEEDHACK =
			"Used speed hack.";
		public const string INVALID_USE_ITEM =
			"Attempted to use an item that he/she does not own.";
		#endregion
		
		#region Misc Messages
		public const string LOGIN_SUCCESS =
			"'{0}' has successfully logged in...";
		public const string COMMAND_DISCONNECT =
			"Disconnected by command.";
		public const string SUCCESS_CREATE =
			"Character created.";
		#endregion
		
		#region Guild Messages
		public const string NEW_GUILD_ANNOUNCEMENT =
			"Welcome to your new guild!";
		public const string SAMPLE_GUILD_ANNOUNCEMENT =
			"Sample guild announcement!";
		#endregion
		
		/// <summary>
		/// Error messages.
		/// All error messages must be prefixed with [ERROR_NANONAME-ERROR_ID]
		/// Ex. [N-1] for Network error 1.
		/// </summary>
		public static class Errors
		{
			#region [N] Network Errors
			public const string FATAL_NETWORK_ERROR_TITLE =
				"Fatal Network Error";
			
			public const string FATAL_NETWORK_ERROR_MSG =
				"[N-1]There was a fatal network error.";
			public const string CLIENT_DISCONNECTED =
				"[N-2]The client has been disconnected.";
			public const string SOCKET_ERROR_RECEIVE_HEAD =
				"[N-3]Socket error during head-receive.";
			public const string SOCKET_ERROR_RECEIVE_BODY =
				"[N-4]Socket error during body-receive.";
			public const string SOCKET_RECEIVE_TOO_BIG =
				"[N-5]The received packet's virtual size is too big.";
			public const string SOCKET_DISCONNECTED =
				"[N-6]The socket has been disconnected.";
			public const string INVALID_AUTHKEY_OR_PROXY =
				"[N-7]Invalid authentication key or using a proxy.";
			#endregion
			
			#region [I] Initialization Errors
			public const string FAILED_TO_INITIALIZE_GLOBAL_DRIVER =
				"[I-1]Failed to initialize the global driver.";
			public const string FAILED_TO_LOAD_WORLD_SERVER_LIST =
				"[I-2]Failed to load the world server list.";
			public const string FAILED_TO_INITIALIZE_CONTROLLERS =
				"[I-3]Failed to initialize the controllers.";
			public const string FAILED_TO_INITIALIZE_WORLD_SERVER =
				"[I-4]Failed to initialize the world server.";
			#endregion
			
			#region [M] Map Errors
			public const string FAILED_TO_ADD_TO_MAP =
				"[M-1]Failed to add the map object to the map.";
			public const string FAILED_TO_REMOVE_FROM_MAP =
				"[M-2]Failed to remove the map object from the map.";
			#endregion
			
			#region [P] Player Errors
			public const string MULTI_LOGIN =
				"[P-1]Multi login.";
			public const string FAILED_TO_ADD_PLAYER_TO_GLOBAL_COLLECTION =
				"[P-2]Failed to add the player to the global collection.";
			public const string JUMP_WHILE_DEAD =
				"[P-3]Tried to jump while dead.";
			public const string INVALID_TRADE_TARGET =
				"[P-4]Invalid trade target.";
			public const string FAIL_BUY_ITEMS =
				"[P-5]Failed to buy items from a shop. ItemId: {0} Amount: {1} Counter: {2} Price: {3} Total Price: {4}";
			public const string FAILED_TO_INITIALIZE_ARENA_INFO =
				"[P-6]Failed to initialize the arena info.";
			#endregion
			
			#region [D] Database Errors
			public const string DB_ERROR =
				"[D-1]Database error.";
			public const string FAILED_TO_LOAD_INVENTORY =
				"[D-2]Failed to load inventory.";
			public const string FAILED_TO_LOAD_EQUIPMENTS =
				"[D-3]Failed to load equipments.";
			public const string FAILED_TO_LOAD_HOUSE =
				"[D-4]Failed to load houses.";
			public const string FAILED_TO_LOAD_WAREHOUSE =
				"[D-5]Failed to load warehouse.";
			#endregion
			
			#region [C] Character Creation Errors
			public const string INVALID_CHARS =
				"[C-1]You have invalid characters in your name.";
			public const string NAME_BANNED =
				"[C-2]This name is banned.";
			public const string NAME_TAKEN =
				"[C-3]The name has already been taken.";
			#endregion
			
			#region [E] Errors
			public const string FATAL_ERROR_TITLE =
				"Fatal Error";
			#endregion
		}
	}
}
