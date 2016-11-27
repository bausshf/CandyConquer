// Project by Bauss
using System;
using System.Collections.Generic;

namespace CandyConquer.Drivers.Repositories.Collections
{
	/// <summary>
	/// A collection of items chained together.
	/// </summary>
	public class Chain<T>
	{
		/// <summary>
		/// The list of items.
		/// </summary>
		private List<T> _list;
		
		/// <summary>
		/// The current index.
		/// </summary>
		private int _index;
		
		/// <summary>
		/// Creates a new chain.
		/// </summary>
		public Chain()
		{
			_list = new List<T>();
		}
		
		/// <summary>
		/// Adds an item to the chain.
		/// </summary>
		/// <param name="item">The item to add.</param>
		public void Add(T item)
		{
			_list.Add(item);
		}
		
		/// <summary>
		/// Gets the next item.
		/// </summary>
		public T Next
		{
			get
			{
				if (_list.Count == 0)
				{
					return default(T);
				}
				
				if (_index >= _list.Count)
				{
					_index = 0;
				}
				
				var item = _list[_index];
				_index++;
				return item;
			}
		}
	}
}
