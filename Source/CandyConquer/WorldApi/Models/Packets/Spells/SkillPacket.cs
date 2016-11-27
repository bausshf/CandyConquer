// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Spells
{
	/// <summary>
	/// Model for the skill packet.
	/// </summary>
	public sealed class SkillPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the experience.
		/// </summary>
		public uint Experience { get; set; }
		
		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		public ushort Id { get; set; }
		
		/// <summary>
		/// Gets or sets the level.
		/// </summary>
		public ushort Level { get; set; }
		
		/// <summary>
		/// Creates a new skill packet.
		/// </summary>
		public SkillPacket()
			: base(20, Data.Constants.PacketTypes.SendSpellPacket)
		{
			
		}
		
		/// <summary>
		/// Implicit conversion from the SkillPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](SkillPacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32(packet.Experience);
			packet.WriteUInt16(packet.Id);
			packet.WriteUInt16(packet.Level);
			
			return packet.Buffer;
		}
	}
}
