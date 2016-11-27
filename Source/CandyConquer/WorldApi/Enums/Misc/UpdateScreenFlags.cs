// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Enums
{
	/// <summary>
	/// Enumeration of update screen flags.
	/// </summary>
	public enum UpdateScreenFlags
	{
		/// <summary>
		/// None.
		/// </summary>
		None,
		/// <summary>
		/// Updates mapobjects with a flag indicating they can do stuff.
		/// This is used to keep an idle state of a mapobject.
		/// Mainly used for monsters.
		/// </summary>
		Idle,
		/// <summary>
		/// Updates the screen with the packet for dead players only.
		/// </summary>
		DeadPlayers
	}
}
