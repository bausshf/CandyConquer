// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A warehouse collection.
	/// </summary>
	public sealed class Warehouse
	{
		/// <summary>
		/// The collection of items in the warehouse.
		/// </summary>
		private ConcurrentDictionary<uint, Models.Items.Item> _items;
		/// <summary>
		/// The player that owns the warehouse.
		/// </summary>
		public Models.Entities.Player Player { get; private set; }
		
		/// <summary>
		/// Gets the id of the warehouse.
		/// </summary>
		public uint Id { get; private set; }
		
		/// <summary>
		/// Gets or sets amount of money in the warehouse.
		/// </summary>
		public uint Money
		{
			get { return Player.WarehouseMoney; }
			set
			{
				Player.WarehouseMoney = value;
			}
		}
		
		/// <summary>
		/// The amount of items in the warehouse.
		/// </summary>
		public int Count
		{
			get { return _items.Count; }
		}
		
		/// <summary>
		/// Creates a new warehouse.
		/// </summary>
		/// <param name="player">The player associated with the warehouse.</param>
		/// <param name="id">The id of the warehouse.</param>
		public Warehouse(uint id, Models.Entities.Player player)
		{
			_items = new ConcurrentDictionary<uint, Models.Items.Item>();
			Player = player;
			Id = id;
		}
		
		/// <summary>
		/// Adds an item to the warehouse.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <returns>True if the item was added, false otherwise.</returns>
		public bool Add(Models.Items.Item item, bool silentAdd = false)
		{
			if (_items.Count >= 20)
			{
				Player.SendSystemMessage("WAREHOUSE_FULL");
				return false;
			}
			
			if (_items.TryAdd(item.ClientId, item))
			{
				if (item.DbOwnerItem == null)
				{
					item.DbOwnerItem = new Database.Models.DbOwnerItem
					{
						PlayerId = Player.DbPlayer.Id,
						ItemId = (uint)item.DbItem.Id,
						LocationId = Id
					};
					item.DbOwnerItem.MaxDura = item.DbItem.Amount;
					item.DbOwnerItem.CurrentDura = item.DbOwnerItem.MaxDura;
					item.Gem1 = Enums.Gem.NoSocket;
					item.Gem2 = Enums.Gem.NoSocket;
					item.DbOwnerItem.OwnedBy.AddItem(Player.Name);
					
					if (!item.DbOwnerItem.Create(Database.Models.DbOwnerItem.Warehouse))
					{
						return false;
					}
				}
				else if (item.DbOwnerItem.Id == 0)
				{
					item.DbOwnerItem.PlayerId = Player.DbPlayer.Id;
					item.DbOwnerItem.LocationId = Id;
					item.DbOwnerItem.OwnedBy.AddItem(Player.Name);
					
					if (!item.DbOwnerItem.Create(Database.Models.DbOwnerItem.Warehouse))
					{
						return false;
					}
				}
				else
				{
					item.DbOwnerItem.PlayerId = Player.DbPlayer.Id;
					item.DbOwnerItem.LocationId = Id;
					item.DbOwnerItem.OwnedBy.AddItem(Player.Name);
					
					if (!item.DbOwnerItem.Update(Database.Models.DbOwnerItem.Warehouse))
					{
						return false;
					}
				}
				
				if (!silentAdd)
				{
					Player.ClientSocket.Send(new Models.Packets.Items.WarehousePacket
					                         {
					                         	WarehouseId = Id,
					                         	Item = item
					                         });
				}
				return true;
			}
			
			return false;
		}
		
		/// <summary>
		/// Removes an item from the warehouse.
		/// </summary>
		/// <param name="id">The client id.</param>
		/// <returns>True if the item was removed, false otherwise.</returns>
		public bool Remove(uint id)
		{
			Models.Items.Item ritem;
			return Remove(id, out ritem);
		}
		
		/// <summary>
		/// Removes an item from the warehouse and gives the removed item.
		/// </summary>
		/// <param name="id">The client id.</param>
		/// <param name="removedItem">The removed item.</param>
		/// <returns>True if the item was removed, false otherwise.</returns>
		private bool Remove(uint id, out Models.Items.Item removedItem)
		{
			if (_items.TryRemove(id, out removedItem))
			{
				if (removedItem.DbOwnerItem.Delete(Database.Models.DbOwnerItem.Warehouse))
				{
					removedItem.DbOwnerItem.Id = 0;
					removedItem.DbOwnerItem.LocationId = null;
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
		private bool RemoveById(uint itemId)
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
		/// Pops an item from the warehouse.
		/// </summary>
		/// <param name="id">The client id of the item to pop.</param>
		/// <returns>The popped item.</returns>
		public Models.Items.Item Pop(uint id)
		{
			Models.Items.Item removedItem;
			if (_items.TryRemove(id, out removedItem))
			{
				if (removedItem.DbOwnerItem.Delete(Database.Models.DbOwnerItem.Warehouse))
				{
					removedItem.DbOwnerItem.Id = 0;
					removedItem.DbOwnerItem.LocationId = 0;
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
		/// Attempts to find a specific item by a predicate.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <returns>The item if found, null otherwise.</returns>
		public Models.Items.Item Find(Func<Models.Items.Item,bool> predicate)
		{
			return _items.Values.Where(predicate).FirstOrDefault();
		}
		
		/// <summary>
		/// Checks if the warehouse contains an item.
		/// </summary>
		/// <param name="id">The client id.</param>
		/// <returns>True if the item is within the warehouse, false otherwise.</returns>
		public bool Contains(uint id)
		{
			return _items.ContainsKey(id);
		}
		
		/// <summary>
		/// Checks if the warehouse contains an item by an item id.
		/// </summary>
		/// <param name="itemId">The item id.</param>
		/// <returns>True if there's at least one item with the id in the warehouse, false otherwise.</returns>
		public bool ContainsById(uint itemId)
		{
			return Find(item => item.DbItem.Id == itemId) != null;
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
		
		/// <summary>
		/// Updates the client with the warehouse.
		/// </summary>
		public void UpdateClient()
		{
			Money = Money; // updates client with warehouse money ...
			
			foreach (var item in _items.Values)
			{
				Player.ClientSocket.Send(new Models.Packets.Items.WarehousePacket
				                         {
				                         	WarehouseId = Id,
				                         	Item = item
				                         });
			}
		}
	}
}
