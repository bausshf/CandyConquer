// Project by Bauss
using System;

namespace CandyConquer.Security.Cryptography.Auth
{
	/// <summary>
	/// Cryptography counter for the authentication crypto.
	/// </summary>
	public class AuthCryptoCounter
	{
		/// <summary>
		/// The counter.
		/// </summary>
		public ushort Counter { get; set; }
		
		/// <summary>
		/// Creates a new instance of AuthCryptoCounter.
		/// </summary>
		/// <param name="counter">The initialization counter.</param>
		public AuthCryptoCounter(ushort counter = 0)
		{
			Counter = counter;
		}
		
		/// <summary>
		/// Gets the first key of the counter.
		/// </summary>
		public byte Key1
		{
			get { return (byte)(Counter & 0xFF); }
		}
		
		/// <summary>
		/// Gets the second key of the counter.
		/// </summary>
		public byte Key2
		{
			get { return (byte)(Counter >> 8); }
		}
	}
}
