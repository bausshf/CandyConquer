// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Trade
{
	/// <summary>
	/// Controller for add item.
	/// </summary>
	[ApiController()]
	public static class AddItem
	{
		/// <summary>
		/// Handles the add item.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.TradePacket,
		         SubIdentity = (uint)Enums.TradeAction.AddItem)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Trade.TradePacket packet)
		{
			if (!player.Trade.Trading)
			{
				return true;
			}
			
			if (!player.Trade.WindowOpen)
			{
				return true;
			}
			
			Models.Items.Item item;
			if (player.Inventory.TryGetItem(packet.TargetClientId, out item))
			{
				if (!item.Discardable)
				{
					player.SendSystemMessage("NO_PERMISSION_ITEM");
					return true;
				}
				
				if ((player.Trade.Partner.Inventory.Count + player.Trade.Items.Count) >= 40)
				{
					player.SendSystemMessage("TARGET_FULL_INVENTORY");
					return true;
				}
				
				player.Trade.Items.Add(item);
				item.UpdateClient(player.Trade.Partner, Enums.ItemUpdateAction.Trade);
			}
			
			return true;
		}
	}
}
