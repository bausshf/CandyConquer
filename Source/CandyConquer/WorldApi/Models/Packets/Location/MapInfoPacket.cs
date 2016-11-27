// Project by Bauss
using System;
using CandyConquer.ApiServer;
using CandyConquer.Database.Models;

namespace CandyConquer.WorldApi.Models.Packets.Location
{
	/// <summary>
	/// The map info packet.
	/// </summary>
	public sealed class MapInfoPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the map associated withe packet.
		/// If you set the datas, do not use this.
		/// If this is set then it will set the datas automatically.
		/// </summary>
		public Models.Maps.Map Map { get; set; }
		/// <summary>
		/// Gets or sets the map id.
		/// </summary>
		public uint MapId { get; set; }
		/// <summary>
		/// Gets or sets the documentation id???
		/// -- been years still don't know what this is,
		/// but I also haven't researched it.
		/// I assume it's probably an id the client uses to base documentation
		/// client-side on. Ex. there may be two maps with different ids, but same info??
		/// I don't know tbh...
		/// </summary>
		public uint DocId { get; set; }
		/// <summary>
		/// Gets or sets the flags associated with the map.
		/// </summary>
		private Enums.MapFlag Flags { get; set; }
		
		/// <summary>
		/// Appends map flags to the packet.
		/// </summary>
		/// <param name="flag">The flag to append.</param>
		public void AppendFlag(Enums.MapFlag flag)
		{
			Flags |= flag;
		}
		
		/// <summary>
		/// Appends map flags by a map.
		/// </summary>
		/// <param name="map">The map.</param>
		public void AppendFlag(Models.Maps.Map map)
		{
			switch (map.MapType)
			{
				case DbMap.DbMapType.NoSkills:
				case DbMap.DbMapType.NoSkillsNoLogin:
					AppendFlag(Enums.MapFlag.BoothEnable);
					break;
				case DbMap.DbMapType.NoPK:
					AppendFlag(Enums.MapFlag.PkDisable);
					break;
			}
		}
		
		/// <summary>
		/// Creates a new MapInfoPacket packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		public MapInfoPacket()
			: base(20, 1110)
		{
		}
		
		/// <summary>
		/// Implicit conversion from the MapInfoPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](MapInfoPacket packet)
		{
			packet.Offset = 4;
			
			if (packet.Map != null)
			{
				packet.WriteInt32(packet.Map.ClientMapId);
				packet.WriteInt32(packet.Map.ClientMapId);
				packet.AppendFlag(packet.Map);
			}
			else
			{
				packet.WriteUInt32(packet.MapId);
				packet.WriteUInt32(packet.DocId);
			}
			packet.WriteUInt64((ulong)packet.Flags);
			
			return packet.Buffer;
		}
	}
}
