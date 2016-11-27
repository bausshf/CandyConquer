// Project by Bauss
using System;
using System.Linq;
using System.Reflection;

namespace CandyConquer.Drivers
{
	/// <summary>
	/// Extensions for attributes.
	/// </summary>
	public static class AttributeExtensions
	{
		/// <summary>
		/// Gets a custom type attribute for a specific type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The custom attribute if found, otherwise null.</returns>
		public static T GetCustomTypeAttribute<T>(this Type type)
			where T : Attribute
		{
			return type.GetCustomAttributes(typeof(T), false)
						.FirstOrDefault() as T;
		}
		
		/// <summary>
		/// Gets a custom type attribute for a specific property type.
		/// </summary>
		/// <param name="type">The property type.</param>
		/// <returns>The custom attribute if found, otherwise null.</returns>
		public static T GetCustomTypeAttribute<T>(this PropertyInfo type)
			where T : Attribute
		{
			return type.GetCustomAttributes(typeof(T), false)
						.FirstOrDefault() as T;
		}
		
		/// <summary>
		/// Gets a custom type attribute for a specific method type.
		/// </summary>
		/// <param name="type">The method type.</param>
		/// <returns>The custom attribute if found, otherwise null.</returns>
		public static T GetCustomTypeAttribute<T>(this MethodInfo type)
			where T : Attribute
		{
			return type.GetCustomAttributes(typeof(T), false)
						.FirstOrDefault() as T;
		}
	}
}
