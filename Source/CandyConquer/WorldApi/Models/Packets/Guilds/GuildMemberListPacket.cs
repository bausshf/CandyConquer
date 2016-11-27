// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Guilds
{
	/// <summary>
	/// Model for the guild member list packet.
	/// </summary>
	public sealed class GuildMemberListPacket : NetworkPacket
	{
		/// <summary>
		/// Gets the start index.
		/// </summary>
		public int StartIndex { get; private set; }
		
		/// <summary>
		/// Gets the guild.
		/// </summary>
		public Models.Guilds.Guild Guild { get; set; }
		
		/// <summary>
		/// Creates a new guild member list packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private GuildMemberListPacket(NetworkPacket packet)
			: base(packet, 8)
		{
			StartIndex = ReadInt32();
		}
		
		/// <summary>
		/// Creates a new guild member list packet.
		/// </summary>
		/// <param name="startIndex">The start index.</param>
		public GuildMemberListPacket(int startIndex)
			: base(20, Data.Constants.PacketTypes.GuildMemberListPacket)
		{
			StartIndex = startIndex;
		}
		
		/// <summary>
		/// Implicit conversion from the GuildMemberList to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](GuildMemberListPacket packet)
		{
			packet.Offset = 8;
			
			var members = packet.Guild.GetPagedMembers(packet.StartIndex);
			
			packet.WriteInt32(0); // You don't write start index apparently ??
			packet.WriteInt32(members.Count);
			packet.Expand(members.Count * 48);
			
			foreach (var member in members)
			{
				packet.WriteStringWithReminder(member.DbGuildRank.PlayerName, 16);
				packet.WriteUInt32(0); // unknown
				packet.WriteUInt32((uint)member.NobilityRank);
				packet.WriteUInt32((uint)member.DbGuildRank.PlayerLevel);
				packet.WriteUInt16((ushort)member.Rank);
				packet.WriteUInt16(0); // unknown
				packet.WriteUInt32(0); // shows some exp thing, idk???
				packet.WriteUInt32(member.DbGuildRank.SilverDonation);
				packet.WriteBool(member.Online);
				// padding 3 bytes
				packet.WriteByte(0);
				packet.WriteByte(0);
				packet.WriteByte(0);
				packet.WriteUInt32(0); // unknown
			}
			
			return packet.Buffer;
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to GuildMemberListPacket.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		/// <returns>The packet.</returns>
		public static implicit operator GuildMemberListPacket(SocketPacket packet)
		{
			return new GuildMemberListPacket(packet);
		}
	}
}
