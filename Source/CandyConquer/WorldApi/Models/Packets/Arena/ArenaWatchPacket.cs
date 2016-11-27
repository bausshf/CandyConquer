// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Arena
{
	/// <summary>
	/// Model for the arena watch packet.
	/// </summary>
	public sealed class ArenaWatchPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the watch type.
		/// </summary>
		public Enums.ArenaWatchType WatchType { get; set; }
		
		/// <summary>
		/// Gets or sets the client id.
		/// </summary>
		public uint ClientId { get; set; }
		
		/// <summary>
		/// Creates a new arena watch packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private ArenaWatchPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			WatchType = (Enums.ArenaWatchType)ReadUInt32();
			ClientId = ReadUInt32();
			
			SubTypeObject = WatchType;
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to ArenaWatchPacket packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator ArenaWatchPacket(SocketPacket packet)
		{
			return new ArenaWatchPacket(packet);
		}
	}
}
