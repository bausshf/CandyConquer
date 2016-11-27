// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Items
{
	/// <summary>
	/// Model for the item action packet.
	/// </summary>
	public sealed class ItemActionPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the client id.
		/// </summary>
		public uint ClientId { get; set; }
		/// <summary>
		/// Gets or sets the primary data.
		/// </summary>
		public uint Data1 { get; set; }
		/// <summary>
		/// Gets or sets the action.
		/// </summary>
		public Enums.ItemAction Action { get; set; }
		/// <summary>
		/// Gets or sets the secondary data.
		/// </summary>
		public uint Data2 { get; set; }
		/// <summary>
		/// Gets or sets the associated player.
		/// </summary>
		public Models.Entities.Player Player { get; set; }
		
		/// <summary>
		/// Offset [8/0x08]
		/// </summary>
		public ushort Data1Low
		{
			get { return (ushort)Data1; }
			set { Data1 = (uint)((Data1High << 16) | value); }
		}

		/// <summary>
		/// Offset [10/0x0a]
		/// </summary>
		public ushort Data1High
		{
			get { return (ushort)(Data1 >> 16); }
			set { Data1 = (uint)((value << 16) | Data1Low); }
		}
		
		/// <summary>
		/// Creates a new ItemActionPacket packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private ItemActionPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			SubTypeOffset = 12;
			
			ClientId = ReadUInt32();
			Data1 = ReadUInt32();
			Action = (Enums.ItemAction)ReadUInt32();
			Offset = 20;
			Data2 = ReadUInt32();
			
			SubTypeObject = Action;
		}
		
		/// <summary>
		/// Creates a new item action packet.
		/// </summary>
		public ItemActionPacket()
			: base(92, 1009)
		{
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to ItemActionPacket packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator ItemActionPacket(SocketPacket packet)
		{
			return new ItemActionPacket(packet);
		}
		
		/// <summary>
		/// Implicit conversion from the ItemActionPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](ItemActionPacket packet)
		{
			// Write ...
			packet.WriteUInt32(packet.ClientId);
			packet.WriteUInt32(packet.Data1);
			packet.WriteUInt32((uint)packet.Action);
			packet.WriteUInt32(0); // skip 4 ...
			packet.WriteUInt32(packet.Data2);
			
			if (packet.Action == Enums.ItemAction.DisplayGears)
			{
				var equipments = packet.Player.Equipments;
				packet.Offset = 32;
				
				var head = equipments.Get(Enums.ItemPosition.Head, true);
				var necklace = equipments.Get(Enums.ItemPosition.Necklace, true);
				var armor = equipments.Get(Enums.ItemPosition.Armor, true);
				var weaponR = equipments.Get(Enums.ItemPosition.WeaponR, true);
				var weaponL = equipments.Get(Enums.ItemPosition.WeaponL, true);
				var ring = equipments.Get(Enums.ItemPosition.Ring, true);
				var bottle = equipments.Get(Enums.ItemPosition.Bottle, true);
				var boots = equipments.Get(Enums.ItemPosition.Boots, true);
				var garment = equipments.Get(Enums.ItemPosition.Garment, true);
				
				var fan = equipments.Get(Enums.ItemPosition.Fan, true);
				var tower = equipments.Get(Enums.ItemPosition.Tower, true);
				
				var steedArmor = equipments.Get(Enums.ItemPosition.SteedArmor, true);
				var steed = equipments.Get(Enums.ItemPosition.Steed, true);
				
				packet.WriteUInt32((uint)(head != null ? head.ClientId : 0));
				packet.WriteUInt32((uint)(necklace != null ? necklace.ClientId : 0));
				packet.WriteUInt32((uint)(armor != null ? armor.ClientId : 0));
				packet.WriteUInt32((uint)(weaponR != null ? weaponR.ClientId : 0));
				packet.WriteUInt32((uint)(weaponL != null ? weaponL.ClientId : 0));
				packet.WriteUInt32((uint)(ring != null ? ring.ClientId : 0));
				packet.WriteUInt32((uint)(bottle != null ? bottle.ClientId : 0));
				packet.WriteUInt32((uint)(boots != null ? boots.ClientId : 0));
				packet.WriteUInt32((uint)(garment != null ? garment.ClientId : 0));
				
				packet.WriteUInt32((uint)(fan != null ? fan.ClientId : 0));
				packet.WriteUInt32((uint)(tower != null ? tower.ClientId : 0));
				
				var steedId = steedArmor != null ? steedArmor.ClientId : 0;
				if (steedId == 0)
				{
					steedId = steed != null ? steed.ClientId : 0;
				}
				packet.WriteUInt32((uint)steedId);
			}
			
			return packet.Buffer;
		}
	}
}
