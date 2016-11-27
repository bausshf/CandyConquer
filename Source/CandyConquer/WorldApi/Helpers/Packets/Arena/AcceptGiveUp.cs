// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Arena
{
	/// <summary>
	/// Controller for accept or give up.
	/// </summary>
	[ApiController()]
	public static class AcceptGiveUp
	{
		/// <summary>
		/// Handles the accept or give up.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ArenaActionPacket,
		         SubIdentity = (uint)Enums.ArenaDialog.AcceptGiveUp)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Arena.ArenaActionPacket packet)
		{
			switch (packet.Option)
			{
				case Enums.ArenaOption.Accept:
					{
						Controllers.Arena.ArenaQualifierController.Accept(player);
						break;
					}
					
				case Enums.ArenaOption.GiveUp:
					{
						Controllers.Arena.ArenaQualifierController.GiveUp(player);
						break;
					}
			}
			return true;
		}
	}
}
