// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Packets.Client;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Controllers.Packets.Client
{
	/// <summary>
	/// The data exchange packet controller.
	/// </summary>
	[ApiController()]
	public static class DataExchangeController
	{
		/// <summary>
		/// Retrieves the data exchange packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.DataExchangePacket, TypeReturner = true)]
		public static DataExchangePacket HandlePacket(Models.Entities.Player player, DataExchangePacket packet, out uint subPacketId)
		{
			subPacketId = (uint)packet.ExchangeType;
			return packet;
		}
	}
}
