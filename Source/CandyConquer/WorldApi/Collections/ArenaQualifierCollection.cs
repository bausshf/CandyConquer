// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// Collection of arena qualifiers.
	/// </summary>
	public static class ArenaQualifierCollection
	{
		/// <summary>
		/// The arena qualifier collection.
		/// </summary>
		private static ConcurrentDictionary<int, Models.Arena.ArenaInfo> _qualifiers;
		
		/// <summary>
		/// Static constructor for ArenaQualifierCollection.
		/// </summary>
		static ArenaQualifierCollection()
		{
			_qualifiers = new ConcurrentDictionary<int, Models.Arena.ArenaInfo>();
		}
		
		/// <summary>
		/// Loads all arena qualifiers.
		/// </summary>
		public static void Load()
		{
			var qualifiers = Database.Dal.PlayerArenaQualifiers.GetAll(Drivers.Settings.WorldSettings.Server);
			
			foreach (var dbQualifier in qualifiers)
			{
				_qualifiers.TryAdd(dbQualifier.PlayerId, new Models.Arena.ArenaInfo(dbQualifier));
			}
		}
		
		/// <summary>
		/// Attempts to add a new arena qualifier.
		/// </summary>
		/// <param name="playerId">The player id.</param>
		/// <param name="arenaInfo">The arena qualifier.</param>
		/// <returns>True if the qualifier was added.</returns>
		public static bool TryAddArenaInfo(int playerId, Models.Arena.ArenaInfo arenaInfo)
		{
			return _qualifiers.TryAdd(playerId, arenaInfo);
		}
		
		/// <summary>
		/// Attempts to get an arena qualifier.
		/// </summary>
		/// <param name="playerId">The player id.</param>
		/// <param name="arenaInfo">The qualifier.</param>
		/// <returns>True if the arena qualifier was retrieved.</returns>
		public static bool TryGetArenaInfo(int playerId, out Models.Arena.ArenaInfo arenaInfo)
		{
			return _qualifiers.TryGetValue(playerId, out arenaInfo);
		}
		
		/// <summary>
		/// Gets the top 10 arena qualifiers.
		/// </summary>
		/// <returns>A read only collection of the qualifiers.</returns>
		public static ReadOnlyCollection<Models.Arena.ArenaInfo> GetTop10()
		{
			var qualifiers = _qualifiers.Values
				.OrderByDescending(qualifier => qualifier.Ratio)
				.ToList();
			
			uint i = 0;
			foreach (var qualifier in qualifiers)
			{
				qualifier.Ranking = i;
				i++;
			}
			
			return qualifiers.Take(Math.Min(10, qualifiers.Count)).ToList().AsReadOnly();;
		}
	}
}
