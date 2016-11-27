// Project by Bauss
using System;
using System.Collections.Generic;
using System.Linq;
using Candy;
using CandyConquer.Database.Models;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for guilds.
	/// </summary>
	public static class Guilds
	{
		/// <summary>
		/// Gets a list of all guilds.
		/// </summary>
		/// <param name="serverName">The server associated with the guilds.</param>
		/// <returns>A list of the guilds.</returns>
		public static List<DbGuild> GetAll(string serverName)
		{
			var pars = Sql.GetParsDict();
			pars.Add("Server", serverName);
			
			return SqlDalHelper.GetAll<DbGuild>(ConnectionStrings.World,
			                                    string.Format("SELECT * FROM Guilds WHERE {0}", Sql.GetSel(pars)),
			                                    pars);
		}
		
		/// <summary>
		/// Gets a list of all members in a guild.
		/// </summary>
		/// <param name="guildId">The guild id.</param>
		/// <returns>A list of the members.</returns>
		public static List<DbGuildRank> GetAllMembers(int guildId)
		{
			var pars = Sql.GetParsDict();
			pars.Add("GuildId", guildId);
			
			return SqlDalHelper.GetAll<DbGuildRank>(ConnectionStrings.World,
			                                               string.Format("SELECT * FROM GuildRanks WHERE {0}", Sql.GetSel(pars)),
			                                               pars)
				.Where(member =>
				        {
				        	var player = Players.GetPlayerById(member.PlayerId);
				        	if (player != null && !string.IsNullOrWhiteSpace(player.Name) && player.Level.HasValue)
				        	{
				        		member.PlayerName = player.Name;
				        		member.PlayerLevel = player.Level.Value;
				        		
				        		return true;
				        	}
				        	
				        	return false;
				        }).ToList();
		}
		
		/// <summary>
		/// Gets a list of all associations by a specific association type.
		/// </summary>
		/// <param name="guildId">The guild.</param>
		/// <param name="associationType">The association type.</param>
		/// <returns>A list of the associations.</returns>
		public static List<DbGuildAssociation> GetAllAssociations(int guildId, string associationType)
		{
			var pars = Sql.GetParsDict();
			pars.Add("GuildId", guildId);
			pars.Add("AssociationType", associationType);
			
			return SqlDalHelper.GetAll<DbGuildAssociation>(ConnectionStrings.World,
			                                               string.Format("SELECT * FROM GuildAssociations WHERE {0}", Sql.GetSel(pars)),
			                                               pars);
		}
		
		/// <summary>
		/// Checks whether a specific association exists.
		/// </summary>
		/// <param name="guildId">The guild id.</param>
		/// <param name="associationGuildId">The association guild id.</param>
		/// <param name="associationType">The association type.</param>
		/// <returns>True if the association exists.</returns>
		public static bool IsAssociation(int guildId, int associationGuildId, string associationType)
		{
			var pars = Sql.GetParsDict();
			pars.Add("GuildId", guildId);
			pars.Add("AssociationGuildId", associationGuildId);
			pars.Add("AssociationType", associationType);
			
			return SqlDalHelper.Get<DbGuildAssociation>(ConnectionStrings.World,
			                                               string.Format("SELECT * FROM GuildAssociations WHERE {0}", Sql.GetSel(pars)),
			                                               pars) != null;
		}
		
		/// <summary>
		/// Deletes an association.
		/// </summary>
		/// <param name="guildId">The guild id.</param>
		/// <param name="associationGuildId">The association guild id.</param>
		public static void DeleteAssociation(int guildId, int associationGuildId)
		{
			var pars = Sql.GetParsDict();
			pars.Add("GuildId", guildId);
			pars.Add("AssociationGuildId", associationGuildId);
			
			Sql.ExecuteNonQuery(ConnectionStrings.World,
			                           string.Format("DELETE FROM GuildAssociations WHERE {0}", Sql.GetSel(pars)),
			                           pars);
		}
		
		/// <summary>
		/// Deletes all associations tied to a guild.
		/// </summary>
		/// <param name="guildId">The guild id.</param>
		public static void DeleteAllAssociations(int guildId)
		{
			var pars = Sql.GetParsDict();
			pars.Add("GuildId", guildId);
			
			Sql.ExecuteNonQuery(ConnectionStrings.World,
			                           string.Format("DELETE FROM GuildAssociations WHERE {0}", Sql.GetSel(pars)),
			                           pars);
			
			pars = Sql.GetParsDict();
			pars.Add("AssociationGuildId", guildId);
			
			Sql.ExecuteNonQuery(ConnectionStrings.World,
			                           string.Format("DELETE FROM GuildAssociations WHERE {0}", Sql.GetSel(pars)),
			                           pars);
		}
		
		/// <summary>
		/// Checks whether a player has a guild or not.
		/// </summary>
		/// <param name="playerId">The player id.</param>
		/// <param name="guildId">The guild id.</param>
		/// <returns>True if the player has a guild.</returns>
		public static bool HasGuild(int playerId, out int guildId)
		{
			var pars = Sql.GetParsDict();
			pars.Add("PlayerId", playerId);
			
			var dbGuildRank = SqlDalHelper.Get<DbGuildRank>(ConnectionStrings.World,
			                                               string.Format("SELECT * FROM GuildRanks WHERE {0}", Sql.GetSel(pars)),
			                                               pars);
			if (dbGuildRank == null)
			{
				guildId = 0;
				return false;
			}
			else
			{
				guildId = dbGuildRank.GuildId;
				return true;
			}
		}
	}
}
