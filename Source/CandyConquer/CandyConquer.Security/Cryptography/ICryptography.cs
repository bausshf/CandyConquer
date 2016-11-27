// Project by Bauss
using System;

namespace CandyConquer.Security.Cryptography
{
	/// <summary>
	/// Cryptography interface.
	/// </summary>
	public interface ICryptography
	{
		byte[] Encrypt(byte[] buffer);
		byte[] Decrypt(byte[] buffer);
	}
}
