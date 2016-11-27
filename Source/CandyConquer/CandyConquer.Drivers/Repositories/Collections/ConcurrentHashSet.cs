// Project by Bauss
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace CandyConquer.Drivers.Repositories.Collections
{
	/// <summary>
	/// A concurrent hash set.
	/// </summary>
	public sealed class ConcurrentHashSet<T>
	{
		/// <summary>
		/// The hash set.
		/// </summary>
		private ConcurrentDictionary<T,byte> _hashSet;
		
		/// <summary>
		/// Creates a new concurrent hash set.
		/// </summary>
		public ConcurrentHashSet()
		{
			_hashSet = new ConcurrentDictionary<T, byte>();
		}
		
		/// <summary>
		/// Attempts to add an item to the hash set.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <returns>True if the item was added, false otherwise.</returns>
		public bool TryAdd(T item)
		{
			return _hashSet.TryAdd(item, 0);
		}
		
		/// <summary>
		/// Attempts to add a range of items to the collection.
		/// </summary>
		/// <param name="collection">The collection of items.</param>
		/// <returns>True if the items were added, false otherwise.</returns>
		public bool TryAddRange(IEnumerable<T> collection)
		{
			bool success = true;
			
			foreach (var item in collection)
			{
				if (!TryAdd(item))
				{
					success = false;
					_hashSet.Clear();
					break;
				}
			}
			
			return success;
		}
		
		/// <summary>
		/// Checks whether the hash set contains a specific item or not.
		/// </summary>
		/// <param name="item">The item to validate.</param>
		/// <returns>True if the item exists within the hash set, false otherwise.</returns>
		public bool Contains(T item)
		{
			return _hashSet.ContainsKey(item);
		}
		
		/// <summary>
		/// Attempts to remove an item from the hash set.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		/// <returns>True if the item was removed, false otherwise.</returns>
		public bool TryRemove(T item)
		{
			byte unused;
			return _hashSet.TryRemove(item, out unused);
		}
		
		/// <summary>
		/// Clears the hash set.
		/// </summary>
		public void Clear()
		{
			_hashSet.Clear();
		}
		
		/// <summary>
		/// Gets all hashes in the hash set.
		/// </summary>
		/// <returns></returns>
		public ICollection<T> GetHashes()
		{
			return _hashSet.Keys;
		}
	}
}
