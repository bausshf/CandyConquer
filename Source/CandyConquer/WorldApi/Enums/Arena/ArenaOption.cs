// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Enums
{
	/// <summary>
	/// Enumeration of arena options.
	/// </summary>
	public enum ArenaOption
	{
		#region Server -> Client
		/// <summary>
		/// Lose.
		/// </summary>
		Lose = 0,
		/// <summary>
		/// Win.
		/// </summary>
		Win = 1,
		/// <summary>
		/// MatchOff.
		/// </summary>
		MatchOff = 0,
		/// <summary>
		/// MatchOn.
		/// </summary>
		MatchOn = 5,
		#endregion
		
		#region Client -> Server
		/// <summary>
		/// AltJoin
		/// </summary>
		AltJoin = 0,
		/// <summary>
		/// Accept
		/// </summary>
		Accept = 1,
		/// <summary>
		/// GiveUp
		/// </summary>
		GiveUp = 2
		#endregion
	}
}
