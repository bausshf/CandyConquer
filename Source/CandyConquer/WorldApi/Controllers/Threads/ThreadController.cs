// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Controllers.Threads
{
	/// <summary>
	/// Controller for all threads.
	/// </summary>
	public static class ThreadController
	{
		/// <summary>
		/// Starts all threads.
		/// </summary>
		public static void StartThreads()
		{
			Collections.ThreadCollection.Run(WeatherThreadController.Handle, 100);
			Collections.ThreadCollection.Run(PlayerThreadController.Handle, 500);
			Collections.ThreadCollection.Run(MonsterThreadController.Handle, 500);
			Collections.ThreadCollection.Run(BroadcastThreadController.Handle, 1000);
			Collections.ThreadCollection.Run(TournamentQueueThread.HandleTournaments, 10000);
			Collections.ThreadCollection.Run(DynamicDatabaseThreadController.Handle, 10000);
			
			TournamentQueueThread.Start();
		}
	}
}
