// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Message
{
	/// <summary>
	/// Controller for shopping.
	/// </summary>
	[ApiController()]
	public static class Shopping
	{
		/// <summary>
		/// Handles buy sub type.
		/// </summary>
		/// <param name="player"></param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ItemActionPacket,
		         SubIdentity = (uint)Enums.ItemAction.Buy)]
		public static bool HandleBuy(Models.Entities.Player player, Models.Packets.Items.ItemActionPacket packet)
		{
			if (player.Inventory.Count == 40)
			{
				player.SendSystemMessage("INVENTORY_FULL");
				return true;
			}
			
			Models.Misc.Shop shop;
			if (Collections.ShopCollection.TryGetShop(packet.ClientId, out shop))
			{
				var npc = shop.Npc;
				if (shop.ShopType != Enums.ShopType.Money || npc != null && Tools.RangeTools.ValidDistance(npc.X, npc.Y, player.X, player.Y))
				{
					uint itemId = packet.Data1;
					if (!shop.Items.Contains(itemId))
					{
						return false;
					}
					
					uint amount = packet.Data2;
					
					int above = (int)((player.Inventory.Count + amount) - 40);
					if (above > 0)
						amount -= (uint)above;
					
					if ((player.Inventory.Count + amount) > 40)
					{
						player.SendSystemMessage("INVENTORY_NO_SPACE");
						return true;
					}
					
					switch (shop.ShopType)
					{
						case Enums.ShopType.Money:
							return HandleBuy(player, itemId, amount, true);
							
						case Enums.ShopType.CPs:
							return HandleBuy(player, itemId, amount, false);
							
						default:
							return true;
					}
				}
			}
			
			return false;
		}
		
		/// <summary>
		/// Handles the actual buying of an item.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="itemId">The item.</param>
		/// <param name="amount">The amount.</param>
		/// <param name="money">Boolean indicating whether the buying is by money or not.</param>
		/// <returns>True if the buy was a success.</returns>
		private static bool HandleBuy(Models.Entities.Player player, uint itemId, uint amount, bool money)
		{
			var tempItem = Collections.ItemCollection.CreateItemById(itemId);
			var price = money ?
				tempItem.DbItem.Price * amount :
				tempItem.DbItem.CPPrice * amount;
			
			if (money && player.Money < price || !money && player.CPs < price)
			{
				player.SendFormattedSystemMessage("LOW_MONEY", true, price, money ? "silvers" : "CPs", amount, tempItem.DbItem.Name);
				return true;
			}
			
			player.AddActionLog("Buy", "IsMoney : " + money + " : " + itemId);
			
			if (money)
			{
				player.Money -= price;
			}
			else
			{
				player.CPs -= price;
			}
			
			for (int i = 0; i < amount; i++)
			{
				if (!player.Inventory.Add(itemId))
				{
					player.ClientSocket.Disconnect(string.Format(Drivers.Messages.Errors.FAIL_BUY_ITEMS, itemId, amount, i, tempItem.DbItem.Price, price));
					return false;
				}
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the sell sub type.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		/// <returns>True if the sale was a success.</returns>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ItemActionPacket,
		         SubIdentity = (uint)Enums.ItemAction.Sell)]
		public static bool HandleSell(Models.Entities.Player player, Models.Packets.Items.ItemActionPacket packet)
		{
			Models.Misc.Shop shop;
			if (Collections.ShopCollection.TryGetShop(packet.ClientId, out shop))
			{
				var npc = shop.Npc;
				if (npc != null && Tools.RangeTools.ValidDistance(npc.X, npc.Y, player.X, player.Y))
				{
					Models.Items.Item item;
					if (player.Inventory.TryGetItem(packet.Data1, out item))
					{
						if (!item.Discardable)
						{
							player.SendSystemMessage("NO_PERMISSION_ITEM");
							return true;
						}
						
						player.AddActionLog("Sell", item.DbOwnerItem.Id);
						uint giveback = (uint)(item.DbOwnerItem.CurrentDura > 0 ? (item.DbItem.Price / 3) : 0);
						if (player.Inventory.Remove(item.ClientId))
						{
							player.Money += giveback;
						}
					}
				}
			}
			
			return true;
		}
	}
}
