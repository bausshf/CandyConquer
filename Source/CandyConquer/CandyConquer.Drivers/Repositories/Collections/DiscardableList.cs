// Project by Bauss
using System;
using System.Collections.Generic;

namespace CandyConquer.Drivers.Repositories.Collections
{
	/// <summary>
	/// A list that discards previous members to make space when it meets its limit.
	/// </summary>
	public sealed class DiscardableList<T>
	{
		/// <summary>
		/// The maximum amount of items.
		/// </summary>
		private int _maxItems;
		
		/// <summary>
		/// The item queue.
		/// </summary>
		private Queue<T> _items;
		
		/// <summary>
		/// Creates a new discardable list.
		/// </summary>
		/// <param name="maxItems">The maximum amount of items that can be present in the list at a time.</param>
		public DiscardableList(int maxItems)
		{
			_maxItems = maxItems;
			
			_items = new Queue<T>();
		}
		
		/// <summary>
		/// Adds an item to the list and discards the oldest item if the list meets its limit.
		/// </summary>
		/// <param name="item">The item to add.</param>
		public void Add(T item)
		{
			if (_items.Count >= _maxItems)
			{
				_items.Dequeue();
			}
			
			_items.Enqueue(item);
		}
		
		/// <summary>
		/// Clones the list into an array.
		/// This does not perform a deep clone.
		/// </summary>
		/// <returns>A clone of the list as an array.</returns>
		public T[] Clone()
		{
			return _items.ToArray();
		}
	}
}
