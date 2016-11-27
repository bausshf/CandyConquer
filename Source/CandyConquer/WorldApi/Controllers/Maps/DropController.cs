// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Controllers.Maps
{
	/// <summary>
	/// Controller for drops.
	/// </summary>
	public class DropController
	{
		/// <summary>
		/// The drop associated with the controller.
		/// </summary>
		public Models.Maps.Drop Drop { get; protected set; }
		
		/// <summary>
		/// Creates a new drop controller.
		/// </summary>
		protected DropController()
		{
		}
		
		/// <summary>
		/// Performs the drop.
		/// </summary>
		/// <param name="map">The map.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="mobId">The monster id.</param>
		/// <param name="player">The player that caused the drop.</param>
		public void Perform(Models.Maps.Map map, ushort x, ushort y, int mobId, Models.Entities.Player player)
		{
			if (Tools.CalculationTools.ChanceSuccess(Drop.OverAllDropChance))
			{
				if (map != null)
				{
					if (Drop.MobIds.Count > 0)
					{
						if (!Drop.MobIds.Contains(mobId))
						{
							return;
						}
					}
					
					uint dropClientId = player != null ? player.ClientId : 0;
					
					#region Always Items
					{
						var items = Drop.AlwaysItemIds;
						
						if (items.Count > 0)
						{
							var itemId = (uint)(items.Count == 1 ? items[0] :
							                    items[Drivers.Repositories.Safe.Random.Next(items.Count)]);
							
							var item = Collections.ItemCollection.CreateItemById(itemId);
							if (item != null)
							{
								PerformPlus(item);
								PerformSockets(item);
								PerformQuality(item);
								
								var location = map.GetValidItemCoordinate(x, y);
								
								item.Drop(map.Id, location.X, location.Y, false, dropClientId);
							}
						}
					}
					#endregion
					#region Common Items
					if (Tools.CalculationTools.ChanceSuccess(50))
					{
						var items = Drop.CommonItemIds;
						
						if (items.Count > 0)
						{
							var itemId = (uint)(items.Count == 1 ? items[0] :
							                    items[Drivers.Repositories.Safe.Random.Next(items.Count)]);
							
							var item = Collections.ItemCollection.CreateItemById(itemId);
							if (item != null)
							{
								PerformPlus(item);
								PerformSockets(item);
								PerformQuality(item);
								
								var location = map.GetValidItemCoordinate(x, y);
								
								item.Drop(map.Id, location.X, location.Y, false, dropClientId);
							}
						}
					}
					#endregion
					#region Uncommon Items
					if (Tools.CalculationTools.ChanceSuccess(10))
					{
						var items = Drop.UncommonItemIds;
						
						if (items.Count > 0)
						{
							var itemId = (uint)(items.Count == 1 ? items[0] :
							                    items[Drivers.Repositories.Safe.Random.Next(items.Count)]);
							
							var item = Collections.ItemCollection.CreateItemById(itemId);
							if (item != null)
							{
								PerformPlus(item);
								PerformSockets(item);
								PerformQuality(item);
								
								var location = map.GetValidItemCoordinate(x, y);
								
								item.Drop(map.Id, location.X, location.Y, false, dropClientId);
							}
						}
					}
					#endregion
					#region Rare Items
					if (Tools.CalculationTools.ChanceSuccess(2))
					{
						var items = Drop.RareItemIds;
						
						if (items.Count > 0)
						{
							var itemId = (uint)(items.Count == 1 ? items[0] :
							                    items[Drivers.Repositories.Safe.Random.Next(items.Count)]);
							
							var item = Collections.ItemCollection.CreateItemById(itemId);
							if (item != null)
							{
								PerformPlus(item);
								PerformSockets(item);
								PerformQuality(item);
								
								var location = map.GetValidItemCoordinate(x, y);
								
								item.Drop(map.Id, location.X, location.Y, false, dropClientId);
							}
						}
					}
					#endregion
					#region Super Rare Items
					if (Tools.CalculationTools.ChanceSuccess(1))
					{
						var items = Drop.SuperRareItemIds;
						
						if (items.Count > 0)
						{
							var itemId = (uint)(items.Count == 1 ? items[0] :
							                    items[Drivers.Repositories.Safe.Random.Next(items.Count)]);
							
							var item = Collections.ItemCollection.CreateItemById(itemId);
							if (item != null)
							{
								PerformPlus(item);
								PerformSockets(item);
								PerformQuality(item);
								
								var location = map.GetValidItemCoordinate(x, y);
								
								item.Drop(map.Id, location.X, location.Y, false, dropClientId);
							}
						}
					}
					#endregion
					
					#region Money
					if (Tools.CalculationTools.ChanceSuccessBig(Drop.MoneyChance))
					{
						DropMoney((uint)Drivers.Repositories.Safe.Random.Next(Drop.MinMoney, Drop.MaxMoney), map, x, y, dropClientId);
					}
					#endregion
					if (player != null)
					{
						#region CPs
						if (Tools.CalculationTools.ChanceSuccessBig(Drop.CPsChance))
						{
							player.CPs += (uint)Drivers.Repositories.Safe.Random.Next(Drop.MinCPs, Drop.MaxCPs);
						}
						#endregion
						#region BoundCPs
						if (Tools.CalculationTools.ChanceSuccessBig(Drop.BoundCPsChance))
						{
							player.BoundCPs += (uint)Drivers.Repositories.Safe.Random.Next(Drop.MinBoundCPs, Drop.MaxBoundCPs);
						}
						#endregion
					}
					
					#region Meteors
					if (Tools.CalculationTools.ChanceSuccessBig(Drop.MeteorChance))
					{
						var amount = Drivers.Repositories.Safe.Random.Next(Drop.MinMeteors, Drop.MaxMeteors + 1);
						uint itemId = 1088001;
						
						for (int i = 0; i < amount; i++)
						{
							var item = Collections.ItemCollection.CreateItemById(itemId);
							if (item != null)
							{
								var location = map.GetValidItemCoordinate(x, y);
								
								item.Drop(map.Id, location.X, location.Y, false, dropClientId);
							}
						}
					}
					#endregion
					#region Dragonballs
					if (Tools.CalculationTools.ChanceSuccessBig(Drop.DragonballChance))
					{
						var amount = Drivers.Repositories.Safe.Random.Next(Drop.MinDragonballs, Drop.MaxDragonballs + 1);
						uint itemId = 1088000;
						
						for (int i = 0; i < amount; i++)
						{
							var item = Collections.ItemCollection.CreateItemById(itemId);
							if (item != null)
							{
								var location = map.GetValidItemCoordinate(x, y);
								
								item.Drop(map.Id, location.X, location.Y, false, dropClientId);
							}
						}
					}
					#endregion
					
					#region Gems
					if (Tools.CalculationTools.ChanceSuccessBig(Drop.NormalGemDropChance))
					{
						var gem = (Enums.Gem)
							Drivers.Repositories.Safe.Random.NextEnum(typeof(Enums.Gem));
						
						switch (gem)
						{
							case Enums.Gem.NormalPhoenixGem:
							case Enums.Gem.RefinedPhoenixGem:
							case Enums.Gem.SuperPhoenixGem:
								gem = Enums.Gem.NormalPhoenixGem;
								break;
								
							case Enums.Gem.NormalDragonGem:
							case Enums.Gem.RefinedDragonGem:
							case Enums.Gem.SuperDragonGem:
								gem = Enums.Gem.NormalDragonGem;
								break;
								
							case Enums.Gem.NormalRainbowGem:
							case Enums.Gem.RefinedRainbowGem:
							case Enums.Gem.SuperRainbowGem:
								gem = Enums.Gem.NormalRainbowGem;
								break;
								
							case Enums.Gem.NormalVioletGem:
							case Enums.Gem.RefinedVioletGem:
							case Enums.Gem.SuperVioletGem:
								gem = Enums.Gem.NormalVioletGem;
								break;
								
							case Enums.Gem.NormalFuryGem:
							case Enums.Gem.RefinedFuryGem:
							case Enums.Gem.SuperFuryGem:
								gem = Enums.Gem.NormalFuryGem;
								break;
								
							case Enums.Gem.NormalKylinGem:
							case Enums.Gem.RefinedKylinGem:
							case Enums.Gem.SuperKylinGem:
								gem = Enums.Gem.NormalKylinGem;
								break;
								
							case Enums.Gem.NormalMoonGem:
							case Enums.Gem.RefinedMoonGem:
							case Enums.Gem.SuperMoonGem:
								gem = Enums.Gem.NormalMoonGem;
								break;
								
							default:
								return;
						}
						
						uint gemId = (uint)(((uint)gem) + 700000);
						
						if (Tools.CalculationTools.ChanceSuccessBig(Drop.RefinedGemDropChance))
						{
							gemId += 1;
							if (Tools.CalculationTools.ChanceSuccessBig(Drop.SuperGemDropChance))
							{
								gemId += 1;
							}
						}
						
						var item = Collections.ItemCollection.CreateItemById(gemId);
						if (item != null)
						{
							var location = map.GetValidItemCoordinate(x, y);
							
							item.Drop(map.Id, location.X, location.Y, false, dropClientId);
						}
					}
					#endregion
				}
			}
		}
		
		/// <summary>
		/// Drops money.
		/// </summary>
		/// <param name="amount">The amount of money to drop.</param>
		/// <param name="map">The map.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="dropClientId">The drop client id.</param>
		private void DropMoney(uint amount, Models.Maps.Map map, ushort x, ushort y, uint dropClientId)
		{
			uint itemId = 1090000;
			
			if (amount >= 10000)
			{
				itemId = 1091020;
			}
			else if (amount >= 5000)
			{
				itemId = 1091010;
			}
			else if (amount >= 1000)
			{
				itemId = 1091000;
			}
			else if (amount >= 100)
			{
				itemId = 1090020;
			}
			else if (amount >= 50)
			{
				itemId = 1090010;
			}
			
			var item = Collections.ItemCollection.CreateItemById(itemId);
			if (item != null)
			{
				var location = map.GetValidItemCoordinate(x, y);
				
				item.DropMoney = amount;
				item.Drop(map.Id, location.X, location.Y, false, dropClientId);
			}
		}
		
		/// <summary>
		/// Performs a plus drop on an item.
		/// </summary>
		/// <param name="item">The item.</param>
		private void PerformPlus(Models.Items.Item item)
		{
			if (!item.IsMisc && Tools.CalculationTools.ChanceSuccessBig(Drop.PlusChance))
			{
				if (item.DbOwnerItem == null)
				{
					item.DbOwnerItem = new Database.Models.DbOwnerItem
					{
						ItemId = (uint)item.DbItem.Id
					};
					item.DbOwnerItem.MaxDura = item.DbItem.Amount;
					item.DbOwnerItem.CurrentDura = item.DbOwnerItem.MaxDura;
					item.Gem1 = Enums.Gem.NoSocket;
					item.Gem2 = Enums.Gem.NoSocket;
				}
				
				item.DbOwnerItem.Plus = (byte)Drivers.Repositories.Safe.Random.Next(Drop.MinPlus, Drop.MaxPlus + 1);
			}
		}
		
		/// <summary>
		/// Performs a socket drop on an item.
		/// </summary>
		/// <param name="item">The item.</param>
		private void PerformSockets(Models.Items.Item item)
		{
			if (item.IsOneHand || item.IsTwoHand)
			{
				if (Drop.OneSocketWeapon && Tools.CalculationTools.ChanceSuccessBig(Drop.WeaponSocketChance))
				{
					if (item.DbOwnerItem == null)
					{
						item.DbOwnerItem = new Database.Models.DbOwnerItem
						{
							ItemId = (uint)item.DbItem.Id
						};
						item.DbOwnerItem.MaxDura = item.DbItem.Amount;
						item.DbOwnerItem.CurrentDura = item.DbOwnerItem.MaxDura;
						item.Gem1 = Enums.Gem.NoSocket;
						item.Gem2 = Enums.Gem.NoSocket;
					}
					
					item.Gem1 = Enums.Gem.EmptySocket;
					
					if (Drop.TwoSocketWeapon && Tools.CalculationTools.ChanceSuccessBig(Drop.WeaponSocketChance / 2))
					{
						item.Gem2 = Enums.Gem.EmptySocket;
					}
				}
			}
			else if (item.IsHead || item.IsNecklace || item.IsRing || item.IsArmor || item.IsBoots ||
			         item.IsFan || item.IsTower)
			{
				if (Drop.OneSocketArmory && Tools.CalculationTools.ChanceSuccessBig(Drop.ArmorySocketChance))
				{
					if (item.DbOwnerItem == null)
					{
						item.DbOwnerItem = new Database.Models.DbOwnerItem
						{
							ItemId = (uint)item.DbItem.Id
						};
						item.DbOwnerItem.MaxDura = item.DbItem.Amount;
						item.DbOwnerItem.CurrentDura = item.DbOwnerItem.MaxDura;
						item.Gem1 = Enums.Gem.NoSocket;
						item.Gem2 = Enums.Gem.NoSocket;
					}
					
					item.Gem1 = Enums.Gem.EmptySocket;
					
					if (Drop.TwoSocketArmory && Tools.CalculationTools.ChanceSuccessBig(Drop.ArmorySocketChance / 2))
					{
						item.Gem2 = Enums.Gem.EmptySocket;
					}
				}
			}
		}
		
		/// <summary>
		/// Performs a quality drop on an item.
		/// </summary>
		/// <param name="item">The item.</param>
		private void PerformQuality(Models.Items.Item item)
		{
			var quality = item.Quality;
			
			if (Tools.CalculationTools.ChanceSuccessBig(Drop.RefinedChance))
			{
				quality = Enums.ItemQuality.Refined;
			}
			
			if (Tools.CalculationTools.ChanceSuccessBig(Drop.UniqueChance))
			{
				quality = Enums.ItemQuality.Unique;
			}
			
			if (Tools.CalculationTools.ChanceSuccessBig(Drop.EliteChance))
			{
				quality = Enums.ItemQuality.Elite;
			}
			
			if (Tools.CalculationTools.ChanceSuccessBig(Drop.SuperChance))
			{
				quality = Enums.ItemQuality.Super;
			}
			
			if (quality != item.Quality)
			{
				item.SetQuality(null, quality);
			}
		}
	}
}
