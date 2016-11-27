// Project by Bauss
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace CandyConquer.Drivers.Repositories.Collections
{
	/// <summary>
	/// A concurrent list.
	/// </summary>
	public sealed class ConcurrentList<T>
	{
		/// <summary>
		/// The list.
		/// </summary>
		private ConcurrentDictionary<int,T> _list;
		
		/// <summary>
		/// Creates a new concurrent list.
		/// </summary>
		public ConcurrentList()
		{
			_list = new ConcurrentDictionary<int,T>();
		}
		
		/// <summary>
		/// Attempts to add an item to the list.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <returns>True if the item was added, false otherwise.</returns>
		public bool TryAdd(T item)
		{
			return _list.TryAdd(_list.Count, item);
		}
		
		/// <summary>
		/// Attempts to add a range of items to the list.
		/// </summary>
		/// <param name="collection">The collection to add.</param>
		/// <returns>True if the items were added, false otherwise.</returns>
		public bool TryAddRange(IEnumerable<T> collection)
		{
			bool success = true;
			
			foreach (var item in collection)
			{
				if (!TryAdd(item))
				{
					success = false;
					_list.Clear();
					break;
				}
			}
			
			return success;
		}
		
		/// <summary>
		/// Clears the list.
		/// </summary>
		public void Clear()
		{
			_list.Clear();
		}
		
		/// <summary>
		/// Finds a range of items based on a predicate.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <returns>IEnumerable.</returns>
		public IEnumerable<T> Where(Func<T,bool> predicate)
		{
			return _list.Values.Where(predicate);
		}
		
		/// <summary>
		/// Counts an amount of items based on a predicate.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <returns>The amount of items in the list where the predicate returns true for.</returns>
		public int Count(Func<T,bool> predicate)
		{
			return _list.Values.Count(predicate);
		}
		
		/// <summary>
		/// Gets a collection of all the items in the list.
		/// </summary>
		/// <returns>ICollection.</returns>
		public ICollection<T> GetItems()
		{
			return _list.Values;
		}
		
		/// <summary>
		/// Gets the item count of the list.
		/// </summary>
		public int ItemCount
		{
			get { return _list.Count; }
		}
	}
}
