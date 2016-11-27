// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Data.Constants
{
	/// <summary>
	/// Movement constants.
	/// </summary>
	public static class Movement
	{
		/// <summary>
		/// Delta x for movements.
		/// </summary>
		public static readonly sbyte[] DeltaX = new sbyte[] { 0, -1, -1, -1, 0, 1, 1, 1, 0 };
		/// <summary>
		/// Delta y for movements.
		/// </summary>
		public static readonly sbyte[] DeltaY = new sbyte[] { 1, 1, 0, -1, -1, -1, 0, 1, 0 };
		/// <summary>
		/// Delta mount x for movements.
		/// </summary>
		public static readonly sbyte[] DeltaMountX = new sbyte[] { 0, -2, -2, -2, 0, 2, 2, 2, 1, 0, -2, 0, 1, 0, 2, 0, 0, -2, 0, -1, 0, 2, 0, 1, 0 };
		/// <summary>
		/// Delta mount y for movements.
		/// </summary>
		public static readonly sbyte[] DeltaMountY = new sbyte[] { 2, 2, 0, -2, -2, -2, 0, 2, 2, 0, -1, 0, -2, 0, 1, 0, 0, 1, 0, -2, 0, -1, 0, 2, 0 };
	}
}
