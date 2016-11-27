// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Entities
{
	/// <summary>
	/// Model for the interaction packet.
	/// </summary>
	public sealed class InteractionPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the timestamp.
		/// </summary>
		public uint Timestamp { get; set; }
		
		/// <summary>
		/// Gets or sets the client id.
		/// </summary>
		public uint ClientId { get; set; }
		
		/// <summary>
		/// Gets or sets the target client id.
		/// </summary>
		public uint TargetClientId { get; set; }
		
		/// <summary>
		/// Gets or sets the x coordinate.
		/// </summary>
		public ushort X { get; set; }
		
		/// <summary>
		/// Gets or sets the y coordinate.
		/// </summary>
		public ushort Y { get; set; }
		
		/// <summary>
		/// Gets or sets the action.
		/// </summary>
		public Enums.InteractionAction Action { get; set; }
		
		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		public uint Data { get; set; }
		
		/// <summary>
		/// Gets or sets the damage.
		/// </summary>
		public ushort Damage
		{
			get { return (ushort)Data; }
			set { Data = (uint)((KOCount << 16) | value); }
		}
		
		/// <summary>
		/// Gets or sets the KO count.
		/// </summary>
		public ushort KOCount
		{
			get { return (ushort)(Data >> 16); }
			set { Data = (uint)((value << 16) | Damage); }
		}
		
		/// <summary>
		/// Gets or sets the magic type.
		/// </summary>
		public ushort MagicType
		{
			get { return (ushort)Data; }
			set { Data = (uint)((MagicLevel << 16) | value); }
		}
		
		/// <summary>
		/// Gets or sets the magic level.
		/// </summary>
		public ushort MagicLevel
		{
			get { return (ushort)(Data >> 16); }
			set { Data = (uint)((value << 16) | MagicType); }
		}
		
		/// <summary>
		/// Gets or sets the activation type.
		/// </summary>
		public uint ActivationType { get; set; }
		
		/// <summary>
		/// Gets or sets the activation value.
		/// </summary>
		public uint ActivationValue { get; set; }
		
		// Not actual part of the packet ...
		/// <summary>
		/// Gets or sets the weapon type right.
		/// </summary>
		public ushort WeaponTypeRight { get; set; }
		
		/// <summary>
		/// Gets or sets the weapon type left.
		/// </summary>
		public ushort WeaponTypeLeft { get; set; }
		
		/// <summary>
		/// Creates a new interaction packet.
		/// </summary>
		public InteractionPacket()
			: base(40, CandyConquer.WorldApi.Data.Constants.PacketTypes.InteractionPacket)
		{
		}
		
		
		/// <summary>
		/// Creates a new interaction packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private InteractionPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			SubTypeOffset = 20;
			
			Timestamp = ReadUInt32();
			ClientId = ReadUInt32();
			TargetClientId = ReadUInt32();
			X = ReadUInt16();
			Y = ReadUInt16();
			Action = (Enums.InteractionAction)ReadUInt32();
			Data = ReadUInt32();
			ReadUInt32(); // unknown
			ActivationType = ReadUInt32();
			ActivationValue = ReadUInt32();
			
			SubTypeObject = Action;
		}
		
		/// <summary>
		/// Decrypts the packet for skill usage.
		/// </summary>
		/// <param name="player">The player.</param>
		public void Decrypt(Models.Entities.Player player)
		{
			if (Action == Enums.InteractionAction.MagicAttack)
			{
				byte[] packet = Buffer;
				
				ushort skillId = Convert.ToUInt16(((long)packet[24] & 0xFF) | (((long)packet[25] & 0xFF) << 8));
				skillId ^= (ushort)0x915d;
				skillId ^= (ushort)player.ClientId;
				skillId = (ushort)(skillId << 0x3 | skillId >> 0xd);
				skillId -= 0xeb42;

				uint targetClientId = ((uint)packet[12] & 0xFF) | (((uint)packet[13] & 0xFF) << 8) | (((uint)packet[14] & 0xFF) << 16) | (((uint)packet[15] & 0xFF) << 24);
				targetClientId = ((((targetClientId & 0xffffe000) >> 13) | ((targetClientId & 0x1fff) << 19)) ^ 0x5F2D2463 ^ player.ClientId) - 0x746F4AE6;

				ushort targetX = 0;
				ushort targetY = 0;
				long xx = (packet[16] & 0xFF) | ((packet[17] & 0xFF) << 8);
				long yy = (packet[18] & 0xFF) | ((packet[19] & 0xFF) << 8);
				xx = xx ^ (player.ClientId & 0xffff) ^ 0x2ed6;
				xx = ((xx << 1) | ((xx & 0x8000) >> 15)) & 0xffff;
				xx |= 0xffff0000;
				xx -= 0xffff22ee;
				yy = yy ^ (player.ClientId & 0xffff) ^ 0xb99b;
				yy = ((yy << 5) | ((yy & 0xF800) >> 11)) & 0xffff;
				yy |= 0xffff0000;
				yy -= 0xffff8922;
				targetX = Convert.ToUInt16(xx);
				targetY = Convert.ToUInt16(yy);
				
				TargetClientId = targetClientId;
				MagicType = skillId;
				X = targetX;
				Y = targetY;
			}
		}
		
		/// <summary>
		/// Implicit conversion from the InteractionPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](InteractionPacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32(packet.Timestamp);
			packet.WriteUInt32(packet.ClientId);
			packet.WriteUInt32(packet.TargetClientId);
			packet.WriteUInt16(packet.X);
			packet.WriteUInt16(packet.Y);
			packet.WriteUInt32((uint)packet.Action);
			packet.WriteUInt32(packet.Data);
			packet.WriteUInt32(0); // unknown
			packet.WriteUInt32(packet.ActivationType);
			packet.WriteUInt32(packet.ActivationValue);
			
			return packet.Buffer;
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to InteractionPacket packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator InteractionPacket(SocketPacket packet)
		{
			return new InteractionPacket(packet);
		}
	}
}
