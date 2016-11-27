// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Controllers.Spells
{
	/// <summary>
	/// Controller for proficiencies.
	/// </summary>
	public class ProficiencyController
	{
		/// <summary>
		/// The proficiency associated with the controller.
		/// </summary>
		public Models.Spells.Proficiency Proficiency { get; protected set; }
		
		/// <summary>
		/// Creates a new proficiency controller.
		/// </summary>
		protected ProficiencyController()
		{
		}
		
		/// <summary>
		/// Sends the proficiency to the client.
		/// </summary>
		protected void SendProficiencyToClient()
		{
			Proficiency.Player.ClientSocket.Send(new Models.Packets.Spells.ProficiencyPacket
			                                     {
			                                     	Id = Proficiency.Id,
			                                     	Level = Proficiency.Level,
			                                     	Experience = Proficiency.Experience,
			                                     	RequiredExperience = Data.Constants.Level.GetProfExperience((byte)Proficiency.Level)
			                                     });
		}
		
		/// <summary>
		/// Raises the level of the proficiency.
		/// </summary>
		/// <param name="newExperience">The new experience.</param>
		/// <param name="currentExperience">The current/new experience.</param>
		/// <param name="level">The current/new level.</param>
		protected void RaiseProficiency(uint newExperience, ref uint currentExperience, ref ushort level)
		{
			if (level >= 20)
			{
				return;
			}
			
			if (Proficiency.Player.Level < Data.Constants.GameMode.MaxLevel &&
			    level >= 12)
			{
				return;
			}
			
			newExperience *= Data.Constants.GameMode.ProficiencyExperienceRate;
			currentExperience += newExperience;
			
			uint requiredExperience = Data.Constants.Level.GetProfExperience((byte)Proficiency.Level);
			
			if (currentExperience >= requiredExperience)
			{
				level++;
				currentExperience = 0;
				
				if (Proficiency.Player.LoggedIn)
				{
					Proficiency.Player.SendSystemMessage("PROF_LEVEL_UP");
				}
			}
			else if (Proficiency.Player.LoggedIn)
			{
				Proficiency.Player.SendSystemMessage("PROF_GAIN_EXP");
			}
		}
	}
}
