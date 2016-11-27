// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Enums
{
	/// <summary>
	/// Enumeration of arena dialogs.
	/// </summary>
	public enum ArenaDialog
	{
		#region Server -> Client
		/// <summary>
		/// ArenaIconOn
		/// </summary>
		ArenaIconOn = 0,
		/// <summary>
		/// ArenaIconOff
		/// </summary>
		ArenaIconOff = 1,
		/// <summary>
		/// StartCountDown
		/// </summary>
		StartCountDown = 2,
		/// <summary>
		/// OpponentGaveUp
		/// </summary>
		OpponentGaveUp = 4,
		/// <summary>
		/// Match
		/// </summary>
		Match = 6,
		/// <summary>
		/// YouAreKicked
		/// </summary>
		YouAreKicked = 7,
		/// <summary>
		/// StartTheFight
		/// </summary>
		StartTheFight = 8,
		/// <summary>
		/// Dialog
		/// </summary>
		Dialog = 9,
		/// <summary>
		/// EndMatchDialog
		/// </summary>
		EndMatchDialog = 10,
		#endregion
		
		#region Client -> Server
		/// <summary>
		/// Join
		/// </summary>
		Join = 0,
		/// <summary>
		/// QuitWait
		/// </summary>
		QuitWait = 1,
		/// <summary>
		/// AcceptGiveUp
		/// </summary>
		AcceptGiveUp = 3,
		/// <summary>
		/// Quit
		/// </summary>
		Quit = 4,
		/// <summary>
		/// JoinAlt1
		/// </summary>
		JoinAlt1 = 10,
		/// <summary>
		/// JoinAlt2
		/// </summary>
		JoinAlt2 = 11
		#endregion
	}
}
