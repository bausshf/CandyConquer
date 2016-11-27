// Project by Bauss
using System;
using System.Collections.Generic;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Spells
{
	/// <summary>
	/// Model for the spell packet.
	/// </summary>
	public class SpellPacket : NetworkPacket
	{
		/// <summary>
		/// Model for a spell target.
		/// </summary>
		public class SpellTarget
		{
			/// <summary>
			/// Gets or sets the associated entity. (Not written to the packet.)
			/// </summary>
			public Controllers.Entities.AttackableEntityController AssociatedEntity { get; set; }
			
			/// <summary>
			/// Gets or sets the client id.
			/// </summary>
			public uint ClientId { get; set; }
			
			/// <summary>
			/// Gets or sets the damage.
			/// </summary>
			public uint Damage { get; set; }
			
			/// <summary>
			/// Gets or sets the hit.
			/// </summary>
			public bool Hit { get; set; }
			
			/// <summary>
			/// Gets or sets the activation type.
			/// </summary>
			public uint ActivationType { get; set; }
			
			/// <summary>
			/// Gets or sets the activation value.
			/// </summary>
			public uint ActivationValue { get; set; }
		}
		
		/// <summary>
		/// Gets or sets a boolean determining whether spell is safe. (Not written to the packet.)
		/// </summary>
		public bool Safe { get; set; }
		
		/// <summary>
		/// Gets or sets a boolean determining whether spell should process attacks. (Not written to the packet.)
		/// </summary>
		public bool Process { get; set; }
		
		/// <summary>
		/// Gets or sets the client id.
		/// </summary>
		public uint ClientId { get; set; }
		
		/// <summary>
		/// Gets or sets the spell x.
		/// </summary>
		public ushort SpellX { get; set; }
		
		/// <summary>
		/// Gets or sets the spell y.
		/// </summary>
		public ushort SpellY { get; set; }
		
		/// <summary>
		/// Gets or sets the spell id.
		/// </summary>
		public ushort SpellId { get; set; }
		
		/// <summary>
		/// Gets or sets the spell level.
		/// </summary>
		public ushort SpellLevel { get; set; }
		
		/// <summary>
		/// Gets or set the list of targets.
		/// This should never be null.
		/// </summary>
		/// <remarks>Auto-initialized to an empty list.</remarks>
		public List<SpellTarget> Targets { get; set; }
		
		/// <summary>
		/// Creates a new spell packet.
		/// </summary>
		public SpellPacket()
			: base(60, Data.Constants.PacketTypes.UseSpellPacket)
		{
			Targets = new List<SpellPacket.SpellTarget>();
		}
		
		/// <summary>
		/// Implicit conversion from the SpellPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](SpellPacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32(packet.ClientId);
			packet.WriteUInt16(packet.SpellX);
			packet.WriteUInt16(packet.SpellY);
			packet.WriteUInt16(packet.SpellId);
			packet.WriteUInt16(packet.SpellLevel);
			packet.WriteUInt32((uint)packet.Targets.Count);
			
			packet.Expand(32 * packet.Targets.Count);
			
			foreach (var target in packet.Targets)
			{
				packet.WriteUInt32(target.ClientId);
				packet.WriteUInt32(target.Damage);
				packet.WriteBool(target.Hit);
				
				// padding 3 bytes ...
				packet.WriteByte(0);
				packet.WriteByte(0);
				packet.WriteByte(0);
				
				packet.WriteUInt32(target.ActivationType);
				packet.WriteUInt32(target.ActivationValue);
				
				packet.Offset += 12;
			}
			
			return packet.Buffer;
		}
	}
}
