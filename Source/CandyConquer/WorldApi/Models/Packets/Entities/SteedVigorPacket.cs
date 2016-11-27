// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Entities
{
	/// <summary>
	/// Model for the steed-vigor packet.
	/// </summary>
	public sealed class SteedVigorPacket : NetworkPacket
	{
		/// <summary>
		/// The type.
		/// </summary>
		public uint Type { get; set; }
		
		/// <summary>
		/// The amount.
		/// </summary>
		public uint Amount { get; set; }
		
		/// <summary>
		/// Creates a new steed vigor packet.
		/// </summary>
		public SteedVigorPacket()
			: base(36, Data.Constants.PacketTypes.DateTimeVigorPacket)
		{
		}
		
		/// <summary>
		/// Implicit conversion from the SteedVigorPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](SteedVigorPacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32(packet.Type);
			packet.WriteUInt32(packet.Amount);
			
			return packet.Buffer;
		}
	}
}
