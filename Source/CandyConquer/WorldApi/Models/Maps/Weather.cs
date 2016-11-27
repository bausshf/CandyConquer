// Project by Bauss
using System;
using CandyConquer.Drivers;
using CandyConquer.Database.Models;

namespace CandyConquer.WorldApi.Models.Maps
{
	/// <summary>
	/// Model for weather.
	/// </summary>
	public sealed class Weather
	{
		/// <summary>
		/// Creates a new weather.
		/// </summary>
		/// <param name="dbWeather">The database weather associated with it.</param>
		public Weather(DbWeather dbWeather)
		{
			MapId = dbWeather.MapId;
			WeatherType = dbWeather.WeatherType.ToEnum<Enums.WeatherType>();
			Intensity = dbWeather.WeatherIntensity.ToEnum<Enums.WeatherIntesity>();
			Appearance = dbWeather.WeatherAppearance.ToEnum<Enums.WeatherAppearance>();
			Chance = dbWeather.WeatherChance;
			Time = dbWeather.WeatherTime.ToEnum<Enums.WeatherTime>();
			Season = dbWeather.WeatherSeason.ToEnum<Enums.WeatherSeason>();
			Thunder = dbWeather.WeatherThunder;
		}
		
		/// <summary>
		/// The map id.
		/// </summary>
		public int MapId { get; private set; }
		
		/// <summary>
		/// The weather type.
		/// </summary>
		public Enums.WeatherType WeatherType { get; private set; }
		
		/// <summary>
		/// The intensity.
		/// </summary>
		public Enums.WeatherIntesity Intensity { get; private set; }
		
		/// <summary>
		/// The appearance.
		/// </summary>
		public Enums.WeatherAppearance Appearance { get; private set; }
		
		/// <summary>
		/// The chance.
		/// </summary>
		public int Chance { get; private set; }
		
		/// <summary>
		/// The time.
		/// </summary>
		public Enums.WeatherTime Time { get; private set; }
		
		/// <summary>
		/// The season.
		/// </summary>
		public Enums.WeatherSeason Season { get; private set; }
		
		/// <summary>
		/// Boolean indicating whether the weather has thunder or not.
		/// </summary>
		public bool Thunder { get; private set; }
	}
}
