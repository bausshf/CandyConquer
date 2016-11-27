// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Warehouse
{
	/// <summary>
	/// Description of Display.
	/// </summary>
	[ApiController()]
	public static class Display
	{
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.WarehousePacket,
		         SubIdentity = (uint)Enums.WarehouseAction.Display)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Items.WarehousePacket packet)
		{
			if (player.Guild != null && player.Guild.InHouse(player))
			{
				player.Guild.Warehouse.UpdateClient(player);
			}
			else
			{
				Collections.Warehouse warehouse;
				if (player.Warehouses.TryGetWarehouse(player.CurrentNpc.ClientId, out warehouse))
				{
					warehouse.UpdateClient();
				}
			}
			
			return true;
		}
	}
}
