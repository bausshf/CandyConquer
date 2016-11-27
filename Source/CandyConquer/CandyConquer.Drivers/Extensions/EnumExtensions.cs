// Project by Bauss
using System;

namespace CandyConquer.Drivers
{
	/// <summary>
	/// Extensions for enums.
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Converts a string to an enum.
		/// </summary>
		/// <param name="name">The string to convert.</param>
		/// <returns>The enum if converted, default enum value otherwise.</returns>
		public static T ToEnum<T>(this string name)
			where T : struct
		{
			T result;
			if (!Enum.TryParse<T>(name, out result))
			{
				result = default(T);
			}
			return result;
		}
	}
}
