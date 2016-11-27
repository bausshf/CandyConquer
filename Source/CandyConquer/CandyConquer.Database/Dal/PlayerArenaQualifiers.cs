// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for player arena qualifiers.
	/// </summary>
	public class PlayerArenaQualifiers
	{
		/// <summary>
		/// Gets all arena qualifier informations.
		/// </summary>
		/// <param name="server">The server.</param>
		/// <returns>A list of all arena qualifier informations.</returns>
		public static List<Models.DbPlayerArenaQualifier> GetAll(string server)
		{
			var pars = Sql.GetParsDict();
			pars.Add("Server", server);
			
			return SqlDalHelper.GetAll<Models.DbPlayerArenaQualifier>(ConnectionStrings.World,
			                                                          string.Format("SELECT * FROM PlayerArenaQualifiers WHERE {0}", Sql.GetSel(pars)),
			                                                          pars);
		}
	}
}
