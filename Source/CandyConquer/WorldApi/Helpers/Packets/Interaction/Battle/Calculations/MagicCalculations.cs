// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Entities;
using CandyConquer.WorldApi.Models.Spells;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Calculations
{
	/// <summary>
	/// Damage calculations for magic attacks.
	/// </summary>
	public static class MagicCalculations
	{
		/// <summary>
		/// Gets the damage between two entities using magic attack calculations.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked.</param>
		/// <param name="spellInfo">The spell info.</param>
		/// <returns>The amount of damage dealt.</returns>
		public static uint GetDamage(IAttackableEntity attacker, IAttackableEntity attacked, SpellInfo spellInfo)
		{
			double damage = 0;
			
			var attackerPlayer = attacker as Player;
			var attackerMonster = attacker as Monster;
			var attackedPlayer = attacked as Player;
			var attackedMonster = attacked as Monster;
			
			if (attackerPlayer != null)
			{
				damage = (double)attackerPlayer.MagicAttack;
				
				if (attackedPlayer != null)
				{
					Player_Player(attackerPlayer, attackedPlayer, spellInfo, ref damage);
				}
				else if (attackedMonster != null)
				{
					Player_Monster(attackerPlayer, attackedMonster, spellInfo, ref damage);
				}
			}
			else if (attackerMonster != null)
			{
				damage = Drivers.Repositories.Safe.Random.Next(attackerMonster.MinAttack, attackerMonster.MaxAttack);
				
				if (attackedPlayer != null)
				{
					Monster_Player(attackerMonster, attackedPlayer, spellInfo, ref damage);
				}
				else if (attackedMonster != null)
				{
					Monster_Monster(attackerMonster, attackedMonster, spellInfo, ref damage);
				}
			}
			
			if (attackerMonster == null|| attackerMonster != null && attackerMonster.Behaviour != Enums.MonsterBehaviour.DeathGuard)
			{
				SharedDamageCalculations.CalculateLevelExtraDamage(attacker.Level, attacked.Level, ref damage);
				SharedDamageCalculations.CalculateRebornExtraDamage(attacker.Reborns, ref damage);
			}
			
			return (uint)Math.Max((int)damage, (int)1);
		}
		
		/// <summary>
		/// Calculates the magic damage between two players.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked.</param>
		/// <param name="spellInfo">The spell info.</param>
		/// <param name="damage">The damage.</param>
		private static void Player_Player(Player attacker, Player attacked, SpellInfo spellInfo, ref double damage)
		{
			damage -= attacked.MagicDefense;
			damage *= 0.65;
			damage += spellInfo.DbSpellInfo.Power;
			damage *= attacker.PhoenixGemPercentage + 1;
			
			double damagePercentage = damage;
			damagePercentage = (damage / 100) * attacked.TortoiseGemPercentage;
			damage -= damagePercentage;
			
			damagePercentage = (damage / 100) * attacked.Bless;
			damage -= damagePercentage;
		}
		
		/// <summary>
		/// Calculates the magic damage from a player to a monster.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked.</param>
		/// <param name="spellInfo">The spell info.</param>
		/// <param name="damage">The damage.</param>
		private static void Player_Monster(Player attacker, Monster attacked, SpellInfo spellInfo, ref double damage)
		{
			if (attacked.Behaviour == Enums.MonsterBehaviour.DeathGuard)
			{
				damage = 1;
				return;
			}
			
			damage -= attacked.MagicDefense;
			damage *= 0.65;
			damage += spellInfo.DbSpellInfo.Power;
			damage *= attacker.PhoenixGemPercentage + 1;
		}
		
		/// <summary>
		/// Calculates the magic damage from a monster to a player.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked.</param>
		/// <param name="spellInfo">The spell info.</param>
		/// <param name="damage">The damage.</param>
		private static void Monster_Player(Monster attacker, Player attacked, SpellInfo spellInfo, ref double damage)
		{
			if (attacker.Behaviour == Enums.MonsterBehaviour.DeathGuard)
			{
				damage = 999999999;
				return;
			}
			
			damage -= attacked.MagicDefense;
			damage *= 0.65;
			damage += spellInfo.DbSpellInfo.Power;
			
			double damagePercentage = damage;
			damagePercentage = (damage / 100) * attacked.TortoiseGemPercentage;
			damage -= damagePercentage;
			
			damagePercentage = (damage / 100) * attacked.Bless;
			damage -= damagePercentage;
		}
		
		/// <summary>
		/// Calculates the magic damage between two monsters.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked.</param>
		/// <param name="spellInfo">The spell info.</param>
		/// <param name="damage">The damage.</param>
		private static void Monster_Monster(Monster attacker, Monster attacked, SpellInfo spellInfo, ref double damage)
		{
			if (attacker.Behaviour == Enums.MonsterBehaviour.DeathGuard)
			{
				damage = 999999999;
				return;
			}
			
			damage -= attacked.MagicDefense;
			damage *= 0.65;
			damage += spellInfo.DbSpellInfo.Power;
		}
	}
}
