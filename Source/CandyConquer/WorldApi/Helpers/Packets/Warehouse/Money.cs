// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Warehouse
{
	/// <summary>
	/// Controller for money sub types.
	/// </summary>
	[ApiController()]
	public static class Money
	{
		/// <summary>
		/// Handles the query money sub type.
		/// </summary>
		/// <param name="player">The player.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ItemActionPacket,
		         SubIdentity = (uint)Enums.ItemAction.QueryMoneySaved)]
		public static bool HandleQueryMoneySaved(Models.Entities.Player player, Models.Packets.Items.ItemActionPacket packet)
		{
			if (player.Guild != null && player.Guild.InHouse(player))
			{
				packet.Data1 = player.Guild.DbGuild.WHMoney;
				player.ClientSocket.Send(packet);
				
				// ????
				player.UpdateClient(Enums.UpdateClientType.WarehouseMoney, packet.Data1);
			}
			else
			{
				packet.Data1 = player.WarehouseMoney;
				player.ClientSocket.Send(packet);
				
				// ????
				player.WarehouseMoney = player.WarehouseMoney;
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the save money sub type.
		/// </summary>
		/// <param name="player">The player.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ItemActionPacket,
		         SubIdentity = (uint)Enums.ItemAction.SaveMoney)]
		public static bool HandleSaveMoney(Models.Entities.Player player, Models.Packets.Items.ItemActionPacket packet)
		{
			if (player.Money < packet.Data1)
			{
				return true;
			}
			
			player.Money -= packet.Data1;
			
			if (player.Guild != null && player.Guild.InHouse(player))
			{
				player.Guild.DbGuild.WHMoney += packet.Data1;
				player.Guild.DbGuild.Update();
			}
			else
			{
				player.WarehouseMoney += packet.Data1;
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the draw money sub type.
		/// </summary>
		/// <param name="player">The player.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ItemActionPacket,
		         SubIdentity = (uint)Enums.ItemAction.DrawMoney)]
		public static bool HandleDrawMoney(Models.Entities.Player player, Models.Packets.Items.ItemActionPacket packet)
		{
			if (player.Guild != null && player.Guild.InHouse(player))
			{
				if (player.Guild.DbGuild.WHMoney < packet.Data1)
				{
					return true;
				}
				
				player.Guild.DbGuild.WHMoney -= packet.Data1;
				player.Guild.DbGuild.Update();
			}
			else
			{
				if (player.WarehouseMoney < packet.Data1)
				{
					return true;
				}
				
				player.WarehouseMoney -= packet.Data1;
			}
			
			player.Money += packet.Data1;
			return true;
		}
	}
}
