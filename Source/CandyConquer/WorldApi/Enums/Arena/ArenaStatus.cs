// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Enums
{
	/// <summary>
	/// Enumeration of arena statuses.
	/// </summary>
	public enum ArenaStatus
	{
		/// <summary>
		/// NotSignedUp
		/// </summary>
		NotSignedUp = 0,
		/// <summary>
		/// WaitingForOpponent
		/// </summary>
		WaitingForOpponent = 1,
		/// <summary>
		/// WaitingInactive
		/// </summary>
		WaitingInactive = 2,
		/// <summary>
		/// InBattle
		/// </summary>
		InBattle = 255
	}
}
