// Project by Bauss
using System;
using CandyConquer.WorldApi.Controllers.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Skills
{
	/// <summary>
	/// Handler for sector skills.
	/// </summary>
	public static class SectorSkills
	{
		/// <summary>
		/// Handles the sector skills.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="packet">The packet.</param>
		/// <param name="spellPacket">The spell packet.</param>
		/// <param name="spellInfo">The spell info.</param>
		/// <param name="isMagic">Boolean determining whether the attack is magic or not.</param>
		/// <param name="isPoison">Booealning determining whether the attack is poisonous (Toxic fog.)</param>
		/// <returns>True if the skill was handled correctly.</returns>
		public static bool Handle(AttackableEntityController attacker,
		                          Models.Packets.Entities.InteractionPacket packet,
		                          Models.Packets.Spells.SpellPacket spellPacket,
		                          Models.Spells.SpellInfo spellInfo,
		                         bool isMagic, bool isPoison = false)
		{
			spellPacket.Process = true;
			
			if (packet.TargetClientId == attacker.AttackableEntity.ClientId)
			{
				return false;
			}
			
			if (isPoison && Tools.RangeTools.GetDistanceU(attacker.MapObject.X, attacker.MapObject.Y, packet.X, packet.Y) >= 9)
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
			
			ushort x = attacker.MapObject.X;
			ushort y = attacker.MapObject.Y;
			
			if (spellInfo.Id == 6001)
			{
				if (Tools.RangeTools.GetDistanceU(x, y, packet.X, packet.Y) > spellInfo.DbSpellInfo.Distance)
				{
					return false;
				}
				
				x = packet.X;
				y = packet.Y;
			}
			
			var sector = new Tools.Sector(x, y,
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
					
					if (isPoison && target.ContainsStatusFlag(Enums.StatusFlag.Poisoned))
					{
						continue;
					}
					
					uint damage = isPoison ?
						(uint)target.AttackableEntity.HP / 10 :
						isMagic ?
						Calculations.MagicCalculations.GetDamage(attacker.AttackableEntity, target.AttackableEntity, spellInfo) :
						Calculations.PhysicalCalculations.GetDamage(attacker.AttackableEntity, target.AttackableEntity);
					
					if (isPoison)
					{
						target.AttackableEntity.PoisonEffect = (spellInfo.DbSpellInfo.Power - 30000);
						target.AddStatusFlag(Enums.StatusFlag.Poisoned, 60000);
					}
					
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
