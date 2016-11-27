// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for the whisper cache.
	/// </summary>
	public static class WhisperCache
	{
		/// <summary>
		/// Gets all whispers by a recipent.
		/// </summary>
		/// <param name="to">The recipent.</param>
		/// <param name="server">The server of the recipent.</param>
		/// <returns>A list of all whispers.</returns>
		public static List<Models.DbWhisper> GetAllByRecipent(string to, string server)
		{
			var pars = Sql.GetParsDict();
			pars.Add("To", to);
			pars.Add("Server", server);
			
			return SqlDalHelper.GetAll<Models.DbWhisper>(ConnectionStrings.World,
			                                             string.Format("SELECT * FROM WhisperCache WHERE {0}", Sql.GetSel(pars)),
			                                             pars);
		}
	}
}
