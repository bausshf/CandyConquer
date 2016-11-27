// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for monsters.
	/// </summary>
	public static class Monsters
	{
		/// <summary>
		/// Gets a list of all monsters.
		/// </summary>
		/// <returns>The list of monsters.</returns>
		public static List<Models.DbMonster> GetAll()
		{
			return SqlDalHelper.GetAll<Models.DbMonster>(ConnectionStrings.World,
			                                             "SELECT * FROM Monsters",
			                                             null);
		}
		
		/// <summary>
		/// Gets all monster spawns.
		/// </summary>
		/// <param name="server">The server associated with the spawns.</param>
		/// <returns>A list of all spawns tied to the specific server.</returns>
		public static List<Models.DbMonsterSpawn> GetAllSpawns(string server)
		{
			var pars = Sql.GetParsDict();
			pars.Add("Server", server);
			
			return SqlDalHelper.GetAll<Models.DbMonsterSpawn>(ConnectionStrings.World,
			                                                  string.Format("SELECT * FROM MonsterSpawns", Sql.GetSel(pars)),
			                                                  pars);
		}
		
		/// <summary>
		/// Gets a list of all guards.
		/// </summary>
		/// <returns>Gets a list of all guard spawns.</returns>
		public static List<Models.DbGuard> GetAllGuards()
		{
			return SqlDalHelper.GetAll<Models.DbGuard>(ConnectionStrings.World,
			                                           "SELECT * FROM Guards",
			                                           null);
		}
	}
}
