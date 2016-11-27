// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Arena
{
	/// <summary>
	/// Model for the arena match packet.
	/// </summary>
	public sealed class ArenaMatchPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the match.
		/// </summary>
		public Models.Arena.ArenaBattle Match { get; set; }
		
		/// <summary>
		/// Creates a new arena match packet.
		/// </summary>
		public ArenaMatchPacket()
			: base(56, Data.Constants.PacketTypes.ArenaMatchPacket)
		{
		}
		
		/// <summary>
		/// Implicit conversion from the ArenaMatchPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](ArenaMatchPacket packet)
		{
			packet.Offset = 4;
			
			var player1 = packet.Match.Player1;
			var player2 = packet.Match.Player2;
			
			packet.WriteUInt32(player1.Player.ClientId);
			packet.WriteStringWithReminder(player1.Player.Name, 16);
			packet.WriteUInt32(player1.Damage);
			
			packet.WriteUInt32(player2.Player.ClientId);
			packet.WriteStringWithReminder(player2.Player.Name, 16);
			packet.WriteUInt32(player2.Damage);
			
			return packet.Buffer;
		}
	}
}
