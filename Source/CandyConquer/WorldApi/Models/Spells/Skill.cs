// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Models.Spells
{
	/// <summary>
	/// Model for a skill.
	/// </summary>
	public class Skill : Controllers.Spells.SkillController, ISpellData
	{
		/// <summary>
		/// Gets the player owning the skill.
		/// </summary>
		public Models.Entities.Player Player { get; private set; }
		
		/// <summary>
		/// The database model associated with the skill.
		/// </summary>
		private Database.Models.DbSpell _dbSpell;
		
		/// <summary>
		/// Gets the id of the skill.
		/// </summary>
		public ushort Id { get { return _dbSpell.SpellId; } }
		
		/// <summary>
		/// Gets the level of the skill.
		/// </summary>
		public ushort Level { get { return _dbSpell.Level; } }
		
		/// <summary>
		/// Gets the experience of the skill.
		/// </summary>
		public uint Experience { get { return _dbSpell.Experience; } }
		
		/// <summary>
		/// Creates a new skill.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="dbSpell">The database model associated with the skill.</param>
		public Skill(Models.Entities.Player player, Database.Models.DbSpell dbSpell)
			: base()
		{
			Player = player;
			_dbSpell = dbSpell;
			_dbSpell.Type = "Skill";
			_dbSpell.PlayerId = player.DbPlayer.Id;
			
			Skill = this;
		}
		
		/// <summary>
		/// Sends the skill model to the client.
		/// </summary>
		public void SendToClient()
		{
			base.SendSkillToClient();
		}
		
		/// <summary>
		/// Raises the level of the skill.
		/// </summary>
		/// <param name="experience">The experience.</param>
		/// <remarks>View the controller for the handling.</remarks>
		public void Raise(uint experience)
		{
			uint currentExperience = _dbSpell.Experience;
			ushort currentLevel = _dbSpell.Level;
			
			base.RaiseSkill(experience, ref currentExperience, ref currentLevel);
			
			_dbSpell.Experience = currentExperience;
			_dbSpell.Level = currentLevel;
			
			_dbSpell.Update();
			SendToClient();
		}
	}
}
