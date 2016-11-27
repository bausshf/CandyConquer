// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for spellls.
	/// </summary>
	public class Spells
	{
		/// <summary>
		/// Gets all spells for a player.
		/// </summary>
		/// <param name="playerId">The player id.</param>
		/// <param name="type">The spell type (Skill or Proficiency)</param>
		/// <returns>A list of all spells.</returns>
		private static List<Models.DbSpell> GetAll(int playerId, string type)
		{
			var pars = Sql.GetParsDict();
			pars.Add("PlayerId", playerId);
			pars.Add("Type", type);
			
			return SqlDalHelper.GetAll<Models.DbSpell>(ConnectionStrings.World,
			                                           string.Format("SELECT * FROM Spells WHERE {0}", Sql.GetSel(pars)),
			                                           pars);
		}
		
		/// <summary>
		/// Gets all proficiencies for a player.
		/// </summary>
		/// <param name="playerId">The player id.</param>
		/// <returns>A list of all proficiencies.</returns>
		public static List<Models.DbSpell> GetAllProficiencies(int playerId)
		{
			return GetAll(playerId, "Proficiency");
		}
		
		/// <summary>
		/// Gets all skills for a player.
		/// </summary>
		/// <param name="playerId">The player id.</param>
		/// <returns>A list of all skills.</returns>
		public static List<Models.DbSpell> GetAllSkills(int playerId)
		{
			return GetAll(playerId, "Skill");
		}
		
		/// <summary>
		/// Gets a list of all spell infos.
		/// </summary>
		/// <returns>A list of all spell infos.</returns>
		public static List<Models.DbSpellInfo> GetAllSpellInfos()
		{
			return SqlDalHelper.GetAll<Models.DbSpellInfo>(ConnectionStrings.World,
			                                               "SELECT * FROM SpellInfos",
			                                               null);
		}
	}
}
