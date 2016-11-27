// Project by Bauss
using System;
using CandyConquer.ApiServer;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Controllers.Packets.Message
{
	/// <summary>
	/// The broadcast packet controller.
	/// </summary>
	[ApiController()]
	public static class BroadcastController
	{
		/// <summary>
		/// Retrieves the broadcast packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.BroadcastPacket)]
		public static bool HandlePacket(Models.Entities.Player player, SocketPacket packet)
		{
			if (player.Battle != null)
			{
				return true;
			}
			
			packet.Offset = 4;
			
			if (packet.ReadByte() > 2)
			{
				packet.Offset = 12;
				var strings = packet.ReadStrings();
				
				if (strings.Length > 0 && strings[0].Length < 255)
				{
					if (player.CPs >= 5)
					{
						player.AddActionLog("Broadcast", strings[0]);
						player.CPs -= 5;
						
						Collections.BroadcastQueue.Enqueue(
							MessageController.CreateBroadcast(player.Name, strings[0])
						);
					}
				}
			}
			
			return true;
		}
	}
}
