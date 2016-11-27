// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Message
{
	/// <summary>
	/// Controller helper for talk.
	/// </summary>
	[ApiController()]
	public static class Talk
	{
		/// <summary>
		/// Handles talk
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.MessagePacket,
		         SubIdentity = (uint)Enums.MessageType.Talk)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Message.MessagePacket packet)
		{
			player.UpdateScreen(false, packet);
			return true;
		}
	}
}
