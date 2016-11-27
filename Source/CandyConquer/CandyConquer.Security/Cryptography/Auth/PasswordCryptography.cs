// Project by Bauss
using System;

namespace CandyConquer.Security.Cryptography.Auth
{
	/// <summary>
	/// The password cryptography.
	/// </summary>
	public class PasswordCryptography
	{
		/// <summary>
		/// The key buffer.
		/// </summary>
		private readonly byte[] key = new byte[0x200];
		
		/// <summary>
		/// The scan codes.
		/// </summary>
		private static byte[] scanCodeToVirtualKeyMap = new byte[] {
			0, 0x1b, 0x31, 50, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0xbd, 0xbb, 8, 9,
			0x51, 0x57, 0x45, 0x52, 0x54, 0x59, 0x55, 0x49, 0x4f, 80, 0xdb, 0xdd, 13, 0x11, 0x41, 0x53,
			0x44, 70, 0x47, 0x48, 0x4a, 0x4b, 0x4c, 0xba, 0xc0, 0xdf, 0x10, 0xde, 90, 0x58, 0x43, 0x56,
			0x42, 0x4e, 0x4d, 0xbc, 190, 0xbf, 0x10, 0x6a, 0x12, 0x20, 20, 0x70, 0x71, 0x72, 0x73, 0x74,
			0x75, 0x76, 0x77, 120, 0x79, 0x90, 0x91, 0x24, 0x26, 0x21, 0x6d, 0x25, 12, 0x27, 0x6b, 0x23,
			40, 0x22, 0x2d, 0x2e, 0x2c, 0, 220, 0x7a, 0x7b, 12, 0xee, 0xf1, 0xea, 0xf9, 0xf5, 0xf3,
			0, 0, 0xfb, 0x2f, 0x7c, 0x7d, 0x7e, 0x7f, 0x80, 0x81, 130, 0x83, 0x84, 0x85, 0x86, 0xed,
			0, 0xe9, 0, 0xc1, 0, 0, 0x87, 0, 0, 0, 0, 0xeb, 9, 0, 0xc2, 0
		};
		
		/// <summary>
		/// The virtual keys.
		/// </summary>
		private static byte[] virtualKeyToScanCodeMap = new byte[] {
			0, 0, 0, 70, 0, 0, 0, 0, 14, 15, 0, 0, 0x4c, 0x1c, 0, 0,
			0x2a, 0x1d, 0x38, 0, 0x3a, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
			0x39, 0x49, 0x51, 0x4f, 0x47, 0x4b, 0x48, 0x4d, 80, 0, 0, 0, 0x54, 0x52, 0x53, 0x63,
			11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0, 0, 0, 0, 0, 0,
			0, 30, 0x30, 0x2e, 0x20, 0x12, 0x21, 0x22, 0x23, 0x17, 0x24, 0x25, 0x26, 50, 0x31, 0x18,
			0x19, 0x10, 0x13, 0x1f, 20, 0x16, 0x2f, 0x11, 0x2d, 0x15, 0x2c, 0x5b, 0x5c, 0x5d, 0, 0x5f,
			0x52, 0x4f, 80, 0x51, 0x4b, 0x4c, 0x4d, 0x47, 0x48, 0x49, 0x37, 0x4e, 0, 0x4a, 0x53, 0x35,
			0x3b, 60, 0x3d, 0x3e, 0x3f, 0x40, 0x41, 0x42, 0x43, 0x44, 0x57, 0x58, 100, 0x65, 0x66, 0x67,
			0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 110, 0x76, 0, 0, 0, 0, 0, 0, 0, 0,
			0x45, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0x2a, 0x36, 0x1d, 0x1d, 0x38, 0x38, 0x6a, 0x69, 0x67, 0x68, 0x65, 0x66, 50, 0x20, 0x2e, 0x30,
			0x19, 0x10, 0x24, 0x22, 0x6c, 0x6d, 0x6b, 0x21, 0, 0, 0x27, 13, 0x33, 12, 0x34, 0x35,
			40, 0x73, 0x7e, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x1a, 0x56, 0x1b, 0x2b, 0x29,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0x71, 0x5c, 0x7b, 0, 0x6f, 90, 0,
			0, 0x5b, 0, 0x5f, 0, 0x5e, 0, 0, 0, 0x5d, 0, 0x62, 0, 0, 0, 0
		};
		
