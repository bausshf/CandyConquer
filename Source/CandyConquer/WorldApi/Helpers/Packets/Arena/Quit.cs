// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Arena
{
	/// <summary>
	/// Controller for quit.
	/// </summary>
	[ApiController()]
	public static class Quit
	{
		/// <summary>
		/// Handles the quit.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ArenaActionPacket,
		         SubIdentity = (uint)Enums.ArenaDialog.Quit)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Arena.ArenaActionPacket packet)
		{
			Controllers.Arena.ArenaQualifierController.Quit(player);
			return true;
		}
	}
}
