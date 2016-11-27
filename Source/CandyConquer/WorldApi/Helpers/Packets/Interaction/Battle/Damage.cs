// Project by Bauss
using System;
using CandyConquer.WorldApi.Controllers.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle
{
	/// <summary>
	/// Handler for dealing player damage, item damage and as well player experience.
	/// </summary>
	public static class Damage
	{
		/// <summary>
		/// Processes a regular damage handling.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked.</param>
		/// <param name="damage">The damage.</param>
		/// <param name="killDamage">Boolean determining whether the datam processing can decrease hp and kill.</param>
		public static void Process(AttackableEntityController attacker, AttackableEntityController attacked, ref uint damage, bool killDamage)
		{
			var attackedPlayer = attacked as Models.Entities.Player;
			var attackedMonster = attacked as Models.Entities.Monster;
			
			#region Attacker = Player
			var attackerPlayer = attacker as Models.Entities.Player;
			
			if (attackerPlayer != null)
			{
				attackerPlayer.LoseAttackDura(damage);
				attackerPlayer.Target = attacked.AttackableEntity;
				
				#region Attacked = Player
				if (attackedPlayer != null)
				{
					if (attackerPlayer.Battle != null)
					{
						if (!attackerPlayer.Battle.HandleAttack(attackerPlayer, attackedPlayer, ref damage))
						{
							damage = 0;
							return;
						}
					}
					else
					{
						if (!attackerPlayer.Map.SafePK &&
						    !attackedPlayer.ContainsStatusFlag(Enums.StatusFlag.BlueName) &&
						    !attackedPlayer.ContainsStatusFlag(Enums.StatusFlag.RedName) &&
						    !attackedPlayer.ContainsStatusFlag(Enums.StatusFlag.BlackName))
						{
							attackerPlayer.AddStatusFlag(Enums.StatusFlag.BlueName, Data.Constants.Time.BlueNameTime);
						}
						
						attackerPlayer.LoseDefenseDura(damage);
					}
				}
				#endregion
				#region Attacked = Monster
				else if (attackedMonster != null)
				{
					if (attackedMonster.IsGuard)
					{
						attackerPlayer.AddStatusFlag(Enums.StatusFlag.BlueName, Data.Constants.Time.BlueNameTime);
					}
					
					if (damage > 0)
					{
						ulong newExperience = Calculations.Experience.GetExperience(attackerPlayer, attackedMonster, damage);
						
						attackerPlayer.AddExperience(newExperience);
					}
				}
				#endregion
			}
			#endregion
			#region Attacker = Monster
			else
			{
				var attackerMonster = attacker as Models.Entities.Monster;
				
				if (attackerMonster != null)
				{
					#region Attacked = Player
					if (attackedPlayer != null)
					{
						attackedPlayer.LoseDefenseDura(damage);
					}
					#endregion
					#region Attacked = Monster
					/*else if (attackedMonster != null)
					{
						
					}*/
					#endregion
				}
			}
			#endregion
			
			if (killDamage)
			{
				Hit(attacker, attacked, damage);
			}
		}
		
		/// <summary>
		/// Processes a hit.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked.</param>
		/// <param name="damage">The damage of the hit.</param>
		public static void Hit(AttackableEntityController attacker, AttackableEntityController attacked, uint damage)
		{
			attacked.AttackableEntity.HP -= (int)damage;
			
			if (attacked.AttackableEntity.HP <= 0)
			{
				attacked.Kill(attacker, damage);
			}
		}
	}
}
