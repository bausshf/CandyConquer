// Project by Bauss
using System;

namespace CandyConquer.Drivers
{
	/// <summary>
	/// Time management utilities.
	/// </summary>
	public static class Time
	{
		/// <summary>
		/// Unix epoch.
		/// </summary>
		private static DateTime _unixEpoch;
		
		/// <summary>
		/// Static constructor for Time.
		/// </summary>
		static Time()
		{
			_unixEpoch = new DateTime(1970, 1, 1);
		}
		
		/// <summary>
		/// Enumeration of time types.
		/// </summary>
		public enum TimeType
		{
			Millisecond = 0,
			Second,
			Minute,
			Hour,
			Day,
			DayTime,
			Stamp
		}
		
		/// <summary>
		/// Gets the system timestamp.
		/// </summary>
		/// <returns>The system timestamp.</returns>
		public static uint GetSystemTime()
		{
			return Native.Winmm.timeGetTime();
		}
		
		/// <summary>
		/// Gets an integer representation of a time.
		/// </summary>
		/// <param name="type">The time type.</param>
		/// <returns>The integer representation of the specific time.</returns>
		public static uint GetTime(TimeType type)
		{
			var time = 0u;

			switch (type)
			{
				case TimeType.Second:
					{
						time = (uint)(DateTime.UtcNow - _unixEpoch).TotalSeconds;
						break;
					}
				case TimeType.Minute:
					{
						var now = DateTime.UtcNow;
						time = (uint)(now.Year % 100 * 100000000 +
						              (now.Month + 1) * 1000000 +
						              now.Day * 10000 +
						              now.Hour * 100 +
						              now.Minute);
						break;
					}
				case TimeType.Hour:
					{
						var now = DateTime.UtcNow;
						time = (uint)(now.Year % 100 * 1000000 +
						              (now.Month + 1) * 10000 +
						              now.Day * 100 +
						              now.Hour);
						break;
					}
				case TimeType.Day:
					{
						var now = DateTime.UtcNow;
						time = (uint)(now.Year * 10000 +
						              now.Month * 100 +
						              now.Day);
						break;
					}
				case TimeType.DayTime:
					{
						var now = DateTime.UtcNow;
						time = (uint)(now.Hour * 10000 +
						              now.Minute * 100 +
						              now.Second);
						break;
					}
				case TimeType.Stamp:
					{
						var now = DateTime.UtcNow;
						time = (uint)((now.Month + 1) * 100000000 +
						              now.Day * 1000000 +
						              now.Hour * 10000 +
						              now.Minute * 100 +
						              now.Second);
						break;
					}
				default:
					{
						time = (uint)Environment.TickCount;
						break;
					}
			}

			return time;
		}
	}
}
