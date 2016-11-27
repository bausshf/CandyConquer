// Project by Bauss
using System;
using System.Collections.Concurrent;
using CandyConquer.WorldApi.Models.Spells;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A spell collection for players.
	/// </summary>
	public sealed class SpellCollection
	{
		/// <summary>
		/// The collection of skills.
		/// </summary>
		private ConcurrentDictionary<ushort, Skill> _skills;
		/// <summary>
		/// The collection of proficiencies.
		/// </summary>
		private ConcurrentDictionary<ushort, Proficiency> _proficiencies;
		/// <summary>
		/// The player that owns the spells.
		/// </summary>
		private Models.Entities.Player _player;
		
		/// <summary>
		/// Creates a new spell collection.
		/// </summary>
		/// <param name="player">The player.</param>
		public SpellCollection(Models.Entities.Player player)
		{
			_player = player;
			
			_skills = new ConcurrentDictionary<ushort, Skill>();
			_proficiencies = new ConcurrentDictionary<ushort, Proficiency>();
		}
		
		/// <summary>
		/// Attempts to add a skill.
		/// </summary>
		/// <param name="skill">The database spell.</param>
		/// <returns>True if the skill was added.</returns>
		public bool TryAddSkill(Database.Models.DbSpell skill)
		{
			return _skills.TryAdd(skill.SpellId, new Models.Spells.Skill(_player, skill));
		}
		
		/// <summary>
		/// Attempts to add a proficiency.
		/// </summary>
		/// <param name="proficiency">The proficiency.</param>
		/// <returns>True if the proficiency was added.</returns>
		public bool TryAddProficiency(Database.Models.DbSpell proficiency)
		{
			return _proficiencies.TryAdd(proficiency.SpellId, new Models.Spells.Proficiency(_player, proficiency));
		}
		
		/// <summary>
		/// Gets or creates a skill if non-existing.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>The skill.</returns>
		public Skill GetOrCreateSkill(ushort id)
		{
			Skill skill;
			if (!_skills.TryGetValue(id, out skill))
			{
				var dbSpell = new Database.Models.DbSpell
				{
					SpellId = id
				};
				skill = new Skill(_player, dbSpell);
				
				if (dbSpell.Create())
				{
					_skills.TryAdd(id, skill);
				}
			}
			
			return skill;
		}
		
		/// <summary>
		/// Checks whether the collection contains a skill or not.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>True if the skill is existing.</returns>
		public bool ContainsSkill(ushort id)
		{
			return _skills.ContainsKey(id);
		}
		
		/// <summary>
		/// Checks whether the collection contains a proficiency or not.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>True if the proficiency is existing.</returns>
		public bool ContainsProficiency(ushort id)
		{
			return _proficiencies.ContainsKey(id);
		}
		
		/// <summary>
		/// Gets or creates a proficiency if non-existing.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>The proficiency.</returns>
		public Proficiency GetOrCreateProficiency(ushort id)
		{
			Proficiency proficiency;
			if (!_proficiencies.TryGetValue(id, out proficiency))
			{
				var dbSpell = new Database.Models.DbSpell
				{
					SpellId = id
				};
				proficiency = new Proficiency(_player, dbSpell);
				
				if (dbSpell.Create())
				{
					_proficiencies.TryAdd(id, proficiency);
				}
			}
			
			return proficiency;
		}
		
		/// <summary>
		/// Sends all proficiencies to the client.
		/// </summary>
		public void SendAllProficiencies()
		{
			foreach (var proficiency in _proficiencies.Values)
			{
				proficiency.SendToClient();
			}
		}
		
		/// <summary>
		/// Sends all skills to the client.
		/// </summary>
		public void SendAllSkills()
		{
			foreach (var skill in _skills.Values)
			{
				skill.SendToClient();
			}
		}
	}
}
