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
	public static class ArenaStatisticPacketController
	{
		/// <summary>
		/// Retrieves the trade packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.ArenaStatisticPacket)]
		public static bool HandlePacket(Models.Entities.Player player, ArenaStatisticPacket packet)
		{
			player.ClientSocket.Send(new Models.Packets.Arena.ArenaStatisticPacket
			                         {
			                         	Player = player
			                         });
			
			return true;
		}
	}
}
