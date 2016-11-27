// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CandyConquer.WorldApi.Models.Items;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// An inventory collection.
	/// </summary>
	public sealed class Inventory
	{
		/// <summary>
		/// The collection of items in the inventory.
		/// </summary>
		private ConcurrentDictionary<uint, Models.Items.Item> _items;
		/// <summary>
		/// The player that owns the inventory.
		/// </summary>
		public Models.Entities.Player Player { get; private set; }
		
		/// <summary>
		/// The amount of items in the inventory.
		/// </summary>
		public int Count
		{
			get { return _items.Count; }
		}
		
		/// <summary>
		/// Creates a new inventory.
		/// </summary>
		/// <param name="player">The player associated with the inventory.</param>
		public Inventory(Models.Entities.Player player)
		{
			_items = new ConcurrentDictionary<uint, Models.Items.Item>();
			Player = player;
		}
		
		/// <summary>
		/// Adds an item to the inventory.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <returns>True if the item was added, false otherwise.</returns>
		public bool Add(Models.Items.Item item)
		{
			if (_items.Count >= 40)
			{
				Player.SendSystemMessage("INVENTORY_FULL");
				return false;
			}
			
			if (_items.TryAdd(item.ClientId, item))
			{
				if (item.DbOwnerItem == null)
				{
					item.DbOwnerItem = new Database.Models.DbOwnerItem
					{
						PlayerId = Player.DbPlayer.Id,
						ItemId = (uint)item.DbItem.Id
					};
					item.DbOwnerItem.MaxDura = item.DbItem.Amount;
					item.DbOwnerItem.CurrentDura = item.DbOwnerItem.MaxDura;
					item.Gem1 = Enums.Gem.NoSocket;
					item.Gem2 = Enums.Gem.NoSocket;
					item.DbOwnerItem.OwnedBy.AddItem(Player.Name);
					
					if (!item.DbOwnerItem.Create(Database.Models.DbOwnerItem.Inventories))
					{
						return false;
					}
				}
				else if (item.DbOwnerItem.Id == 0)
				{
					item.DbOwnerItem.PlayerId = Player.DbPlayer.Id;
					item.DbOwnerItem.OwnedBy.AddItem(Player.Name);
					
					if (!item.DbOwnerItem.Create(Database.Models.DbOwnerItem.Inventories))
					{
						return false;
					}
				}
				else
				{
					item.DbOwnerItem.PlayerId = Player.DbPlayer.Id;
					item.DbOwnerItem.OwnedBy.AddItem(Player.Name);
					
					if (!item.DbOwnerItem.Update(Database.Models.DbOwnerItem.Inventories))
					{
						return false;
					}
				}
				
				item.Position = Enums.ItemPosition.Inventory;
				item.UpdateClient(Player, Enums.ItemUpdateAction.Add);
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>
		/// Adds an item to the inventory.
		/// </summary>
		/// <param name="itemId">The item id.</param>
		/// <param name="plus">The plus.</param>
		/// <param name="bless">The bless.</param>
		/// <param name="enchant">The enchantment.</param>
		/// <param name="gem1">The first socket.</param>
		/// <param name="gem2">The second socket.</param>
		/// <returns>True if the item was added, false otherwise.</returns>
		public bool Add(uint itemId, byte plus = 0, byte bless = 0, byte enchant = 0, Enums.Gem gem1 = Enums.Gem.NoSocket, Enums.Gem gem2 = Enums.Gem.NoSocket)
		{
			if (_items.Count >= 40)
			{
				Player.SendSystemMessage("INVENTORY_FULL");
				return false;
			}
			
			var item = Collections.ItemCollection.CreateItemById(itemId);
			if (item == null)
			{
				return false;
			}
			
			item.DbOwnerItem = new Database.Models.DbOwnerItem
			{
				PlayerId = Player.DbPlayer.Id,
				ItemId = itemId,
				Plus = plus,
				Bless = bless,
				Enchant = enchant,
			};
			item.DbOwnerItem.MaxDura = item.DbItem.Amount;
			item.DbOwnerItem.CurrentDura = item.DbOwnerItem.MaxDura;
			item.DbOwnerItem.OwnedBy.AddItem(Player.Name);
			item.Gem1 = gem1;
			item.Gem2 = gem2;
			
			if (item.DbOwnerItem.Create(Database.Models.DbOwnerItem.Inventories))
			{
				if (_items.TryAdd(item.ClientId, item))
				{
					item.Position = Enums.ItemPosition.Inventory;
					item.UpdateClient(Player, Enums.ItemUpdateAction.Add);
					
					return true;
				}
				else
				{
					item.DbOwnerItem.Delete(Database.Models.DbOwnerItem.Inventories);
					return true;
				}
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>
		/// Removes an item from the inventory.
		/// </summary>
		/// <param name="id">The client id.</param>
		/// <returns>True if the item was removed, false otherwise.</returns>
		public bool Remove(uint id)
		{
			Models.Items.Item ritem;
			return Remove(id, out ritem);
		}
		
		/// <summary>
		/// Removes an item from the inventory and gives the removed item.
		/// </summary>
		/// <param name="id">The client id.</param>
		/// <param name="removedItem">The removed item.</param>
		/// <returns>True if the item was removed, false otherwise.</returns>
		public bool Remove(uint id, out Models.Items.Item removedItem)
		{
			if (_items.TryRemove(id, out removedItem))
			{
				if (removedItem.DbOwnerItem.Delete(Database.Models.DbOwnerItem.Inventories))
				{
					removedItem.DbOwnerItem.Id = 0;
					
					var itempacket = new Models.Packets.Items.ItemActionPacket();
					itempacket.Action = Enums.ItemAction.Remove;
					itempacket.ClientId = id;
					
					Player.ClientSocket.Send(itempacket);
					return true;
				}
			}
			return false;
		}
		
		/// <summary>
		/// Removes an item by its item id.
		/// </summary>
		/// <param name="itemId">The item id.</param>
		/// <returns>True if the item was removed, false otherwise.</returns>
		public bool RemoveById(uint itemId)
		{
			foreach (var item in _items.Values)
			{
				if (item.DbItem.Id == itemId && Remove(item.ClientId))
				{
					return true;
				}
			}
			
			return false;
		}
		
		/// <summary>
		/// Removes a specific amount of items by their item id.
		/// </summary>
		/// <param name="itemId">The item id.</param>
		/// <param name="count">The amount to remove.</param>
		/// <returns>True if all items were removed, false otherwise.</returns>
		public bool RemoveByCount(uint itemId, int count = 1)
		{
			int i = 0;
			int failed = 0;
			foreach (var item in _items.Values)
			{
				if (i < count && item.DbItem.Id == itemId)
				{
					if (Remove(item.ClientId))
					{
						i++;
					}
					else
					{
						failed++;
					}
				}
			}
			
			return failed == 0;
		}
		
		/// <summary>
		/// Pops an item from the inventory.
		/// </summary>
		/// <param name="id">The client id of the item to pop.</param>
		/// <returns>The popped item.</returns>
		public Models.Items.Item Pop(uint id)
		{
			Models.Items.Item removedItem;
			if (_items.TryRemove(id, out removedItem))
			{
				if (removedItem.DbOwnerItem.Delete(Database.Models.DbOwnerItem.Inventories))
				{
					removedItem.DbOwnerItem.Id = 0;
					
					var itempacket = new Models.Packets.Items.ItemActionPacket();
					itempacket.Action = Enums.ItemAction.Remove;
					itempacket.ClientId = id;
					Player.ClientSocket.Send(itempacket);
				}
			}
			
			return removedItem;
		}
		
		/// <summary>
		/// Pops an item by its item id.
		/// </summary>
		/// <param name="itemId">The item id.</param>
		/// <returns>The item popped.</returns>
		public Models.Items.Item PopById(uint itemId)
		{
			foreach (var item in _items.Values)
			{
				if (item.DbItem.Id == itemId)
				{
					return Pop(item.ClientId);
				}
			}
			
			return null;
		}
		
		/// <summary>
		/// Pops an amount of items by their item id.
		/// </summary>
		/// <param name="itemId">The item id.</param>
		/// <param name="count">The amount of items to pop.</param>
		/// <returns>A list of the items popped.</returns>
		public List<Models.Items.Item> PopByCount(uint itemId, int count = 1)
		{
			var removedItems = new List<Models.Items.Item>();
			foreach (var item in _items.Values)
			{
				if (removedItems.Count < count && item.DbItem.Id == itemId)
				{
					var poppedItem = Pop(item.ClientId);
					if (poppedItem != null)
					{
						removedItems.Add(poppedItem);
					}
				}
			}
			
			return removedItems;
		}
		
		/// <summary>
		/// Attempts to find a specific item by a predicate.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <returns>The item if found, null otherwise.</returns>
		public Models.Items.Item Find(Func<Models.Items.Item,bool> predicate)
		{
			return _items.Values.Where(predicate).FirstOrDefault();
		}
		
		/// <summary>
		/// Checks if the inventory contains an item.
		/// </summary>
		/// <param name="id">The client id.</param>
		/// <returns>True if the item is within the inventory, false otherwise.</returns>
		public bool Contains(uint id)
		{
			return _items.ContainsKey(id);
		}
		
		/// <summary>
		/// Checks if the inventory contains an item by an item id.
		/// </summary>
		/// <param name="itemId">The item id.</param>
		/// <returns>True if there's at least one item with the id in the inventory, false otherwise.</returns>
		public bool ContainsById(uint itemId)
		{
			return Find(item => item.DbItem.Id == itemId) != null;
		}
		
		/// <summary>
		/// Checks if the inventory contains an item by an item id.
		/// </summary>
		/// <param name="itemId">The item id.</param>
		/// <returns>True if there's at least one item with the id in the inventory, false otherwise.</returns>
		public bool ContainsById(uint itemId, out int itemCount)
		{
			itemCount = _items.Values.Count(Item => Item.DbItem.Id == itemId);
			return itemCount > 0;
		}
		
		/// <summary>
		/// Attempts to get an item by its client id.
		/// </summary>
		/// <param name="id">The client id.</param>
		/// <param name="item">The item.</param>
		/// <returns>True if the item was retrieved, false otherwise.</returns>
		public bool TryGetItem(uint id, out Models.Items.Item item)
		{
			return _items.TryGetValue(id, out item);
		}
	}
}
