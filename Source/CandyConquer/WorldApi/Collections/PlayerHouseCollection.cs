// Project by Bauss
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using CandyConquer.WorldApi.Models.Maps;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A collection of player houses.
	/// </summary>
	public class PlayerHouseCollection
	{
		/// <summary>
		/// The player owning the houses.
		/// </summary>
		private Models.Entities.Player _player;
		
		/// <summary>
		/// The collection of houses.
		/// </summary>
		private ConcurrentDictionary<int, PlayerHouse> _houses;
		
		/// <summary>
		/// Creates a new player house collection.
		/// </summary>
		/// <param name="player">The player.</param>
		public PlayerHouseCollection(Models.Entities.Player player)
		{
			_player = player;
			_houses = new ConcurrentDictionary<int, PlayerHouse>();
		}
		
		/// <summary>
		/// Adds a new house to the collection.
		/// </summary>
		/// <param name="house">The house.</param>
		public void Add(PlayerHouse house)
		{
			_houses.TryAdd(house.DbPlayerHouse.MapId, house);
		}
		
		/// <summary>
		/// Attempts to get a house from the collection.
		/// </summary>
		/// <param name="mapId">The map id.</param>
		/// <param name="house">The house.</param>
		/// <returns>True if the house was found, false otherwise.</returns>
		public bool TryGetHouse(int mapId, out PlayerHouse house)
		{
			return _houses.TryGetValue(mapId, out house);
		}
		
		/// <summary>
		/// Gets the total amout of houses in the collection.
		/// </summary>
		public int Count
		{
			get { return _houses.Count; }
		}
		
		/// <summary>
		/// Gets all houses.
		/// </summary>
		/// <returns>ICollection of the houses.</returns>
		public ICollection<PlayerHouse> GetAll()
		{
			return _houses.Values;
		}
		
		/// <summary>
		/// Removes a house from the collection.
		/// </summary>
		/// <param name="house">The house to remove.</param>
		public void Remove(PlayerHouse house)
		{
			PlayerHouse removedHouse;
			_houses.TryRemove(house.DbPlayerHouse.MapId, out removedHouse);
		}
	}
}
