// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Arena
{
	/// <summary>
	/// Controller for join.
	/// </summary>
	[ApiController()]
	public static class Join
	{
		/// <summary>
		/// Handles the regular join.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ArenaActionPacket,
		         SubIdentity = (uint)Enums.ArenaDialog.Join)]
		public static bool HandleRegularJoin(Models.Entities.Player player, Models.Packets.Arena.ArenaActionPacket packet)
		{
			Controllers.Arena.ArenaQualifierController.Join(player);
			
			player.ClientSocket.Send(packet);
			return true;
		}
		
		/// <summary>
		/// Handles the alternative join 1.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ArenaActionPacket,
		         SubIdentity = (uint)Enums.ArenaDialog.JoinAlt1)]
		public static bool HandleAltJoin1(Models.Entities.Player player, Models.Packets.Arena.ArenaActionPacket packet)
		{
			if (packet.Option != Enums.ArenaOption.AltJoin)
			{
				return true;
			}
			
			return HandleRegularJoin(player, packet);
		}
		
		/// <summary>
		/// Handles the alternative join 2.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ArenaActionPacket,
		         SubIdentity = (uint)Enums.ArenaDialog.JoinAlt2)]
		public static bool HandleAltJoin2(Models.Entities.Player player, Models.Packets.Arena.ArenaActionPacket packet)
		{
			if (packet.Option != Enums.ArenaOption.AltJoin)
			{
				return true;
			}
			
			return HandleRegularJoin(player, packet);
		}
	}
}
