// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Location
{
	/// <summary>
	/// Model for the walk packet.
	/// </summary>
	public sealed class WalkPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		public Enums.Direction Direction { get; set; }
		/// <summary>
		/// Gets or sets the client id.
		/// </summary>
		public uint ClientId { get; set; }
		/// <summary>
		/// Gets or sets the walk mode.
		/// </summary>
		public Enums.WalkMode Mode { get; set; }
		/// <summary>
		/// Gets or sets the timestamp.
		/// </summary>
		public uint Timestamp { get; set; }
		
		/// <summary>
		/// Creates a new WalkPacket packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private WalkPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			SubTypeOffset = 12;
			
			Direction = (Enums.Direction)ReadUInt32();
			ClientId = ReadUInt32();
			Mode = (Enums.WalkMode)ReadUInt32();
			Timestamp = ReadUInt32();
		}
		
		/// <summary>
		/// Creates a new walk packet.
		/// </summary>
		public WalkPacket()
			: base(24, 10005)
		{
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to WalkPacket packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator WalkPacket(SocketPacket packet)
		{
			return new WalkPacket(packet);
		}
		
		/// <summary>
		/// Implicit conversion from the WalkPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](WalkPacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32((uint)packet.Direction);
			packet.WriteUInt32((uint)packet.ClientId);
			packet.WriteUInt32((uint)packet.Mode);
			packet.WriteUInt32((uint)packet.Timestamp);
			
			return packet.Buffer;
		}
	}
}
