// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Calculations
{
	/// <summary>
	/// Experience calculations.
	/// </summary>
	public static class Experience
	{
		/// <summary>
		/// Calculating the normal player experience.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="monster">The monster.</param>
		/// <param name="damage">The damage.</param>
		/// <returns>The amount of experience.</returns>
		public static ulong GetExperience(Player player, Monster monster, uint damage)
		{
			ulong newExperience = (ulong)(damage <= 3 ? 1 : Drivers.Repositories.Safe.Random.Next((int)(damage / 2), (int)damage));
			
			int levelDifferent = (monster.Level - player.Level);
			
			if (levelDifferent > 20 && player.Level < 90)
			{
				newExperience *= 10;
			}
			
			if (levelDifferent >= -5 && levelDifferent <= 10)
			{
				newExperience *= 2;
				
				if (levelDifferent >= 0)
				{
					newExperience += (newExperience / 2);
				}
			}
			
			if (levelDifferent < -5)
			{
				if (levelDifferent < -10)
				{
					newExperience = monster.ExtraExperience;
				}
				else
				{
					newExperience /= 2;
				}
			}
			
			return newExperience;
		}
		
		/// <summary>
		/// Calculating the proficiency experience.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="monster">The monster.</param>
		/// <param name="damage">The damage.</param>
		/// <returns>The amount of experience.</returns>
		public static uint GetProficiencyExperience(Player player, Monster monster, uint damage)
		{
			uint newExperience = (uint)(damage <= 3 ? 1 : Drivers.Repositories.Safe.Random.Next((int)(damage / 2), (int)damage));
			
			if (monster.Level > (player.Level + 10))
			{
				newExperience *= 2;
			}
			else if (monster.Level > (player.Level + 10))
			{
				newExperience = (uint)Drivers.Repositories.Safe.Random.Next(1, (int)monster.Level);
			}
			
			return newExperience;
		}
		
		/// <summary>
		/// Calculating the spell experience.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="monster">The monster.</param>
		/// <param name="damage">The damage.</param>
		/// <returns>The amount of experience.</returns>
		public static uint GetSpellExperience(Player player, Monster monster, uint damage)
		{
			uint newExperience = (uint)(damage <= 3 ? 1 : Drivers.Repositories.Safe.Random.Next((int)(damage / 2), (int)damage));
			
			if (monster.Level > (player.Level + 10))
			{
				newExperience *= 2;
			}
			else if (monster.Level > (player.Level + 10))
			{
				newExperience = (uint)Drivers.Repositories.Safe.Random.Next(1, (int)monster.Level);
			}
			
			return newExperience;
		}
	}
}
