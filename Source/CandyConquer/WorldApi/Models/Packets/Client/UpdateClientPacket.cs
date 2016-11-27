// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Client
{
	/// <summary>
	/// The update client packet.
	/// </summary>
	public sealed class UpdateClientPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the client id.
		/// </summary>
		public uint ClientId { get; set; }
		
		/// <summary>
		/// Gets or sets the update type.
		/// </summary>
		public Enums.UpdateClientType UpdateType { get; set; }
		
		/// <summary>
		/// Gets or sets the value associated with the update.
		/// For signed values, simply cast.
		/// </summary>
		public ulong Value { get; set; }
		
		/// <summary>
		/// Creates a new update client packet.
		/// </summary>
		public UpdateClientPacket()
			: base(12 + 8 + 8, 10017)
		{
			
		}
		
		/// <summary>
		/// Implicit conversion from the update client to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](UpdateClientPacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32(packet.ClientId);
			packet.WriteUInt32(1);
			packet.WriteUInt32((uint)packet.UpdateType);
			packet.WriteUInt64(packet.Value);
			
			return packet.Buffer;
		}
	}
}
