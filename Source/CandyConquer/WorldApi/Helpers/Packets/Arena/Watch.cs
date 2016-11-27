// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Arena
{
	/// <summary>
	/// Controller for watch.
	/// </summary>
	[ApiController()]
	public static class Watch
	{
		/// <summary>
		/// Handles the quit.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ArenaWatchPacket,
		         SubIdentity = (uint)Enums.ArenaWatchType.Watch)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Arena.ArenaWatchPacket packet)
		{
			if (player.Battle != null || player.Map.IsDynamic)
			{
				return true;
			}
			
			var fighter = Collections.PlayerCollection.GetPlayerByClientId(packet.ClientId);
			
			if (fighter != null)
			{
				var match = fighter.Battle as Controllers.Arena.ArenaBattleController;
				
				if (match != null)
				{
					match.JoinAsWatcher(player);
				}
			}
			
			return true;
		}
	}
}
