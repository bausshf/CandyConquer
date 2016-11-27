// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CandyConquer.Drivers.Repositories.Collections;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// An collection of items.
	/// </summary>
	public static class ItemCollection
	{
		/// <summary>
		/// The item collection.
		/// </summary>
		private static ConcurrentDictionary<uint, Models.Items.Item> _items;
		
		/// <summary>
		/// The item additions.
		/// </summary>
		private static MultiConcurrentDictionary<uint, byte, Models.Items.ItemAddition> _itemAdditions;
		
		/// <summary>
		/// Static constructor for ItemCollection.
		/// </summary>
		static ItemCollection()
		{
			_items = new ConcurrentDictionary<uint, CandyConquer.WorldApi.Models.Items.Item>();
			_itemAdditions = new MultiConcurrentDictionary<uint, byte, CandyConquer.WorldApi.Models.Items.ItemAddition>();
		}
		
		/// <summary>
		/// Loads the collection.
		/// </summary>
		public static void Load()
		{
			var items = Database.Dal.Items.GetAll();
			
			foreach (var dbItem in items)
			{
				var item = new Models.Items.Item(dbItem);
				_items.TryAdd((uint)item.DbItem.Id, item);
			}
		}
		
		/// <summary>
		/// Loads item additions.
		/// </summary>
		public static void LoadAdditions()
		{
			foreach (var line in System.IO.File.ReadAllLines(Drivers.Settings.DatabaseSettings.WorldFlatDatabase + "\\Items\\ItemAdd.dat"))
			{
				if (string.IsNullOrWhiteSpace(line))
					continue;
				
				var data = line.Split(' ');
				var addition = new Models.Items.ItemAddition
				{
					ItemId = uint.Parse(data[0]),
					Plus = byte.Parse(data[1]),
					HP = ushort.Parse(data[2]),
					MinAttack = uint.Parse(data[3]),
					MaxAttack = uint.Parse(data[4]),
					Defense = ushort.Parse(data[5]),
					MagicAttack = ushort.Parse(data[6]),
					MagicDefense = ushort.Parse(data[7]),
					Dexterity = ushort.Parse(data[8]),
					Dodge = byte.Parse(data[9])
				};
				
				if (!_itemAdditions.ContainsKey(addition.ItemId))
				{
					if (!_itemAdditions.TryAdd(addition.ItemId))
					{
						throw new TypeLoadException("Failed to load item additions.");
					}
				}
				else
				{
					if (!_itemAdditions[addition.ItemId].TryAdd(addition.Plus, addition))
						throw new TypeLoadException("Failed to load item additions.");
				}
			}
		}
		
		/// <summary>
		/// Creates an item by an item id.
		/// </summary>
		/// <param name="itemId">The item id.</param>
		/// <returns>The item created.</returns>
		public static Models.Items.Item CreateItemById(uint itemId)
		{
			Models.Items.Item item;
			if (_items.TryGetValue(itemId, out item))
			{
				item = item.Copy();
			}
			return item;
		}
		
		/// <summary>
		/// Creates an item by a name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>The item created.</returns>
		public static Models.Items.Item CreateItemByName(string name)
		{
			var item = _items.Values.Where(x => x.DbItem.Name == name).FirstOrDefault();
			if (item != null)
			{
				item = item.Copy();
			}
			return item;
		}
		
		/// <summary>
		/// Gets an item addition.
		/// </summary>
		/// <param name="itemId">The item id.</param>
		/// <param name="plus">The plus.</param>
		/// <returns>The item addition if found, null otherwise.</returns>
		public static Models.Items.ItemAddition GetItemAddition(uint itemId, byte plus)
		{
			if (!_itemAdditions.ContainsKey(itemId))
			{
				return null;
			}
			
			if (!_itemAdditions[itemId].ContainsKey(plus))
			{
				return null;
			}
			
			return _itemAdditions[itemId][plus];
		}
		
		/// <summary>
		/// Checks whether the collection contains an item.
		/// </summary>
		/// <param name="itemId">The item id.</param>
		/// <returns>True if the item exists.</returns>
		public static bool Contains(uint itemId)
		{
			return _items.ContainsKey(itemId);
		}
	}
}
