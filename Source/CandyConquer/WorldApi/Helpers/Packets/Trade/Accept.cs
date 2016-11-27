// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Trade
{
	/// <summary>
	/// Controller for accept.
	/// </summary>
	[ApiController()]
	public static class Accept
	{
		/// <summary>
		/// Handles the accept.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.TradePacket,
		         SubIdentity = (uint)Enums.TradeAction.Accept)]
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
			
			if (!player.Trade.Accepted && !player.Trade.PartnerAccepted)
			{
				player.Trade.Accepted = true;
				player.Trade.Partner.Trade.Accepted = true;
				packet.TargetClientId = player.ClientId;
				player.Trade.Partner.ClientSocket.Send(packet);
			}
			else if (player.Trade.Accepted && player.Trade.Partner.Trade.Accepted)
			{
				bool tradeSuccess = true;
				
				foreach (var item in player.Trade.Items)
				{
					if (!player.Inventory.Contains(item.ClientId))
						tradeSuccess = false;
				}
				
				foreach (var item in player.Trade.PartnerItems)
				{
					if (!player.Trade.Partner.Inventory.Contains(item.ClientId))
						tradeSuccess = false;
				}
				
				if (player.Money < player.Trade.Money)
					tradeSuccess = false;
				if (player.Trade.Partner.Money < player.Trade.PartnerMoney)
					tradeSuccess = false;
				
				if (player.CPs < player.Trade.CPs)
					tradeSuccess = false;
				if (player.Trade.Partner.CPs < player.Trade.PartnerCPs)
					tradeSuccess = false;
				
				if (tradeSuccess)
				{
					foreach (var item in player.Trade.Items)
					{
						player.AddActionLog("TradeItem", string.Format("{0} -> {1} : {2}", player.DbPlayer.Id, player.Trade.Partner.DbPlayer.Id, item.DbItem.Id));
						
						var removedItem = player.Inventory.Pop(item.ClientId);
						if (removedItem != null)
						{
							player.Trade.Partner.Inventory.Add(removedItem);
						}
					}
					
					foreach (var item in player.Trade.PartnerItems)
					{
						player.AddActionLog("TradeItem", string.Format("{0} -> {1} : {2}", player.Trade.Partner.DbPlayer.Id, player.DbPlayer.Id, item.DbItem.Id));
						
						var removedItem = player.Trade.Partner.Inventory.Pop(item.ClientId);
						if (removedItem != null)
						{
							player.Inventory.Add(item);
						}
					}
					
					if (player.Money >= player.Trade.Money)
					{
						player.AddActionLog("TradeMoney", player.Trade.Money);
						player.Money -= player.Trade.Money;
						player.Trade.Partner.Money += player.Trade.Money;
					}
					
					if (player.Trade.Partner.Money >= player.Trade.PartnerMoney)
					{
						player.AddActionLog("TradePartnerMoney", player.Trade.PartnerMoney);
						player.Trade.Partner.Money -= player.Trade.PartnerMoney;
						player.Money += player.Trade.PartnerMoney;
					}
					
					if (player.CPs >= player.Trade.CPs)
					{
						player.AddActionLog("TradeCPs", player.Trade.CPs);
						player.CPs -= player.Trade.CPs;
						player.Trade.Partner.CPs += player.Trade.CPs;
					}
					
					if (player.Trade.Partner.CPs >= player.Trade.PartnerCPs)
					{
						player.AddActionLog("TradePartnerCPs", player.Trade.PartnerCPs);
						player.Trade.Partner.CPs -= player.Trade.PartnerCPs;
						player.CPs += player.Trade.PartnerCPs;
					}

					var partner = player.Trade.Partner;
					
					partner.Trade.Reset();
					player.Trade.Reset();
					
					packet.Action = Enums.TradeAction.HideTable;
					packet.TargetClientId = partner.ClientId;
					player.ClientSocket.Send(packet);
					packet.TargetClientId = player.ClientId;
					partner.ClientSocket.Send(packet);
					
					player.SendSystemMessage("TRADE_SUCCESS");
					partner.SendSystemMessage("TRADE_SUCCESS");
				}
				else
				{
					var partner = player.Trade.Partner;
					
					partner.Trade.Reset();
					player.Trade.Reset();
					
					packet.Action = Enums.TradeAction.HideTable;
					packet.TargetClientId = partner.ClientId;
					player.ClientSocket.Send(packet);
					packet.TargetClientId = player.ClientId;
					partner.ClientSocket.Send(packet);
					
					player.SendSystemMessage("TRADE_FAIL");
					partner.SendSystemMessage("TRADE_FAIL");
				}
			}
			
			return true;
		}
	}
}
