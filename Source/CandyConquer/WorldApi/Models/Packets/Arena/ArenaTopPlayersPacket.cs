// Project by Bauss
using System;
using CandyConquer.Drivers;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Arena
{
	/// <summary>
	/// Model for the arena top players packet.
	/// </summary>
	public sealed class ArenaTopPlayersPacket : NetworkPacket
	{
		/// <summary>
		/// Creates a new arena top players packet.
		/// </summary>
		public ArenaTopPlayersPacket()
			: base(8, Data.Constants.PacketTypes.ArenaTopPlayersPacket)
		{
		}
		
		/// <summary>
		/// Creates a new top players packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private ArenaTopPlayersPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to ArenaTopPlayersPacket packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator ArenaTopPlayersPacket(SocketPacket packet)
		{
			return new ArenaTopPlayersPacket(packet);
		}
		
		/// <summary>
		/// Implicit conversion from the ArenaTopPlayersPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](ArenaTopPlayersPacket packet)
		{
			packet.Offset = 4;
		
			var topPlayers = Collections.ArenaQualifierCollection.GetTop10();
			packet.WriteUInt32((uint)topPlayers.Count);
			packet.Expand(52 * topPlayers.Count);
			
			foreach (var topPlayer in topPlayers)
			{
				packet.WriteUInt32((uint)topPlayer.DbPlayerArenaQualifier.Id);
				packet.WriteStringWithReminder(topPlayer.DbPlayerArenaQualifier.Name, 16);
				packet.WriteUInt32(topPlayer.DbPlayerArenaQualifier.Mesh);
				packet.WriteUInt32(topPlayer.DbPlayerArenaQualifier.Level);
				packet.WriteUInt32((uint)topPlayer.DbPlayerArenaQualifier.Job.ToEnum<Enums.Job>());
				packet.WriteUInt32(topPlayer.Ranking + 1);
				packet.WriteUInt32(topPlayer.Ranking + 1);
				packet.WriteUInt32(topPlayer.Ratio);
				packet.WriteUInt32(topPlayer.Ranking + 1);
				packet.WriteUInt32(topPlayer.Ranking + 1);
			}
			
			return packet.Buffer;
		}
	}
}
