// Project by Bauss
using System;
using CandyConquer.WorldApi.Controllers.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Skills
{
	/// <summary>
	/// Handler for target finalization.
	/// </summary>
	public static class TargetFinalization
	{
		/// <summary>
		/// Finalizes a target and adds it to the spell targets.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="target">The target.</param>
		/// <param name="spellPacket">The spell packet.</param>
		/// <param name="damage">The damage.</param>
		public static void SkillFinalize(AttackableEntityController attacker,
		                                 AttackableEntityController target,
		                                 Models.Packets.Spells.SpellPacket spellPacket,
		                                 uint damage)
		{
			var attackerPlayer = attacker as Models.Entities.Player;
			var targetMonster = target as Models.Entities.Monster;
			
			if (attackerPlayer != null && targetMonster != null && !attackerPlayer.MaskedSkills.Contains(spellPacket.SpellId))
			{
				uint newExperience = Calculations.Experience.GetSpellExperience(attackerPlayer, targetMonster, damage);
				
				var skill = attackerPlayer.Spells.GetOrCreateSkill(spellPacket.SpellId);
				if (skill != null)
				{
					skill.Raise(newExperience);
				}
			}
			
			if (target != null)
			{
				spellPacket.Targets.Add(new Models.Packets.Spells.SpellPacket.SpellTarget
				                        {
				                        	AssociatedEntity = target,
				                        	ClientId = target.AttackableEntity.ClientId,
				                        	Damage = damage,
				                        	Hit = true,
				                        	ActivationValue = 0,
				                        	ActivationType = 0
				                        });
			}
			else
			{
				spellPacket.Targets.Add(new Models.Packets.Spells.SpellPacket.SpellTarget
				                        {
				                        	AssociatedEntity = attacker,
				                        	ClientId = attacker.AttackableEntity.ClientId,
				                        	Damage = damage,
				                        	Hit = true,
				                        	ActivationValue = 0,
				                        	ActivationType = 0
				                        });
			}
		}
	}
}
