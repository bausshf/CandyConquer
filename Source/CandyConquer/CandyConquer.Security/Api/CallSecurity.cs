// Project by Bauss
using System;

namespace CandyConquer.Security.Api
{
	/// <summary>
	/// Call security for api calls.
	/// </summary>
	public enum CallSecurity
	{
		/// <summary>
		/// The call can be done always.
		/// </summary>
		Always,
		/// <summary>
		/// The call can only be done when the client isn't idle.
		/// </summary>
		NonIdle,
		/// <summary>
		/// The call can only be done when the client is idle.
		/// </summary>
		Idle,
		/// <summary>
		/// The call may only be done once.
		/// </summary>
		Once
	}
}
