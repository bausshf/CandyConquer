// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// Equipment collection for players.
	/// </summary>
	public sealed class Equipments
	{
		/// <summary>
		/// The equipments.
		/// </summary>
		private ConcurrentDictionary<Enums.ItemPosition, Models.Items.Item> _equipments;
		/// <summary>
		/// The masked equipments.
		/// </summary>
		private ConcurrentDictionary<Enums.ItemPosition, Models.Items.Item> _maskedEquipments;
		
		/// <summary>
		/// The player that owns the equipments.
		/// </summary>
		public Models.Entities.Player Player { get; private set; }
		
		/// <summary>
		/// Creates a new equipment collection.
		/// </summary>
		/// <param name="player">The player that owns the equipments.</param>
		public Equipments(Models.Entities.Player player)
		{
			_equipments = new ConcurrentDictionary<CandyConquer.WorldApi.Enums.ItemPosition, CandyConquer.WorldApi.Models.Items.Item>();
			_maskedEquipments = new ConcurrentDictionary<CandyConquer.WorldApi.Enums.ItemPosition, Models.Items.Item>();
			
			Player = player;
		}
		
		/// <summary>
		/// Attempts to get an item by a position.
		/// </summary>
		/// <param name="position">The position of the item.</param>
		/// <param name="item">The item.</param>
		/// <returns>True if the item was retrieved.</returns>
		/// <remarks>This does not check masked equipments.</remarks>
		public bool TryGetItem(Enums.ItemPosition position, out Models.Items.Item item)
		{
			return _equipments.TryGetValue(position, out item);
		}
		
		/// <summary>
		/// Gets the total number of pluses in the equipment collection.
		/// </summary>
		public ushort NumberOfPlus
		{
			get
			{
				ushort plus = 0;
				
				foreach (var item in _equipments.Values)
				{
					plus += item.DbOwnerItem.Plus;
				}
				
				return plus;
			}
		}
		
		/// <summary>
		/// Equips an item.
		/// </summary>
		/// <param name="item">The item to equip.</param>
		/// <param name="position">The position to equip it at.</param>
		/// <param name="unequip">Set to true if existing equipment should be unequipped.</param>
		/// <param name="forceEquip">Set to true if the equipping should be forced.</param>
		/// <param name="init">Set to true if the equipping is during player initialization.</param>
		/// <returns>True if the equipment was a success, false otherwise.</returns>
		public bool Equip(Models.Items.Item item, Enums.ItemPosition position, bool unequip, bool forceEquip = false, bool init = false)
		{
			if (!Player.Alive)
			{
				return false;
			}
			
			if (Player.Battle != null)
			{
				return false;
			}
			
			if (!init)
			{
				#region Player Validation
				if (!forceEquip)
				{
					#region Gender Check
					if (item.IsFemale && !Player.IsFemale)
					{
						Player.SendSystemMessage("ITEM_FEMALE_ONLY");
						return false;
					}
					#endregion
					
					#region Job Check
					if (item.DbItem.Job > 0 &&
					    Tools.JobTools.GetBaseJob((Enums.Job)item.DbItem.Job) != Tools.JobTools.GetBaseJob(Player.Job))
					{
						if (Player.Reborns == 0 || item.DbItem.Level > 70)
						{
							Player.SendSystemMessage("ITEM_INVALID_JOB");
							return false;
						}
					}
					#endregion
					
					#region Level Check
					if (Player.Level < item.DbItem.Level)
					{
						Player.SendSystemMessage("ITEM_INVALID_LEVEL");
						return false;
					}
					#endregion
					
					#region Stats Check
					if (Player.Strength < item.DbItem.Strength ||
					    Player.Agility < item.DbItem.Agility ||
					    Player.Vitality < item.DbItem.Vitality ||
					    Player.Spirit < item.DbItem.Spirit)
					{
						Player.SendSystemMessage("ITEM_STATS_LOW");
						return false;
					}
					#endregion
					
					#region Prof Check
					if (!Player.Spells.ContainsProficiency(item.DbItem.BaseId))
					{
						var prof = Player.Spells.GetOrCreateProficiency(item.DbItem.BaseId);
						if (prof.Level < item.DbItem.WeaponSkill)
						{
							if (Player.Reborns == 0 || item.DbItem.Level > 70)
							{
								Player.SendSystemMessage("ITEM_INVALID_PROF");
								return false;
							}
						}
					}
					#endregion
				}
				#endregion
				
				#region Position Validation
				if (!forceEquip)
				{
					if (_maskedEquipments.ContainsKey(position))
					{
						SendInvalidEquipMessage(1, item, position);
						return false;
					}
					
					switch (position)
					{
							#region Head
						case Enums.ItemPosition.Head:
							{
								if (!item.IsHead)
								{
									SendInvalidEquipMessage(2, item, position);
									return false;
								}
								break;
							}
							#endregion
							#region Necklace
						case Enums.ItemPosition.Necklace:
							{
								if (!item.IsNecklace)
								{
									SendInvalidEquipMessage(3, item, position);
									return false;
								}
								break;
							}
							#endregion
							#region Ring
						case Enums.ItemPosition.Ring:
							{
								if (!item.IsRing)
								{
									SendInvalidEquipMessage(4, item, position);
									return false;
								}
								break;
							}
							#endregion
							#region Armor
						case Enums.ItemPosition.Armor:
							{
								if (!item.IsArmor)
								{
									SendInvalidEquipMessage(5, item, position);
									return false;
								}
								break;
							}
							#endregion
							#region Boots
						case Enums.ItemPosition.Boots:
							{
								if (!item.IsBoots)
								{
									SendInvalidEquipMessage(6, item, position);
									return false;
								}
								break;
							}
							#endregion
							#region Bottle
						case Enums.ItemPosition.Bottle:
							{
								if (!item.IsBottle)
								{
									SendInvalidEquipMessage(7, item, position);
									return false;
								}
								break;
							}
							#endregion
							#region Garment
						case Enums.ItemPosition.Garment:
							{
								if (!item.IsGarment)
								{
									SendInvalidEquipMessage(8, item, position);
									return false;
								}
								break;
							}
							#endregion
							#region Steed
						case Enums.ItemPosition.Steed:
							{
								if (!Data.Constants.GameMode.AllowSteed)
								{
									SendInvalidEquipMessage(9, item, position);
									return false;
								}
								
								if (Player.ContainsStatusFlag(Enums.StatusFlag.Riding))
								{
									SendInvalidEquipMessage(10, item, position);
									return false;
								}
								
								if (!item.IsSteed)
								{
									SendInvalidEquipMessage(11, item, position);
									return false;
								}
								break;
							}
							#endregion
							#region SteedArmor
						case Enums.ItemPosition.SteedArmor:
							{
								if (!Data.Constants.GameMode.AllowSteed)
								{
									SendInvalidEquipMessage(12, item, position);
									return false;
								}
								
								if (Player.ContainsStatusFlag(Enums.StatusFlag.Riding))
								{
									SendInvalidEquipMessage(13, item, position);
									return false;
								}
								
								if (!item.IsMountArmor)
								{
									SendInvalidEquipMessage(14, item, position);
									return false;
								}
								break;
							}
							#endregion
							#region Fan
						case Enums.ItemPosition.Fan:
							{
								if (!Data.Constants.GameMode.AllowFan)
								{
									SendInvalidEquipMessage(15, item, position);
									return false;
								}
								
								if (!item.IsFan)
								{
									SendInvalidEquipMessage(16, item, position);
									return false;
								}
								break;
							}
							#endregion
							#region Tower
						case Enums.ItemPosition.Tower:
							{
								if (!Data.Constants.GameMode.AllowTower)
								{
									SendInvalidEquipMessage(17, item, position);
									return false;
								}
								
								if (!item.IsTower)
								{
									SendInvalidEquipMessage(18, item, position);
									return false;
								}
								break;
							}
							#endregion
							#region Right
						case Enums.ItemPosition.WeaponR:
							{
								if (!item.IsOneHand && !item.IsTwoHand)
								{
									SendInvalidEquipMessage(19, item, position);
									return false;
								}
								
								if (_equipments.ContainsKey(Enums.ItemPosition.WeaponL) &&
								    item.IsTwoHand)
								{
									SendInvalidEquipMessage(20, item, position);
									return false;
								}
								break;
							}
							#endregion
							#region Left
						case Enums.ItemPosition.WeaponL:
							{
								if (!item.IsOneHand && !item.IsShield && !item.IsArrow)
								{
									SendInvalidEquipMessage(21, item, position);
									return false;
								}
								
								Models.Items.Item rightItem;
								if (!TryGetItem(Enums.ItemPosition.WeaponR, out rightItem))
								{
									SendInvalidEquipMessage(22, item, position);
									return false;
								}
								
								if (item.IsArrow && !rightItem.IsBow)
								{
									SendInvalidEquipMessage(23, item, position);
									return false;
								}
								
								if (item.IsShield && !rightItem.IsOneHand)
								{
									SendInvalidEquipMessage(24, item, position);
									return false;
								}
								break;
							}
							#endregion
							
							#region default
						default:
							{
								SendInvalidEquipMessage(-1, item, position);
								return false;
							}
							#endregion
					}
				}
				#endregion
				
				if (!forceEquip)
				{
					if (!Player.Inventory.Remove(item.ClientId))
					{
						return false;
					}
					
					if (unequip && _equipments.ContainsKey(position))
					{
						if (!Unequip(position))
						{
							return false;
						}
					}
				}
			}
			
			if (forceEquip && !init || _equipments.TryAdd(position, item))
			{
				item.Position = position;
				
				if (!forceEquip && !init)
				{
					if (!item.DbOwnerItem.Create(Database.Models.DbOwnerItem.Equipments))
					{
						return false;
					}
				}
				
				if (Player.LoggedIn)
				{
					item.UpdateClient(Player, Enums.ItemUpdateAction.Add);
					Player.ClientSocket.Send(new Models.Packets.Items.ItemActionPacket
					                         {
					                         	Player = this.Player,
					                         	ClientId = item.ClientId,
					                         	Action = Enums.ItemAction.Equip,
					                         	Data1Low = (ushort)position
					                         });
					SendGears();
				}
				
				Player.UpdateBaseStats();
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>
		/// Sends a message to the player that the equipment cannot be done due to an error.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		/// <param name="item">The item that was attempted to be equipped.</param>
		/// <param name="position">The position it was equipped at.</param>
		private void SendInvalidEquipMessage(int errorCode, Models.Items.Item item, Enums.ItemPosition position)
		{
			Player.SendFormattedSystemMessage("ITEM_EQUIP_INVALID", false,
			                                  errorCode, item.DbItem, position.ToString());
		}
		
		/// <summary>
		/// Unequips an item.
		/// </summary>
		/// <param name="position">The position the item is at.</param>
		/// <returns>True if the unequip was done successfully, false otherwise.</returns>
		public bool Unequip(Enums.ItemPosition position)
		{
			if (Player.ContainsStatusFlag(Enums.StatusFlag.Riding))
			{
				return false;
			}
			
			if (_maskedEquipments.ContainsKey(position))
			{
				return false;
			}
			
			if (Player.Inventory.Count >= 40)
			{
				return false;
			}
			
			if (position == Enums.ItemPosition.WeaponR && _equipments.ContainsKey(Enums.ItemPosition.WeaponL))
			{
				return false;
			}
			
			Models.Items.Item item;
			var success = _equipments.TryRemove(position, out item) &&
				item.DbOwnerItem.Delete(Database.Models.DbOwnerItem.Equipments);
			
			if (success)
			{
				Player.ClientSocket.Send(new Models.Packets.Items.ItemActionPacket
				                         {
				                         	Player = this.Player,
				                         	ClientId = item.ClientId,
				                         	Action = Enums.ItemAction.Unequip,
				                         	Data1Low = (ushort)item.Position
				                         });
				
				Player.UpdateBaseStats();
				
				item.DbOwnerItem.Id = 0;
				Player.Inventory.Add(item);
				SendGears();
			}
			
			return success;
		}
		
		/// <summary>
		/// Masks an equipment with another item.
		/// </summary>
		/// <param name="position">The position of the item.</param>
		/// <param name="itemId">The item id.</param>
		/// <returns>True if the mask was done successfully.</returns>
		public bool Mask(Enums.ItemPosition position, uint itemId)
		{
			var tempItem = Collections.ItemCollection.CreateItemById(itemId);
			if (tempItem == null)
			{
				return false;
			}
			
			tempItem.DbOwnerItem = new CandyConquer.Database.Models.DbOwnerItem();
			
			var success = _maskedEquipments.TryAdd(position, tempItem);
			
			if (success)
			{
				tempItem.Position = position;
				tempItem.UpdateClient(Player, Enums.ItemUpdateAction.Add);
				
				Player.ClientSocket.Send(new Models.Packets.Items.ItemActionPacket
				                         {
				                         	Player = this.Player,
				                         	ClientId = (uint)tempItem.ClientId,
				                         	Action = Enums.ItemAction.Equip,
				                         	Data1Low = (ushort)tempItem.Position
				                         });
				SendGears();
			}
			
			return success;
		}
		
		/// <summary>
		/// Unmasks an equipment.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <returns>True if the unmask was done successfully.</returns>
		public bool Unmask(Enums.ItemPosition position)
		{
			Models.Items.Item removedItem;
			var success = _maskedEquipments.TryRemove(position, out removedItem);
			
			Models.Items.Item equipment;
			if (success)
			{
				Player.ClientSocket.Send(new Models.Packets.Items.ItemActionPacket
				                         {
				                         	Player = this.Player,
				                         	ClientId = removedItem.ClientId,
				                         	Action = Enums.ItemAction.Unequip,
				                         	Data1Low = (ushort)position
				                         });
				var itempacket = new Models.Packets.Items.ItemActionPacket();
				itempacket.Action = Enums.ItemAction.Remove;
				itempacket.ClientId = removedItem.ClientId;
				
				Player.ClientSocket.Send(itempacket);
				
				if (_equipments.TryGetValue(position, out equipment))
				{
					Equip(equipment, position, false, true);
				}
			}
			
			return success;
		}
		
		/// <summary>
		/// Unmasks all masked equipments.
		/// </summary>
		public void UnmaskAll()
		{
			foreach (var maskedEquip in _maskedEquipments.Values)
			{
				Player.ClientSocket.Send(new Models.Packets.Items.ItemActionPacket
				                         {
				                         	Player = this.Player,
				                         	ClientId = maskedEquip.ClientId,
				                         	Action = Enums.ItemAction.Unequip,
				                         	Data1Low = (ushort)maskedEquip.Position
				                         });
				var itempacket = new Models.Packets.Items.ItemActionPacket();
				itempacket.Action = Enums.ItemAction.Remove;
				itempacket.ClientId = maskedEquip.ClientId;
				
				Player.ClientSocket.Send(itempacket);
			}
			
			_maskedEquipments.Clear();
			foreach (var equip in _equipments.Values.ToArray())
			{
				Equip(equip, equip.Position, false, true);
			}
		}
		
		/// <summary>
		/// Forces equipments from this player to another.
		/// This should generally only be used by AI bots.
		/// </summary>
		/// <param name="player">The player to force the equipments upon.</param>
		public void ForceEquipments(Models.Entities.Player player)
		{
			player.Equipments._equipments = _equipments;
			SendGears();
			player.Equipments.SendGears();
		}
		
		/// <summary>
		/// Sends the gears to the client.
		/// </summary>
		public void SendGears()
		{
			Player.UpdateScreen(true);
			Player.ClientSocket.Send(new Models.Packets.Items.ItemActionPacket
			                         {
			                         	ClientId = Player.ClientId,
			                         	Action = Enums.ItemAction.DisplayGears,
			                         	Data1 = 255,
			                         	Player = this.Player
			                         });
		}
		
		/// <summary>
		/// Gets a collection of all the items in the equipments.
		/// Use this to iterate over the equipments.
		/// </summary>
		/// <returns>A collection of the equipments.</returns>
		public ICollection<Models.Items.Item> GetAll()
		{
			return _equipments.Values;
		}
		
		/// <summary>
		/// Gets a specific equipment. If an equipment is masked, then the masked equipment is returned.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="checkMasked">Set to true if masked equipments should be checked.</param>
		/// <returns>The item if found, null otherwise.</returns>
		public Models.Items.Item Get(Enums.ItemPosition position, bool checkMasked = true)
		{
			Models.Items.Item item = null;
			checkMasked = checkMasked && _maskedEquipments.Count > 0
				&& _maskedEquipments.TryGetValue(position, out item);

			if (!checkMasked)
			{
				_equipments.TryGetValue(position, out item);
			}
			
			return item;
		}
	}
}
