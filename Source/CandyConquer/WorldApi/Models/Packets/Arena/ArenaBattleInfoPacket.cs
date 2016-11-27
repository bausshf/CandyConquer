// Project by Bauss
using System;
using System.Collections.Generic;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Arena
{
	/// <summary>
	/// Model for the arena battle info packet.
	/// </summary>
	public sealed class ArenaBattleInfoPacket : NetworkPacket
	{
		/// <summary>
		/// Gets the page.
		/// </summary>
		public uint Page { get; private set; }
		
		/// <summary>
		/// Creates a new arena battle info packet.
		/// </summary>
		public ArenaBattleInfoPacket()
			: base(24, Data.Constants.PacketTypes.ArenaBattleInfoPacket)
		{
			Page = 1;
		}
		
		/// <summary>
		/// Creates a new arena battle info packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private ArenaBattleInfoPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			Page = ReadUInt32();
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to ArenaBattleInfoPacket packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator ArenaBattleInfoPacket(SocketPacket packet)
		{
			return new ArenaBattleInfoPacket(packet);
		}
		
		/// <summary>
		/// Implicit conversion from the ArenaBattleInfoPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](ArenaBattleInfoPacket packet)
		{
			packet.Offset = 4;
			
			var matches = Controllers.Arena.ArenaQualifierController.GetMatches();
			
			packet.WriteUInt32(packet.Page);
			packet.WriteUInt32(0x6);
			packet.WriteUInt32((uint)matches.Length);
			packet.WriteUInt32((uint)Controllers.Arena.ArenaQualifierController.PlayerQueueCount);
			packet.WriteUInt32(0); // unknown
			packet.Page--;
			packet.WriteUInt32((uint)(matches.Length - packet.Page));
			
			int max = (int)Math.Min(matches.Length, packet.Page + 6);
			packet.Expand(152 * max);
			
			for (int count = (int)packet.Page; count < max; count++)
			{
				var match = matches[count];
				var player1 = match.Player1.Player;
				var player2 = match.Player2.Player;
				
				int offset = packet.Offset;
				
				packet.WriteUInt32(player1.ClientId);
				packet.WriteUInt32(player1.Mesh);
				packet.WriteStringWithReminder(player1.Name, 16);
				packet.WriteUInt32(player1.Level);
				packet.WriteUInt32((uint)player1.Job);
				packet.WriteUInt32(0); // unknown
				packet.WriteUInt32(player1.ArenaInfo.Ranking);
				packet.WriteUInt32(player1.ArenaInfo.DbPlayerArenaQualifier.ArenaPoints);
				packet.WriteUInt32(player1.ArenaInfo.DbPlayerArenaQualifier.TotalWinsToday);
				packet.WriteUInt32(player1.ArenaInfo.DbPlayerArenaQualifier.TotalLossToday);
				packet.WriteUInt32(player1.ArenaInfo.DbPlayerArenaQualifier.HonorPoints);
				// total honor, but by design choice honor points are a currency.
				packet.WriteUInt32(player1.ArenaInfo.DbPlayerArenaQualifier.HonorPoints);
				
				packet.WriteUInt32(player2.ClientId);
				packet.WriteUInt32(player2.Mesh);
				packet.WriteStringWithReminder(player2.Name, 16);
				packet.WriteUInt32(player2.Level);
				packet.WriteUInt32((uint)player2.Job);
				packet.WriteUInt32(0); // unknown
				packet.WriteUInt32(player2.ArenaInfo.Ranking);
				packet.WriteUInt32(player2.ArenaInfo.DbPlayerArenaQualifier.ArenaPoints);
				packet.WriteUInt32(player2.ArenaInfo.DbPlayerArenaQualifier.TotalWinsToday);
				packet.WriteUInt32(player2.ArenaInfo.DbPlayerArenaQualifier.TotalLossToday);
				packet.WriteUInt32(player2.ArenaInfo.DbPlayerArenaQualifier.HonorPoints);
				// total honor, but by design choice honor points are a currency.
				packet.WriteUInt32(player2.ArenaInfo.DbPlayerArenaQualifier.HonorPoints);
				
				if (count < (max - 1))
				{
					var size = (packet.Offset - offset);
					packet.Offset = (152 - size);
				}
            }
				
			return packet.Buffer;
		}
	}
}
