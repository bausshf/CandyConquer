// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Linq;
using CandyConquer.WorldApi.Models.Guilds;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A guild collection.
	/// </summary>
	public static class GuildCollection
	{
		/// <summary>
		/// The collection of guilds.
		/// </summary>
		private static ConcurrentDictionary<int,Guild> _guilds;
		
		/// <summary>
		/// Static constructor for GuildCollection.
		/// </summary>
		static GuildCollection()
		{
			_guilds = new ConcurrentDictionary<int, Guild>();
		}
		
		/// <summary>
		/// Loads all guilds.
		/// </summary>
		public static void Load()
		{
			var dbGuilds = Database.Dal.Guilds.GetAll(Drivers.Settings.WorldSettings.Server);
			foreach (var dbGuild in dbGuilds)
			{
				var guild = new Guild(dbGuild);
				
				// Loads members and info
				guild.Load();
				
				_guilds.TryAdd(guild.Id, guild);
			}
			
			foreach (var guild in _guilds.Values)
			{
				guild.LoadAssociations();
			}
		}
		
		/// <summary>
		/// Attempts to get a guild.
		/// </summary>
		/// <param name="guildId">The guild id.</param>
		/// <param name="guild">The guild.</param>
		/// <returns>True if the guild was retrieved.</returns>
		public static bool TryGetGuild(int guildId, out Guild guild)
		{
			return _guilds.TryGetValue(guildId, out guild);
		}
		
		/// <summary>
		/// Gets a guild by a name.
		/// </summary>
		/// <param name="name">The name of the guild.</param>
		/// <returns>The guild if found, null otherwise.</returns>
		public static Guild GetGuildByName(string name)
		{
			return _guilds.Values.Where(guild => guild.DbGuild.Name == name).FirstOrDefault();
		}
		
		/// <summary>
		/// Attempts to create a guild.
		/// </summary>
		/// <param name="player">The player who becomes the leader.</param>
		/// <param name="name">The name of the guild.</param>
		/// <returns>True if the guild was created.</returns>
		public static bool Create(Models.Entities.Player player, string name)
		{
			if (GetGuildByName(name) != null)
			{
				return false;
			}
			
			var guild = new Guild(new Database.Models.DbGuild
			                      {
			                      	Name = name,
			                      	Announcement = Drivers.Messages.NEW_GUILD_ANNOUNCEMENT,
			                      	AnnouncementDate = Drivers.Time.GetTime(Drivers.Time.TimeType.Day),
			                      	Fund = 0,
			                      	CPsFund = 0,
			                      	Level = 0,
			                      	Server = Drivers.Settings.WorldSettings.Server
			                      });
			if (guild.DbGuild.Create() && _guilds.TryAdd(guild.Id, guild))
			{
				guild.AddMember(player, Enums.GuildRank.GuildLeader, false);
				
				player.UpdateClientGuild();
				
				player.UpdateScreen(true);
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>
		/// Removes a guild from the collection.
		/// </summary>
		/// <param name="guild">The guild to remove.</param>
		/// <returns>True if the guild was removed.</returns>
		public static bool RemoveGuild(Guild guild)
		{
			Guild removedGuild;
			return _guilds.TryRemove(guild.Id, out removedGuild);
		}
	}
}
