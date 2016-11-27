// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'Items' table.
	/// </summary>
	[DataEntry(Name = "Items", EntryPoint = ConnectionStrings.World)]
	public sealed class DbItem : SqlModel<DbItem>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		[DataIgnore()]
		public ushort BaseId
		{
			get
			{
				return (ushort)(Id / 1000);
			}
		}
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string Name { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public byte Job { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public byte WeaponSkill { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public byte Level { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string Sex { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort Strength { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort Agility { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort Vitality { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort Spirit { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint Price { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort MinAttack { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort MaxAttack { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort Defense { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort Dodge { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort HP { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort MP { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort Amount { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort MagicAttack { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort MagicDefense { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort AttackRange { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort AttackSpeed { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint CPPrice { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint CriticalStrike { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint SkillCriticalStrike { get;  set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint Immunity { get;  set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint Penetration { get;  set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint Block { get;  set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint BreakThrough { get;  set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint CounterAction { get;  set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint StackLimit { get;  set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint ResistMetal { get;  set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint ResistWood { get;  set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint ResistWater { get;  set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint ResistFire { get;  set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint ResistEarth { get;  set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string TypeName { get;  set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string Description { get;  set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint DragonSoulPhase { get;  set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint DragonSoulRequirements { get;  set; }
	}
}
