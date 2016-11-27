// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Message
{
	/// <summary>
	/// Controller helper for Guild.
	/// </summary>
	[ApiController()]
	public static class Guild
	{
		/// <summary>
		/// Handles guild talk.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.MessagePacket,
		         SubIdentity = (uint)Enums.MessageType.Guild)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Message.MessagePacket packet)
		{
			if (player.Guild != null)
			{
				player.Guild.BroadcastMessage(packet);
			}
			
			return true;
		}
	}
}
