// Project by Bauss
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CandyConquer.Database.Models;

namespace CandyConquer.WorldApi.Models.Maps
{
	/// <summary>
	/// Model for drops.
	/// </summary>
	public sealed class Drop : Controllers.Maps.DropController
	{
		/// <summary>
		/// Creates a new drop.
		/// </summary>
		/// <param name="dbDrop">The database drop associated with the drop.</param>
		public Drop(DbDrop dbDrop)
		{
			MapId = dbDrop.MapId;
			OverAllDropChance = dbDrop.OverAllDropChance;
			
			MobIds = string.IsNullOrWhiteSpace(dbDrop.MobIds) ?
				new ReadOnlyCollection<int>(new List<int>()) :
				dbDrop.MobIds.Split('-')
				.Where(input => !string.IsNullOrWhiteSpace(input))
				.Select(id => int.Parse(id)).ToList().AsReadOnly();
			
			AlwaysItemIds = string.IsNullOrWhiteSpace(dbDrop.AlwaysItemIds) ?
				new ReadOnlyCollection<uint>(new List<uint>()) :
				dbDrop.AlwaysItemIds.Split('-')
				.Where(input => !string.IsNullOrWhiteSpace(input))
				.Select(id => uint.Parse(id)).ToList().AsReadOnly();
			
			CommonItemIds = string.IsNullOrWhiteSpace(dbDrop.CommonItemIds) ?
				new ReadOnlyCollection<uint>(new List<uint>()) :
				dbDrop.CommonItemIds.Split('-')
				.Where(input => !string.IsNullOrWhiteSpace(input))
				.Select(id => uint.Parse(id)).ToList().AsReadOnly();
			
			UncommonItemIds = string.IsNullOrWhiteSpace(dbDrop.UncommonItemIds) ?
				new ReadOnlyCollection<uint>(new List<uint>()) :
				dbDrop.UncommonItemIds.Split('-')
				.Where(input => !string.IsNullOrWhiteSpace(input))
				.Select(id => uint.Parse(id)).ToList().AsReadOnly();
			
			RareItemIds = string.IsNullOrWhiteSpace(dbDrop.RareItemIds) ?
				new ReadOnlyCollection<uint>(new List<uint>()) :
				dbDrop.RareItemIds.Split('-')
				.Where(input => !string.IsNullOrWhiteSpace(input))
				.Select(id => uint.Parse(id)).ToList().AsReadOnly();
			
			SuperRareItemIds = string.IsNullOrWhiteSpace(dbDrop.SuperRareItemIds) ?
				new ReadOnlyCollection<uint>(new List<uint>()) :
				dbDrop.SuperRareItemIds.Split('-')
				.Where(input => !string.IsNullOrWhiteSpace(input))
				.Select(id => uint.Parse(id)).ToList().AsReadOnly();
			
			MinMoney = dbDrop.MinMoney;
			MaxMoney = dbDrop.MaxMoney;
			MinCPs = dbDrop.MinCPs;
			MaxCPs = dbDrop.MaxCPs;
			MinBoundCPs = dbDrop.MinBoundCPs;
			MaxBoundCPs = dbDrop.MaxBoundCPs;
			
			OneSocketWeapon = dbDrop.OneSocketWeapon;
			TwoSocketWeapon = dbDrop.TwoSocketWeapon;
			OneSocketArmory = dbDrop.OneSocketArmory;
			TwoSocketArmory = dbDrop.TwoSocketArmory;
			
			PlusChance = dbDrop.PlusChance;
			MinPlus = dbDrop.MinPlus;
			MaxPlus = dbDrop.MaxPlus;
			
			RefinedChance = dbDrop.RefinedChance;
			UniqueChance = dbDrop.UniqueChance;
			EliteChance = dbDrop.EliteChance;
			SuperChance = dbDrop.SuperChance;
			
			WeaponSocketChance = dbDrop.WeaponSocketChance;
			ArmorySocketChance = dbDrop.ArmorySocketChance;
			
			MoneyChance = dbDrop.MoneyChance;
			CPsChance = dbDrop.CPsChance;
			BoundCPsChance = dbDrop.BoundCPsChance;
			
			MinMeteors = dbDrop.MinMeteors;
			MaxMeteors = dbDrop.MaxMeteors;
			MeteorChance = dbDrop.MeteorChance;
			
			MinDragonballs = dbDrop.MinDragonballs;
			MaxDragonballs = dbDrop.MaxDragonballs;
			DragonballChance = dbDrop.DragonballChance;
			
			NormalGemDropChance = dbDrop.NormalGemDropChance;
			RefinedGemDropChance = dbDrop.RefinedGemDropChance;
			SuperGemDropChance = dbDrop.SuperGemDropChance;
			
			Drop = this;
		}
		
		/// <summary>
		/// The map id.
		/// </summary>
		public int MapId { get; private set; }
		
		/// <summary>
		/// The over all drop chance.
		/// </summary>
		public int OverAllDropChance { get; private set; }
		