		/// <summary>
		/// Creates a new password cryptography.
		/// </summary>
		/// <param name="key">The key.</param>
		public PasswordCryptography(string key)
		{
			int seed = 0;
			foreach (byte num2 in System.Text.Encoding.ASCII.GetBytes(key))
			{
				seed += num2;
			}
			Native.Msvcrt.srand(seed);
			byte[] buffer = new byte[0x10];
			for (int i = 0; i < 0x10; i++)
			{
				buffer[i] = (byte)Native.Msvcrt.rand();
			}
			for (int j = 1; j < 0x100; j++)
			{
				this.key[j * 2] = (byte)j;
				this.key[(j * 2) + 1] = (byte)(j ^ buffer[j & 15]);
			}
			for (int k = 1; k < 0x100; k++)
			{
				for (int m = 1 + k; m < 0x100; m++)
				{
					if (this.key[(k * 2) + 1] < this.key[(m * 2) + 1])
					{
						this.key[k * 2] = (byte)(this.key[k * 2] ^ this.key[m * 2]);
						this.key[m * 2] = (byte)(this.key[m * 2] ^ this.key[k * 2]);
						this.key[k * 2] = (byte)(this.key[k * 2] ^ this.key[m * 2]);
						this.key[(k * 2) + 1] = (byte)(this.key[(k * 2) + 1] ^ this.key[(m * 2) + 1]);
						this.key[(m * 2) + 1] = (byte)(this.key[(m * 2) + 1] ^ this.key[(k * 2) + 1]);
						this.key[(k * 2) + 1] = (byte)(this.key[(k * 2) + 1] ^ this.key[(m * 2) + 1]);
					}
				}
			}
		}
		
		/// <summary>
		/// Decrypts a password.
		/// </summary>
		/// <param name="data">The password buffer.</param>
		/// <param name="length">The length.</param>
		/// <returns>The decrypted password.</returns>
		public byte[] Decrypt(byte[] data, int length)
		{
			byte[] buffer = new byte[length];
			for (int i = 0; i < length; i++)
			{
				bool flag = false;
				if (data[i] == 0)
				{
					return buffer;
				}
				byte index = this.key[data[i] * 2];
				if (index >= 0x80)
				{
					index = (byte)(this.key[data[i] * 2] - 0x80);
					flag = true;
				}
				buffer[i] = (byte)(buffer[i] + scanCodeToVirtualKeyMap[index]);
				if ((!flag && (buffer[i] >= 0x41)) && (buffer[i] <= 90))
				{
					buffer[i] = (byte)(buffer[i] + 0x20);
				}
			}
			return buffer;
		}
		
		/// <summary>
		/// Encrypts a password.
		/// </summary>
		/// <param name="data">The password buffer.</param>
		/// <param name="length">The length.</param>
		/// <returns>The password buffer.</returns>
		public byte[] Encrypt(byte[] data, int length)
		{
			byte[] buffer = new byte[length];
			for (int i = 0; i < length; i++)
			{
				byte num2 = data[i];
				if ((data[i] >= 0x61) && (data[i] <= 0x7a))
				{
					data[i] = (byte)(data[i] - 0x20);
				}
				byte num3 = virtualKeyToScanCodeMap[data[i]];
				if ((num2 >= 0x41) && (num2 <= 90))
				{
					num3 = (byte)(num3 + 0x80);
				}
				for (byte j = 0; j <= 0xff; j = (byte)(j + 1))
				{
					byte num5 = this.key[j * 2];
					if (num5 == num3)
					{
						buffer[i] = j;
						break;
					}
				}
			}
			return buffer;
		}
	}
}
