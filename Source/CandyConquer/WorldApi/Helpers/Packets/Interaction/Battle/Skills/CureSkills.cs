// Project by Bauss
using System;
using CandyConquer.WorldApi.Controllers.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Skills
{
	/// <summary>
	/// Handler for cure skills.
	/// </summary>
	public static class CureSkills
	{
		/// <summary>
		/// Handles a self curing skill.
		/// </summary>
		/// <param name="attacker">The attack.</param>
		/// <param name="target">The target.</param>
		/// <param name="packet">The packet.</param>
		/// <param name="spellPacket">The spell packet.</param>
		/// <param name="spellInfo">The spell info.</param>
		/// <returns>True if the skill was handled correct.</returns>
		public static bool HandleSelf(AttackableEntityController attacker, AttackableEntityController target,
		                              Models.Packets.Entities.InteractionPacket packet,
		                              Models.Packets.Spells.SpellPacket spellPacket, Models.Spells.SpellInfo spellInfo)
		{
			if (target == null)
			{
				target = attacker;
			}
			
			if (target.AttackableEntity.ClientId != attacker.AttackableEntity.ClientId)
			{
				return false;
			}
			
			var attackerPlayer = attacker as Models.Entities.Player;
			if (attackerPlayer != null)
			{
				if (DateTime.UtcNow < attackerPlayer.NextSmallLongSkill)
				{
					attackerPlayer.SendSystemMessage("REST");
					return false;
				}
				
				attackerPlayer.NextSmallLongSkill = DateTime.UtcNow.AddMilliseconds(Data.Constants.Time.SmallLongSkillTime);
			}
			
			if (spellInfo.Id == 1190 || spellInfo.Id == 7016)
			{
				attacker.AttackableEntity.HP += spellInfo.DbSpellInfo.Power;
			}
			else
			{
				attacker.AttackableEntity.MP += spellInfo.DbSpellInfo.Power;
			}
			
			if (attackerPlayer != null)
			{
				uint newExperience = (uint)Drivers.Repositories.Safe.Random.Next((int)spellInfo.DbSpellInfo.Power, ((int)spellInfo.DbSpellInfo.Power * 2));
				var skill = attackerPlayer.Spells.GetOrCreateSkill(spellInfo.Id);
				if (skill != null)
				{
					skill.Raise(newExperience);
				}
			}
			
			TargetFinalization.SkillFinalize(attacker, null, spellPacket, spellInfo.DbSpellInfo.Power);
			
			return true;
		}
		
		/// <summary>
		/// Handles cure skills for surroundings.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="target">The target.</param>
		/// <param name="packet">The packet.</param>
		/// <param name="spellPacket">The spell packet.</param>
		/// <param name="spellInfo">The spell info.</param>
		/// <returns>True if the skill was handled correct.</returns>
		public static bool HandleSurroundings(AttackableEntityController attacker, AttackableEntityController target,
		                                      Models.Packets.Entities.InteractionPacket packet,
		                                      Models.Packets.Spells.SpellPacket spellPacket, Models.Spells.SpellInfo spellInfo)
		{
			spellPacket.Safe = true;
			
			if (target == null)
			{
				return false;
			}
			
			var attackerPlayer = attacker as Models.Entities.Player;
			if (attackerPlayer != null)
			{
				if (DateTime.UtcNow < attackerPlayer.NextSmallLongSkill)
				{
					attackerPlayer.SendSystemMessage("REST");
					return false;
				}
				
				attackerPlayer.NextSmallLongSkill = DateTime.UtcNow.AddMilliseconds(Data.Constants.Time.SmallLongSkillTime);
			}
			
			var targetMonster = target as Models.Entities.Monster;
			if (targetMonster != null)
			{
				if (targetMonster.IsGuard)
				{
					return false;
				}
			}
			
			target.AttackableEntity.HP += spellInfo.DbSpellInfo.Power;
			TargetFinalization.SkillFinalize(attacker, target, spellPacket, spellInfo.DbSpellInfo.Power);
			
			if (spellInfo.Id == 1055)
			{
				foreach (var possibleTarget in attacker.GetAllInScreen())
				{
					if (possibleTarget.ClientId == target.AttackableEntity.ClientId)
					{
						continue;
					}
					
					if (Tools.RangeTools.GetDistanceU(possibleTarget.X, possibleTarget.Y, target.MapObject.X, target.MapObject.Y) > spellInfo.DbSpellInfo.Distance)
					{
						continue;
					}
					
					target = possibleTarget as AttackableEntityController;
					if (target == null)
					{
						continue;
					}
					
					targetMonster = possibleTarget as Models.Entities.Monster;
					if (targetMonster != null)
					{
						if (targetMonster.IsGuard)
						{
							continue;
						}
					}
					
					var targetPlayer = possibleTarget as Models.Entities.Player;
					if (targetPlayer != null)
					{
						if (!targetPlayer.LoggedIn)
						{
							continue;
						}
					}
					
					TargetFinalization.SkillFinalize(attacker, target, spellPacket, spellInfo.DbSpellInfo.Power);
				}
			}
			
			if (attackerPlayer != null)
			{
				uint newExperience = (uint)Drivers.Repositories.Safe.Random.Next((int)spellInfo.DbSpellInfo.Power, (int)(spellInfo.DbSpellInfo.Power * 2));
				newExperience *= (uint)spellPacket.Targets.Count;
				
				var skill = attackerPlayer.Spells.GetOrCreateSkill(spellInfo.Id);
				if (skill != null)
				{
					skill.Raise(newExperience);
				}
			}
			
			return true;
		}
	}
}
