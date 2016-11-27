// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Spells
{
	/// <summary>
	/// Creates a new proficiency packet.
	/// </summary>
	public sealed class ProficiencyPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		public uint Id { get; set; }
		
		/// <summary>
		/// Gets or sets the level.
		/// </summary>
		public uint Level { get; set; }
		
		/// <summary>
		/// Gets or sets the experience.
		/// </summary>
		public uint Experience { get; set; }
		
		/// <summary>
		/// Gets or sets the required experience.
		/// </summary>
		public uint RequiredExperience { get; set; }
		
		/// <summary>
		/// Creates a new proficiency packet.
		/// </summary>
		public ProficiencyPacket()
			: base(20, Data.Constants.PacketTypes.SendProfPacket)
		{
		}
		
		/// <summary>
		/// Implicit conversion from the ProficiencyPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](ProficiencyPacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32(packet.Id);
			packet.WriteUInt32(packet.Level);
			packet.WriteUInt32(packet.Experience);
			packet.WriteUInt32(packet.RequiredExperience);
			
			return packet.Buffer;
		}
	}
}
