// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.AuthApi.Models.Packets
{
	/// <summary>
	/// The password seed packet.
	/// </summary>
	public class PasswordSeedPacket : NetworkPacket
	{
		/// <summary>
		/// The password seed.
		/// </summary>
		public uint Seed { get; set; }
		
		/// <summary>
		/// Creates a new password seed packet.
		/// </summary>
		public PasswordSeedPacket()
			: base(8, 1059)
		{
		}
		
		/// <summary>
		/// Implicit conversion from PasswordSeedPacket to byte[].
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>byte[]</returns>
		public static implicit operator byte[](PasswordSeedPacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32(packet.Seed);
			
			return packet.Buffer;
		}
	}
}
