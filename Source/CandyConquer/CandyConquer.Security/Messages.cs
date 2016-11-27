// Project by Bauss
using System;

namespace CandyConquer.Security
{
	/// <summary>
	/// A collection of all messages.
	/// </summary>
	public static class Messages
	{
		/// <summary>
		/// Error messages.
		/// All error messages must be prefixed with [ERROR_NANONAME-ERROR_ID]
		/// Ex. [KE-1] for Key-Exchange error 1.
		/// </summary>
		public static class Errors
		{
			public const string KEYEXCHANGE_FAIL =
				"[KE-1]Failed to exchange cryptography keys.";
			public const string RC5_INVALID_DATA_LENGTH =
				"[RC5-1]Invalid data length. Must be 16 bytes";
			public const string RC5_INVALID_BLOCK_SIZE =
				"[RC5-2]Invalid block size.";
			public const string RC5_INVALID_PASSWORD_SIZE =
				"[RC5-3]Invalid password size.";
			public const string RC5_INVALID_PASSWORD_SIZE_8 =
				"[RC5-4]Invalid password length. Must be multiple of 8";
			public const string RC5_INVALID_PASSWORD_SIZE_0 =
				"[RC5-5]Invalid password length. Must be greater than 0 bytes.";
		}
	}
}