		/// <summary>
		/// The monster ids associated with the drop.
		/// </summary>
		public ReadOnlyCollection<int> MobIds { get; private set; }
		
		/// <summary>
		/// The items always to drop.
		/// </summary>
		public ReadOnlyCollection<uint> AlwaysItemIds { get; private set; }
		
		/// <summary>
		/// The items to commonly drop.
		/// </summary>
		public ReadOnlyCollection<uint> CommonItemIds { get; private set; }
		
		/// <summary>
		/// The items to uncommonly drop.
		/// </summary>
		public ReadOnlyCollection<uint> UncommonItemIds { get; private set; }
		
		/// <summary>
		/// The items to rarely drop.
		/// </summary>
		public ReadOnlyCollection<uint> RareItemIds { get; private set; }
		
		/// <summary>
		/// The items to almost never drop. (Rare than rare.)
		/// </summary>
		public ReadOnlyCollection<uint> SuperRareItemIds { get; private set; }
		
		/// <summary>
		/// The minimum money to drop.
		/// </summary>
		public int MinMoney { get; private set; }
		
		/// <summary>
		/// The maximum money to drop.
		/// </summary>
		public int MaxMoney { get; private set; }
		
		/// <summary>
		/// The minimum cps to drop.
		/// </summary>
		public int MinCPs { get; private set; }
		
		/// <summary>
		/// The maximum cps to drop.
		/// </summary>
		public int MaxCPs { get; private set; }
		
		/// <summary>
		/// The minimum bound cps to drop.
		/// </summary>
		public int MinBoundCPs { get; private set; }
		
		/// <summary>
		/// The maximum bound cps to drop.
		/// </summary>
		public int MaxBoundCPs { get; private set; }
		
		/// <summary>
		/// Boolean indicating whether one socket weapons can be dropped.
		/// </summary>
		public bool OneSocketWeapon { get; private set; }
		
		/// <summary>
		/// Boolean indicating whether two socket weapons can be dropped.
		/// </summary>
		public bool TwoSocketWeapon { get; private set; }
		
		/// <summary>
		/// Boolean indicating whether one socket armory can be dropped.
		/// </summary>
		public bool OneSocketArmory { get; private set; }
		
		/// <summary>
		/// Boolean indicating whether two socket armory can be dropped.
		/// </summary>
		public bool TwoSocketArmory { get; private set; }
		
		/// <summary>
		/// The minimum plus.
		/// </summary>
		public int MinPlus { get; private set; }
		
		/// <summary>
		/// The maximum plus.
		/// </summary>
		public int MaxPlus { get; private set; }
		
		/// <summary>
		/// The refined item chance.
		/// </summary>
		public int RefinedChance { get; private set; }
		
		/// <summary>
		/// The unique item chance.
		/// </summary>
		public int UniqueChance { get; private set; }
		
		/// <summary>
		/// The elite item chance.
		/// </summary>
		public int EliteChance { get; private set; }
		
		/// <summary>
		/// The super item chance.
		/// </summary>
		public int SuperChance { get; private set; }
		
		/// <summary>
		/// The plus chance.
		/// </summary>
		public int PlusChance { get; private set; }
		
		/// <summary>
		/// The weapon socket chance.
		/// </summary>
		public int WeaponSocketChance { get; private set; }
		
		/// <summary>
		/// The armory socket chance.
		/// </summary>
		public int ArmorySocketChance { get; private set; }
		
		/// <summary>
		/// The money chance.
		/// </summary>
		public int MoneyChance { get; private set; }
		
		/// <summary>
		/// The cps chance.
		/// </summary>
		public int CPsChance { get; private set; }
		
		/// <summary>
		/// The bound cps chance.
		/// </summary>
		public int BoundCPsChance { get; private set; }
		
		/// <summary>
		/// The minimum meteors to drop.
		/// </summary>
		public int MinMeteors { get; private set; }
		
		/// <summary>
		/// The maximum meteors to drop.
		/// </summary>
		public int MaxMeteors { get; private set; }
		
		/// <summary>
		/// The chance of a meteor drop.
		/// </summary>
		public int MeteorChance { get; private set; }
		
		/// <summary>
		/// The mimimum dragonballs to drop.
		/// </summary>
		public int MinDragonballs { get; private set; }
		
		/// <summary>
		/// The maximum dragonballs to drop.
		/// </summary>
		public int MaxDragonballs { get; private set; }
		
		/// <summary>
		/// The chance of a dragonball to drop.
		/// </summary>
		public int DragonballChance { get; private set; }
		
		/// <summary>
		/// The chance of a normal gem drop.
		/// </summary>
		public int NormalGemDropChance { get; private set; }
		
		/// <summary>
		/// The chance of a refined gem drop.
		/// </summary>
		public int RefinedGemDropChance { get; private set; }
		
		/// <summary>
		/// The chance of a super gem drop.
		/// </summary>
		public int SuperGemDropChance { get; private set; }
	}
}
