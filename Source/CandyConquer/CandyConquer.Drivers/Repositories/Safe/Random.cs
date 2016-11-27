// Project by Bauss
using System;
using CandyConquer.Security.Cryptography;

namespace CandyConquer.Drivers.Repositories.Safe
{
	/// <summary>
	/// Thread-safe random generator.
	/// </summary>
	public static class Random
	{
		/// <summary>
		/// Returns a random number.
		/// </summary>
		/// <returns>The random number.</returns>
		public static int Next()
		{
			return CryptographicRandom.Next();
		}
		
		/// <summary>
		/// Returns a random number based on a min and max value.
		/// </summary>
		/// <param name="minValue">The minimum value.</param>
		/// <param name="maxValue">The maximum value.</param>
		/// <returns>The random number.</returns>
		public static int Next(int minValue, int maxValue)
		{
			return CryptographicRandom.Next(minValue, maxValue);
		}
		
		/// <summary>
		/// Returns a random number based on a max value.
		/// </summary>
		/// <param name="maxValue">The maximum value.</param>
		/// <returns>The random number.</returns>
		public static int Next(int maxValue)
		{
			return CryptographicRandom.Next(maxValue);
		}
		
		/// <summary>
		/// Returns a big random number.
		/// </summary>
		/// <returns>The random number.</returns>
		public static long NextBig()
		{
			return CryptographicRandom.NextBig();
		}
		
		/// <summary>
		/// Returns a big random number based on a minimum value.
		/// The maximum value is (min * 3)
		/// </summary>
		/// <param name="min">The minimum value.</param>
		/// <returns>The random number.</returns>
		public static long NextBig(int min)
		{
			return CryptographicRandom.NextBig(min);
		}
		
		
		/// <summary>
		/// Fills the buffer with random bytes.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		public static void NextBytes(byte[] buffer)
		{
			CryptographicRandom.NextBytes(buffer);
		}
		
		/// <summary>
		/// Gets the next enum value.
		/// </summary>
		/// <param name="enumType">The enum type.</param>
		/// <returns>The next enum value.</returns>
		public static object NextEnum(Type enumType)
		{
			var array = Enum.GetValues(enumType);
			return array.GetValue(Next(0, array.Length));
		}
	}
}
