// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Calculations
{
	/// <summary>
	/// Damage calculations for ranged attacks.
	/// </summary>
	public static class RangedCalculations
	{
		/// <summary>
		/// Gets the damage between two entities using ranged attack calculations.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked.</param>
		/// <returns>The amount of damage dealt.</returns>
		public static uint GetDamage(IAttackableEntity attacker, IAttackableEntity attacked)
		{
			double damage = 0;
			
			var attackerPlayer = attacker as Player;
			
			if (attackerPlayer != null)
			{
				var attackedPlayer = attacked as Player;
				var attackedMonster = attacked as Monster;
				
				damage = Drivers.Repositories.Safe.Random.Next(attackerPlayer.MinAttack, attackerPlayer.MaxAttack);
				
				if (attackedPlayer != null)
				{
					Player_Player(attackerPlayer, attackedPlayer, ref damage);
				}
				else if (attackedMonster != null)
				{
					Player_Monster(attackerPlayer, attackedMonster, ref damage);
				}
			}
			
			SharedDamageCalculations.CalculateLevelExtraDamage(attacker.Level, attacked.Level, ref damage);
			SharedDamageCalculations.CalculateRebornExtraDamage(attacker.Reborns, ref damage);
			
			return (uint)Math.Max((int)damage, (int)1);
		}
		
		/// <summary>
		/// Calculates the ranged damage between two players.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked.</param>
		/// <param name="damage">The damage.</param>
		private static void Player_Player(Player attacker, Player attacked, ref double damage)
		{
			double damagePercentage = (damage / 100);
			int dodge = Math.Min(95, attacked.Dodge);
			
			if (dodge > 0)
			{
				damage -= (damagePercentage * dodge);
			}
			
			damage *= 0.45;
			damage *= attacker.DragonGemPercentage + 1;
			
			if (attacker.ContainsStatusFlag(Enums.StatusFlag.Stigma))
			{
				damage *= 1.75;
			}
			
			damagePercentage = damage;
			damagePercentage = (damage / 100) * attacked.TortoiseGemPercentage;
			damage -= damagePercentage;
			
			damagePercentage = (damage / 100) * attacked.Bless;
			damage -= damagePercentage;
		}
		
		/// <summary>
		/// Calculates the damage from a player to a monster.
		/// </summary>
		/// <param name="attacker">The attack.</param>
		/// <param name="attacked">The attacked.</param>
		/// <param name="damage">The damage.</param>
		private static void Player_Monster(Player attacker, Monster attacked, ref double damage)
		{
			double damagePercent = (damage / 100);
			int dodge = Math.Min(95, attacked.Dodge);
			
			if (dodge > 0)
			{
				damage -= (damagePercent * dodge);
			}
			
			damage *= 0.45;
			damage *= attacker.DragonGemPercentage + 1;
			
			if (attacker.ContainsStatusFlag(Enums.StatusFlag.Stigma))
			{
				damage *= 1.75;
			}
		}
	}
}
