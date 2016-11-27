// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Models.Spells
{
	/// <summary>
	/// Model for proficiency.
	/// </summary>
	public class Proficiency : Controllers.Spells.ProficiencyController, ISpellData
	{
		/// <summary>
		/// The player owning the proficiency.
		/// </summary>
		public Models.Entities.Player Player { get; private set; }
		/// <summary>
		/// The database model tied with the proficiency.
		/// </summary>
		private Database.Models.DbSpell _dbSpell;
		
		/// <summary>
		/// Gets the id of the proficiency.
		/// </summary>
		public ushort Id { get { return _dbSpell.SpellId; } }
		
		/// <summary>
		/// Gets the level of the proficiency.
		/// </summary>
		public ushort Level { get { return _dbSpell.Level; } }
		
		/// <summary>
		/// Gets the experience of the proficiency.
		/// </summary>
		public uint Experience { get { return _dbSpell.Experience; } }
		
		/// <summary>
		/// Creates a new proficiency.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="dbSpell">The database model tied with it.</param>
		public Proficiency(Models.Entities.Player player, Database.Models.DbSpell dbSpell)
			: base()
		{
			Player = player;
			_dbSpell = dbSpell;
			_dbSpell.Type = "Proficiency";
			_dbSpell.PlayerId = player.DbPlayer.Id;
			
			Proficiency = this;
		}
		
		/// <summary>
		/// Sends the proficiency model to the client.
		/// </summary>
		public void SendToClient()
		{
			base.SendProficiencyToClient();
		}
		
		/// <summary>
		/// Raises the level of the proficiency.
		/// </summary>
		/// <param name="experience">The experience.</param>
		/// <remarks>View the controller for the handling.</remarks>
		public void Raise(uint experience)
		{
			uint currentExperience = _dbSpell.Experience;
			ushort currentLevel = _dbSpell.Level;
			
			base.RaiseProficiency(experience, ref currentExperience, ref currentLevel);
			
			_dbSpell.Experience = currentExperience;
			_dbSpell.Level = currentLevel;
			
			_dbSpell.Update();
			SendToClient();
		}
	}
}
