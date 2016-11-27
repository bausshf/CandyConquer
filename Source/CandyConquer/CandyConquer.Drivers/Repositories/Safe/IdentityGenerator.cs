// Project by Bauss
using System;
using System.Collections.Generic;

namespace CandyConquer.Drivers.Repositories.Safe
{
	/// <summary>
	/// An identity creator.
	/// </summary>
	public static class IdentityGenerator
	{
		/// <summary>
		/// Client ids.
		/// </summary>
		private static SortedSet<uint> clientIds;
		
		/// <summary>
		/// Thecurrent dynamic map id.
		/// </summary>
		private static int _dynamicMapId;
		
		/// <summary>
		/// The current item id.
		/// </summary>
		private static uint _itemId;
		
		/// <summary>
		/// The current monster id.
		/// </summary>
		private static uint _monsterId;
		
		/// <summary>
		/// Static constructor for the identity generator.
		/// </summary>
		static IdentityGenerator()
		{
			clientIds = new SortedSet<uint>();
			_dynamicMapId = 1000000;
			_itemId = 1000000000;
			_monsterId = 400001;
		}
		
		/// <summary>
		/// Gets a unique client id.
		/// </summary>
		/// <returns>The unique client id.</returns>
		public static uint GetClientId()
		{
			lock (Locks.IdentityLock)
			{
				uint id;
				do
				{
					id = (uint)Random.Next(1000000, 500000000);
				}
				while(clientIds.Contains(id));
				clientIds.Add(id);
				return id;
			}
		}
		
		/// <summary>
		/// Gets a dynamic map id.
		/// </summary>
		/// <returns>The dynamic map id.</returns>
		public static int GetDynamicMapId()
		{
			lock (Locks.IdentityLock)
			{
				_dynamicMapId++;
				return _dynamicMapId;
			}
		}
		
		/// <summary>
		/// Gets an item id.
		/// </summary>
		/// <returns>The item id.</returns>
		public static uint GetItemId()
		{
			lock (Locks.IdentityLock)
			{
				_itemId++;
				return _itemId;
			}
		}
		
		/// <summary>
		/// Gets a monster id.
		/// </summary>
		/// <returns>The monster id.</returns>
		public static uint GetMonsterId()
		{
			lock (Locks.IdentityLock)
			{
				_monsterId++;
				return _monsterId;
			}
		}
	}
}
