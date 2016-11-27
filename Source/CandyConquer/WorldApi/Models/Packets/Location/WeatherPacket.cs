// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Location
{
	/// <summary>
	/// Model for the weather packet.
	/// </summary>
	public sealed class WeatherPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the weather.
		/// </summary>
		public Models.Maps.Weather Weather { get; set; }
		
		/// <summary>
		/// Gets a sets a boolean whether the weather should be cleared.
		/// </summary>
		public bool Clear { get; set; }
		
		/// <summary>
		/// Creates a new weather packet.
		/// </summary>
		public WeatherPacket()
			: base(20, Data.Constants.PacketTypes.WeatherPacket)
		{
		}
		
		/// <summary>
		/// Implicit conversion from the WeatherPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](WeatherPacket packet)
		{
			packet.Offset = 4;
			
			if (packet.Clear)
			{
				packet.WriteUInt32((uint)Enums.WeatherType.Nothing);
				packet.WriteUInt32(0);
				packet.WriteUInt32(0);
				packet.WriteUInt32(0);
			}
			else
			{
				var weather = packet.Weather;
				
				packet.WriteUInt32((uint)weather.WeatherType);
				packet.WriteUInt32((uint)weather.Intensity);
				packet.WriteUInt32((uint)Enums.Direction.South);
				packet.WriteUInt32((uint)weather.Appearance);
			}
			
			return packet.Buffer;
		}
	}
}
