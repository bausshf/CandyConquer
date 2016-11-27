// Project by Bauss
using System;
using System.Threading;

namespace CandyConquer.Security.Cryptography
{
	/// <summary>
	/// Thread-safe random generator.
	/// </summary>
	public static class CryptographicRandom
	{
		/// <summary>
		/// The random generator.
		/// </summary>
		private static ThreadLocal<Random> _rand;
		
		/// <summary>
		/// Static constructor for random.
		/// </summary>
		static CryptographicRandom()
		{
			_rand = new ThreadLocal<Random>(() => { return new System.Random(); });
		}
		
		/// <summary>
		/// Returns a random number.
		/// </summary>
		/// <returns>The random number.</returns>
		public static int Next()
		{
			return _rand.Value.Next();
		}
		
		/// <summary>
		/// Returns a random number based on a min and max value.
		/// </summary>
		/// <param name="minValue">The minimum value.</param>
		/// <param name="maxValue">The maximum value.</param>
		/// <returns>The random number.</returns>
		public static int Next(int minValue, int maxValue)
		{
			return _rand.Value.Next(minValue, maxValue);
		}
		
		/// <summary>
		/// Returns a random number based on a max value.
		/// </summary>
		/// <param name="maxValue">The maximum value.</param>
		/// <returns>The random number.</returns>
		public static int Next(int maxValue)
		{
			return _rand.Value.Next(maxValue);
		}
		
		/// <summary>
		/// Returns a big random number.
		/// </summary>
		/// <returns>The random number.</returns>
		public static long NextBig()
		{
			return Next() + Next() + Next();
		}
		
		/// <summary>
		/// Returns a big random number based on a minimum value.
		/// The maximum value is (min * 3)
		/// </summary>
		/// <param name="min">The minimum value.</param>
		/// <returns>The random number.</returns>
		public static long NextBig(int min)
		{
			return Next(min) + Next(min) + Next(min);
		}
		
		
		/// <summary>
		/// Fills the buffer with random bytes.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		public static void NextBytes(byte[] buffer)
		{
			_rand.Value.NextBytes(buffer);
		}
	}
}
