// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Models.Maps
{
	/// <summary>
	/// Coordinate wrapper.
	/// </summary>
	public sealed class Coordinate
	{
		/// <summary>
		/// Gets the x coordinate.
		/// </summary>
		public ushort X { get; private set; }
		/// <summary>
		/// Gets the y coordinate.
		/// </summary>
		public ushort Y { get; private set; }
		
		/// <summary>
		/// Gets a boolean indicating whether the coordinate is valid or not.
		/// </summary>
		public bool Valid
		{
			get
			{
				return X > 0 && Y > 0;
			}
		}
		
		/// <summary>
		/// Creates a new coordinate.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public Coordinate(ushort x = 0, ushort y = 0)
		{
			X = x;
			Y = y;
		}
	}
}
