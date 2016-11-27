// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Client
{
	/// <summary>
	/// The data exchange packet.
	/// Also known as 'General Data'
	/// </summary>
	public sealed class DataExchangePacket : NetworkPacket
	{
		/// <summary>
		/// The client id of the 'object' associated with the packet.
		/// </summary>
		public uint ClientId { get; set; }
		/// <summary>
		/// The timestamp.
		/// </summary>
		public uint Timestamp { get; set; }
		/// <summary>
		/// The exchange type.
		/// </summary>
		public Enums.ExchangeType ExchangeType { get; set; }
		/// <summary>
		/// The direction.
		/// </summary>
		public Enums.Direction Direction { get; set; }
		
		#region Data - Data may be different depending on sub types.
		/// <summary>
		/// Data1.
		/// </summary>
		public uint Data1 { get; set; }
		
		/// <summary>
		/// Data2.
		/// </summary>
		public uint Data2 { get; set; }
		
		/// <summary>
		/// Data3.
		/// </summary>
		public uint Data3 { get; set; }
		
		/// <summary>
		/// Data4.
		/// </summary>
		public uint Data4 { get; set; }
		
		/// <summary>
		/// Data5.
		/// </summary>
		public uint Data5 { get; set; }
		
		/// <summary>
		/// Data6.
		/// </summary>
		public byte Data6 { get; set; }
		
		#region Data1
		/// <summary>
		/// Data1 16 first bits.
		/// </summary>
		public ushort Data1Low
		{
			get { return (ushort)Data1; }
			set { Data1 = (uint)((Data1High << 16) | value); }
		}

		/// <summary>
		/// Data1 16 last bits.
		/// </summary>
		public ushort Data1High
		{
			get { return (ushort)(Data1 >> 16); }
			set { Data1 = (uint)((value << 16) | Data1Low); }
		}

		#endregion

		#region Data2

		/// <summary>
		/// Data2 first 16 bits.
		/// </summary>
		public ushort Data2Low
		{
			get { return (ushort)Data2; }
			set { Data2 = (uint)((Data2High << 16) | value); }
		}

		/// <summary>
		/// Data2 last 16 bits.
		/// </summary>
		public ushort Data2High
		{
			get { return (ushort)(Data2 >> 16); }
			set { Data2 = (uint)((value << 16) | Data2Low); }
		}

		#endregion

		#region Data3

		/// <summary>
		/// Data3 first 16 bits.
		/// </summary>
		public ushort Data3Low
		{
			get { return (ushort)Data3; }
			set { Data3 = (uint)((Data3High << 16) | value); }
		}

		/// <summary>
		/// Data3 last 16 bits.
		/// </summary>
		public ushort Data3High
		{
			get { return (ushort)(Data3 >> 16); }
			set { Data3 = (uint)((value << 16) | Data3Low); }
		}

		#endregion

		#region Data4

		/// <summary>
		/// Data4 first 16 bits.
		/// </summary>
		public ushort Data4Low
		{
			get { return (ushort)Data4; }
			set { Data4 = (uint)((Data4High << 16) | value); }
		}

		/// <summary>
		/// Data4 last 16 bits.
		/// </summary>
		public ushort Data4High
		{
			get { return (ushort)(Data4 >> 16); }
			set { Data4 = (uint)((value << 16) | Data4Low); }
		}

		#endregion

		#region Data5

		/// <summary>
		/// Data5 first 16 bits.
		/// </summary>
		public ushort Data5Low
		{
			get { return (ushort)Data5; }
			set { Data5 = (uint)((Data5High << 16) | value); }
		}

		/// <summary>
		/// Data5 last 16 bits.
		/// </summary>
		public ushort Data5High
		{
			get { return (ushort)(Data5 >> 16); }
			set { Data5 = (uint)((value << 16) | Data5Low); }
		}

		#endregion
		#endregion
		
		/// <summary>
		/// Creates a new DataExchangePacket packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private DataExchangePacket(NetworkPacket packet)
			: base(packet, 4)
		{
			SubTypeOffset = 20;
			
			ClientId = ReadUInt32();
			Data1 = ReadUInt32();
			Data2 = ReadUInt32();
			Timestamp = ReadUInt32();
			ExchangeType = (Enums.ExchangeType)ReadUInt16();
			Direction = (Enums.Direction)ReadUInt16();
			Data3 = ReadUInt32();
			Data4 = ReadUInt32();
			Data5 = ReadUInt32();
			Data6 = ReadByte();
			
			SubTypeObject = ExchangeType;
		}
		
		public DataExchangePacket()
			: base(37, 10010)
		{
			
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to DataExchangePacket packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator DataExchangePacket(SocketPacket packet)
		{
			return new DataExchangePacket(packet);
		}
		
		/// <summary>
		/// Implicit conversion from the DataExchangePacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](DataExchangePacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32(packet.ClientId);
			packet.WriteUInt32(packet.Data1);
			packet.WriteUInt32(packet.Data2);
			packet.WriteUInt32(packet.Timestamp);
			packet.WriteUInt16((ushort)packet.ExchangeType);
			packet.WriteUInt16((ushort)packet.Direction);
			packet.WriteUInt32(packet.Data3);
			packet.WriteUInt32(packet.Data4);
			packet.WriteUInt32(packet.Data5);
			packet.WriteByte(packet.Data6);
			
			return packet.Buffer;
		}
	}
}
