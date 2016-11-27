// Project by Bauss
using System;
using CandyConquer.WorldApi.Controllers.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Skills
{
	/// <summary>
	/// Handler for single skills.
	/// </summary>
	public static class SingleSkills
	{
		/// <summary>
		/// Handles single skills.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="target">The target.</param>
		/// <param name="packet">The packet.</param>
		/// <param name="spellPacket">The spell packet.</param>
		/// <param name="spellInfo">The spell info.</param>
		/// <param name="isMagic">Boolean determining whether the single attack is magic.</param>
		/// <param name="isRanged">Boolean determining whether the single attack is ranged.</param>
		/// <returns>True if the skill was handled correctly.</returns>
		/// <remarks>If both isMagic and isRanged is set to false then it's assumed as a physical skill.</remarks>
		public static bool Handle(AttackableEntityController attacker, AttackableEntityController target,
		                          Models.Packets.Entities.InteractionPacket packet,
		                          Models.Packets.Spells.SpellPacket spellPacket, Models.Spells.SpellInfo spellInfo,
		                          bool isMagic, bool isRanged = false)
		{
			if (target == null)
			{
				return false;
			}
			
			if (attacker.AttackableEntity.ClientId == target.AttackableEntity.ClientId)
			{
				return false;
			}
			
			if (!TargetValidation.Validate(attacker, target))
			{
				return false;
			}
			
			var attackerPlayer = attacker as Models.Entities.Player;
			
			if (isRanged)
			{
				if (attackerPlayer != null)
				{
					if (!Ranged.ProcessPlayer(attackerPlayer, packet, 1))
					{
						return false;
					}
				}
				
				Ranged.DecreaseArrows(attackerPlayer, 1);
			}
			
			uint damage = isRanged ?
				Calculations.RangedCalculations.GetDamage(attacker.AttackableEntity, target.AttackableEntity) :
				isMagic ?
				Calculations.MagicCalculations.GetDamage(attacker.AttackableEntity, target.AttackableEntity, spellInfo):
				Calculations.PhysicalCalculations.GetDamage(attacker.AttackableEntity, target.AttackableEntity);
			
			if (!isMagic)
			{
				damage = (uint)Math.Max(1, (int)(((double)damage) * (1.1 + (0.1 * spellInfo.Level))));
				
				if (spellInfo.Id == 1290 && damage > 0 && !isRanged)
				{
					double damagePercentage = (double)((damage / 100) * 26.6);
					damage += (uint)(damagePercentage * spellInfo.Level);
				}
				
				if (spellInfo.Id == 6000 && damage > 0 && !isRanged)
				{
					damage = (uint)((damage / 100) * (spellInfo.DbSpellInfo.Power - 30000));
				}
			}
			
			Damage.Process(attacker, target, ref damage, true);
			
			if (damage > 0)
			{
				TargetFinalization.SkillFinalize(attacker, target, spellPacket, damage);
				
				return true;
			}
			
			return false;
		}
	}
}
