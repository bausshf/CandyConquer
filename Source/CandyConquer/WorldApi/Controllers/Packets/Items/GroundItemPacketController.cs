// Project by Bauss
using System;
using System.Threading.Tasks;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.ApiServer;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Controllers.Packets.Items
{
	/// <summary>
	/// Controller for the ground item packet.
	/// </summary>
	[ApiController()]
	public static class GroundItemPacketController
	{
		/// <summary>
		/// Handles the GrounditemPacket packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = 1101)]
		public static bool HandlePacket(Models.Entities.Player player, Models.Packets.Items.GroundItemPacket packet)
		{
			if (packet.Action == Enums.GroundItemAction.Pickup && player.Map != null)
			{
				Models.Maps.IMapObject mapObject;
				if (player.GetFromScreen(packet.ClientId, out mapObject))
				{
					var item = mapObject as Models.Items.Item;
					
					if (item != null && Tools.RangeTools.GetDistanceU(player.X, player.Y, item.X, item.Y) < 2)
					{
						if (!item.PlayerDrop && DateTime.UtcNow < item.DropTime.AddMilliseconds(Data.Constants.Time.DropTimeShare) &&
						    item.DropClientId != player.ClientId)
						{
							player.SendSystemMessage("NOT_OWNER_ITEM");
						}
						else
						{
							Task.Run(async() => await item
							         .ResetLocationAsync(true, () =>
							                             {
							                             	if (item.DropMoney > 0)
							                             	{
							                             		player.AddActionLog("PickUpMoney", item.DropMoney);
							                             		player.Money += item.DropMoney;
							                             	}
							                             	else
							                             	{
							                             		player.AddActionLog("PickUpItem", item.DbItem.Id);
							                             		player.Inventory.Add(item);
							                             	}
							                             }, (e) =>
							                             {
							                             	player.ClientSocket.LastException = e;
							                             	player.ClientSocket.Disconnect(Drivers.Messages.Errors.FATAL_ERROR_TITLE);
							                             }));
						}
					}
				}
				
				return true;
			}
			
			return false;
		}
	}
}
