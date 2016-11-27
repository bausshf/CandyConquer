// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Message
{
	/// <summary>
	/// The message packet.
	/// </summary>
	public sealed class MessagePacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the color of the message.
		/// </summary>
		public uint Color { get; set; }
		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		public Enums.MessageType MessageType { get; set; }
		/// <summary>
		/// Gets or sets the timestamp.
		/// </summary>
		public uint Timestamp { get; set; }
		/// <summary>
		/// Gets or sets the receiver mesh.
		/// </summary>
		public uint ToMesh { get; set; }
		/// <summary>
		/// Gets or sets the sender mesh.
		/// </summary>
		public uint FromMesh { get; set; }
		/// <summary>
		/// Gets or sets the receiver.
		/// </summary>
		public string From { get; set; }
		/// <summary>
		/// Gets or sets the sender.
		/// </summary>
		public string To { get; set; }
		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		public string Message { get; set; }
		
		/// <summary>
		/// Creates a new message packet.
		/// </summary>
		public MessagePacket()
				: base(24, 1004)
		{
			SubTypeOffset = 8;
		}
		
		/// <summary>
		/// Creates a new message packet based on a network packet.
		/// </summary>
		/// <param name="packet">The packet to inherit.</param>
		private MessagePacket(NetworkPacket packet)
			: base(packet, 4)
		{
			SubTypeOffset = 8;
			
			Color = packet.ReadUInt32();
			MessageType = (Enums.MessageType)packet.ReadUInt32();
			Timestamp = packet.ReadUInt32();
			ToMesh = packet.ReadUInt32();
			FromMesh = packet.ReadUInt32();
			var strings = packet.ReadStrings();
			From = strings[0];
			To = strings[1];
			Message = strings[3];
			
			SubTypeObject = MessageType;
		}
		
		/// <summary>
		/// Implicit conversion from message packet to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](MessagePacket packet)
		{
			packet.Offset = 4;
			packet.WriteUInt32(packet.Color);
			packet.WriteUInt32((uint)packet.MessageType);
			packet.WriteUInt32(packet.Timestamp);
			packet.WriteUInt32(packet.ToMesh);
			packet.WriteUInt32(packet.FromMesh);
			packet.WriteStrings(packet.From, packet.To, string.Empty, packet.Message);
			
			return packet.Buffer;
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to message packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator MessagePacket(SocketPacket packet)
		{
			return new MessagePacket(packet);
		}
	}
}
