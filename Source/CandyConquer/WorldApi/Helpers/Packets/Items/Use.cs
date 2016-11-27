// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Items
{
	/// <summary>
	/// Helper class for the item action packet's subtype for use.
	/// </summary>
	[ApiController()]
	public static class Use
	{
		/// <summary>
		/// Handles the use subtype of the item action packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		/// <returns>True if the packet was handled correctly.</returns>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ItemActionPacket,
		         SubIdentity = (uint)Enums.ItemAction.Use)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Items.ItemActionPacket packet)
		{
			var arenaBattle = player.Battle as Controllers.Arena.ArenaBattleController;
			if (player.Battle != null && !(player.Battle is Controllers.Arena.ArenaBattleController))
			{
				return true;
			}
			
			Models.Items.Item item;
			if (!player.Inventory.TryGetItem(packet.ClientId, out item))
			{
				Database.Dal.Accounts.Ban(
					player.DbPlayer.Account, Drivers.Messages.INVALID_USE_ITEM,
					Database.Models.DbAccount.BanRangeType.Perm);
				player.ClientSocket.Disconnect(Drivers.Messages.INVALID_USE_ITEM);
				return false;
			}
			
			if (item.IsMisc)
			{
				player.AddActionLog("ItemUsage", item.DbItem.Id);
				if (!Collections.ItemScriptCollection.Invoke(player, item.DbItem.Id))
				{
					player.SendFormattedSystemMessage("ITEM_USAGE_NOT_FOUND", true, item.DbItem.Name, item.DbItem.Id);
				}
			}
			else
			{
				var position = (Enums.ItemPosition)packet.Data1;
				if (position != Enums.ItemPosition.Inventory)
				{
					player.Equipments.Equip(item, position, true);
				}
			}
			
			return true;
		}
	}
}
