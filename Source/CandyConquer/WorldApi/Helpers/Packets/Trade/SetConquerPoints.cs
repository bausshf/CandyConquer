// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Trade
{
	/// <summary>
	/// Controller for set conquer points.
	/// </summary>
	[ApiController()]
	public static class SetConquerPoints
	{
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.TradePacket,
		         SubIdentity = (uint)Enums.TradeAction.SetConquerPoints)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Trade.TradePacket packet)
		{
			if (!player.Trade.Trading)
			{
				return true;
			}
			
			if (!player.Trade.WindowOpen)
			{
				return true;
			}
			
			uint cps = packet.TargetClientId;
			if (cps > player.CPs)
			{
				player.SendSystemMessage("LOW_CPS_TRADE");
				return true;
			}
			
			player.Trade.CPs = cps;
			packet.Action = Enums.TradeAction.ShowConquerPoints;
			player.Trade.Partner.ClientSocket.Send(packet);
			
			return true;
		}
	}
}
