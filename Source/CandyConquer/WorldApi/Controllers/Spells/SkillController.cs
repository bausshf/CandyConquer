// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Controllers.Spells
{
	/// <summary>
	/// Controller for a skill.
	/// </summary>
	public class SkillController
	{
		/// <summary>
		/// The skill associated with the controller.
		/// </summary>
		public Models.Spells.Skill Skill { get; protected set; }
		
		/// <summary>
		/// Creates a new skill controller.
		/// </summary>
		protected SkillController()
		{
		}
		
		/// <summary>
		/// Sends the skill to the client.
		/// </summary>
		protected void SendSkillToClient()
		{
			Skill.Player.ClientSocket.Send(new Models.Packets.Spells.SkillPacket
			                               {
			                               	Experience = Skill.Experience,
			                               	Id = Skill.Id,
			                               	Level = Skill.Level
			                               });
		}
		
		/// <summary>
		/// Raises the level of the skill.
		/// </summary>
		/// <param name="newExperience">The new experience.</param>
		/// <param name="currentExperience">The current/new experience.</param>
		/// <param name="level">The current/new level.</param>
		protected void RaiseSkill(uint newExperience, ref uint currentExperience, ref ushort level)
		{
			newExperience *= Data.Constants.GameMode.SpellExperienceRate;
			
			if (Skill.Player.MoonGemPercentage > 0)
			{
				currentExperience += (uint)(newExperience * Skill.Player.MoonGemPercentage);
			}
			else
			{
				currentExperience += newExperience;
			}
			
			var spellInfo = Collections.SpellInfoCollection.GetSpellInfo(Skill.Id, (byte)level);
			if (spellInfo == null)
			{
				// ban ?? do something ?? idfk ...
				// should never actually reach here anyway ...
				return;
			}
			
			if (!Collections.SpellInfoCollection.ContainsSpell(Skill.Id, (byte)(level + 1)))
			{
				currentExperience = 0;
				return;
			}
			
			if (Skill.Player.Level < spellInfo.DbSpellInfo.NeedLevel)
			{
				return;
			}
			
			if (currentExperience >= spellInfo.DbSpellInfo.NeedExperience)
			{
				level++;
				currentExperience = 0;
				
				if (Skill.Player.LoggedIn)
				{
					Skill.Player.SendSystemMessage("SPELL_LEVEL_UP");
				}
			}
			else if (Skill.Player.LoggedIn)
			{
				Skill.Player.SendSystemMessage("SPELL_GAIN_EXP");
			}
		}
	}
}
