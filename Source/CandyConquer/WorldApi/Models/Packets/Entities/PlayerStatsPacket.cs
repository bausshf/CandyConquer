// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Entities
{
	/// <summary>
	/// The player stats packet.
	/// </summary>
	public sealed class PlayerStatsPacket : NetworkPacket
	{
		/// <summary>
		/// The player.
		/// </summary>
		public Models.Entities.Player Player { get; set; }
		
		/// <summary>
		/// The client id.
		/// </summary>
		public uint ClientId { get; set; }
		
		/// <summary>
		/// Creates a new player stats packet.
		/// </summary>
		public PlayerStatsPacket()
			: base(136, 1040)
		{
			
		}
		
		/// <summary>
		/// Creates a new player stats packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private PlayerStatsPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			ClientId = ReadUInt32();
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to PlayerStatsPacket packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator PlayerStatsPacket(SocketPacket packet)
		{
			return new PlayerStatsPacket(packet);
		}
		
		/// <summary>
		/// Implicit conversion from the PlayerStatsPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](PlayerStatsPacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32(packet.Player.ClientId);
			packet.WriteUInt32((uint)packet.Player.MaxHP);
			packet.WriteUInt32((uint)packet.Player.MaxMP);
			packet.WriteUInt32((uint)packet.Player.MaxAttack);
			packet.WriteUInt32((uint)packet.Player.MinAttack);
			packet.WriteUInt32((uint)packet.Player.Defense);
			packet.WriteUInt32((uint)packet.Player.MagicAttack);
			packet.WriteUInt32((uint)packet.Player.MagicDefense);
			packet.WriteUInt32((uint)packet.Player.Dodge);
			packet.WriteUInt32(packet.Player.Agility);
			packet.WriteUInt32((uint)packet.Player.Accuracy);
			packet.WriteUInt32((uint)(packet.Player.DragonGemPercentage * 100));
			packet.WriteUInt32((uint)(packet.Player.PhoenixGemPercentage * 100));
			packet.WriteUInt32((uint)(packet.Player.MagicDefensePercentage * 100));
			
			packet.Offset = 64;
			
			packet.WriteUInt32((uint)(packet.Player.Bless * 100));
			packet.WriteUInt32((uint)packet.Player.CriticalStrike);
			packet.WriteUInt32((uint)packet.Player.SkillCriticalStrike);
			packet.WriteUInt32((uint)packet.Player.Immunity);
			packet.WriteUInt32((uint)packet.Player.Penetration);
			packet.WriteUInt32((uint)packet.Player.Block);
			packet.WriteUInt32((uint)packet.Player.BreakThrough);
			packet.WriteUInt32((uint)packet.Player.CounterAction);
			packet.WriteUInt32((uint)packet.Player.Detoxication);
			packet.WriteUInt32((uint)packet.Player.FinalPhysicalDamage);
			packet.WriteUInt32((uint)packet.Player.FinalMagicDamage);
			packet.WriteUInt32((uint)packet.Player.FinalPhysicalDefense);
			packet.WriteUInt32((uint)packet.Player.FinalMagicDefense);
			packet.WriteUInt32((uint)packet.Player.MetalDefense);
			packet.WriteUInt32((uint)packet.Player.WoodDefense);
			packet.WriteUInt32((uint)packet.Player.WaterDefense);
			packet.WriteUInt32((uint)packet.Player.FireDefense);
			packet.WriteUInt32((uint)packet.Player.EarthDefense);
			
			return packet.Buffer;
		}
	}
}
