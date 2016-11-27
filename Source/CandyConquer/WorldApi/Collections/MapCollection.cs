// Project by Bauss
using System;
using System.Collections.Concurrent;
using CandyConquer.WorldApi.Models.Maps;
using CandyConquer.Database.Models;
using CandyConquer.Database.Dal;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A collection of map data.
	/// </summary>
	public static class MapCollection
	{
		/// <summary>
		/// The map collection.
		/// </summary>
		private static ConcurrentDictionary<int,Map> _maps;
		
		/// <summary>
		/// The global default coordinates. (Coordinates not tied to a map.)
		/// </summary>
		private static ConcurrentDictionary<string, DbDefaultCoordinate> _defaultCoordinates;
		
		/// <summary>
		/// The dynamic maps.
		/// </summary>
		private static ConcurrentDictionary<int, DynamicMap> _dynamicMaps;
		
		/// <summary>
		/// Value indicating whether the maps has been loaded or not.
		/// </summary>
		public static bool LoadedMaps { get; private set; }
		
		/// <summary>
		/// Static constructor for MapCollection.
		/// </summary>
		static MapCollection()
		{
			_maps = new ConcurrentDictionary<int, Map>();
			_defaultCoordinates = new ConcurrentDictionary<string, DbDefaultCoordinate>();
			_dynamicMaps = new ConcurrentDictionary<int, DynamicMap>();
		}
		
		/// <summary>
		/// Loads all maps.
		/// </summary>
		public static void LoadMaps()
		{
			var server = Drivers.Settings.WorldSettings.Server;
			var maps = Database.Dal.Maps.GetAllByServer(server);
			foreach (var dbMap in maps)
			{
				var map = new Map(dbMap);
				var drops = Database.Dal.Drops.GetAll(map.Id, server);
				foreach (var dbDrop in drops)
				{
					map.Drops.Add(new Drop(dbDrop));
				}
				
				_maps.TryAdd(map.Id, map);
			}
			
			LoadedMaps = true;
		}
		
		/// <summary>
		/// Loads all default coordinates.
		/// </summary>
		public static void LoadDefaultCoordinates()
		{
			var coords = Database.Dal.Maps.GetDefaultCoordinates();
			foreach (var coord in coords)
			{
				if (coord.MapId == -1)
				{
					_defaultCoordinates.TryAdd(coord.Name, coord);
				}
				else
				{
					Map map;
					if (_maps.TryGetValue(coord.MapId, out map))
					{
						map.DefaultCoordinates.TryAdd(coord.Name, coord);
					}
				}
			}
		}
		
		/// <summary>
		/// Gets a map based on an id.
		/// </summary>
		/// <param name="id">The map id.</param>
		/// <returns>The map if existing, null otherwise.</returns>
		public static Map GetMap(int id)
		{
			Map map;
			_maps.TryGetValue(id, out map);
			return map;
		}
		
		/// <summary>
		/// Gets the valid login map based on a map object.
		/// </summary>
		/// <param name="map">The map.</param>
		/// <param name="lastMap">The last map.</param>
		/// <returns>The valid login map.</returns>
		public static Map GetValidLoginMap(Map map, Map lastMap)
		{
			if (map == null && lastMap == null)
			{
				return GetMap(1002);
			}
			else if (map == null || map.MapType == DbMap.DbMapType.NoSkillsNoLogin ||
			         map.MapType == DbMap.DbMapType.NoLogin ||
			         map.MapType == DbMap.DbMapType.Tournament)
			{
				return GetValidLoginMap(lastMap, null);
			}
			
			return map;
		}
		
		/// <summary>
		/// Gets and creates a new dynamic map.
		/// </summary>
		/// <param name="original">The original map id.</param>
		/// <returns>The created dynamic map.</returns>
		public static DynamicMap GetDynamicMap(int original)
		{
			return new DynamicMap(GetMap(original));
		}
		
		/// <summary>
		/// Tries to add a dynamic map.
		/// </summary>
		/// <param name="map">The dynamic map.</param>
		/// <returns>True if the dynamic map was added, false otherwise.</returns>
		public static bool TryAddDynamicMap(DynamicMap map)
		{
			return _dynamicMaps.TryAdd(map.Id, map);
		}
		
		/// <summary>
		/// Tries to get a dynamic map.
		/// </summary>
		/// <param name="dynamicId">The dynamic map id.</param>
		/// <param name="map">The dynamic map.</param>
		/// <returns>True if the dynamic map was found, false otherwise.</returns>
		public static bool TryGetDynamicMap(int dynamicId, out DynamicMap map)
		{
			return _dynamicMaps.TryGetValue(dynamicId, out map);
		}
	}
}
