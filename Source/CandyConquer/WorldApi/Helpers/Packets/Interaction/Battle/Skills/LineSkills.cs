//Project by BaussHacker aka. L33TS

using System;
using CandyConquer.WorldApi.Controllers.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Skills
{
	/// <summary>
	/// Handling line skills.
	/// </summary>
	public static class LineSkills
	{
		/// <summary>
		/// Handles line skills.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="packet">The packet.</param>
		/// <param name="spellPacket">The spell packet.</param>
		/// <param name="spellInfo">The spell info.</param>
		/// <returns>True if the skill was handled.</returns>
		public static bool Handle(AttackableEntityController attacker,
		                          Models.Packets.Entities.InteractionPacket packet,
		                          Models.Packets.Spells.SpellPacket spellPacket,
		                          Models.Spells.SpellInfo spellInfo)
		{
			spellPacket.Process = true;
			
			if (packet.TargetClientId == attacker.AttackableEntity.ClientId)
			{
				return false;
			}
			
			var ila = new Tools.ILA(
				attacker.MapObject.X, packet.X,
				attacker.MapObject.Y, packet.Y,
				(byte)spellInfo.DbSpellInfo.Range);
			
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
					
					if (!ila.InLine(target.MapObject.X, target.MapObject.Y))
					{
						continue;
					}
					
					uint damage = Calculations.PhysicalCalculations.GetDamage(attacker.AttackableEntity, target.AttackableEntity);
					Damage.Process(attacker, target, ref damage, false);
					
					if (damage > 0)
					{
						TargetFinalization.SkillFinalize(attacker, target, spellPacket, damage);
					}
				}
			}
			
			return true;
		}
	}
}
