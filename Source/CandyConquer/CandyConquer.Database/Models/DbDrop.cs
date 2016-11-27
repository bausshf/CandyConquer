// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'Drops' table.
	/// </summary>
	[DataEntry(Name = "Drops", EntryPoint = ConnectionStrings.World)]
	public sealed class DbDrop : SqlModel<DbDrop>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MapId { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int OverAllDropChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string MobIds { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string AlwaysItemIds { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string CommonItemIds { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string UncommonItemIds { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string RareItemIds { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string SuperRareItemIds { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MinMoney { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MaxMoney { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MinCPs { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MaxCPs { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MinBoundCPs { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MaxBoundCPs { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public bool OneSocketWeapon { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public bool TwoSocketWeapon { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public bool OneSocketArmory { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public bool TwoSocketArmory { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MinPlus { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MaxPlus { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int RefinedChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int UniqueChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int EliteChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int SuperChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int PlusChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int WeaponSocketChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int ArmorySocketChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MoneyChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int CPsChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int BoundCPsChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MinMeteors { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MaxMeteors { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MeteorChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MinDragonballs { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MaxDragonballs { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int DragonballChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int NormalGemDropChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int RefinedGemDropChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int SuperGemDropChance { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string Server { get; set; }
	}
}
