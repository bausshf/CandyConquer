// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Warehouse
{
	/// <summary>
	/// Description of Remove.
	/// </summary>
	[ApiController()]
	public static class Remove
	{
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.WarehousePacket,
		         SubIdentity = (uint)Enums.WarehouseAction.Remove)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Items.WarehousePacket packet)
		{
			if (player.Guild != null && player.Guild.InHouse(player))
			{
				if (player.Inventory.Count >= 40)
				{
					player.SendSystemMessage("WAREHOUSE_FULL");
					return true;
				}
				
				var item = player.Guild.Warehouse.Pop(packet.ClientId);
				if (item != null)
				{
					player.Inventory.Add(item);
				}
				
				player.ClientSocket.Send(packet);
			}
			else
			{
				Collections.Warehouse warehouse;
				if (player.Warehouses.TryGetWarehouse(player.CurrentNpc.ClientId, out warehouse))
				{
					if (player.Inventory.Count >= 20)
					{
						player.SendSystemMessage("WAREHOUSE_FULL");
						return true;
					}
					
					var item = warehouse.Pop(packet.ClientId);
					if (item != null)
					{
						player.Inventory.Add(item);
					}
					
					player.ClientSocket.Send(packet);
				}
			}
			
			return true;
		}
	}
}
