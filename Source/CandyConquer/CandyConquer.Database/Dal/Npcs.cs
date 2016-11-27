// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;
using CandyConquer.Database.Models;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for npcs.
	/// </summary>
	public static class Npcs
	{
		/// <summary>
		/// Gets all npcs tied to a server.
		/// </summary>
		/// <param name="server">The server.</param>
		/// <returns>A list of all the npcs.</returns>
		public static List<DbNpc> GetAll(string server)
		{
			var pars = Sql.GetParsDict();
			pars.Add("Server", server);
			pars.Add("AllServers", "All");
			
			return SqlDalHelper.GetAll<DbNpc>(ConnectionStrings.World,
			                                   "SELECT * FROM NPCs WHERE Server = @Server OR Server = @AllServers",
			                                   pars);
		}
	}
}
