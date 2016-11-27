// Project by Bauss
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Candy
{
	/// <summary>
	/// Data extensions.
	/// </summary>
	public static class DataExtensions
	{
		/// <summary>
		/// Adds a range of items to an ICollection.
		/// </summary>
		/// <param name="collection">The ICollection.</param>
		/// <param name="items">The range of items.</param>
		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				collection.Add(item);
			}
		}
		
		/// <summary>
		/// Gets a custom attribute for a property.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns>The attribute if found, null otherwise.</returns>
		public static T GetCustomAttribute<T>(this PropertyInfo property)
			where T : Attribute
		{
			return property.GetCustomAttributes(typeof(T), false)
						.FirstOrDefault() as T;
		}
	}
}
