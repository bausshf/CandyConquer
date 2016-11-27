// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for drops.
	/// </summary>
	public static class Drops
	{
		/// <summary>
		/// Gets a list of all drops.
		/// </summary>
		/// <param name="mapId">The map id.</param>
		/// <param name="server">The server.</param>
		/// <returns>The list of drops.</returns>
		public static List<Models.DbDrop> GetAll(int mapId, string server)
		{
			var pars = Sql.GetParsDict();
			pars.Add("MapId", mapId);
			pars.Add("Server", server);
			
			return SqlDalHelper.GetAll<Models.DbDrop>(ConnectionStrings.World,
			                                          string.Format("SELECT * FROM Drops WHERE {0}", Sql.GetSel(pars)),
			                                            pars);
		}
	}
}
