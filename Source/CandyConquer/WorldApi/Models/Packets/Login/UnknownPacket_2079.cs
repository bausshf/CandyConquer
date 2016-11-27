// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Login
{
	/// <summary>
	/// Unknown packet 2079
	/// </summary>
	public sealed class UnknownPacket_2079 : NetworkPacket
	{
		/// <summary>
		/// Creates a new unknown packet 2079.
		/// </summary>
		public UnknownPacket_2079()
			: base(8, 2079)
		{
		}
		
		/// <summary>
		/// Implicit conversion from the unknown packet to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](UnknownPacket_2079 packet)
		{
			packet.Offset = 4;
			packet.WriteUInt32(0);
			
			return packet.Buffer;
		}
	}
}
