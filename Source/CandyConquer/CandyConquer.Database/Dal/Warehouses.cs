// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;
using CandyConquer.Database.Models;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for warehouses.
	/// </summary>
	public static class Warehouses
	{
		/// <summary>
		/// Gets a list of all warehouse items for a player.
		/// </summary>
		/// <param name="playerId">The player id.</param>
		/// <returns>The list of all warehouse items.</returns>
		public static List<DbOwnerItem> GetWarehouseItems(int playerId)
		{
			var pars = Sql.GetParsDict();
			pars.Add("PlayerId", playerId);
			
			return SqlDalHelper.GetAll<DbOwnerItem>(ConnectionStrings.World,
			                                        string.Format("SELECT * FROM PlayerWarehouses WHERE {0} AND [Deleted] = 'False'", Sql.GetSel(pars)),
			                                        pars);
		}
		
		/// <summary>
		/// Gets a list of all guild warehouse items.
		/// </summary>
		/// <param name="guildId">The guild id.</param>
		/// <returns>A list of all guild warehouse items.</returns>
		public static List<DbOwnerItem> GetGuildWarehouseItems(int guildId)
		{
			var pars = Sql.GetParsDict();
			pars.Add("PlayerId", guildId);
			
			return SqlDalHelper.GetAll<DbOwnerItem>(ConnectionStrings.World,
			                                        string.Format("SELECT * FROM GuildWarehouses WHERE {0} AND [Deleted] = 'False'", Sql.GetSel(pars)),
			                                        pars);
		}
	}
}
