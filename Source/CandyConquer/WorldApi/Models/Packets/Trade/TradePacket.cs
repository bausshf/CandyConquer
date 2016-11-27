// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Trade
{
	/// <summary>
	/// Model for the packet.
	/// </summary>
	public class TradePacket : NetworkPacket
	{
		/// <summary>
		/// The trade target client id.
		/// </summary>
		public uint TargetClientId { get; set; }
		
		/// <summary>
		/// The action.
		/// </summary>
		public Enums.TradeAction Action { get; set; }
		
		/// <summary>
		/// Creates a new trade packet.
		/// </summary>
		public TradePacket()
			: base(20, Data.Constants.PacketTypes.TradePacket)
		{
		}
		
		/// <summary>
		/// Creates a new trade packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private TradePacket(NetworkPacket packet)
			: base(packet, 4)
		{
			TargetClientId = ReadUInt32();
			Action = (Enums.TradeAction)ReadUInt32();
			
			SubTypeObject = Action;
		}
		
		/// <summary>
		/// Implicit conversion from the TradePacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](TradePacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32(packet.TargetClientId);
			packet.WriteUInt32((uint)packet.Action);
			
			return packet.Buffer;
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to TradePacket.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The packet.</returns>
		public static implicit operator TradePacket(SocketPacket packet)
		{
			return new TradePacket(packet);
		}
	}
}
