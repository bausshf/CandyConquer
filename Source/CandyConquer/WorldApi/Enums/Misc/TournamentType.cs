// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Enums
{
	/// <summary>
	/// Enumeration of tournament types.
	/// </summary>
	public enum TournamentType
	{
		/// <summary>
		/// Queue based tournament.
		/// </summary>
		/// <remarks>Part of the queue based tournament system where tournaments run one after another in the tournament queue.</remarks>
		Queue,
		/// <summary>
		/// Time based tournament.
		/// </summary>
		/// <remarks>Part of the time based tournament queue where tournaments run according to specific times.</remarks>
		Time,
		/// <summary>
		/// Manually based tournament.
		/// </summary>
		/// <remarks>Tournament that must be started manual by ex. commands or npcs.</remarks>
		Manual
	}
}
