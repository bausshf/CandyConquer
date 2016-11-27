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
	public static class ArenaTopPlayersPacketController
	{
		/// <summary>
		/// Retrieves the trade packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.ArenaTopPlayersPacket)]
		public static bool HandlePacket(Models.Entities.Player player, ArenaTopPlayersPacket packet)
		{
			player.ClientSocket.Send(new Models.Packets.Arena.ArenaTopPlayersPacket());
			
			return true;
		}
	}
}
