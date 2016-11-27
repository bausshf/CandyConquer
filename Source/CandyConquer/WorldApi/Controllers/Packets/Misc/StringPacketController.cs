// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Packets.Misc;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Controllers.Packets.Misc
{
	/// <summary>
	/// The string packet controller.
	/// </summary>
	[ApiController()]
	public static class StringPacketController
	{
		/// <summary>
		/// Retrieves the string packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.StringPacket, TypeReturner = true)]
		public static StringPacket HandlePacket(Models.Entities.Player player, StringPacket packet, out uint subPacketId)
		{
			subPacketId = (uint)packet.Action;
			return packet;
		}
	}
}
