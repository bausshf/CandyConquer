// Project by Bauss
using System;
using System.Runtime.InteropServices;

namespace CandyConquer.Native
{
	/// <summary>
	/// msvcrt.dll
	/// </summary>
	public static class Msvcrt
	{
		[DllImport(LibraryReferences.Msvcrt, EntryPoint = "srand", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
		public static extern void srand(int seed);

		[DllImport(LibraryReferences.Msvcrt, EntryPoint = "rand", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
		public static extern short rand();
	}
}
