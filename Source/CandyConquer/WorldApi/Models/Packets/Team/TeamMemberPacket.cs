// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Team
{
	/// <summary>
	/// Model for the team member packet.
	/// </summary>
	public sealed class TeamMemberPacket : NetworkPacket
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
		/// Gets or sets the mesh.
		/// </summary>
		public uint Mesh { get; set; }
		
		/// <summary>
		/// Gets or sets the max hp.
		/// </summary>
		public int MaxHP { get; set; }
		
		/// <summary>
		/// Gets or sets the hp.
		/// </summary>
		public int HP { get; set; }
		
		/// <summary>
		/// Creates a new team member packet.
		/// </summary>
		public TeamMemberPacket()
			: base(160, Data.Constants.PacketTypes.TeamMemberPacket)
		{
		}
		
		/// <summary>
		/// Implicit conversion from the TeamMemberPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](TeamMemberPacket packet)
		{
			packet.Offset = 5;
			packet.WriteByte(1); // unknown
			
			packet.Offset = 8;
			packet.WriteStringWithReminder(packet.Name, 16);
			packet.WriteUInt32(packet.ClientId);
			packet.WriteUInt32(packet.Mesh);
			packet.WriteUInt16(100); // 100% max hp
			packet.WriteUInt16((ushort)(100 / (packet.MaxHP / packet.HP)));
			
			return packet.Buffer;
		}
	}
}
