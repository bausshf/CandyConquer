// Project by Bauss
using System;
using System.Linq;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Client.DataExchange
{
	/// <summary>
	/// Helper for the change sub types of the data exchange packet.
	/// </summary>
	[ApiController()]
	public static class ChangeExchange
	{
		/// <summary>
		/// Handles the change direction.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.ChangeDirection)]
		public static bool ChangeDirection(Models.Entities.Player player, Models.Packets.Client.DataExchangePacket packet)
		{
			player.Direction = packet.Direction;
			player.AttackPacket = null;
			
			player.UpdateScreen(false, packet);
			return true;
		}
		
		/// <summary>
		/// Handles the change pk mode.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.SetPkMode)]
		public static bool ChangePKMode(Models.Entities.Player player, Models.Packets.Client.DataExchangePacket packet)
		{
			player.PKMode = (Enums.PKMode)packet.Data1Low;
			player.AttackPacket = null;
			
			player.ClientSocket.Send(packet);
			return true;
		}
		
		/// <summary>
		/// Handles the change action.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.ChangeAction)]
		public static bool ChangeAction(Models.Entities.Player player, Models.Packets.Client.DataExchangePacket packet)
		{
			var action = (Enums.PlayerAction)packet.Data1Low;
			player.AttackPacket = null;
			
			if (player.Action != Enums.PlayerAction.Sit || player.Action != Enums.PlayerAction.Lie)
			{
				if (action == Enums.PlayerAction.Sit || action == Enums.PlayerAction.Lie)
				{
					player.UpdateStamina();
				}
			}
			
			player.Action = action;
			
			player.UpdateScreen(false, packet);
			return true;
		}
		
		/// <summary>
		/// Handles the change map.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.ChangeMap)]
		public static bool ChangeMap(Models.Entities.Player player, Models.Packets.Client.DataExchangePacket packet)
		{
			if (player.Map.IsDynamic)
			{
				if (player.Houses.Count > 0 && player.Houses.GetAll().Any(house => house.DynamicMapId == player.MapId))
				{
					player.AddActionLog("LeaveHouse");
					player.TeleportToLastMap();
					return true;
				}
				
				if (player.Guild.InHouse(player))
				{
					player.AddActionLog("LeaveGuildHouse");
					player.TeleportToLastMap();
					return true;
				}
			}
			
			var coordinate = new Models.Maps.Coordinate(packet.Data1Low, packet.Data1High);
			if (Tools.RangeTools.GetDistanceU(player.X, player.Y, coordinate.X, coordinate.Y) <= 5)
			{
				int mapId = player.MapId;
				
				if (Collections.PortalCollection.Teleport(player, mapId, coordinate.X, coordinate.Y))
				{
					player.AddActionLog("Portal", string.Format("{0} : {1},{2}", mapId, coordinate.X, coordinate.Y));
					return true;
				}
			}
			
			player.Pullback();
			return true;
		}
	}
}
