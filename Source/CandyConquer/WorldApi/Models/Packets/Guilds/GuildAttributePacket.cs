// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Guilds
{
	/// <summary>
	/// The guild attribute packet.
	/// </summary>
	public sealed class GuildAttributePacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the guild id.
		/// </summary>
		public uint GuildId { get; set; }
		
		/// <summary>
		/// Gets or sets the silver fund.
		/// </summary>
		public ulong SilverFund { get; set; }
		
		/// <summary>
		/// Gets or sets the cps fund.
		/// </summary>
		public uint CPsFund { get; set; }
		
		/// <summary>
		/// Gets or sets the amount.
		/// </summary>
		public int Amount { get; set; }
		
		/// <summary>
		/// Gets or sets the rank.
		/// </summary>
		public Enums.GuildRank Rank { get; set; }
		
		/// <summary>
		/// Gets or sets the guild leader.
		/// </summary>
		public string GuildLeader { get; set; }
		
		/// <summary>
		/// Gets or sets the required level.
		/// </summary>
		public int RequiredLevel { get; set; }
		
		/// <summary>
		/// Gets or sets the required metempsychosis.
		/// </summary>
		public int RequiredMetempsychosis { get; set; }
		
		/// <summary>
		/// Gets or sets the required profession.
		/// </summary>
		public int RequiredProfession { get; set; }
		
		/// <summary>
		/// Gets or sets the level.
		/// </summary>
		public byte Level { get; set; }
		
		/// <summary>
		/// Gets or sets the enrollment date.
		/// </summary>
		public uint EnrollmentDate { get; set; }
		
		/// <summary>
		/// Creates a new guild attribute packet.
		/// </summary>
		public GuildAttributePacket()
			: base(92, Data.Constants.PacketTypes.GuildAttributePacket)
		{
		}
		
		/// <summary>
		/// Implicit conversion from the GuildAttributePacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](GuildAttributePacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32(packet.GuildId);
			packet.WriteUInt32(0); // unknown
			packet.WriteUInt64(packet.SilverFund);
			packet.WriteUInt32(packet.CPsFund);
			packet.WriteInt32(packet.Amount);
			packet.WriteUInt16((ushort)packet.Rank);
			packet.WriteUInt16(0); // unknown or padding
			packet.WriteStringWithReminder(packet.GuildLeader, 16);
			packet.WriteInt32(packet.RequiredLevel);
			packet.WriteInt32(packet.RequiredMetempsychosis);
			packet.WriteInt32(packet.RequiredProfession);
			packet.WriteByte(packet.Level);
			packet.WriteUInt16(0); // unknown
			packet.WriteUInt32(0); // unknown
			packet.WriteUInt32(packet.EnrollmentDate);
			packet.WriteByte(0); // unknown
			packet.WriteUInt32(0); // unknown
			packet.WriteUInt16(0); // unknown
			packet.WriteByte(0); // unknown
			packet.WriteUInt32(0); // unknown
			packet.WriteUInt32(0); // unknown
			packet.WriteUInt32(0); // unknown
			
			return packet.Buffer;
		}
	}
}
