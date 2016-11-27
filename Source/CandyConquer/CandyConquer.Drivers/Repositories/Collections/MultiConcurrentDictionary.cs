// Project by Bauss
using System;
using System.Collections.Concurrent;

namespace CandyConquer.Drivers.Repositories.Collections
{
	/// <summary>
	/// A concurrent dictionary storing a concurrent dictionary.
	/// </summary>
	public sealed class MultiConcurrentDictionary<TKey1, TKey2, TValue>
	{
		/// <summary>
		/// The internal dictionary.
		/// </summary>
		private ConcurrentDictionary<TKey1, ConcurrentDictionary<TKey2, TValue>> _dictionary;
		
		/// <summary>
		/// Creates a new multi concurrent dictionary.
		/// </summary>
		public MultiConcurrentDictionary()
		{
			_dictionary = new ConcurrentDictionary<TKey1, ConcurrentDictionary<TKey2, TValue>>();
		}
		
		/// <summary>
		/// Gets an internal concurrent dictionary by its key.
		/// </summary>
		public ConcurrentDictionary<TKey2, TValue> this[TKey1 key]
		{
			get { return _dictionary[key]; }
		}
		
		/// <summary>
		/// Attempts to add a concurrent dictionary.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The concurrent dictionary.</param>
		/// <returns>True if the dictionary was added, false otherwise.</returns>
		public bool TryAdd(TKey1 key, ConcurrentDictionary<TKey2, TValue> value)
		{
			return _dictionary.TryAdd(key, value);
		}
		
		/// <summary>
		/// Attempts to add a concurrent dictionary by a key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>True if the concurrent dictionary was added.</returns>
		public bool TryAdd(TKey1 key)
		{
			return TryAdd(key, new ConcurrentDictionary<TKey2, TValue>());
		}
		
		/// <summary>
		/// Attempts to remove a concurrent dictionary.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The removed dictionary.</param>
		/// <returns>True if the dictionary was removed.</returns>
		public bool TryRemove(TKey1 key, out ConcurrentDictionary<TKey2, TValue> value)
		{
			return _dictionary.TryRemove(key, out value);
		}
		
		/// <summary>
		/// Attempts to get a concurrent dictionary.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The concurrent dictionary.</param>
		/// <returns>True if the concurrent dictionary was retrieved.</returns>
		public bool TryGetValue(TKey1 key, out ConcurrentDictionary<TKey2, TValue> value)
		{
			return _dictionary.TryGetValue(key, out value);
		}
		
		/// <summary>
		/// Gets the count of dictionaries.
		/// </summary>
		public int Count
		{
			get { return _dictionary.Count; }
		}
		
		/// <summary>
		/// Checks if the dictionary contains a specific key.
		/// </summary>
		/// <param name="key1">The key.</param>
		/// <returns>True if the dictionary contains the key.</returns>
		public bool ContainsKey(TKey1 key1)
		{
			return _dictionary.ContainsKey(key1);
		}
	}
}
