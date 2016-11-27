// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Calculations
{
	/// <summary>
	/// Shared calculations for damage.
	/// </summary>
	public static class SharedDamageCalculations
	{
		/// <summary>
		/// Calculates the extra damage by level.
		/// </summary>
		/// <param name="attackerLevel">The attacker's level.</param>
		/// <param name="attackedLevel">The attacked's level.</param>
		/// <param name="damage">The damage.</param>
		public static void CalculateLevelExtraDamage(byte attackerLevel, byte attackedLevel, ref double damage)
		{
			if (damage > 1.0)
			{
				if (attackerLevel > (attackedLevel + 10))
				{
					damage *= 1.25;
				}
				else if (attackedLevel > (attackerLevel + 10))
				{
					damage *= 0.75;
				}
			}
		}
		
		/// <summary>
		/// Calculates the extra damage for reborns.
		/// </summary>
		/// <param name="reborns">The amount of reborns.</param>
		/// <param name="damage">The damage.</param>
		public static void CalculateRebornExtraDamage(byte reborns, ref double damage)
		{
			if (damage > 1.0)
			{
				damage += ((damage / 2) * reborns);
			}
		}
	}
}
