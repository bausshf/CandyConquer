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
	public static class ArenaActionPacketController
	{
		/// <summary>
		/// Retrieves the trade packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.ArenaActionPacket, TypeReturner = true)]
		public static ArenaActionPacket HandlePacket(Models.Entities.Player player, ArenaActionPacket packet, out uint subPacketId)
		{
			if (player.Battle != null)
			{
				subPacketId = SubCallState.DontHandle;
				return packet;
			}
			
			subPacketId = (uint)packet.Dialog;
			return packet;
		}
	}
}
