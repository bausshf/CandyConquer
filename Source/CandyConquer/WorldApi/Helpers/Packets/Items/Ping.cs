// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Items
{
	/// <summary>
	/// Controller for the ping sub type.
	/// </summary>
	[ApiController()]
	public static class Ping
	{
		/// <summary>
		/// Handles the ping sub type.
		/// </summary>
		/// <param name="player">The player.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ItemActionPacket,
		         SubIdentity = (uint)Enums.ItemAction.Ping)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Items.ItemActionPacket packet)
		{
			player.ClientSocket.Send(packet);
			return true;
		}
	}
}
