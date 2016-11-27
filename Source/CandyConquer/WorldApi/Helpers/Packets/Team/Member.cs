// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Team
{
	/// <summary>
	/// Controller for member.
	/// </summary>
	[ApiController()]
	public static class Member
	{
		/// <summary>
		/// Handles the kick.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.TeamActionPacket,
		         SubIdentity = (uint)Enums.TeamAction.Kick)]
		public static bool HandleKick(Models.Entities.Player player, Models.Packets.Team.TeamActionPacket packet)
		{
			if (player.Team == null)
			{
				return true;
			}
			
			if (!player.Team.IsLeader(player))
			{
				return true;
			}
			
			if (player.ClientId == packet.ClientId)
			{
				return true;
			}
			
			Models.Entities.Player removedPlayer;
			if (player.Team.Remove(packet.ClientId, out removedPlayer))
			{
				foreach (var member in player.Team.GetMembers())
				{
					member.ClientSocket.Send(packet);
				}
				
				removedPlayer.ClientSocket.Send(packet);
				
				removedPlayer.Team = null;
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the leave.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.TeamActionPacket,
		         SubIdentity = (uint)Enums.TeamAction.LeaveTeam)]
		public static bool HandleLeave(Models.Entities.Player player, Models.Packets.Team.TeamActionPacket packet)
		{
			if (player.Team == null)
			{
				return true;
			}
			
			if (player.Team.IsLeader(player))
			{
				return true;
			}
			
			if (player.ClientId != packet.ClientId)
			{
				return true;
			}
			
			Models.Entities.Player removedPlayer;
			if (player.Team.Remove(packet.ClientId, out removedPlayer))
			{
				foreach (var member in player.Team.GetMembers())
				{
					member.ClientSocket.Send(packet);
				}
				
				player.ClientSocket.Send(packet);
				
				player.Team = null;
			}
			
			return true;
		}
	}
}