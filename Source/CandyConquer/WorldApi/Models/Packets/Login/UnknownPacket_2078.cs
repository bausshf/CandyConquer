// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Login
{
	/// <summary>
	/// Unknown packet 2078
	/// </summary>
	public sealed class UnknownPacket_2078 : NetworkPacket
	{
		/// <summary>
		/// Creates a new unknown packet 2078.
		/// </summary>
		public UnknownPacket_2078()
			: base(264, 2078)
		{
		}
		
		/// <summary>
		/// Implicit conversion from the unknown packet to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](UnknownPacket_2078 packet)
		{
			packet.Offset = 4;
			packet.WriteUInt32(0x4e591dba);
			
			return packet.Buffer;
		}
	}
}
