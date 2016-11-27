// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Entities
{
	/// <summary>
	/// The model for the npc response packet.
	/// </summary>
	public sealed class NpcResponsePacket : NetworkPacket
	{
		/// <summary>
		/// Gets the input data of the response.
		/// </summary>
		public string InputData { get; private set; }
		
		/// <summary>
		/// Creates a new npc response packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private NpcResponsePacket(NetworkPacket packet)
			: base(packet, 4)
		{
			Offset = 10;
			Option = ReadByte();
			Action = (Enums.NpcDialogAction)ReadByte();
			
			var strings = ReadStrings();
			if (strings.Length >= 1)
			{
				InputData = strings[0];
			}
		}
		
		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		public string Text { get; set; }
		
		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		public ushort Data { get; set; }
		
		/// <summary>
		/// Gets or sets the option.
		/// </summary>
		public byte Option { get; set; }
		
		/// <summary>
		/// Gets or sets the action.
		/// </summary>
		public Enums.NpcDialogAction Action { get; set; }
		
		/// <summary>
		/// Creates a new npc response packet.
		/// </summary>
		public NpcResponsePacket()
			: base(12, 2032)
		{
			
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to NpcResponsePacket packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator NpcResponsePacket(SocketPacket packet)
		{
			return new NpcResponsePacket(packet);
		}
		
		/// <summary>
		/// Implicit conversion from the NpcResponsePacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](NpcResponsePacket packet)
		{
			packet.Offset = 8;
			
			packet.WriteUInt16(packet.Data);
			packet.WriteByte(packet.Option);
			packet.WriteByte((byte)packet.Action);
			packet.WriteStrings(packet.Text);
			
			return packet.Buffer;
		}
	}
}
