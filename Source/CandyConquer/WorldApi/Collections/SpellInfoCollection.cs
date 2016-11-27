// Project by Bauss
using System;
using System.Collections.Concurrent;
using CandyConquer.Drivers.Repositories.Collections;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A collection of spell information.
	/// </summary>
	public static class SpellInfoCollection
	{
		/// <summary>
		/// The collection of spell infos.
		/// </summary>
		private static MultiConcurrentDictionary<ushort, byte, Models.Spells.SpellInfo> _spells;
		/// <summary>
		/// The collection of weapon spells.
		/// </summary>
		private static ConcurrentDictionary<ushort, ushort> _weaponSpells;
		
		/// <summary>
		/// Static constructor for SpellInfoCollection.
		/// </summary>
		static SpellInfoCollection()
		{
			_spells = new MultiConcurrentDictionary<ushort, byte, CandyConquer.WorldApi.Models.Spells.SpellInfo>();
			_weaponSpells = new ConcurrentDictionary<ushort, ushort>();
		}
		
		/// <summary>
		/// Loads all the spell infos.
		/// </summary>
		public static void Load()
		{
			var spells = Database.Dal.Spells.GetAllSpellInfos();
			foreach (var dbSpell in spells)
			{
				var spellInfo = new Models.Spells.SpellInfo(dbSpell);
				
				if (_spells.ContainsKey(spellInfo.Id))
				{
					_spells[spellInfo.Id].TryAdd(spellInfo.Level, spellInfo);
				}
				else
				{
					if (_spells.TryAdd(spellInfo.Id))
					{
						_spells[spellInfo.Id].TryAdd(spellInfo.Level, spellInfo);
					}
				}
				
				switch (spellInfo.Id)
				{
					case 5010:
					case 7020:
					case 1290:
					case 1260:
					case 5030:
					case 5040:
					case 7000:
					case 7010:
					case 7030:
					case 7040:
					case 1250:
					case 5050:
					case 5020:
					case 10490:
					case 1300:
						{
							if (spellInfo.DbSpellInfo.Distance >= 3)
							{
								spellInfo.DbSpellInfo.Distance = 3;
							}
							
							if (spellInfo.DbSpellInfo.Range > 3)
							{
								spellInfo.DbSpellInfo.Range = 3;
							}
							
							_weaponSpells.TryAdd(spellInfo.WeaponSubtype, spellInfo.Id);
							break;
						}
				}
			}
		}
		
		/// <summary>
		/// Checks whether the collection contains a speill by id and level.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="level">The level.</param>
		/// <returns>True if the spell exists.</returns>
		public static bool ContainsSpell(ushort id, byte level)
		{
			return _spells.ContainsKey(id) && _spells[id].ContainsKey(level);
		}
		
		/// <summary>
		/// Checks whether the collection contains a spell by id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>True if the spell exists.</returns>
		public static bool ContainsSpell(ushort id)
		{
			return _spells.ContainsKey(id);
		}
		
		/// <summary>
		/// Checks if the collection contains a weapon spell by id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>True if the weapon spell exists.</returns>
		public static bool ContainsWeaponSpell(ushort id)
		{
			return _weaponSpells.ContainsKey(id);
		}
		
		/// <summary>
		/// Gets a weapon spell.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>The weapon spell id.</returns>
		public static ushort GetWeaponSpell(ushort id)
		{
			return _weaponSpells[id];
		}
		
		/// <summary>
		/// Gets a spell info.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="level">The level.</param>
		/// <returns>The spell info if existing, null otherwise.</returns>
		public static Models.Spells.SpellInfo GetSpellInfo(ushort id, byte level)
		{
			Models.Spells.SpellInfo spellInfo = null;
			if (_spells.ContainsKey(id))
			{
				_spells[id].TryGetValue(level, out spellInfo);
			}
			
			return spellInfo;
		}
		
		/// <summary>
		/// Gets the higest level spell for a specific spell.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>The higest level spell for that specific spell.</returns>
		public static Models.Spells.SpellInfo GetHighestSpell(ushort id)
		{
			Models.Spells.SpellInfo spellInfo = null;
			if(_spells.ContainsKey(id))
			{
				var spellCollection = _spells[id];
				
				spellInfo = spellCollection[(byte)(spellCollection.Count - 1)];
			}
			return spellInfo;
		}
	}
}
