// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Packets.Team;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Controllers.Packets.Team
{
	/// <summary>
	/// The team packet controller.
	/// </summary>
	[ApiController()]
	public static class TeamPacketController
	{
		/// <summary>
		/// Retrieves the team action packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.TeamActionPacket, TypeReturner = true)]
		public static TeamActionPacket HandlePacket(Models.Entities.Player player, TeamActionPacket packet, out uint subPacketId)
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
