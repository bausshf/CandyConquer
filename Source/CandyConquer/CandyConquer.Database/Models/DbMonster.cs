// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'Monsters' table.
	/// </summary>
	[DataEntry(Name = "Monsters", EntryPoint = ConnectionStrings.World)]
	public class DbMonster : SqlModel<DbMonster>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MobId { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string Name { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public byte Level { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort Mesh { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MinAttack { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MaxAttack { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int Defense { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int Dexterity { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int Dodge { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int AttackRange { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int ViewRange { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int AttackSpeed { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MoveSpeed { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int AttackType { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string Behaviour { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MagicType { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MagicDefense { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MagicHitRate { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public long ExtraExperience { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int ExtraDamage { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int Boss { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int Action { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int HP { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MP { get; set; }
	}
}
