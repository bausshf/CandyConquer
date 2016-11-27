// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Items
{
	/// <summary>
	/// Model for the warehouse packet.
	/// </summary>
	public sealed class WarehousePacket : NetworkPacket
	{
		/// <summary>
		/// The warehouse id.
		/// </summary>
		public uint WarehouseId { get; set; }
		
		/// <summary>
		/// The action.
		/// </summary>
		public Enums.WarehouseAction Action { get; set; }
		
		/// <summary>
		/// The warehouse type.
		/// </summary>
		public byte WhType { get; set; }
		
		/// <summary>
		/// The client id.
		/// </summary>
		public uint ClientId { get; set; }
		
		/// <summary>
		/// The item.
		/// </summary>
		public Models.Items.Item Item { get; set; }
		
		/// <summary>
		/// Creates a new warehouse packet.
		/// </summary>
		public WarehousePacket()
			: base(24 + (72 * 1), Data.Constants.PacketTypes.WarehousePacket)
		{
			Action = Enums.WarehouseAction.Add;
			WhType = 20;
		}
		
		/// <summary>
		/// Creates a new warehouse packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private WarehousePacket(NetworkPacket packet)
			: base(packet, 4)
		{
			SubTypeObject = 8;
			
			WarehouseId = ReadUInt32();
			Action = (Enums.WarehouseAction)ReadByte();
			WhType = ReadByte();
			ReadUInt16(); // unknown
			ReadUInt32(); // unknown
			ClientId = ReadUInt32();
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to WarehousePacket packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator WarehousePacket(SocketPacket packet)
		{
			return new WarehousePacket(packet);
		}
		
		/// <summary>
		/// Implicit conversion from the WarehousePacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](WarehousePacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32(packet.WarehouseId); // 4
			packet.WriteByte((byte)packet.Action); // 8
			packet.WriteByte(packet.WhType); // 9
			packet.WriteUInt16(0); // unknown // 10
			packet.WriteUInt32(0); // unknown // 12
			packet.WriteUInt32(packet.ClientId); // 16
			
			var item = packet.Item;
			if (item != null)
			{
				packet.WriteByte(1); // 20
				packet.WriteByte(0); // padding ??
				packet.WriteByte(0);
				packet.WriteByte(0);
				packet.WriteUInt32(item.ClientId); // 24
				packet.WriteUInt32((uint)item.DbItem.Id); // 28
				packet.WriteByte(0); // unknown 32
				packet.WriteByte((byte)item.Gem1); // 33
				packet.WriteByte((byte)item.Gem2); // 34
				// dura somewhere here ??
				packet.Offset = 41;
				packet.WriteByte(item.DbOwnerItem.Plus);
				packet.WriteByte(item.DbOwnerItem.Bless);
				packet.WriteBool(item.DbOwnerItem.Free);
				packet.WriteUInt16(item.DbOwnerItem.Enchant);// 44
				packet.WriteUInt16(item.DbOwnerItem.RebornEffect); // 46
				packet.WriteBool(item.DbOwnerItem.Locked); // 48
				packet.WriteBool(item.DbOwnerItem.Suspicious); // 49
				packet.WriteByte(0); // unknown, padding maybe ??
				packet.WriteByte((byte)item.Color); // 51
				packet.WriteUInt32(item.DbOwnerItem.SocketRGB); // 52
				packet.WriteUInt32(item.DbOwnerItem.Composition); // 56
			}
            
			return packet.Buffer;
		}
	}
}
