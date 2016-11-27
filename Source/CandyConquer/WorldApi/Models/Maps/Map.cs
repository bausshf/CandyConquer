// Project by Bauss
using System;
using System.Collections.Concurrent;
using CandyConquer.Database.Models;

namespace CandyConquer.WorldApi.Models.Maps
{
	/// <summary>
	/// Model for a map.
	/// </summary>
	public class Map : Controllers.Maps.MapController
	{
		/// <summary>
		/// The database map associated with the map.
		/// </summary>
		protected DbMap _map;
		/// <summary>
		/// An alternative id to use.
		/// </summary>
		protected int _id;
		
		/// <summary>
		/// Gets a collection of all players associated with this map.
		/// </summary>
		public ConcurrentDictionary<uint, Models.Entities.Player> Players { get; private set; }
		/// <summary>
		/// Gets a collection of all map objects associated with this map.
		/// </summary>
		public ConcurrentDictionary<uint, IMapObject> MapObjects { get; private set; }
		/// <summary>
		/// Gets the default coordinates for the map.
		/// </summary>
		public ConcurrentDictionary<string, DbDefaultCoordinate> DefaultCoordinates { get; private set; }
		/// <summary>
		/// Gets the drops for the map.
		/// </summary>
		public ConcurrentBag<Drop> Drops { get; private set; }
		
		/// <summary>
		/// The weather of the map.
		/// </summary>
		private Weather _weather;
		
		/// <summary>
		/// Creates a new map.
		/// </summary>
		/// <param name="map">The database map associated with it.</param>
		public Map(DbMap map)
		{
			// Sets controller
			Map = this;
			
			// Default data ...
			_map = map;
			_id = -1;
			
			Players = new ConcurrentDictionary<uint, Models.Entities.Player>();
			MapObjects = new ConcurrentDictionary<uint, IMapObject>();
			DefaultCoordinates = new ConcurrentDictionary<string, DbDefaultCoordinate>();
			Drops = new ConcurrentBag<Drop>();
		}
		
		/// <summary>
		/// Gets the database map associated with this map.
		/// </summary>
		public DbMap DbMap
		{
			get { return _map; }
		}
		
		/// <summary>
		/// Gets a boolean indicating whether the map is dynamic or not.
		/// </summary>
		public bool IsDynamic
		{
			get { return Id > 1000000; }
		}
		
		/// <summary>
		/// Gets the map id.
		/// </summary>
		public int Id
		{
			get { return _id != -1 ? _id : _map.Id; }
		}
		
		/// <summary>
		/// Gets the client map id.
		/// </summary>
		public int ClientMapId
		{
			get { return _map.ClientMapId; }
		}
		
		/// <summary>
		/// Gets the name of the map.
		/// </summary>
		public string Name
		{
			get { return _map.MapName; }
		}
		
		/// <summary>
		/// Gets the type of the map.
		/// </summary>
		public DbMap.DbMapType MapType
		{
			get { return _map.MapType; }
		}
		
		/// <summary>
		/// Gets a boolean determining whether the pk mode in the map is safe.
		/// </summary>
		public bool SafePK
		{
			get { return MapType == Database.Models.DbMap.DbMapType.SafePK || MapType == Database.Models.DbMap.DbMapType.Tournament; }
		}
		
		/// <summary>
		/// Gets the dmap.
		/// </summary>
		public CandyConquer.Maps.FCQMap DMap
		{
			get { return _map.DMap; }
		}
		
		/// <summary>
		/// Gets or sets the weather of the map.
		/// </summary>
		public Weather Weather
		{
			get { return _weather; }
			set
			{
				_weather = value;
				
				DisplayWeather();
			}
		}
		
		/// <summary>
		/// Gets or sets the next time of thunder.
		/// </summary>
		public DateTime NextThunderTime { get; set; }
	}
}
