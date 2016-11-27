// Project by Bauss
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CandyConquer.Drivers.Repositories.Collections;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A collection of weathers.
	/// </summary>
	public static class WeatherCollection
	{
		/// <summary>
		/// The collection of weathers.
		/// </summary>
		private static ConcurrentList<Models.Maps.Weather> _weathers;
		/// <summary>
		/// A list of relevant map ids. (Maps that have weather.)
		/// </summary>
		private static ConcurrentList<int> _relevantMapIds;
		
		/// <summary>
		/// Static constructor for WeatherCollection.
		/// </summary>
		static WeatherCollection()
		{
			_weathers = new ConcurrentList<CandyConquer.WorldApi.Models.Maps.Weather>();
			_relevantMapIds = new ConcurrentList<int>();
		}
		
		/// <summary>
		/// Loads all weathers.
		/// </summary>
		public static void Load()
		{
			_weathers.TryAddRange(
				Database.Dal.Weathers.GetAll()
				.Select(weather =>
				        {
				        	_relevantMapIds.TryAdd(weather.MapId);
				        	return new Models.Maps.Weather(weather);
				        })
			);
			
			var distinctMapIds = _relevantMapIds.GetItems().Distinct();
			_relevantMapIds.Clear();
			_relevantMapIds.TryAddRange(distinctMapIds);
		}
		
		/// <summary>
		/// Gets all relevant weathers to a map.
		/// </summary>
		/// <param name="mapId">The map id.</param>
		/// <returns>A read only collection of all relevant weathers.</returns>
		public static ReadOnlyCollection<Models.Maps.Weather> GetAllRelevantWeathers(int mapId)
		{
			var now = DateTime.UtcNow;
			
			Enums.WeatherSeason season;
			if (now.Month >= 1 && now.Month <= 2 ||
			    now.Month >= 11)
			{
				season = Enums.WeatherSeason.Winter;
			}
			else if (now.Month >= 3 && now.Month <= 5)
			{
				season = Enums.WeatherSeason.Spring;
			}
			else if (now.Month >= 6 && now.Month <= 8)
			{
				season = Enums.WeatherSeason.Summer;
			}
			else // (implicit) if (now.Month >= 9 && now.Month <= 10)
			{
				season = Enums.WeatherSeason.Fall;
			}
			
			Enums.WeatherTime time;
			if (now.Hour <= 6)
			{
				time = Enums.WeatherTime.Night;
			}
			else if (now.Hour <= 12)
			{
				time = Enums.WeatherTime.Morning;
			}
			else if (now.Hour <= 6)
			{
				time = Enums.WeatherTime.Day;
			}
			else // (implicit) if (now.Hour <= 24)
			{
				time = Enums.WeatherTime.Evening;
			}
			
			return _weathers
				.Where(weather =>
				       {
				       	var success = (weather.Season == season || weather.Season == Enums.WeatherSeason.All) &&
				       		(weather.Time == time || weather.Time == Enums.WeatherTime.Always) &&
				       		weather.MapId == mapId;
				       	
				       	if (success && weather.Chance > 0)
				       	{
				       		return Tools.CalculationTools.ChanceSuccess(weather.Chance);
				       	}
				       	
				       	return success;
				       })
				.ToList().AsReadOnly();
		}
		
		/// <summary>
		/// Gets a collection of all relevant maps.
		/// </summary>
		/// <returns>A collection of all relevant maps.</returns>
		public static ICollection<int> GetMaps()
		{
			return _relevantMapIds.GetItems();
		}
	}
}
