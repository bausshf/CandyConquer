// Project by Bauss
using System;

namespace CandyConquer.Security.Api
{
	/// <summary>
	/// The sub call state values.
	/// </summary>
	public static class SubCallState
	{
		/// <summary>
		/// A value indicating not to handle sub calls.
		/// </summary>
		public static readonly uint DontHandle = uint.MaxValue;
		
		/// <summary>
		/// A value indicating the call was invalid.
		/// </summary>
		public static readonly uint Invalid = uint.MaxValue - 1;
	}
}
