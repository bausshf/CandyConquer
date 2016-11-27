// Project by Bauss
using System;
using CandyConquer.Database.Models;

namespace CandyConquer.WorldApi.Models.Guilds
{
	/// <summary>
	/// Model of a guild.
	/// </summary>
	public class Guild : Controllers.Guilds.GuildController
	{
		/// <summary>
		/// Gets the database model for the guild.
		/// </summary>
		public DbGuild DbGuild { get; private set; }
		
		/// <summary>
		/// Gets or sets the leader of the guild.
		/// </summary>
		public GuildMember GuildLeader { get; set; }
		
		/// <summary>
		/// Gets or sets the warehouse of the guild.
		/// </summary>
		public Collections.GuildWarehouse Warehouse { get; set; }
		
		/// <summary>
		/// Gets the id of the guild.
		/// </summary>
		public int Id
		{
			get { return DbGuild.Id; }
		}
		
		/// <summary>
		/// Gets the string information of the guild.
		/// </summary>
		public string StringInfo
		{
			get
			{
				return string.Format("{0} {1} {2} {3}", DbGuild.Name, GuildLeader.DbGuildRank.PlayerName, DbGuild.Level, MemberCount);
			}
		}
		
		/// <summary>
		/// Gets or sets the alliance id of the guild.
		/// </summary>
		public int AllianceId { get; set; }
		
		/// <summary>
		/// Creates a new guild.
		/// </summary>
		/// <param name="dbGuild">The database model association with the guild.</param>
		public Guild(DbGuild dbGuild)
			: base()
		{
			DbGuild = dbGuild;
			
			// Sets controller
			Guild = this;
			
			if (DbGuild.HasHouse)
			{
				CreateHouse();
			}
		}
		
		/// <summary>
		/// Creates a new guild with an empty database model.
		/// </summary>
		public Guild()
			: base()
		{
			DbGuild = new DbGuild();
			
			// Sets controller
			Guild = this;
		}
	}
}
