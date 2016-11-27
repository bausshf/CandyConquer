// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Strings
{
	/// <summary>
	/// Controller for query.
	/// </summary>
	[ApiController()]
	public static class Query
	{
		/// <summary>
		/// Handles query mate.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.StringPacket,
		         SubIdentity = (uint)Enums.StringAction.QueryMate)]
		public static bool HandleQueryMate(Models.Entities.Player player, Models.Packets.Misc.StringPacket packet)
		{
			Models.Entities.Player requestPlayer = Collections.PlayerCollection.GetPlayerByClientId(packet.Data);
			if (requestPlayer != null)
			{
				player.ClientSocket.Send(new Models.Packets.Misc.StringPacket
				                         {
				                         	String = requestPlayer.Spouse,
				                         	Action = Enums.StringAction.QueryMate
				                         });
			}
			
			return true;
		}
	}
}
