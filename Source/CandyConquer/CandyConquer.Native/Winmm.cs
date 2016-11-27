// Project by Bauss
using System;
using System.Runtime.InteropServices;

namespace CandyConquer.Native
{
	/// <summary>
	/// winmm.dll
	/// </summary>
	public static class Winmm
	{
		[DllImport(LibraryReferences.Winmm)]
		public static extern uint timeGetTime();
	}
}
