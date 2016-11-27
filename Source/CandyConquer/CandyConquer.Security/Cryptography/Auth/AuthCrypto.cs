// Project by Bauss
using System;
using System.Threading;

namespace CandyConquer.Security.Cryptography.Auth
{
	/// <summary>
	/// The authentication crypto.
	/// </summary>
	public class AuthCrypto : ICryptography
	{
		#region Thread-local
		/// <summary>
		/// Thread local key buffer wrapper for copying.
		/// </summary>
		private struct KeyBuffer
		{
			/// <summary>
			/// The first key buffer.
			/// </summary>
			public byte[] keyBuffer1;
			
			/// <summary>
			/// The second key buffer.
			/// </summary>
			public byte[] keyBuffer2;
		}
		/// <summary>
		/// Thread local key buffer.
		/// </summary>
		private static ThreadLocal<KeyBuffer> keyBuffer;
		
		/// <summary>
		/// The initialization function for the thread local key buffer.
		/// </summary>
		/// <returns>The key buffer.</returns>
		private static KeyBuffer InitializeKeyBuffers()
		{
			var keyBuffer = new KeyBuffer();
			
			keyBuffer.keyBuffer1 = new byte[0x100];
			keyBuffer.keyBuffer2 = new byte[0x100];
			byte i_key1 = 0x9D;
			byte i_key2 = 0x62;
			for (int i = 0; i < 0x100; i++)
			{
				keyBuffer.keyBuffer1[i] = i_key1;
				keyBuffer.keyBuffer2[i] = i_key2;
				i_key1 = (byte)((0x0F + (byte)(i_key1 * 0xFA)) * i_key1 + 0x13);
				i_key2 = (byte)((0x79 - (byte)(i_key2 * 0x5C)) * i_key2 + 0x6D);
			}
			
			return keyBuffer;
		}
		
		/// <summary>
		/// Static constructor for AuthCrypto.
		/// </summary>
		static AuthCrypto()
		{
			keyBuffer = new ThreadLocal<AuthCrypto.KeyBuffer>(InitializeKeyBuffers);
		}
		#endregion
		
		/// <summary>
		/// The decrypt counter.
		/// </summary>
		private AuthCryptoCounter decryptCounter;
		/// <summary>
		/// The encrypt counter.
		/// </summary>
		private AuthCryptoCounter encryptCounter;
		
		/// <summary>
		/// The first key buffer.
		/// </summary>
		private byte[] keys1;
		/// <summary>
		/// The second key buffer.
		/// </summary>
		private byte[] keys2;
		
		/// <summary>
		/// Creates a new auth crypto.
		/// </summary>
		public AuthCrypto()
		{
			decryptCounter = new AuthCryptoCounter();
			encryptCounter = new AuthCryptoCounter();
			
			var keys = keyBuffer.Value;
			
			keys1 = new byte[keys.keyBuffer1.Length];
			System.Buffer.BlockCopy(keys.keyBuffer1, 0, keys1, 0, keys1.Length);
			
			keys2 = new byte[keys.keyBuffer2.Length];
			System.Buffer.BlockCopy(keys.keyBuffer2, 0, keys2, 0, keys2.Length);
		}
		
		/// <summary>
		/// Encrypts a buffer.
		/// </summary>
		/// <param name="buffer">The buffer to encrypt.</param>
		/// <returns>An encrypted buffer.</returns>
		public byte[] Encrypt(byte[] buffer)
		{
			byte[] encryptedBuffer = new byte[buffer.Length];
			System.Buffer.BlockCopy(buffer, 0, encryptedBuffer, 0, encryptedBuffer.Length);
			
			for (int i = 0; i < buffer.Length; i++)
			{
				encryptedBuffer[i] ^= (byte)0xAB;
				encryptedBuffer[i] = (byte)(encryptedBuffer[i] >> 4 | encryptedBuffer[i] << 4);
				encryptedBuffer[i] ^= (byte)(keys1[encryptCounter.Key1] ^ keys2[encryptCounter.Key2]);
				encryptCounter.Counter++;
			}
			
			return encryptedBuffer;
		}
		
		/// <summary>
		/// Decrypts a buffer.
		/// </summary>
		/// <param name="buffer">The buffer to decrypt.</param>
		/// <returns>A decrypted buffer.</returns>
		public byte[] Decrypt(byte[] buffer)
		{
			byte[] decryptedBuffer = new byte[buffer.Length];
			System.Buffer.BlockCopy(buffer, 0, decryptedBuffer, 0, decryptedBuffer.Length);
			
			for (int i = 0; i < decryptedBuffer.Length; i++)
			{
				decryptedBuffer[i] ^= (byte)0xAB;
				decryptedBuffer[i] = (byte)(decryptedBuffer[i] >> 4 | decryptedBuffer[i] << 4);
				decryptedBuffer[i] ^= (byte)(keys2[decryptCounter.Key2] ^ keys1[decryptCounter.Key1]);
				decryptCounter.Counter++;
			}
			
			return decryptedBuffer;
		}
	}
}
