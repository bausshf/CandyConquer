// Project by Bauss
using System;
using System.Collections.Generic;
using System.Linq;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// A list that can be converted to a string..
	/// For some reason storing List<T> as variable causes a stackoverflow when compiling into memory LOL?
	/// </summary>
	public sealed class StringList<T> : List<T>
	{
		/// <summary>
		/// Ctor.
		/// </summary>
		public StringList()
			: base()
		{
			
		}
		
		/// <summary>
		/// Ctor.
		/// </summary>
		/// <param name="collection">The collection to initialize with.</param>
		public StringList(IEnumerable<T> collection)
			: base(collection)
		{
		}
		
		/// <summary>
		/// Adds an item to the string list.
		/// </summary>
		/// <param name="item">The item.</param>
		public void AddItem(T item)
		{
			if (!base.Contains(item))
			{
				base.Add(item);
			}
		}
		
		/// <summary>
		/// Gets the string equal to the list.
		/// </summary>
		/// <returns>A string equal to the list items. Null if the list is empty.</returns>
		public override string ToString()
		{
			if (base.Count == 0)
			{
				return null;
			}
			
			return string.Join(",", this);
		}

	}
}
