// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Packets.Client;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Client.DataExchange
{
	/// <summary>
	/// Helper for the enter map sub type of the data exchange packet.
	/// </summary>
	[ApiController()]
	public static class EnterMap
	{
		/// <summary>
		/// Handles the enter map.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.EnterMap)]
		public static bool Handle(Models.Entities.Player player, DataExchangePacket packet)
		{
			if (!player.LoggedIn)
			{
				// First time logging in ...
				// Set map ...
				
				player.Teleport(player.DbPlayer.MapId.Value, player.DbPlayer.X.Value, player.DbPlayer.Y.Value);
				player.LastMap = Collections.MapCollection.GetMap(player.DbPlayer.LastMapId.Value);
				player.LastMapX = player.DbPlayer.LastMapX.Value;
				player.LastMapY = player.DbPlayer.LastMapY.Value;
				
				player.LoginProtectionEndTime = DateTime.UtcNow.AddSeconds(10);
				
				player.ValidateLoginMap();
			}
			
			if (!Collections.PlayerCollection.ContainsClientId(player.ClientId))
			{
				Database.Dal.Accounts.Ban(
					player.DbPlayer.Account, Drivers.Messages.BYPASS_LOGIN_ENTER_MAP,
					Database.Models.DbAccount.BanRangeType.Perm);
				player.ClientSocket.Disconnect(Drivers.Messages.BYPASS_LOGIN_ENTER_MAP);
				return false;
			}
			
			player.ClientSocket.Send(new Models.Packets.Client.DataExchangePacket
			                         {
			                         	ExchangeType = Enums.ExchangeType.EnterMap,
			                         	ClientId = (uint)player.Map.ClientMapId,
			                         	Data1 = (uint)player.Map.ClientMapId,
			                         	Data3Low = player.X,
			                         	Data3High = player.Y
			                         });
			player.ClientSocket.Send(new Models.Packets.Location.MapInfoPacket
			                         {
			                         	Map = player.Map
			                         });
			
			return true;
		}
	}
}
