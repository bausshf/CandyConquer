// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Enums
{
	/// <summary>
	/// Enumeration of team actions.
	/// </summary>
	public enum TeamAction
	{
		/// <summary>
		/// Create
		/// </summary>
		Create = 0,
		/// <summary>
		/// RequestJoin
		/// </summary>
        RequestJoin,
        /// <summary>
        /// LeaveTeam
        /// </summary>
        LeaveTeam,
        /// <summary>
        /// AcceptInvite
        /// </summary>
        AcceptInvite,
        /// <summary>
        /// RequestInvite
        /// </summary>
        RequestInvite,
        /// <summary>
        /// AcceptJoin
        /// </summary>
        AcceptJoin,
        /// <summary>
        /// Dismiss
        /// </summary>
        Dismiss,
        /// <summary>
        /// Kick
        /// </summary>
        Kick,
        /// <summary>
        /// Leader
        /// </summary>
        Leader = 15
	}
}
