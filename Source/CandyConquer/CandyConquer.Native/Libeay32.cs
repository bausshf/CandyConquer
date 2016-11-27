// Project by Bauss
using System;
using System.Runtime.InteropServices;

namespace CandyConquer.Native
{
	/// <summary>
	/// libeay32.dll
	/// </summary>
	public static class Libeay32
	{
		[DllImport(LibraryReferences.Libeay32, CallingConvention = CallingConvention.Cdecl)]
		public extern static void CAST_set_key(IntPtr _key, int len, byte[] data);

		[DllImport(LibraryReferences.Libeay32, CallingConvention = CallingConvention.Cdecl)]
		public extern static void CAST_ecb_encrypt(byte[] in_, byte[] out_, IntPtr schedule, int enc);

		[DllImport(LibraryReferences.Libeay32, CallingConvention = CallingConvention.Cdecl)]
		public extern static void CAST_cbc_encrypt(byte[] in_, byte[] out_, int length, IntPtr schedule, byte[] ivec, int enc);

		[DllImport(LibraryReferences.Libeay32, CallingConvention = CallingConvention.Cdecl)]
		public extern static void CAST_cfb64_encrypt(byte[] in_, byte[] out_, int length, IntPtr schedule, byte[] ivec, ref int num, int enc);

		[DllImport(LibraryReferences.Libeay32, CallingConvention = CallingConvention.Cdecl)]
		public extern static void CAST_ofb64_encrypt(byte[] in_, byte[] out_, int length, IntPtr schedule, byte[] ivec, out int num);
	}
}
