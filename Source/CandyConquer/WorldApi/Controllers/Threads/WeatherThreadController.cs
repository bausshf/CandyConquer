// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Controllers.Threads
{
	/// <summary>
	/// The weather thread controller.
	/// </summary>
	public static class WeatherThreadController
	{
		/// <summary>
		/// The next weather time.
		/// </summary>
		private static DateTime _nextWeather = DateTime.UtcNow.AddMinutes(-20);
		
		/// <summary>
		/// Handles the weather thread.
		/// </summary>
		public static void Handle()
		{
			if (DateTime.UtcNow >= _nextWeather)
			{
				_nextWeather = DateTime.UtcNow.AddMinutes(20);
				
				foreach (var mapId in Collections.WeatherCollection.GetMaps())
				{
					var weathers = Collections.WeatherCollection.GetAllRelevantWeathers(mapId);
					if (weathers != null && weathers.Count > 0)
					{
						var map = Collections.MapCollection.GetMap(mapId);
						if (map != null)
						{
							if (Tools.CalculationTools.ChanceSuccess(50))
							{
								map.Weather = weathers.Count == 1 ? weathers[0] : weathers[Drivers.Repositories.Safe.Random.Next(weathers.Count)];
							}
							else
							{
								map.Weather = null;
							}
						}
					}
				}
			}
			
			HandleThunder();
		}
		
		/// <summary>
		/// Handles thunder.
		/// </summary>
		private static void HandleThunder()
		{
			foreach (var mapId in Collections.WeatherCollection.GetMaps())
			{
				var map = Collections.MapCollection.GetMap(mapId);
				if (map != null && map.Weather != null && map.Weather.Thunder)
				{
					if (DateTime.UtcNow >= map.NextThunderTime)
					{
						map.NextThunderTime = DateTime.UtcNow.AddMilliseconds(Drivers.Repositories.Safe.Random.Next(100, 10000));
						var amount = Drivers.Repositories.Safe.Random.Next(17);
						if (amount <= 11)
						{
							amount = 1;
						}
						else
						{
							amount -= 10;
						}
						
						for (int i = 0; i < amount; i++)
						{
							map.DisplayThunder();
						}
					}
				}
			}
		}
	}
}
