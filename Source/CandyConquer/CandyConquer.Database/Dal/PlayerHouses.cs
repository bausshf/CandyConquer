// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for player houses.
	/// </summary>
	public static class PlayerHouses
	{
		/// <summary>
		/// Gets all houses associated with a player.
		/// </summary>
		/// <param name="playerId">The player id.</param>
		/// <returns>A list of all associated houses.</returns>
		public static List<Models.DbPlayerHouse> GetAll(int playerId)
		{
			var pars = Sql.GetParsDict();
			pars.Add("PlayerId", playerId);
			
			return SqlDalHelper.GetAll<Models.DbPlayerHouse>(ConnectionStrings.World,
			                                                 string.Format("SELECT * FROM PlayerHouses WHERE {0}", Sql.GetSel(pars)),
			                                                 pars);
		}
	}
}
