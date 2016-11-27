// Project by Bauss
using System;
using CandyConquer.WorldApi.Controllers.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Skills
{
	/// <summary>
	/// Handler for buffs and curse skills.
	/// </summary>
	public static class BuffCurseSkills
	{
		/// <summary>
		/// Handling buffs and curse skills
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="target">The target.</param>
		/// <param name="packet">The packet.</param>
		/// <param name="spellPacket">The spell packet.</param>
		/// <param name="spellInfo">The spell info.</param>
		/// <param name="curse">Boolean determining whether it should handle the skill as a curse.</param>
		/// <param name="disspell">Boolean determining whether it should handle the skill as a disspelling skill.</param>
		/// <returns>True if the skill was handled correct.</returns>
		public static bool Handle(AttackableEntityController attacker, AttackableEntityController target,
		                          Models.Packets.Entities.InteractionPacket packet,
		                          Models.Packets.Spells.SpellPacket spellPacket, Models.Spells.SpellInfo spellInfo,
		                          bool curse = false, bool disspell = false)
		{
			if (target == null)
			{
				return false;
			}
			
			var targetPlayer = target as Models.Entities.Player;
			
			if (targetPlayer == null)
			{
				return false;
			}
			
			if (!attacker.AttackableEntity.Alive)
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
			
			
			if (!targetPlayer.LoggedIn)
			{
				return false;
			}
			
			uint damage = 0;
			
			if (curse)
			{
				if (!TargetValidation.Validate(attacker, target))
				{
					return false;
				}
				
				targetPlayer.AddStatusFlag(Enums.StatusFlag.NoPotion, (5000 * (spellInfo.Level + 1)));
			}
			else if (disspell)
			{
				targetPlayer.RemoveStatusFlag(Enums.StatusFlag.Fly);
				
				damage = (uint)Math.Max(1, (targetPlayer.HP / 10));
				
				if (damage > 0)
				{
					Damage.Process(attacker, target, ref damage, false);
				}
			}
			else
			{
				var duration = spellInfo.DbSpellInfo.StepSecs * 1000;
				
				switch (spellInfo.Id)
				{
						case 1075: targetPlayer.AddStatusFlag(Enums.StatusFlag.PartiallyInvisible, duration); break;
						case 1085: targetPlayer.AddStatusFlag(Enums.StatusFlag.StarOfAccuracy, duration); break;
						case 1090: targetPlayer.AddStatusFlag(Enums.StatusFlag.Shield, duration); break;
						case 1095: targetPlayer.AddStatusFlag(Enums.StatusFlag.Stigma, duration); break;
				}
			}
			
			var maxExp = (int)(Math.Max(25, (int)targetPlayer.Level) / 2);
			
			uint newExperience = (uint)Drivers.Repositories.Safe.Random.Next(maxExp / 2, maxExp);
			var skill = targetPlayer.Spells.GetOrCreateSkill(spellInfo.Id);
			if (skill != null)
			{
				skill.Raise(newExperience);
			}
			
			TargetFinalization.SkillFinalize(attackerPlayer, targetPlayer, spellPacket, damage);
			
			return true;
		}
	}
}
