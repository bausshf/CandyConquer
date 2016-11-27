// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Location
{
	/// <summary>
	/// The npc spawn packet.
	/// </summary>
	public sealed class NpcSpawnPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// Gets or sets the client id.
		/// </summary>
		public uint ClientId { get; set; }
		
		/// <summary>
		/// Gets or sets the npc id.
		/// </summary>
		public uint NpcId { get; set; }
		
		/// <summary>
		/// Gets or sets the x coordinate.
		/// </summary>
		public ushort X { get; set; }
		
		/// <summary>
		/// Gets or sets the y coordinate.
		/// </summary>
		public ushort Y { get; set; }
		
		/// <summary>
		/// Gets or sets the mesh.
		/// </summary>
		public ushort Mesh { get; set; }
		
		/// <summary>
		/// Gets or sets the flag.
		/// </summary>
		public uint Flag { get; set; }
		
		/// <summary>
		/// Creates a new npc spawn packet.
		/// </summary>
		public NpcSpawnPacket()
			: base(24, 2030)
		{
			
		}
		
		/// <summary>
		/// Implicit conversion from the NpcSpawnPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](NpcSpawnPacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32(packet.ClientId);
			packet.WriteUInt32(packet.NpcId);
			packet.WriteUInt16(packet.X);
			packet.WriteUInt16(packet.Y);
			packet.WriteUInt16(packet.Mesh);
			packet.WriteUInt32(packet.Flag);
			packet.WriteStrings(packet.Name);
			
			return packet.Buffer;
		}
	}
}
