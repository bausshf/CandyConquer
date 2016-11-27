// Project by Bauss
using System;

namespace CandyConquer.Drivers
{
	/// <summary>
	/// Locks to be used.
	/// </summary>
	public static class Locks
	{
		/// <summary>
		/// Global lock used by all threads accessing global data.
		/// </summary>
		public static object GlobalLock = new Object();

		/// <summary>
		/// Network lock used internally by the sockets.
		/// </summary>
		public static object NetworkLock = new Object();
		
		/// <summary>
		/// Lock used internally for the identity generator.
		/// </summary>
		internal static object IdentityLock = new Object();
	}
}
