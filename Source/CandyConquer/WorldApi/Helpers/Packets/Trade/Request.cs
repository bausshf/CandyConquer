// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Trade
{
	/// <summary>
	/// Controller for request.
	/// </summary>
	[ApiController()]
	public static class Request
	{
		/// <summary>
		/// Handles the request.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.TradePacket,
		         SubIdentity = (uint)Enums.TradeAction.Request)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Trade.TradePacket packet)
		{
			if (player.Trade.Trading && player.Trade.Partner == null)
			{
				player.SendSystemMessage("SELF_ALREADY_TRADE");
				return true;
			}
			else if (player.Trade.Trading)
			{
				if (!player.Trade.Requesting)
				{
					packet.Action = Enums.TradeAction.ShowTable;
					packet.TargetClientId = player.Trade.Partner.ClientId;
					player.ClientSocket.Send(packet);
					
					packet.TargetClientId = player.ClientId;
					player.Trade.Partner.ClientSocket.Send(packet);
					
					player.Trade.WindowOpen = true;
					player.Trade.Partner.Trade.WindowOpen = true;
				}
				
				return true;
			}
			
			Models.Maps.IMapObject obj;
			if (player.GetFromScreen(packet.TargetClientId, out obj))
			{
				var target = obj as Models.Entities.Player;
				if (!target.Trade.Trading)
				{
					player.Trade.Begin(target);
					target.Trade.Begin(player);
					
					target.Trade.Requesting = false;
					player.Trade.Requesting = true;
					
					packet.TargetClientId = player.ClientId;
					target.ClientSocket.Send(packet);
				}
				else
				{
					player.SendSystemMessage("TARGET_ALREADY_TRADE");
				}
			}
			else
			{
				player.ClientSocket.Disconnect(Drivers.Messages.Errors.INVALID_TRADE_TARGET);
			}
			
			return true;
		}
	}
}
