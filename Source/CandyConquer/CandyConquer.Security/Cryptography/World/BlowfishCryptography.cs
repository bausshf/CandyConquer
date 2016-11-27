// Project by Bauss
using System;
using System.Runtime.InteropServices;
using CandyConquer.Native;

namespace CandyConquer.Security.Cryptography.World
{
	/// <summary>
	/// Blowfish cryptography wrapper.
	/// </summary>
	public class BlowfishCryptography : ICryptography, IDisposable
	{
		/// <summary>
		/// Enumeration for blowfish algorithm.
		/// </summary>
		public enum BlowfishAlgorithm
		{
			/// <summary>
			/// Electronic codebook.
			/// </summary>
			ECB,
			
			/// <summary>
			/// Cipher-block chaining.
			/// </summary>
			CBC,
			
			/// <summary>
			/// Cipher feedback.
			/// </summary>
			CFB64,
			
			/// <summary>
			/// Output feedback.
			/// </summary>
			OFB64,
		}
		
		[StructLayout(LayoutKind.Sequential)]
		struct bf_key_st
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
			public UInt32[] P;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
			public UInt32[] S;
		}
		
		/// <summary>
		/// The algorithm.
		/// </summary>
		private BlowfishAlgorithm _algorithm;
		/// <summary>
		/// The key.
		/// </summary>
		private IntPtr _key;
		/// <summary>
		/// The encrypt Iv.
		/// </summary>
		private byte[] _encryptIv;
		/// <summary>
		/// The decrypt Iv.
		/// </summary>
		private byte[] _decryptIv;
		/// <summary>
		/// The encrypt num.
		/// </summary>
		private int _encryptNum;
		/// <summary>
		/// The decrypt num.
		/// </summary>
		private int _decryptNum;
		
		/// <summary>
		/// Creates a new blowfish cryptography.
		/// </summary>
		/// <param name="algorithm">The algorithm.</param>
		public BlowfishCryptography(BlowfishAlgorithm algorithm)
		{
			_algorithm = algorithm;
			_encryptIv = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
			_decryptIv = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
			bf_key_st key = new bf_key_st();
			key.P = new UInt32[16 + 2];
			key.S = new UInt32[4 * 256];
			_key = Marshal.AllocHGlobal(key.P.Length * sizeof(UInt32) + key.S.Length * sizeof(UInt32));
			Marshal.StructureToPtr(key, _key, false);
			_encryptNum = 0;
			_decryptNum = 0;
		}
		
		/// <summary>
		/// Disposes the cryptography.
		/// </summary>
		public void Dispose()
		{
			Marshal.FreeHGlobal(_key);
		}
		
		/// <summary>
		/// Sets the key for the cryptography.
		/// </summary>
		/// <param name="data">The key</param>
		public void SetKey(byte[] data)
		{
			_encryptNum = 0;
			_decryptNum = 0;
			Libeay32.CAST_set_key(_key, data.Length, data);
		}
		
		/// <summary>
		/// Encrypts a buffer.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <returns>An encrypted buffer.</returns>
		public byte[] Encrypt(byte[] buffer)
		{
			byte[] ret = new byte[buffer.Length];
			switch (_algorithm)
			{
				case BlowfishAlgorithm.ECB:
					Libeay32.CAST_ecb_encrypt(buffer, ret, _key, 1);
					break;
				case BlowfishAlgorithm.CBC:
					Libeay32.CAST_cbc_encrypt(buffer, ret, buffer.Length, _key, _encryptIv, 1);
					break;
				case BlowfishAlgorithm.CFB64:
					Libeay32.CAST_cfb64_encrypt(buffer, ret, buffer.Length, _key, _encryptIv, ref _encryptNum, 1);
					break;
				case BlowfishAlgorithm.OFB64:
					Libeay32.CAST_ofb64_encrypt(buffer, ret, buffer.Length, _key, _encryptIv, out _encryptNum);
					break;
			}
			return ret;
		}
		
		/// <summary>
		/// Decrypts a buffer.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <returns>The decrypted buffer.</returns>
		public byte[] Decrypt(byte[] buffer)
		{
			byte[] ret = new byte[buffer.Length];
			switch (_algorithm)
			{
				case BlowfishAlgorithm.ECB:
					Libeay32.CAST_ecb_encrypt(buffer, ret, _key, 0);
					break;
				case BlowfishAlgorithm.CBC:
					Libeay32.CAST_cbc_encrypt(buffer, ret, buffer.Length, _key, _decryptIv, 0);
					break;
				case BlowfishAlgorithm.CFB64:
					Libeay32.CAST_cfb64_encrypt(buffer, ret, buffer.Length, _key, _decryptIv, ref _decryptNum, 0);
					break;
				case BlowfishAlgorithm.OFB64:
					Libeay32.CAST_ofb64_encrypt(buffer, ret, buffer.Length, _key, _decryptIv, out _decryptNum);
					break;
			}
			return ret;
		}
		
		/// <summary>
		/// Gets or sets the encrypt IV.
		/// </summary>
		public byte[] EncryptIV
		{
			get { return _encryptIv; }
			set { System.Buffer.BlockCopy(value, 0, _encryptIv, 0, 8); }
		}
		
		/// <summary>
		/// Gets or sets the decrypt IV.
		/// </summary>
		public byte[] DecryptIV
		{
			get { return _decryptIv; }
			set { System.Buffer.BlockCopy(value, 0, _decryptIv, 0, 8); }
		}
	}
}
