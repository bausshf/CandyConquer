// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for nobility.
	/// </summary>
	public static class Nobility
	{
		/// <summary>
		/// Gets all nobility records.
		/// </summary>
		/// <param name="server">The server.</param>
		/// <returns>A list of all player's nobility.</returns>
		public static List<Models.DbPlayerNobility> GetAll(string server)
		{
			var pars = Sql.GetParsDict();
			pars.Add("Server", server);
			
			return SqlDalHelper.GetAll<Models.DbPlayerNobility>(ConnectionStrings.World,
			                                                    string.Format("SELECT * FROM PlayerNobility WHERE {0}", Sql.GetSel(pars)),
			                                                    pars);
		}
	}
}
