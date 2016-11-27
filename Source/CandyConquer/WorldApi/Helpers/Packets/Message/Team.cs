// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Message
{
	/// <summary>
	/// Controller helper for Team.
	/// </summary>
	[ApiController()]
	public static class Team
	{
		/// <summary>
		/// Handles team talk.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.MessagePacket,
		         SubIdentity = (uint)Enums.MessageType.Team)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Message.MessagePacket packet)
		{
			if (player.TournamentTeam != null)
			{
				foreach (var member in player.TournamentTeam.Members.Values)
				{
					if (member.ClientId != player.ClientId)
					{
						member.ClientSocket.Send(packet);
					}
				}
			}
			else if (player.Team != null)
			{
				foreach (var teamMember in player.Team.GetMembers())
				{
					teamMember.ClientSocket.Send(packet);
				}
			}
			
			return true;
		}
	}
}
