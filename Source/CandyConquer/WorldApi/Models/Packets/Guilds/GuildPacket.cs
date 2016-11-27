// Project by Bauss
using System;
using System.Collections.Generic;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Guilds
{
	/// <summary>
	/// Model for the guild packet.
	/// </summary>
	public sealed class GuildPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the action.
		/// </summary>
		public Enums.GuildAction Action { get; set; }
		
		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		public uint Data { get; set; }
		
		/// <summary>
		/// Gets or sets the required level.
		/// </summary>
		public int RequiredLevel { get; set; }
		
		/// <summary>
		/// Gets or sets the required metempsychosis.
		/// </summary>
		public int RequiredMetempsychosis { get; set; }
		
		/// <summary>
		/// Gets or sets the required profession.
		/// </summary>
		public int RequiredProfession { get; set; }
		
		/// <summary>
		/// Gets or sets the announcement.
		/// </summary>
		public string Announcement { get; set; }
		
		/// <summary>
		/// Gets the strings.
		/// </summary>
		public List<string> Strings { get; private set; }
		
		/// <summary>
		/// Creates a new guild packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private GuildPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			Strings = new List<string>();
			
			Action = (Enums.GuildAction)ReadUInt32();
			Data = ReadUInt32();
			RequiredLevel = ReadInt32();
			RequiredMetempsychosis = ReadInt32();
			RequiredProfession = ReadInt32();
			
			var strings = ReadStrings();
			Strings.AddRange(strings);
			
			if (Strings.Count > 0)
			{
				Announcement = Strings[0];
			}
			
			SubTypeObject = Action;
		}
		
		/// <summary>
		/// Creates a new guild packet.
		/// </summary>
		public GuildPacket()
			: base(28, CandyConquer.WorldApi.Data.Constants.PacketTypes.GuildPacket)
		{
			Strings = new List<string>();
		}
		
		/// <summary>
		/// Implicit conversion from the GuildPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](GuildPacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32((uint)packet.Action);
			packet.WriteUInt32(packet.Data);
			packet.WriteInt32(packet.RequiredLevel);
			packet.WriteInt32(packet.RequiredMetempsychosis);
			packet.WriteInt32(packet.RequiredProfession);
			if (!string.IsNullOrWhiteSpace(packet.Announcement))
			{
				packet.WriteStrings(packet.Announcement);
			}
			else
			{
				packet.WriteStrings(packet.Strings.ToArray());
			}
			
			return packet.Buffer;
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to GuildPacket.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		/// <returns>The packet.</returns>
		public static implicit operator GuildPacket(SocketPacket packet)
		{
			return new GuildPacket(packet);
		}
	}
}
