// Project by Bauss
using System;
using System.Collections.Generic;
using System.Linq;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Misc
{
	/// <summary>
	/// The string packet.
	/// </summary>
	public sealed class StringPacket : NetworkPacket
	{
		/// <summary>
		/// Gets the strings.
		/// </summary>
		public List<string> Strings { get; private set; }
		
		/// <summary>
		/// Gets or sets the string.
		/// </summary>
		public string String { get; set; }
		
		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		public uint Data { get; set; }

		/// <summary>
		/// Gets or sets the position x.
		/// </summary>
		public ushort PositionX
		{
			get { return (ushort)Data; }
			set { Data = (uint)((PositionY << 16) | value); }
		}

		/// <summary>
		/// Gets or sets the position y.
		/// </summary>
		public ushort PositionY
		{
			get { return (ushort)(Data >> 16); }
			set { Data = (uint)((value << 16) | PositionX); }
		}
		
		/// <summary>
		/// Gets or sets the action.
		/// </summary>
		public Enums.StringAction Action { get; set; }
		
		/// <summary>
		/// Creates a new string packet.
		/// </summary>
		public StringPacket()
			: base(9, CandyConquer.WorldApi.Data.Constants.PacketTypes.StringPacket)
		{
			Strings = new List<string>();
		}
		
		/// <summary>
		/// Creates a new string packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private StringPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			SubTypeOffset = 8;
			
			Data = ReadUInt32();
			Action = (Enums.StringAction)ReadByte();
			Strings = ReadStrings().ToList();
			
			SubTypeObject = Action;
		}
		
		/// <summary>
		/// Implicit conversion from the StringPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](StringPacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32(packet.Data);
			packet.WriteByte((byte)packet.Action);
			if (!string.IsNullOrWhiteSpace(packet.String))
			{
				packet.WriteStrings(packet.String);
			}
			else
			{
				packet.WriteStrings(packet.Strings.ToArray());
			}
			
			return packet.Buffer;
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to StringPacket.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		/// <returns>The packet.</returns>
		public static implicit operator StringPacket(SocketPacket packet)
		{
			return new StringPacket(packet);
		}
	}
}
