// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Client.DataExchange
{
	/// <summary>
	/// Helper for the EndFly sub type of the data exchange packet.
	/// </summary>
	[ApiController()]
	public static class EndFly
	{
		/// <summary>
		/// Handles the EndFly.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.EndFly)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Client.DataExchangePacket packet)
		{
			player.RemoveStatusFlag(Enums.StatusFlag.Fly);
			
			return true;
		}
	}
}
