// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Trade
{
	/// <summary>
	/// Controller for set money.
	/// </summary>
	[ApiController()]
	public static class SetMoney
	{
		/// <summary>
		/// Handles the set money.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.TradePacket,
		         SubIdentity = (uint)Enums.TradeAction.SetMoney)]
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
			
			uint money = packet.TargetClientId;
			if (money > player.Money)
			{
				player.SendSystemMessage("LOW_MONEY_TRADE");
				return true;
			}
			
			player.Trade.Money = money;
			packet.Action = Enums.TradeAction.ShowMoney;
			player.Trade.Partner.ClientSocket.Send(packet);
			
			return true;
		}
	}
}
