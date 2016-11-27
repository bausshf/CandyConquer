// Project by Bauss
using System;
using CandyConquer.WorldApi.Controllers.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Skills
{
	/// <summary>
	/// Handler for the scatter skill.
	/// </summary>
	public static class ScatterSkill
	{
		/// <summary>
		/// Handles the scatter skill.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="packet">The packet.</param>
		/// <param name="spellPacket">The spell packet.</param>
		/// <param name="spellInfo">The spell info.</param>
		/// <returns>True if the skill was handled correctly.</returns>
		public static bool Handle(AttackableEntityController attacker,
		                          Models.Packets.Entities.InteractionPacket packet,
		                          Models.Packets.Spells.SpellPacket spellPacket,
		                          Models.Spells.SpellInfo spellInfo)
		{
			spellPacket.Process = true;
			
			var attackerPlayer = attacker as Models.Entities.Player;
			if (attackerPlayer != null)
			{
				if (!Ranged.ProcessPlayer(attackerPlayer, packet, 3))
				{
					return false;
				}
			}
			
			var sector = new Tools.Sector(attacker.MapObject.X, attacker.MapObject.Y,
			                              packet.X, packet.Y);
			sector.Arrange(spellInfo.Sector, spellInfo.DbSpellInfo.Range);
			
			foreach (var possibleTarget in attacker.GetAllInScreen())
			{
				if (spellPacket.Targets.Count > 8)
				{
					return true;
				}
				
				var target = possibleTarget as AttackableEntityController;
				
				if (target != null)
				{
					if (!TargetValidation.Validate(attacker, target))
					{
						continue;
					}
					
					if (target.ContainsStatusFlag(Enums.StatusFlag.Fly))
					{
						continue;
					}
					
					if (!sector.Inside(target.MapObject.X, target.MapObject.Y))
					{
						continue;
					}
					
					uint damage = Calculations.RangedCalculations.GetDamage(attacker.AttackableEntity, target.AttackableEntity);
					Damage.Process(attacker, target, ref damage, false);
					
					if (damage > 0)
					{
						TargetFinalization.SkillFinalize(attacker, target, spellPacket, damage);
					}
				}
			}
			
			if (attackerPlayer != null && spellPacket.Targets.Count > 0)
			{
				Ranged.DecreaseArrows(attackerPlayer, 3);
			}
			
			return true;
		}
	}
}
