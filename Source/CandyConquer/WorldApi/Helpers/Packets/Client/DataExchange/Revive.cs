// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Client.DataExchange
{
	/// <summary>
	/// Helper for the revive sub type of the data exchange packet.
	/// </summary>
	[ApiController()]
	public static class Revive
	{
		/// <summary>
		/// Handles the Revive.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.Revive)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Client.DataExchangePacket packet)
		{
			if (!player.Alive && DateTime.UtcNow >= player.ReviveTime)
			{
				player.AddActionLog("RevivePacket");
				player.Revive(false);
			}
			
			return true;
		}
	}
}
