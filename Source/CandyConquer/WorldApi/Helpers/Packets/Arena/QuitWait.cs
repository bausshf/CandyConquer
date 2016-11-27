// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Arena
{
	/// <summary>
	/// Controller for quit wait.
	/// </summary>
	[ApiController()]
	public static class QuitWait
	{
		/// <summary>
		/// Handles the quit wait.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ArenaActionPacket,
		         SubIdentity = (uint)Enums.ArenaDialog.QuitWait)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Arena.ArenaActionPacket packet)
		{
			Controllers.Arena.ArenaQualifierController.QuitWait(player);
			
			player.ClientSocket.Send(packet);
			return true;
		}
	}
}
