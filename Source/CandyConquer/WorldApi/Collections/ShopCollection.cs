// Project by Bauss
using System;
using System.IO;
using System.Collections.Concurrent;
using System.Linq;
using CandyConquer.Drivers.Repositories.IO;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A collection of shops.
	/// </summary>
	public static class ShopCollection
	{
		/// <summary>
		/// The shop collection.
		/// </summary>
		private static ConcurrentDictionary<uint, Models.Misc.Shop> _shops;
		
		/// <summary>
		/// Static constructor for ShopCollection.
		/// </summary>
		static ShopCollection()
		{
			_shops = new ConcurrentDictionary<uint, CandyConquer.WorldApi.Models.Misc.Shop>();
		}
		
		/// <summary>
		/// Loads all shops.
		/// </summary>
		public static void LoadShops()
		{
			var ini = new IniFile(Drivers.Settings.DatabaseSettings.WorldFlatDatabase + "\\Shops\\Shops.ini");
			if (ini.Exists())
			{
				ini.Open();
				
				var count = Convert.ToInt32(ini.GetSection("Header").GetValue("Amount"));
				for (int i = 0; i < count; i++)
				{
					var shopSection = ini.GetSection("Shop" + i);
					uint npcId = Convert.ToUInt32(shopSection.GetValue("ID"));
					var shop = new Models.Misc.Shop(npcId, Enums.ShopType.Money);
					
					int itemAmount = Convert.ToInt32(shopSection.GetValue("ItemAmount"));
					for (int j = 0; j < itemAmount; j++)
					{
						uint itemId = Convert.ToUInt32(shopSection.GetValue("Item" + j));
						
						shop.Items.TryAdd(itemId);
					}
					
					_shops.TryAdd(npcId, shop);
				}
			}
		}
		
		/// <summary>
		/// Loads all cp shops.
		/// </summary>
		public static void LoadCpShops()
		{
			var shopIds = File.ReadAllLines(Drivers.Settings.DatabaseSettings.WorldFlatDatabase + "\\Shops\\CpShops.dat")[0]
				.Split(',')
				.Select(shopId => Convert.ToUInt32(shopId));
			
			var itemIds = File.ReadAllLines(Drivers.Settings.DatabaseSettings.WorldFlatDatabase + "\\Shops\\CpShop.dat")
				.Where(line => !string.IsNullOrWhiteSpace(line))
				.Select(itemId => Convert.ToUInt32(itemId));
			
			foreach (var shopId in shopIds)
			{
				var shop = new Models.Misc.Shop(shopId, Enums.ShopType.CPs);
				
				if (shop.Items.TryAddRange(itemIds))
				{
					_shops.TryAdd(shopId, shop);
				}
			}
		}
		
		/// <summary>
		/// Attempts to get a shop.
		/// </summary>
		/// <param name="shopId">The shop id.</param>
		/// <param name="shop">The shop.</param>
		/// <returns>True if the shop was retrieved.</returns>
		public static bool TryGetShop(uint shopId, out Models.Misc.Shop shop)
		{
			return _shops.TryGetValue(shopId, out shop);
		}
		
		/// <summary>
		/// Checks if a shop exists.
		/// </summary>
		/// <param name="shopId">The shop id.</param>
		/// <returns>True if the shop exists.</returns>
		public static bool ContainsShop(uint shopId)
		{
			return _shops.ContainsKey(shopId);
		}
	}
}
