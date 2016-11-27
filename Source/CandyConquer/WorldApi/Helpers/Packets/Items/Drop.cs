// Project by Bauss
using System;
using System.Threading.Tasks;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Items
{
	/// <summary>
	/// Helper class for the item action packet's subtype for drop.
	/// </summary>
	[ApiController()]
	public static class Drop
	{
		/// <summary>
		/// Handles the drop subtype of the item action packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		/// <returns>True if the packet was handled correctly.</returns>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ItemActionPacket,
		         SubIdentity = (uint)Enums.ItemAction.Drop)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Items.ItemActionPacket packet)
		{
			if (!player.Alive)
			{
				return true;
			}
			
			if (player.Battle != null)
			{
				return true;
			}
			
			var location = player.Map.GetValidItemCoordinate(player.X, player.Y);
			if (location.Valid)
			{
				Models.Items.Item item = player.Inventory.Find(i => i.ClientId == packet.ClientId);
				if (item != null && item.Discardable)
				{
					player.AddActionLog("ItemDrop", item.DbOwnerItem.Id);
					
					if (player.Inventory.Remove(item.ClientId))
					{
						item.Drop(player.MapId, location.X, location.Y, true, player.ClientId);
					}
				}
				else
				{
					Database.Dal.Accounts.Ban(
						player.DbPlayer.Account, Drivers.Messages.INVALID_DROP_ITEM,
						Database.Models.DbAccount.BanRangeType.Perm);
					player.ClientSocket.Disconnect(Drivers.Messages.INVALID_DROP_ITEM);
					return false;
				}
			}
			
			return true;
		}
	}
}
