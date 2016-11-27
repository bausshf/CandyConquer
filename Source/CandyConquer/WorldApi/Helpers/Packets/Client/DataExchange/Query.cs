// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Client.DataExchange
{
	/// <summary>
	/// Helper for the query sub types of the data exchange packet.
	/// </summary>
	[ApiController()]
	public static class Query
	{
		/// <summary>
		/// Handles the query player.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.QueryPlayer)]
		public static bool HandleQueryPlayer(Models.Entities.Player player, Models.Packets.Client.DataExchangePacket packet)
		{
			Models.Maps.IMapObject obj;
			if (packet.Data1 > 1000000 && player.GetFromScreen(packet.Data1, out obj))
			{
				var objPlayer = (obj as Models.Entities.Player);
				
				objPlayer.ClientSocket.Send(player.GetSpawnPacket());
				player.ClientSocket.Send(objPlayer.GetSpawnPacket());
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the query stats.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.QueryStatInfo)]
		public static bool HandleQueryStats(Models.Entities.Player player, Models.Packets.Client.DataExchangePacket packet)
		{
			if (packet.Data1 > 1000000 && packet.Data1 != player.ClientId)
			{
				var queryPlayer = Collections.PlayerCollection.GetPlayerByClientId(packet.Data1);
				if (queryPlayer != null)
				{
					queryPlayer.UpdateBaseStats();
					player.ClientSocket.Send(new Models.Packets.Entities.PlayerStatsPacket
					                         {
					                         	Player = queryPlayer
					                         });
				}
			}
			else
			{
				player.UpdateBaseStats();
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the query friend equip.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.QueryFriendEquip)]
		public static bool HandleQueryFriendEquip(Models.Entities.Player player, Models.Packets.Client.DataExchangePacket packet)
		{
			// TODO: Handle this correct ??
			return HandleQueryEquipments(player, packet);
		}
		
		/// <summary>
		/// Handles the query equipment.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.QueryEquipment)]
		public static bool HandleQueryEquipments(Models.Entities.Player player, Models.Packets.Client.DataExchangePacket packet)
		{
			Models.Entities.Player viewPlayer = Collections.PlayerCollection.GetPlayerByClientId(packet.Data1);
			if (viewPlayer != null)
			{
				player.ClientSocket.Send(viewPlayer.GetSpawnPacket());
				foreach (var item in viewPlayer.Equipments.GetAll())
				{
					item.ViewItem(viewPlayer, player);
				}
				
				if (viewPlayer.LoggedIn)
				{
					viewPlayer.SendFormattedSystemMessage("VIEW_EQUIPMENTS", true, player.Name);
				}
				
				player.ClientSocket.Send(new Models.Packets.Misc.StringPacket
				                         {
				                         	String = viewPlayer.Spouse,
				                         	Action = Enums.StringAction.QueryMate
				                         });
			}
			
			return true;
		}
	}
}
