// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Packets.Arena;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Controllers.Packets.Arena
{
	/// <summary>
	/// The arena action packet controller.
	/// </summary>
	[ApiController()]
	public static class ArenaWatchPacketController
	{
		/// <summary>
		/// Retrieves the trade packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.ArenaWatchPacket, TypeReturner = true)]
		public static ArenaWatchPacket HandlePacket(Models.Entities.Player player, ArenaWatchPacket packet, out uint subPacketId)
		{
			subPacketId = (uint)packet.WatchType;
			return packet;
		}
	}
}
