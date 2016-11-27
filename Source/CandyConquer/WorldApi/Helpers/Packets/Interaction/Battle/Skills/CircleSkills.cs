// Project by Bauss
using System;
using CandyConquer.WorldApi.Controllers.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Skills
{
	/// <summary>
	/// Handler for skills in a circular range.
	/// </summary>
	public static class CircleSkills
	{
		/// <summary>
		/// Handles a circular skill.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="packet">The packet.</param>
		/// <param name="spellPacket">The spell packet.</param>
		/// <param name="spellInfo">The spell info.</param>
		/// <param name="isMagic">Boolean determining whether the circular skill is a magic skill or not.</param>
		/// <returns>True if the skill was handled correct.</returns>
		/// <remarks>This handles all types of circular skills (Physical, Magic, Ranged.) however ranged and physical is determined from either the skill id or whether isMagic is set.</remarks>
		public static bool Handle(AttackableEntityController attacker,
		                          Models.Packets.Entities.InteractionPacket packet,
		                          Models.Packets.Spells.SpellPacket spellPacket,
		                          Models.Spells.SpellInfo spellInfo,
		                         bool isMagic)
		{
			spellPacket.Process = true;
			
			if (!isMagic && spellInfo.Id < 8000 && 
			    packet.TargetClientId == attacker.AttackableEntity.ClientId)
			{
				return false;
			}
			
			var attackerPlayer = attacker as Models.Entities.Player;
			if (attackerPlayer != null)
			{
				if (DateTime.UtcNow < attackerPlayer.NextLongSkill)
				{
					attackerPlayer.SendSystemMessage("REST");
					return false;
				}
				
				attackerPlayer.NextLongSkill = DateTime.UtcNow.AddMilliseconds(Data.Constants.Time.LongSkillTime);
			}
			
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
					
					if (Tools.RangeTools.GetDistanceU(attacker.MapObject.X, attacker.MapObject.Y, target.MapObject.X, target.MapObject.Y) >= 8)
					{
						continue;
					}
					
					bool isRanged = (spellInfo.Id == 8030 || spellInfo.Id == 10308 || spellInfo.Id == 7013 || spellInfo.Id > 10360);
					
					uint damage = isRanged ?
						Calculations.RangedCalculations.GetDamage(attacker.AttackableEntity, target.AttackableEntity) :
						isMagic ?
						Calculations.MagicCalculations.GetDamage(attacker.AttackableEntity, target.AttackableEntity, spellInfo) :
						Calculations.PhysicalCalculations.GetDamage(attacker.AttackableEntity, target.AttackableEntity);
					
					Damage.Process(attacker, target, ref damage, false);
					
					if (isRanged && attackerPlayer != null)
					{
						Ranged.DecreaseArrows(attackerPlayer, 3);
					}
					
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
