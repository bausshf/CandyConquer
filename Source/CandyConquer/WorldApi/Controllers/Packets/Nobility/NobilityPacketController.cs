// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Packets.Nobility;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Controllers.Packets.Nobility
{
	/// <summary>
	/// The Nobility packet controller.
	/// </summary>
	[ApiController()]
	public static class NobilityPacketController
	{
		/// <summary>
		/// Retrieves the Nobility action packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.NobilityPacket, TypeReturner = true)]
		public static NobilityPacket HandlePacket(Models.Entities.Player player, NobilityPacket packet, out uint subPacketId)
		{
			var action = packet.Action;
			packet.SubTypeObject = action;
			subPacketId = (uint)action;
			
			return packet;
		}
	}
}
