// TournamentQueueThread
using System;
using System.Threading.Tasks;
using System.Linq;
using CandyConquer.Drivers.Repositories.Collections;

namespace CandyConquer.WorldApi.Controllers.Threads
{
	/// <summary>
	/// The tournament queue thread handler.
	/// </summary>
	public static class TournamentQueueThread
	{
		/// <summary>
		/// Collection of the tournament queue.
		/// </summary>
		private static ConcurrentList<Models.Tournaments.ITournamentBase> _queue;
		
		/// <summary>
		/// Gets or sets a boolean determining whether the tournaments should be assembled.
		/// </summary>
		public static bool ShouldAssemble { get; set; }
		
		/// <summary>
		/// Static constructor for TournamentQueueThread.
		/// </summary>
		static TournamentQueueThread()
		{
			_queue = new ConcurrentList<Models.Tournaments.ITournamentBase>();
		}
		
		/// <summary>
		/// Adds a tournament to the queue.
		/// </summary>
		/// <param name="tournament">The tournament.</param>
		public static void Add(Models.Tournaments.ITournamentBase tournament)
		{
			_queue.TryAdd(tournament);
		}
		
		/// <summary>
		/// Handles all tournaments that should be queued by time.
		/// </summary>
		public static void HandleTournaments()
		{
			foreach (var tournament in Collections.TournamentCollection.GetAllTournaments()
			         .Where(tournament => tournament.TournamentType == Enums.TournamentType.Time && !tournament.Running))
			{
				var time = DateTime.UtcNow;
				
				if (tournament.IsWeekly &&
				    time.DayOfWeek != tournament.DayOfWeek)
				{
					continue;
				}
				
				if (time.Hour == tournament.Hour &&
				    time.Minute == tournament.Minute)
				{
					RunNewTournament(tournament);
				}
			}
		}
		
		/// <summary>
		/// Starts the tournament queue.
		/// </summary>
		public static void Start()
		{
			Task.Run(async() => await HandleTournamentQueueAsync());
		}
		
		/// <summary>
		/// Handles queued tournaments asynchronously.
		/// </summary>
		private static async Task HandleTournamentQueueAsync()
		{
			try
			{
				if (ShouldAssemble)
				{
					ShouldAssemble = false;
					
					_queue.Clear();
					Collections.TournamentScriptCollection.Assemble();
					Start();
					return;
				}
				
				foreach (var tournament in _queue.GetItems())
				{
					if (ShouldAssemble)
					{
						ShouldAssemble = false;
						
						_queue.Clear();
						Collections.TournamentScriptCollection.Assemble();
						break;
					}
					
					tournament.Running = true;
					await RunTournamentAsync(tournament, true);
				}
			}
			catch (Exception e)
			{
				Drivers.Global.RaiseException(e);
			}
			
			await Task.Delay(100);
			Start();
		}
		
		/// <summary>
		/// Runs a new tournament.
		/// </summary>
		/// <param name="tournament">The tournament.</param>
		public static void RunNewTournament(Models.Tournaments.ITournamentBase tournament)
		{
			if (!tournament.Running)
			{
				tournament.Running = true;
				
				Task.Run(async() => await RunTournamentAsync(tournament, false));
			}
		}
		
		/// <summary>
		/// Runs a tournament asynchronously.
		/// </summary>
		/// <param name="tournament">The tournament.</param>
		/// <param name="queueBased">Boolean determining whether the tournament is queue based.</param>
		private static async Task RunTournamentAsync(Models.Tournaments.ITournamentBase tournament, bool queueBased)
		{
			try
			{
				tournament.OnSignUp();
				Collections.PlayerCollection.BroadcastFormattedMessage("TOURNAMENT_SIGNUP", tournament.Name);
				
				await Task.Delay(60000);
				tournament.OnSend();
				Collections.PlayerCollection.BroadcastFormattedMessage("TOURNAMENT_SEND", tournament.Name);
				
				if (tournament.Timeout > 0)
				{
					int timeOut = tournament.Timeout;
					
					while (timeOut > 0)
					{
						await Task.Delay(100);
						timeOut -= 100;
						
						tournament.OnTime();
					}
					
					tournament.OnEndTimeOut();
				}
				else
				{
					while (!tournament.Ended)
					{
						await Task.Delay(100);
						
						tournament.OnTime();
					}
				}
				
				tournament.Running = false;
				
				if (queueBased)
				{
					await Task.Delay(Data.Constants.Time.TournamentQueueDelayTime);
				}
			}
			catch (Exception e)
			{
				Drivers.Global.RaiseException(e);
			}
		}
	}
}
