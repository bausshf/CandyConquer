// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;
using CandyConquer.WorldApi.Models.Packets.Items;

namespace CandyConquer.WorldApi.Controllers.Packets.Items
{
	/// <summary>
	/// Controller for the item action packet.
	/// </summary>
	[ApiController()]
	public static class WarehousePacketController
	{
		/// <summary>
		/// Retrieves the item action packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.WarehousePacket, TypeReturner = true)]
		public static WarehousePacket HandlePacket(Models.Entities.Player player, WarehousePacket packet, out uint subPacketId)
		{
			if (player.CurrentNpc == null || !Tools.RangeTools.ValidDistance(player.CurrentNpc.X, player.CurrentNpc.Y, player.X, player.Y))
			{
				subPacketId = SubCallState.DontHandle;
			}
			else
			{
				subPacketId = (uint)packet.Action;
			}
			
			return packet;
		}
	}
}
