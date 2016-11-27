// Project by Bauss
using System;
using System.Linq;
using System.Collections.Concurrent;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A collection of warehouses.
	/// </summary>
	public sealed class WarehouseCollection
	{
		/// <summary>
		/// The collection of warehouses.
		/// </summary>
		private ConcurrentDictionary<uint, Warehouse> _warehouses;
		/// <summary>
		/// The player that owns the warehouses.
		/// </summary>
		public Models.Entities.Player Player { get; private set; }
		
		/// <summary>
		/// Creates a new warehouse collection.
		/// </summary>
		/// <param name="player">The player.</param>
		public WarehouseCollection(Models.Entities.Player player)
		{
			_warehouses = new ConcurrentDictionary<uint, Warehouse>();
			
			Player = player;
			
			CreateWarehouse(8);
			CreateWarehouse(10012);
			CreateWarehouse(10028);
			CreateWarehouse(10011);
			CreateWarehouse(1027);
			CreateWarehouse(4101);
		}
		
		/// <summary>
		/// Attempts to get a warehouse from the collection.
		/// </summary>
		/// <param name="warehouseId">The warehouse id.</param>
		/// <param name="warehouse">The warehouse.</param>
		/// <returns>True if the warehouse was retrieved, false otherwise.</returns>
		public bool TryGetWarehouse(uint warehouseId, out Warehouse warehouse)
		{
			return _warehouses.TryGetValue(warehouseId, out warehouse);
		}
		
		/// <summary>
		/// Attempts to create a warehouse.
		/// </summary>
		/// <param name="warehouseId">The id of the warehouse.</param>
		/// <returns>True if the warehouse was created.</returns>
		public bool CreateWarehouse(uint warehouseId)
		{
			var warehouse = new Warehouse(warehouseId, Player);
			return _warehouses.TryAdd(warehouse.Id, warehouse);
		}
	}
}
