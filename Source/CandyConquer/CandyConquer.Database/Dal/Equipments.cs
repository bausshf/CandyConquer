// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;
using CandyConquer.Database.Models;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for equipments.
	/// </summary>
	public static class Equipments
	{
		/// <summary>
		/// Gets the equipment for a specific player.
		/// </summary>
		/// <param name="playerId">The player id.</param>
		/// <returns>A list of the equipments.</returns>
		public static List<DbOwnerItem> GetEquipments(int playerId)
		{
			var pars = Sql.GetParsDict();
			pars.Add("PlayerId", playerId);
			
			return SqlDalHelper.GetAll<DbOwnerItem>(ConnectionStrings.World,
			                                   string.Format("SELECT * FROM Equipments WHERE {0} AND [Deleted] = 'False'", Sql.GetSel(pars)),
			                                   pars);
		}
	}
}
