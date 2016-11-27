// Project by Bauss
using System;
using System.Collections.Concurrent;
using CandyConquer.Maps;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for dmaps.
	/// </summary>
	internal static class DMaps
	{
		/// <summary>
		/// The dmaps dictionary.
		/// </summary>
		private static ConcurrentDictionary<ushort,FCQMap> _maps;
		/// <summary>
		/// The loaded event.
		/// </summary>
		private static Action _loadedEvent;
		
		/// <summary>
		/// Loads all dmaps.
		/// </summary>
		internal static void Load()
		{
			_maps = new ConcurrentDictionary<ushort, FCQMap>();
			FCQMapLoader.GetMaps(Drivers.Settings.DatabaseSettings.WorldFlatDatabase + "\\Maps",
			                     (map) => { _maps.TryAdd(map.Id, map); });
			
			if (_loadedEvent != null)
			{
				_loadedEvent();
			}
		}
		
		/// <summary>
		/// Sets an event for when the dmaps has been loaded.
		/// </summary>
		/// <param name="loadedEvent">The event.</param>
		internal static void SetLoadedEvent(Action loadedEvent)
		{
			_loadedEvent = loadedEvent;
		}
		
		/// <summary>
		/// Gets a dmap based on a map id.
		/// </summary>
		/// <param name="mapId">The map id.</param>
		/// <returns>The dmap if existing, null otherwise.</returns>
		internal static FCQMap Get(ushort mapId)
		{
			FCQMap map;
			_maps.TryGetValue(mapId, out map);
			return map;
		}
	}
}
