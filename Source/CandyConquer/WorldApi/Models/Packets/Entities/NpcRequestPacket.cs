// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Entities
{
	/// <summary>
	/// Model for the npc request packet.
	/// </summary>
	public sealed class NpcRequestPacket : NetworkPacket
	{
		/// <summary>
		/// Gets the npc id.
		/// </summary>
		public uint NpcId { get; private set; }
		
		/// <summary>
		/// Gets the option.
		/// </summary>
		public byte Option { get; private set; }
		
		/// <summary>
		/// Creates a new npc request packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private NpcRequestPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			NpcId = ReadUInt32();
			packet.Offset = 10;
			Option = ReadByte();
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to NpcRequestPacket packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator NpcRequestPacket(SocketPacket packet)
		{
			return new NpcRequestPacket(packet);
		}
	}
}
