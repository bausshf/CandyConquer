// Project by Bauss
using System;
using System.Collections.Generic;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Team
{
	/// <summary>
	/// Controller for team.
	/// </summary>
	[ApiController()]
	public static class Team
	{
		/// <summary>
		/// Handles the creation.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.TeamActionPacket,
		         SubIdentity = (uint)Enums.TeamAction.Create)]
		public static bool HandleCreate(Models.Entities.Player player, Models.Packets.Team.TeamActionPacket packet)
		{
			if (player.Team != null)
			{
				return true;
			}
			
			if (Collections.TeamCollection.Create(player))
			{
				player.ClientSocket.Send(new Models.Packets.Team.TeamActionPacket
				                         {
				                         	ClientId = player.ClientId,
				                         	Action = Enums.TeamAction.Leader
				                         });
				player.ClientSocket.Send(new Models.Packets.Team.TeamActionPacket
				                         {
				                         	ClientId = player.ClientId,
				                         	Action = Enums.TeamAction.Create
				                         });
				player.AddStatusFlag(Enums.StatusFlag.TeamLeader);
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the dismiss.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.TeamActionPacket,
		         SubIdentity = (uint)Enums.TeamAction.Dismiss)]
		public static bool HandleDismiss(Models.Entities.Player player, Models.Packets.Team.TeamActionPacket packet)
		{
			if (player.Team == null)
			{
				return true;
			}
			
			if (!player.Team.IsLeader(player))
			{
				return true;
			}
			
			if (!player.Alive)
			{
				return true;
			}
			
			player.RemoveStatusFlag(Enums.StatusFlag.TeamLeader);
			var members = player.Team.Delete();
			
			foreach (var member in members)
			{
				member.Team = null;
				member.ClientSocket.Send(packet);
			}
			
			return true;
		}
	}
}