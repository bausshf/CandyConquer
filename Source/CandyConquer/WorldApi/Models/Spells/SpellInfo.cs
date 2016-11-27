// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Models.Spells
{
	/// <summary>
	/// Model for spell info.
	/// </summary>
	public class SpellInfo
	{
		/// <summary>
		/// Gets the database model associated with the spell info.
		/// </summary>
		public Database.Models.DbSpellInfo DbSpellInfo { get; private set; }
		
		/// <summary>
		/// Gets the id of the spell. (DbSpellInfo.Type)
		/// </summary>
		public ushort Id
		{
			get { return DbSpellInfo.Type; }
		}
		
		/// <summary>
		/// Gets the weapon subtype of the spell. (DbSpellInfo.Weaponsubtype)
		/// </summary>
		public ushort WeaponSubtype
		{
			get { return DbSpellInfo.WeaponSubtype; }
		}
		
		/// <summary>
		/// Gets the level of the spell. (DbSpellInfo.Level)
		/// </summary>
		public byte Level
		{
			get { return DbSpellInfo.Level; }
		}
		
		/// <summary>
		/// Creates a new spell info.
		/// </summary>
		/// <param name="dbSpellInfo">The database model associated with the spell info.</param>
		public SpellInfo(Database.Models.DbSpellInfo dbSpellInfo)
		{
			DbSpellInfo = dbSpellInfo;
			
			PowerPercentage = (float)(DbSpellInfo.Power == 0 ? 1 : (DbSpellInfo.Power % 1000) / 100);
			
			DbSpellInfo.Distance = (ushort)(
				DbSpellInfo.Distance >= 4 ?
				DbSpellInfo.Distance - 1 :
				DbSpellInfo.Distance > 17 ? 17 :
				DbSpellInfo.Distance
			);
			
			Sector = (DbSpellInfo.Range * 20);
		}
		
		/// <summary>
		/// Boolean determining whether the spell uses arrows.
		/// </summary>
		public bool UseArrow
		{
			get { return DbSpellInfo.UseItem == 50; }
		}
		
		/// <summary>
		/// Gets the sector of the spell.
		/// </summary>
		public int Sector { get; private set; }
		
		/// <summary>
		/// Gets the power percentage of the spell.
		/// </summary>
		public float PowerPercentage { get; private set; }
	}
}
