// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Arena
{
	/// <summary>
	/// Model for the arena action packet.
	/// </summary>
	public sealed class ArenaActionPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the dialog.
		/// </summary>
		public Enums.ArenaDialog Dialog { get; set; }
		
		/// <summary>
		/// Gets or sets the option.
		/// </summary>
		public Enums.ArenaOption Option { get; set; }
		
		/// <summary>
		/// Gets or sets the client id.
		/// </summary>
		public uint ClientId { get; set; }
		
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// Gets or sets the rank.
		/// </summary>
		public uint Rank { get; set; }
		
		/// <summary>
		/// Gets or sets the unknown.
		/// </summary>
		public uint Unknown { get; set; }
		
		/// <summary>
		/// Gets or sets the job.
		/// </summary>
		public Enums.Job Job { get; set; }
		
		/// <summary>
		/// Gets or sets the arena points.
		/// </summary>
		public uint ArenaPoints { get; set; }
		
		/// <summary>
		/// Gets or sets the level.
		/// </summary>
		public byte Level { get; set; }
		
		/// <summary>
		/// Creates a new arena action packet.
		/// </summary>
		public ArenaActionPacket()
			: base(56, Data.Constants.PacketTypes.ArenaActionPacket)
		{
		}
		
		/// <summary>
		/// Creates a new arena action packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private ArenaActionPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			SubTypeOffset = 4;
			
			Dialog = (Enums.ArenaDialog)ReadUInt32();
			Option = (Enums.ArenaOption)ReadUInt32();
			ClientId = ReadUInt32();
			Name = ReadString(16);
			Rank = ReadUInt32();
			Job = (Enums.Job)ReadUInt32();
			ReadUInt32(); // unknown
			ArenaPoints = ReadUInt32();
			Level = (byte)ReadUInt32();
			
			SubTypeObject = Dialog;
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to ArenaActionPacket packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator ArenaActionPacket(SocketPacket packet)
		{
			return new ArenaActionPacket(packet);
		}
		
		/// <summary>
		/// Implicit conversion from the ArenaActionPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](ArenaActionPacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32((uint)packet.Dialog);
			packet.WriteUInt32((uint)packet.Option);
			packet.WriteUInt32(packet.ClientId);
			packet.WriteStringWithReminder(packet.Name, 16);
			packet.WriteUInt32(packet.Rank);
			packet.WriteUInt32((uint)packet.Job);
			packet.WriteUInt32(packet.Unknown); // unknown
			packet.WriteUInt32(packet.ArenaPoints);
			packet.WriteUInt32(packet.Level);
			
			return packet.Buffer;
		}
	}
}
