﻿// Project by Bauss
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
	public static class ArenaBattleInfoPacketController
	{
		/// <summary>
		/// Retrieves the trade packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.ArenaBattleInfoPacket)]
		public static bool HandlePacket(Models.Entities.Player player, ArenaBattleInfoPacket packet)
		{
			player.ClientSocket.Send(new Models.Packets.Arena.ArenaBattleInfoPacket());
			
			return true;
		}
	}
}
