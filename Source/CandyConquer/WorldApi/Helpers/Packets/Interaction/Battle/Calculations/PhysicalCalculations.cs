// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Calculations
{
	/// <summary>
	/// Damage calculations for physical attacks.
	/// </summary>
	public static class PhysicalCalculations
	{
		/// <summary>
		/// Gets the damage between two entities using physical attack calculations.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked.</param>
		/// <returns>The amount of damage dealt.</returns>
		public static uint GetDamage(IAttackableEntity attacker, IAttackableEntity attacked)
		{
			double damage = 0;
			
			var attackerPlayer = attacker as Player;
			var attackerMonster = attacker as Monster;
			var attackedPlayer = attacked as Player;
			var attackedMonster = attacked as Monster;
			
			if (attackerPlayer != null)
			{
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
			else if (attackerMonster != null)
			{
				damage = Drivers.Repositories.Safe.Random.Next(attackerMonster.MinAttack, attackerMonster.MaxAttack);
				
				if (attackedPlayer != null)
				{
					Monster_Player(attackerMonster, attackedPlayer, ref damage);
				}
				else if (attackedMonster != null)
				{
					Monster_Monster(attackerMonster, attackedMonster, ref damage);
				}
			}
			
			SharedDamageCalculations.CalculateLevelExtraDamage(attacker.Level, attacked.Level, ref damage);
			SharedDamageCalculations.CalculateRebornExtraDamage(attacker.Reborns, ref damage);
			
			return (uint)Math.Max((int)damage, (int)1);
		}
		
		/// <summary>
		/// Calculates the physical damage between two players.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked.</param>
		/// <param name="damage">The damage.</param>
		private static void Player_Player(Player attacker, Player attacked, ref double damage)
		{
			damage *= attacker.DragonGemPercentage + 1;
			damage -= attacked.Defense;
			
			if (attacked.ContainsStatusFlag(Enums.StatusFlag.Shield))
			{
				damage *= 0.5;
			}
			
			if (attacker.ContainsStatusFlag(Enums.StatusFlag.Stigma))
			{
				damage *= 1.75;
			}
			
			double damagePercentage = damage;
			damagePercentage = (damage / 100) * attacked.TortoiseGemPercentage;
			damage -= damagePercentage;
			
			damagePercentage = (damage / 100) * attacked.Bless;
			damage -= damagePercentage;
		}
		
		/// <summary>
		/// Calculates the physical damage from a player to a monster.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The player.</param>
		/// <param name="damage">The damage.</param>
		private static void Player_Monster(Player attacker, Monster attacked, ref double damage)
		{
			damage *= attacker.DragonGemPercentage + 1;
			damage -= attacked.Defense;
			
			if (attacked.ContainsStatusFlag(Enums.StatusFlag.Shield))
			{
				damage *= 0.5;
			}
			
			if (attacker.ContainsStatusFlag(Enums.StatusFlag.Stigma))
			{
				damage *= 1.75;
			}
		}
		
		/// <summary>
		/// Calculates the physical damage from a monster to a player.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked.</param>
		/// <param name="damage">The damage.</param>
		private static void Monster_Player(Monster attacker, Player attacked, ref double damage)
		{
			damage -= attacked.Defense;
			
			if (attacked.ContainsStatusFlag(Enums.StatusFlag.Shield))
			{
				damage *= 0.5;
			}
			
			if (attacker.ContainsStatusFlag(Enums.StatusFlag.Stigma))
			{
				damage *= 1.75;
			}
			
			double damagePercentage = damage;
			damagePercentage = (damage / 100) * attacked.TortoiseGemPercentage;
			damage -= damagePercentage;
			
			damagePercentage = (damage / 100) * attacked.Bless;
			damage -= damagePercentage;
		}
		
		/// <summary>
		/// Calculates the physical damage between two monsters.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked.</param>
		/// <param name="damage">The damage.</param>
		private static void Monster_Monster(Monster attacker, Monster attacked, ref double damage)
		{
			damage -= attacked.Defense;
			
			if (attacked.ContainsStatusFlag(Enums.StatusFlag.Shield))
			{
				damage *= 0.5;
			}
			
			if (attacker.ContainsStatusFlag(Enums.StatusFlag.Stigma))
			{
				damage *= 1.75;
			}
		}
	}
}
