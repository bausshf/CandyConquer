// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Packets.Guilds;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Controllers.Packets.Guilds
{
	/// <summary>
	/// The guild packet controller.
	/// </summary>
	[ApiController()]
	public static class GuildPacketController
	{
		/// <summary>
		/// Retrieves the guild packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.GuildPacket, TypeReturner = true)]
		public static GuildPacket HandlePacket(Models.Entities.Player player, GuildPacket packet, out uint subPacketId)
		{
			subPacketId = (uint)packet.Action;
			return packet;
		}
		
		/// <summary>
		/// Handles the member list packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.GuildMemberListPacket)]
		public static bool HandleMemberList(Models.Entities.Player player, GuildMemberListPacket packet)
		{
			if (player.Guild != null)
			{
				player.ClientSocket.Send(new GuildMemberListPacket(packet.StartIndex)
				                         {
				                         	Guild = player.Guild
				                         });
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the donation packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.GuildDonationPacket)]
		public static bool HandleDonation(Models.Entities.Player player, GuildDonationPacket packet)
		{
			if (player.Guild != null)
			{
				player.ClientSocket.Send(new Models.Packets.Guilds.GuildDonationPacket
				                         {
				                         	Flags = Enums.GuildDonationFlags.AllDonations,
				                         	Money = player.GuildMember.DbGuildRank.SilverDonation,
				                         	CPs = player.GuildMember.DbGuildRank.CPDonation
				                         });
			}
			
			return true;
		}
	}
}
