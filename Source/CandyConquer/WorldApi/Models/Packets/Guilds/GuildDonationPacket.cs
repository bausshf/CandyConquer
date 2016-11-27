// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Guilds
{
	/// <summary>
	/// The guild donation packet.
	/// </summary>
	public sealed class GuildDonationPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the flags.
		/// </summary>
		public Enums.GuildDonationFlags Flags { get; set; }
		
		/// <summary>
		/// Gets or sets the cps.
		/// </summary>
		public uint CPs { get; set; }
		
		/// <summary>
		/// Gets or sets the money.
		/// </summary>
		public uint Money { get; set; }
		
		/// <summary>
		/// Creates a new guild donation packet.
		/// </summary>
		public GuildDonationPacket()
			: base(52, Data.Constants.PacketTypes.GuildDonationPacket)
		{
		}
		
		/// <summary>
		/// Creates a new guild donation packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private GuildDonationPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			Flags = (Enums.GuildDonationFlags)packet.ReadUInt32();
			CPs = ReadUInt32();
			Money = ReadUInt32();
		}
		
		/// <summary>
		/// Implicit conversion from the GuildDonationPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](GuildDonationPacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32((uint)packet.Flags);
			if (packet.Money == 0) packet.WriteUInt32(0);
			else packet.WriteUInt32((uint)(packet.Money / 10000));
			packet.WriteUInt32((uint)(packet.CPs * 20));
			
			return packet.Buffer;
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to GuildDonationPacket.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		/// <returns>The packet.</returns>
		public static implicit operator GuildDonationPacket(SocketPacket packet)
		{
			return new GuildDonationPacket(packet);
		}
	}
}
