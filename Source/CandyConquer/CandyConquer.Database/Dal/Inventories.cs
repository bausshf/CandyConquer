// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;
using CandyConquer.Database.Models;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for inventories.
	/// </summary>
	public static class Inventories
	{
		/// <summary>
		/// Gets an inventory for a specific player.
		/// </summary>
		/// <param name="playerId">The player id.</param>
		/// <returns>A list of the inventory items.</returns>
		public static List<DbOwnerItem> GetInventory(int playerId)
		{
			var pars = Sql.GetParsDict();
			pars.Add("PlayerId", playerId);
			
			return SqlDalHelper.GetAll<DbOwnerItem>(ConnectionStrings.World,
			                                   string.Format("SELECT * FROM Inventories WHERE {0} AND [Deleted] = 'False'", Sql.GetSel(pars)),
			                                   pars);
		}
	}
}
