// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Packets.Message;

namespace CandyConquer.WorldApi.Controllers.Threads
{
	/// <summary>
	/// The broadcast thread controller.
	/// </summary>
	public static class BroadcastThreadController
	{
		/// <summary>
		/// The next broadcast time.
		/// </summary>
		private static DateTime _nextBroadcast = DateTime.UtcNow;
		
		/// <summary>
		/// Handles the broadcast thread.
		/// </summary>
		public static void Handle()
		{
			if (DateTime.UtcNow >= _nextBroadcast)
			{
				MessagePacket broadcast;
				if (Collections.BroadcastQueue.TryGetBroadcast(out broadcast))
				{
					_nextBroadcast = DateTime.UtcNow.AddMilliseconds(Data.Constants.Time.BroadcastWaitTime);
					
					Collections.PlayerCollection.Broadcast(broadcast);
				}
			}
		}
	}
}
