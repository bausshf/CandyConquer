// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Login
{
	/// <summary>
	/// The character creation packet.
	/// </summary>
	public sealed class CharacterCreationPacket : NetworkPacket
	{
		/// <summary>
		/// Gets the name.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets the model.
		/// </summary>
		public ushort Model { get; private set; }
		/// <summary>
		/// Gets the job.
		/// </summary>
		public Enums.Job Job { get; private set; }
		/// <summary>
		/// Gets the client id.
		/// </summary>
		public uint ClientId { get; private set; }
		
		/// <summary>
		/// Creates a new character creation packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private CharacterCreationPacket(NetworkPacket packet)
			: base(packet, 20)
		{
			Name = ReadString(16);
			Offset = 72;
			Model = ReadUInt16();
			Job = (Enums.Job)ReadUInt16();
			ClientId = ReadUInt32();
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to character creation packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator CharacterCreationPacket(SocketPacket packet)
		{
			return new CharacterCreationPacket(packet);
		}
	}
}
