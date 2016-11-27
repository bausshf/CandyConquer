// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Enums
{
	/// <summary>
	/// Enumeration of npc dialog actions.
	/// </summary>
	public enum NpcDialogAction : byte
	{
		/// <summary>
		/// None
		/// </summary>
		None = 0,
		/// <summary>
		/// Text
		/// </summary>
		Text = 1,
		/// <summary>
		/// Link
		/// </summary>
        Link,
        /// <summary>
        /// Edit
        /// </summary>
        Edit,
        /// <summary>
        /// Pic
        /// </summary>
        Pic,
        /// <summary>
        /// ListLine
        /// </summary>
        ListLine,
        /// <summary>
        /// Popup
        /// </summary>
        Popup,
        /// <summary>
        /// Create
        /// </summary>
        Create = 100,
        /// <summary>
        /// Answer
        /// </summary>
        Answer,
        /// <summary>
        /// TaskId
        /// </summary>
        TaskId,
        /// <summary>
        /// PathNotes
        /// </summary>
        PatchNotes = 112
	}
}
