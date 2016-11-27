// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Login
{
	/// <summary>
	/// The character init (initialization) packet.
	/// Also known as 'Character Information'
	/// </summary>
	public sealed class CharacterInitPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the player to create the packet from.
		/// </summary>
		public Models.Entities.Player Player { get; set; }
		
		/// <summary>
		/// Creates a new character init packet.
		/// </summary>
		public CharacterInitPacket()
			: base(112, 1006)
		{
		}
		
		/// <summary>
		/// Implicit conversion from the character init packet to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](CharacterInitPacket packet)
		{
			if (packet.Player != null)
			{
				var player = packet.Player;
				packet.Offset = 4;
				packet.WriteUInt32(player.ClientId);
				packet.Offset = 10;
				player.Model = player.Model; // forces a mesh creation
				packet.WriteUInt32(player.Mesh);
				packet.WriteUInt16(player.Hair);
				packet.WriteUInt32(player.Money);
				packet.WriteUInt32(player.CPs);
				packet.WriteUInt64(player.Experience);
				packet.Offset = 52;
				packet.WriteUInt16(player.Strength);
				packet.WriteUInt16(player.Agility);
				packet.WriteUInt16(player.Vitality);
				packet.WriteUInt16(player.Spirit);
				packet.WriteUInt16(player.AttributePoints);
				packet.WriteUInt16((ushort)Math.Min((int)ushort.MaxValue, player.HP));
				packet.WriteUInt16((ushort)Math.Min((int)ushort.MaxValue, player.MP));
				packet.WriteInt16(player.PKPoints);
				packet.WriteByte(player.Level);
				packet.WriteByte((byte)player.Job);
				packet.Offset = 74;
				packet.WriteBool(true); // name displayed
				packet.Offset = 91;
				packet.WriteByte((byte)player.Title);
				packet.Offset = 110;
				packet.WriteStrings(player.Name, string.Empty, player.Spouse);
			}
			
			return packet.Buffer;
		}
	}
}
