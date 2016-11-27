// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'SpellInfos' table.
	/// </summary>
	[DataEntry(Name = "SpellInfos", EntryPoint = ConnectionStrings.World)]
	public sealed class DbSpellInfo : SqlModel<DbSpellInfo>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort SpellId { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort Type { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public byte Sort { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public bool Crime { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public bool Ground { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public bool Multi { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public byte Target { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public byte Level { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort UseMP { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort Power { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort IntoneSpeed { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public byte SpellPercent { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public byte StepSecs { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort Range { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort Distance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ulong Status { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint NeedExperience { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public byte NeedLevel { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public byte UseXP { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort WeaponSubtype { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public byte UseEP { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort NextMagic { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public byte UseItem { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public byte UseItemNum { get; set; }
	}
}
