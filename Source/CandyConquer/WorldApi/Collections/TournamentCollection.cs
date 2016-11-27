// Project by Bauss
using System;
using System.Linq;
using System.Collections.Generic;
using CandyConquer.Drivers.Repositories.Collections;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A collection of tournaments.
	/// </summary>
	public static class TournamentCollection
	{
		/// <summary>
		/// The tournament collection.
		/// </summary>
		private static ConcurrentList<Models.Tournaments.ITournamentBase> _tournaments;
		
		/// <summary>
		/// Static constructor for the tournaments.
		/// </summary>
		static TournamentCollection()
		{
			_tournaments = new ConcurrentList<Models.Tournaments.ITournamentBase>();
		}
		
		/// <summary>
		/// Adds a tournament to the collection.
		/// </summary>
		/// <param name="tournament">The tournament to add.</param>
		public static void Add(Models.Tournaments.ITournamentBase tournament)
		{
			int id = _tournaments.ItemCount;
			tournament.Id = id;
			_tournaments.TryAdd(tournament);
		}
		
		/// <summary>
		/// Clears all tournaments.
		/// </summary>
		public static void Clear()
		{
			_tournaments.Clear();
		}
		
		/// <summary>
		/// Gets a tournament by its id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>The tournament.</returns>
		public static Models.Tournaments.ITournamentBase GetTournamentById(int id)
		{
			return _tournaments.Where(tournament => tournament.Id == id).FirstOrDefault();
		}
		
		/// <summary>
		/// Gets all tournaments open for sign up.
		/// </summary>
		/// <returns>IEnumerable of all tournaments open for sign up.</returns>
		public static IEnumerable<Models.Tournaments.ITournamentBase> GetAllTournamentsForSignUp()
		{
			return _tournaments
				.Where(tournament => tournament.CanSignUp);
		}
		
		/// <summary>
		/// Gets all tournaments.
		/// </summary>
		/// <returns>IEnumerable of all tournaments.</returns>
		public static IEnumerable<Models.Tournaments.ITournamentBase> GetAllTournaments()
		{
			return _tournaments.GetItems()
				.OrderBy(tournament => (int)tournament.TournamentType);
		}
	}
}
