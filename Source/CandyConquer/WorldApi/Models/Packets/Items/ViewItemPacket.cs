// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Items
{
	/// <summary>
	/// Model for the view item packet.
	/// </summary>
	public sealed class ViewItemPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the owner client id.
		/// </summary>
		public uint OwnerClientId { get; set; }
		
		/// <summary>
		/// Gets or sets the item.
		/// </summary>
		public Models.Items.Item Item { get; set; }
		
		/// <summary>
		/// Gets or sets the price.
		/// </summary>
		public uint Price { get; set; }
		
		/// <summary>
		/// Gets or sets the cp price.
		/// </summary>
		public bool CpPrice { get; set; }
		
		/// <summary>
		/// Creates a new view item packet.
		/// </summary>
		public ViewItemPacket()
			: base(84, 1108)
		{
		}
		
		/// <summary>
		/// Implicit conversion from the ViewItemPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](ViewItemPacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32(packet.Item.ClientId);
			packet.WriteUInt32(packet.OwnerClientId);
			packet.WriteUInt32(packet.Price);
			packet.WriteUInt32((uint)packet.Item.DbItem.Id);
			packet.WriteUInt16((ushort)packet.Item.DbOwnerItem.CurrentDura);
			packet.WriteUInt16((ushort)packet.Item.DbOwnerItem.MaxDura);
			if (packet.Price > 0)
			{
				packet.WriteUInt16((ushort)(packet.CpPrice ? 3 : 1));
			}
			else
			{
				packet.WriteUInt16(4);
			}
			packet.WriteUInt16((ushort)packet.Item.Position);
			packet.WriteUInt32(0); // unknown
			packet.WriteByte((byte)packet.Item.Gem1);
			packet.WriteByte((byte)packet.Item.Gem2);
			packet.WriteUInt32(0); // unknown
			packet.WriteUInt16(0); // unknown
			packet.WriteByte(0); // unknown
			packet.WriteByte(packet.Item.DbOwnerItem.Plus);
			packet.WriteByte(packet.Item.DbOwnerItem.Bless);
			packet.WriteBool(packet.Item.DbOwnerItem.Free);
			packet.WriteByte(packet.Item.DbOwnerItem.Enchant);
			packet.Offset = 53;
			packet.WriteBool(packet.Item.DbOwnerItem.Suspicious);
			packet.Offset = 56;
			packet.WriteByte((byte)packet.Item.Color);
			packet.Offset = 60;
			packet.WriteUInt32(packet.Item.DbOwnerItem.Composition);
			
			return packet.Buffer;
		}
	}
}
