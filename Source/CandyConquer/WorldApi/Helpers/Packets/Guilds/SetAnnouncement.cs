// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Guilds
{
	/// <summary>
	/// Controller for set announcement.
	/// </summary>
	[ApiController()]
	public static class SetAnnouncement
	{
		/// <summary>
		/// Handles the set announce.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.GuildPacket,
		         SubIdentity = (uint)Enums.GuildAction.SetAnnounce)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Guilds.GuildPacket packet)
		{
			if (player.Guild != null && player.GuildMember.Rank == Enums.GuildRank.GuildLeader)
			{
				player.AddActionLog("SetGuildAnnouncement", packet.Announcement);
				player.Guild.SetAnnouncement(packet.Announcement);
			}
			
			return true;
		}
	}
}
