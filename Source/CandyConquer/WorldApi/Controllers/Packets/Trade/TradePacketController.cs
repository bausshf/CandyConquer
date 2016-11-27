// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Packets.Trade;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Controllers.Packets.Trade
{
	/// <summary>
	/// The trade packet controller.
	/// </summary>
	[ApiController()]
	public static class TradePacketController
	{
		/// <summary>
		/// Retrieves the trade packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.TradePacket, TypeReturner = true)]
		public static TradePacket HandlePacket(Models.Entities.Player player, TradePacket packet, out uint subPacketId)
		{
			if (player.Battle != null)
			{
				subPacketId = SubCallState.DontHandle;
				return packet;
			}
			
			subPacketId = (uint)packet.Action;
			return packet;
		}
	}
}
