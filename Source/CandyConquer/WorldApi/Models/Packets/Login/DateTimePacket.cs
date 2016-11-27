// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Login
{
	/// <summary>
	/// Model for the date time packet.
	/// </summary>
	public sealed class DateTimePacket : NetworkPacket
	{
		/// <summary>
		/// Creates a new date time packet.
		/// </summary>
		public DateTimePacket()
			: base(36, 1033)
		{
		}
		
		/// <summary>
		/// Implicit conversion from the DateTimePacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](DateTimePacket packet)
		{
			packet.Offset = 8;
			var time = DateTime.UtcNow;
			packet.WriteInt32(time.Year - 1900);
			packet.WriteInt32(time.Month - 1);
			packet.WriteInt32(time.DayOfYear - 1);
			packet.WriteInt32(time.Day);
			packet.WriteInt32(time.Hour);
			packet.WriteInt32(time.Minute);
			packet.WriteInt32(time.Second);
			
			return packet.Buffer;
		}
	}
}
