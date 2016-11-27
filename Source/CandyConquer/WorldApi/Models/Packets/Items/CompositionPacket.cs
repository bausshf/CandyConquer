// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Items
{
	/// <summary>
	/// Model for the composition packet.
	/// </summary>
	public sealed class CompositionPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the action.
		/// </summary>
		public Enums.CompositionAction Action { get; set; }
		
		/// <summary>
		/// Gets or sets the main item's client id.
		/// </summary>
		public uint MainItemClientId { get; set; }
		
		/// <summary>
		/// Gets or sets the minor item's client id.
		/// </summary>
		public uint MinorItemClientId { get; set; }
		
		/// <summary>
		/// Creates a new composition packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private CompositionPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			Action = (Enums.CompositionAction)ReadUInt32();
			MainItemClientId = ReadUInt32();
			MinorItemClientId = ReadUInt32();
			
			SubTypeObject = Action;
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to CompositionPacket packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator CompositionPacket(SocketPacket packet)
		{
			return new CompositionPacket(packet);
		}
	}
}
