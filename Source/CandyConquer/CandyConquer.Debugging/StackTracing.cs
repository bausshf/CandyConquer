// Project by Bauss
using System;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CandyConquer.Debugging
{
	/// <summary>
	/// Stack tracing utilities.
	/// </summary>
	public class StackTracing
	{
		/// <summary>
		/// Gets the current method executed.
		/// </summary>
		/// <param name="stackLevel">The stack level frame.</param>
		/// <returns>The current method.</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static MethodBase GetCurrentMethod(int stackLevel = 1)
		{
			return new StackTrace().GetFrame(stackLevel).GetMethod();
		}
	}
}
