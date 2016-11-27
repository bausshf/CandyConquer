// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Login
{
	/// <summary>
	/// The world authentication packet.
	/// </summary>
	public sealed class WorldAuthenticationPacket : NetworkPacket
	{
		/// <summary>
		/// Gets the client id.
		/// </summary>
		public uint ClientId { get; private set; }
		
		/// <summary>
		/// Gets the key.
		/// </summary>
		public uint Key { get; private set; }
		
		/// <summary>
		/// Creates a new world authentication packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private WorldAuthenticationPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			ClientId = ReadUInt32();
			Key = ReadUInt32();
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to world authentication packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator WorldAuthenticationPacket(SocketPacket packet)
		{
			return new WorldAuthenticationPacket(packet);
		}
	}
}
