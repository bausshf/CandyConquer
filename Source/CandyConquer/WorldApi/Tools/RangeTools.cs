// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Tools
{
	/// <summary>
	/// Tools for range based math.
	/// </summary>
	public static class RangeTools
	{
		/// <summary>
		/// Checks if a distance is valid between two ranges.
		/// </summary>
		/// <param name="x1">The first x coord.</param>
		/// <param name="y1">The first y coord.</param>
		/// <param name="x2">The second x coord.</param>
		/// <param name="y2">The second y coord.</param>
		/// <returns>True if the distance is valid, false otherwise.</returns>
		public static bool ValidDistance(long x1, long y1, long x2, long y2)
		{
			return GetDistance(x1, y1, x2, y2) <= 18;
		}
		
		/// <summary>
		/// Checks if a distance is valid between two ranges.
		/// </summary>
		/// <param name="x1">The first x coord.</param>
		/// <param name="y1">The first y coord.</param>
		/// <param name="x2">The second x coord.</param>
		/// <param name="y2">The second y coord.</param>
		/// <returns>True if the distance is valid, false otherwise.</returns>
		public static bool ValidDistance(ulong x1, ulong y1, ulong x2, ulong y2)
		{
			return GetDistanceU(x1, y1, x2, y2) <= 18;
		}
		
		/// <summary>
		/// Checks if a distance is valid between two ranges.
		/// </summary>
		/// <param name="x1">The first x coord.</param>
		/// <param name="y1">The first y coord.</param>
		/// <param name="x2">The second x coord.</param>
		/// <param name="y2">The second y coord.</param>
		/// <returns>True if the distance is valid, false otherwise.</returns>
		public static bool ValidDistance(double x1, double y1, double x2, double y2)
		{
			return GetDistanceF(x1, y1, x2, y2) <= 18;
		}
		
		/// <summary>
		/// Gets the distance between two points.
		/// </summary>
		/// <param name="x1">The first x coord.</param>
		/// <param name="y1">The first y coord.</param>
		/// <param name="x2">The second x coord.</param>
		/// <param name="y2">The second y coord.</param>
		/// <returns>The distance between the two points.</returns>
		public static long GetDistance(long x1, long y1, long x2, long y2)
		{
			return (long)GetDistanceF(x1, y1, x2, y2);
		}
		
		/// <summary>
		/// Gets the distance between two points.
		/// </summary>
		/// <param name="x1">The first x coord.</param>
		/// <param name="y1">The first y coord.</param>
		/// <param name="x2">The second x coord.</param>
		/// <param name="y2">The second y coord.</param>
		/// <returns>The distance between the two points.</returns>
		public static ulong GetDistanceU(ulong x1, ulong y1, ulong x2, ulong y2)
		{
			return (ulong)GetDistanceF(x1, y1, x2, y2);
		}
		
		/// <summary>
		/// Gets the distance between two points.
		/// </summary>
		/// <param name="x1">The first x coord.</param>
		/// <param name="y1">The first y coord.</param>
		/// <param name="x2">The second x coord.</param>
		/// <param name="y2">The second y coord.</param>
		/// <returns>The distance between the two points.</returns>
		public static double GetDistanceF(double x1, double y1, double x2, double y2)
		{
			return Math.Sqrt(((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2)));
		}
		
		/// <summary>
		/// Gets the degree of two coordinates.
		/// </summary>
		/// <param name="x1">The first x coordinate.</param>
		/// <param name="x2">The second x coordinate.</param>
		/// <param name="y1">The first x coordinate.</param>
		/// <param name="y2">The second y coordinate.</param>
		/// <returns>The degree.</returns>
		public static int GetDegree(int x1, int x2, int y1, int y2)
		{
			int direction = 0;

			double AddX = x2 - x1;
			double AddY = y2 - y1;
			double r = (double)Math.Atan2(AddY, AddX);
			if (r < 0) r += (double)Math.PI * 2;

			direction = (int)(360 - (r * 180 / Math.PI));

			return direction;
		}
		
		/// <summary>
		/// Gets an angle.
		/// </summary>
		/// <param name="x1">The first x coordinate.</param>
		/// <param name="y1">The first y coordinate.</param>
		/// <param name="x2">The second x coordinate.</param>
		/// <param name="y2">The second y coordinate.</param>
		/// <returns>The angle.</returns>
		public static short GetAngle(ushort x1, ushort y1, ushort x2, ushort y2)
		{
			double r = Math.Atan2(y2 - y1, x2 - x1);
			if (r < 0)
				r += Math.PI * 2;
			return (short)Math.Round(r * 180 / Math.PI);
		}
		
		/// <summary>
		/// Gets a direction facing based on an angle.
		/// </summary>
		/// <param name="angle">The angle.</param>
		/// <returns>The direction.</returns>
		public static Enums.Direction GetFacing(short angle)
		{
			sbyte c_angle = (sbyte)((angle / 46) - 1);
			return (c_angle == -1) ? Enums.Direction.South : (Enums.Direction)c_angle;
		}
	}
}
