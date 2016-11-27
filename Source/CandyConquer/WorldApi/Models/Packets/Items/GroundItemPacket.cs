// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Items
{
	/// <summary>
	/// The ground item packet.
	/// </summary>
	public sealed class GroundItemPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the client id.
		/// </summary>
		public uint ClientId { get; set; }
		/// <summary>
		/// Gets or sets the item id.
		/// </summary>
		public uint ItemId { get; set; }
		/// <summary>
		/// Gets or sets the x coordinate.
		/// </summary>
		public ushort X { get; set; }
		/// <summary>
		/// Gets or sets the y coordinate.
		/// </summary>
		public ushort Y { get; set; }
		/// <summary>
		/// Gets or sets the action.
		/// </summary>
		public Enums.GroundItemAction Action { get; set; }
		/// <summary>
		/// Gets or sets a boolean determining whether its a removal or not.
		/// </summary>
		public bool Remove { get; set; }
		
		/// <summary>
		/// Creates a new GroundItemPacket packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private GroundItemPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			ClientId = ReadUInt32();
			ItemId = ReadUInt32();
			X = ReadUInt16();
			Y = ReadUInt16();
			Action = (Enums.GroundItemAction)ReadUInt16();
			Remove = ReadUInt16() == 2;
		}
		
		/// <summary>
		/// Creates a new ground item packet.
		/// </summary>
		public GroundItemPacket()
			: base(32, 1101)
		{
			Action = Enums.GroundItemAction.Add;
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to GroundItemPacket packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator GroundItemPacket(SocketPacket packet)
		{
			return new GroundItemPacket(packet);
		}
		
		/// <summary>
		/// Implicit conversion from the GroundItemPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](GroundItemPacket packet)
		{
			packet.WriteUInt32(packet.ClientId);
			packet.WriteUInt32(packet.ItemId);
			packet.WriteUInt16(packet.X);
			packet.WriteUInt16(packet.Y);
			packet.WriteUInt16((ushort)packet.Action);
			packet.WriteUInt16((ushort)(packet.Remove ? 2 : 1));
			
			return packet.Buffer;
		}
	}
}
