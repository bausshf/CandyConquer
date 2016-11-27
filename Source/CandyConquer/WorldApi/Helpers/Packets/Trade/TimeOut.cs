// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Trade
{
	/// <summary>
	/// Controller for time out.
	/// </summary>
	[ApiController()]
	public static class TimeOut
	{
		/// <summary>
		/// Handles the time out.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.TradePacket,
		         SubIdentity = (uint)Enums.TradeAction.TimeOut)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Trade.TradePacket packet)
		{
			if (player.Trade.Partner != null)
			{
				player.Trade.Partner.Trade.Reset();
			}
			player.Trade.Reset();
			
			return true;
		}
	}
}
