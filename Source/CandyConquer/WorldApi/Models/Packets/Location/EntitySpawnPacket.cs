// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Location
{
	/// <summary>
	/// The entity spawn packet. (Most known as the spawn packet for mobs and players.)
	/// </summary>
	public sealed class EntitySpawnPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the entity associated with the spawn packet.
		/// </summary>
		public Models.Entities.IEntity Entity { get; set; }
		
		/// <summary>
		/// Creates a new entity spawn packet.
		/// </summary>
		public EntitySpawnPacket()
			: base(218, 10014)
		{
		}
		
		/// <summary>
		/// Implicit conversion from the EntitySpawnPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](EntitySpawnPacket packet)
		{
			packet.Offset = 4;
			
			if (packet.Entity is Models.Entities.Player)
			{
				var player = (Models.Entities.Player)packet.Entity;
				packet.WriteUInt32(player.Mesh);
				packet.WriteUInt32(player.ClientId);
				
				if (player.Guild == null)
				{
					packet.WriteUInt32(0);
					packet.WriteUInt16(0);
				}
				else
				{
					packet.WriteUInt32((uint)player.Guild.Id);
					packet.WriteUInt32((uint)player.GuildMember.Rank);
				}
				
				packet.Offset = 22;
				packet.WriteUInt64(player.StatusFlag); // effect 1
				packet.WriteUInt64(0); // effect 2
				
				packet.Offset = 40;
				var headGear = player.Equipments.Get(Enums.ItemPosition.Head);
				var garment = player.Equipments.Get(Enums.ItemPosition.Garment);
				var armor = player.Equipments.Get(Enums.ItemPosition.Armor);
				var weaponL = player.Equipments.Get(Enums.ItemPosition.WeaponL);
				var weaponR = player.Equipments.Get(Enums.ItemPosition.WeaponR);
				var accessoryR = player.Equipments.Get(Enums.ItemPosition.AccessoryR);
				var accessoryL = player.Equipments.Get(Enums.ItemPosition.AccessoryL);
				var steed = player.Equipments.Get(Enums.ItemPosition.Steed);
				var steedArmor = player.Equipments.Get(Enums.ItemPosition.SteedArmor);
				
				packet.WriteUInt32((uint)(headGear != null ? headGear.DbItem.Id : 0)); // helmet
				packet.WriteUInt32((uint)(garment != null ? garment.DbItem.Id : 0)); // garment
				packet.WriteUInt32((uint)(armor != null ? armor.DbItem.Id : 0)); // armor
				packet.WriteUInt32((uint)(weaponL != null ? weaponL.DbItem.Id : 0)); // left
				packet.WriteUInt32((uint)(weaponR != null ? weaponR.DbItem.Id : 0)); // right
				packet.WriteUInt32((uint)(accessoryR != null ? accessoryR.DbItem.Id : 0)); // accessory right
				packet.WriteUInt32((uint)(accessoryL != null ? accessoryL.DbItem.Id : 0)); // accessory left
				packet.WriteUInt32((uint)(steed != null ? steed.DbItem.Id : 0)); // steed
				packet.WriteUInt32((uint)(steedArmor != null ? steedArmor.DbItem.Id : 0)); // steed armor
				
				packet.Offset = 80;
				packet.WriteUInt16((ushort)Math.Min((int)ushort.MaxValue, player.HP));
				packet.WriteUInt16(0); // mob level
				packet.WriteUInt16(player.Hair);
				packet.WriteUInt16(player.X);
				packet.WriteUInt16(player.Y);
				packet.WriteByte((byte)player.Direction);
				packet.WriteByte((byte)player.Action);
				
				packet.WriteUInt16(0); // uknown
				packet.WriteUInt32(0); // unknown
				packet.WriteByte(player.Reborns);
				packet.WriteByte(player.Level);
				
				packet.Offset = 102;
				packet.WriteUInt32(0); // away
				
				packet.Offset = 119;
				if (player.Nobility != null)
				{
					packet.WriteByte((byte)player.Nobility.Rank);
				}
				else
				{
					packet.WriteByte(0);
				}
				
				packet.Offset = 123;
				packet.WriteUInt16((ushort)(armor != null ? (ushort)armor.Color : 0)); // armor color
				packet.WriteUInt16((ushort)(weaponL != null && weaponL.IsShield ? (ushort)weaponL.Color : 0)); // shield color
				packet.WriteUInt16((ushort)(headGear != null ? (ushort)headGear.Color : 0)); // helmet color
				packet.WriteUInt32(0); // quiz points
				packet.WriteByte(0); // mount plus
				
				packet.Offset = 139;
				packet.WriteUInt32(steed != null ? steed.DbOwnerItem.SocketRGB : 0); // mount color
				
				packet.Offset = 167;
				packet.WriteByte((byte)player.Title);
				
				packet.Offset = 181;
				packet.WriteBool(false); // boss
				packet.WriteUInt32(0); // helmet artifact
				packet.WriteUInt32(0); // armor artifact
				packet.WriteUInt32(0); // right artifact
				packet.WriteUInt32(0); // left artifact
				
				packet.Offset = 210;
				packet.WriteByte((byte)player.Job);
				
				packet.Offset = 218;
				packet.WriteStrings(player.Name, string.Empty, string.Empty);
			}
			else if (packet.Entity is Models.Entities.Monster)
			{
				var monster = (Models.Entities.Monster)packet.Entity;
				packet.WriteUInt32(monster.Mesh);
				packet.WriteUInt32(monster.ClientId);
				
//				if (monster.Guild == null)
//				{
					packet.WriteUInt32(0);
					packet.WriteUInt16(0);
//				}
//				else
//				{
//					packet.WriteUInt32((uint)monster.Guild.Id);
//					packet.WriteUInt32((uint)monster.GuildMember.Rank);
//				}
				
				packet.Offset = 22;
				packet.WriteUInt64(monster.StatusFlag); // effect 1
				packet.WriteUInt64(0); // effect 2
				
				packet.Offset = 40;
				packet.WriteUInt32(0); // helmet
				packet.WriteUInt32(0); // garment
				packet.WriteUInt32(0); // armor
				packet.WriteUInt32(0); // left
				packet.WriteUInt32(0); // right
				packet.WriteUInt32(0); // accessory right
				packet.WriteUInt32(0); // accessory left
				packet.WriteUInt32(0); // steed
				packet.WriteUInt32(0); // steed armor
				
				packet.Offset = 80;
				packet.WriteUInt16((ushort)(monster.Boss ? ((long)monster.HP * 10000 / monster.MaxHP) : monster.HP));
				packet.WriteUInt16(monster.Level); // mob level
				packet.WriteUInt16(0); // hair
				packet.WriteUInt16(monster.X);
				packet.WriteUInt16(monster.Y);
				packet.WriteByte((byte)monster.Direction);
				packet.WriteByte(0); // player action
				
				packet.WriteUInt16(0); // uknown
				packet.WriteUInt32(0); // unknown
				packet.WriteByte(0); // player reborns
				packet.WriteByte(0); // player level
				
				packet.Offset = 102;
				packet.WriteUInt32(0); // away
				
				packet.Offset = 119;
				packet.WriteByte(0); // nobility
				
				packet.Offset = 123;
				packet.WriteUInt16(0); // armor color
				packet.WriteUInt16(0); // shield color
				packet.WriteUInt16(0); // helmet color
				packet.WriteUInt32(0); // quiz points
				packet.WriteByte(0); // mount plus
				
				packet.Offset = 139;
				packet.WriteUInt32(0); // mount color
				
				packet.Offset = 167;
				packet.WriteByte(0); // player title
				
				packet.Offset = 181;
				packet.WriteBool(monster.Boss); // boss
				packet.WriteUInt32(0); // helmet artifact
				packet.WriteUInt32(0); // armor artifact
				packet.WriteUInt32(0); // right artifact
				packet.WriteUInt32(0); // left artifact
				
				packet.Offset = 210;
				packet.WriteByte(0); // player job
				
				packet.Offset = 218;
				packet.WriteStrings(monster.Name, string.Empty, string.Empty);
			}
			
			return packet.Buffer;
		}
	}
}
