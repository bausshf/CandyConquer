// Project by Bauss
using System;
using System.Collections.Concurrent;

namespace CandyConquer.WorldApi.Models.Arena
{
	/// <summary>
	/// Model for an arena battle.
	/// </summary>
	public sealed class ArenaBattle : Controllers.Arena.ArenaBattleController
	{
		/// <summary>
		/// Gets a collection of the battle's watchers.
		/// </summary>
		public ConcurrentDictionary<uint, Models.Entities.Player> Watchers { get; private set; }
		
		/// <summary>
		/// Arena battle player model.
		/// </summary>
		public class ArenaBattlePlayer
		{
			/// <summary>
			/// Gets or sets a boolean determining whether the player has accepted or not.
			/// </summary>
			public bool Accepted { get; set; }
			
			/// <summary>
			/// Gets or sets the damage of the player.
			/// </summary>
			public uint Damage { get; set; }
			
			/// <summary>
			/// Gets or sets the player associated with the battle player.
			/// </summary>
			public Models.Entities.Player Player { get; set; }
		}
		
		/// <summary>
		/// Gets the first player.
		/// </summary>
		public ArenaBattlePlayer Player1 { get; private set; }
		
		/// <summary>
		/// Gets the secondary player.
		/// </summary>
		public ArenaBattlePlayer Player2 { get; private set; }
		
		/// <summary>
		/// Gets or sets a boolean whether the battle has started or not.
		/// </summary>
		public bool Started { get; set; }
		
		/// <summary>
		/// Gets or sets a boolean whether the battle has ended already.
		/// </summary>
		public bool EndedAlready { get; set; }
		
		/// <summary>
		/// Gets or sets the time the match started.
		/// </summary>
		public DateTime MatchStartTime { get; set; }
		
		/// <summary>
		/// Gets a boolean determining whether the match has started or not.
		/// </summary>
		public bool MatchStarted
		{
			get { return (Started && DateTime.UtcNow >= MatchStartTime.AddMilliseconds(10000)); }
		}
		
		/// <summary>
		/// Creates a new arena battle.
		/// </summary>
		public ArenaBattle()
			: base()
		{
			Watchers = new ConcurrentDictionary<uint, CandyConquer.WorldApi.Models.Entities.Player>();
			
			Player1 = new ArenaBattle.ArenaBattlePlayer();
			Player2 = new ArenaBattle.ArenaBattlePlayer();
			
			ArenaBattle = this;
		}
	}
}
