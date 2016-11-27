// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Warehouse
{
	/// <summary>
	/// Description of Add.
	/// </summary>
	[ApiController()]
	public static class Add
	{
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.WarehousePacket,
		         SubIdentity = (uint)Enums.WarehouseAction.Add)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Items.WarehousePacket packet)
		{
			if (player.Guild != null && player.Guild.InHouse(player))
			{
				if (player.Guild.Warehouse.Count >= 20)
				{
					player.SendSystemMessage("WAREHOUSE_FULL");
					return true;
				}
				
				var item = player.Inventory.Pop(packet.ClientId);
				if (item != null)
				{
					player.AddActionLog("AddGuildWarehouseItem", item.DbItem.Id);
					player.Guild.Warehouse.Add(item, player);
				}
			}
			else
			{
				Collections.Warehouse warehouse;
				if (player.Warehouses.TryGetWarehouse(player.CurrentNpc.ClientId, out warehouse))
				{
					if (warehouse.Count >= 20)
					{
						player.SendSystemMessage("WAREHOUSE_FULL");
						return true;
					}
					
					var item = player.Inventory.Pop(packet.ClientId);
					if (item != null)
					{
						player.AddActionLog("AddWarehouseItem", item.DbItem.Id + " :" + warehouse.Id);
						warehouse.Add(item);
					}
				}
			}
			
			return true;
		}
	}
}
