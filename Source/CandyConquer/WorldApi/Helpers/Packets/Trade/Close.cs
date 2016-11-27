// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Trade
{
	/// <summary>
	/// Controller for close.
	/// </summary>
	[ApiController()]
	public static class Close
	{
		/// <summary>
		/// Handles the close.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.TradePacket,
		         SubIdentity = (uint)Enums.TradeAction.Close)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Trade.TradePacket packet)
		{
			if (player.Trade.Partner != null)
			{
				var partner = player.Trade.Partner;
				
				partner.Trade.Reset();
				player.Trade.Reset();
				
				packet.Action = Enums.TradeAction.HideTable;
				packet.TargetClientId = partner.ClientId;
				player.ClientSocket.Send(packet);						
				packet.TargetClientId = player.ClientId;
				partner.ClientSocket.Send(packet);
			}
			
			return true;
		}
	}
}
