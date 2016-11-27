// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Arena
{
	/// <summary>
	/// Model for the arena statistic packet.
	/// </summary>
	public sealed class ArenaStatisticPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the player.
		/// </summary>
		public Models.Entities.Player Player { get; set; }
		
		/// <summary>
		/// Creates a new arena statistic packet.
		/// </summary>
		public ArenaStatisticPacket()
			: base(52, Data.Constants.PacketTypes.ArenaStatisticPacket)
		{
		}
		
		/// <summary>
		/// Creates a new arena statistic packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private ArenaStatisticPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to ArenaStatisticPacket packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator ArenaStatisticPacket(SocketPacket packet)
		{
			return new ArenaStatisticPacket(packet);
		}
		
		/// <summary>
		/// Implicit conversion from the ArenaStatisticPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](ArenaStatisticPacket packet)
		{
			packet.Offset = 4;
			
			var arenaInfo = packet.Player.ArenaInfo;
			
			packet.WriteUInt32(arenaInfo.Ranking + 1);
			packet.WriteUInt32(0); // unknown
			packet.WriteUInt32((uint)arenaInfo.Status);
			packet.WriteUInt32(arenaInfo.DbPlayerArenaQualifier.TotalWins);
			packet.WriteUInt32(arenaInfo.DbPlayerArenaQualifier.TotalLoss);
			packet.WriteUInt32(arenaInfo.DbPlayerArenaQualifier.TotalWinsToday);
			packet.WriteUInt32(arenaInfo.DbPlayerArenaQualifier.TotalLossToday);
			// total honor, but by design choice honor points are a currency
			packet.WriteUInt32(arenaInfo.DbPlayerArenaQualifier.HonorPoints);
			packet.WriteUInt32(arenaInfo.DbPlayerArenaQualifier.HonorPoints);
			packet.WriteUInt32(arenaInfo.DbPlayerArenaQualifier.ArenaPoints);
			packet.WriteUInt32(arenaInfo.DbPlayerArenaQualifier.TotalWinsSeason);
			packet.WriteUInt32(arenaInfo.DbPlayerArenaQualifier.TotalLossSeason);
			
			return packet.Buffer;
		}
	}
}
