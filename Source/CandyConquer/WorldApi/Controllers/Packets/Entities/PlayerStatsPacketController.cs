// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.ApiServer;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Controllers.Packets.Entities
{
	/// <summary>
	/// Controller for the player stats packet.
	/// </summary>
	[ApiController()]
	public static class PlayerStatsPacketController
	{
		/// <summary>
		/// Handles the PlayerStatsPacket packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = 1040)]
		public static bool HandlePacket(Models.Entities.Player player, Models.Packets.Entities.PlayerStatsPacket packet)
		{
			if (packet.ClientId != player.ClientId)
			{
				var viewPlayer = Collections.PlayerCollection.GetPlayerByClientId(packet.ClientId);
				if (viewPlayer != null)
				{
					player.ClientSocket.Send(new Models.Packets.Entities.PlayerStatsPacket
					                         {
					                         	Player = viewPlayer
					                         });
				}
			}
			else
			{
				player.ClientSocket.Send(new Models.Packets.Entities.PlayerStatsPacket
				                         {
				                         	Player = player
				                         });
			}
			
			return true;
		}
	}
}
