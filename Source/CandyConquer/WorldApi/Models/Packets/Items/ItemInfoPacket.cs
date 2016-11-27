// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Items
{
	/// <summary>
	/// Model for the item info packet.
	/// </summary>
	public sealed class ItemInfoPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the item.
		/// </summary>
		public Models.Items.Item Item { get; set; }
		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Enums.ItemPosition Position { get; set; }
		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		public ushort Data { get; set; }
		/// <summary>
		/// Gets or sets an unknown value.
		/// </summary>
		public ushort Unknown { get; set; }
		/// <summary>
		/// Gets or sets the green text.
		/// </summary>
		public uint GreenText { get; set; }
		
		/// <summary>
		/// Creates a new ItemInfoPacket packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		public ItemInfoPacket()
			: base(68, 1008)
		{
		}
		
		/// <summary>
		/// Implicit conversion from the ItemInfoPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](ItemInfoPacket packet)
		{
			var item = packet.Item;
			packet.Offset = 4;
			
			packet.WriteUInt32(item.ClientId);
			packet.WriteUInt32((uint)item.DbItem.Id);
			packet.WriteUInt16((ushort)item.DbOwnerItem.CurrentDura);
			packet.WriteUInt16((ushort)item.DbOwnerItem.MaxDura);
			packet.WriteUInt16(packet.Data);
			packet.WriteUInt16((ushort)packet.Position);
			packet.WriteUInt32(item.DbOwnerItem.SocketRGB);
			packet.WriteByte((byte)item.Gem1);
			packet.WriteByte((byte)item.Gem2);
			packet.WriteUInt16((ushort)packet.Unknown);
			packet.WriteUInt16((ushort)item.DbOwnerItem.RebornEffect);
			packet.Offset = 33;
			packet.WriteByte(item.DbOwnerItem.Plus);
			packet.WriteByte(item.DbOwnerItem.Bless);
			packet.WriteBool(item.DbOwnerItem.Free);
			packet.WriteByte(item.DbOwnerItem.Enchant);
			packet.Offset = 40;
			packet.WriteUInt32(packet.GreenText);
			packet.WriteBool(item.DbOwnerItem.Suspicious);
			packet.WriteByte(0);//skip one ...
			packet.WriteBool(item.DbOwnerItem.Locked);
			packet.WriteByte(0); // skip one ...
			packet.WriteByte((byte)item.Color);
			packet.Offset = 52;
			packet.WriteUInt32(item.DbOwnerItem.Composition);
			packet.WriteUInt32(0); // INS
			packet.WriteUInt32(0); // lock time
			packet.WriteUInt32(item.DbOwnerItem.Amount);
			/*
byte plus 33
byte bless 34
bool free 35
byte enchant 36
uint greentext 40
bool suspicious 44
bool locked 46
byte color 48
uint composition 52
uint ins 56
uint locktime 60
ushort amount 64*/			/*uint uid 4
uint itemid 8
ushort dura 12
ushort maxdura 14
ushort data 16
ushort location/position 18
uint composition 20
byte gem1 24
byte2 gem2 25
ushort unknown2 26
ushort reborn 28*/
			
			return packet.Buffer;
		}
	}
}
