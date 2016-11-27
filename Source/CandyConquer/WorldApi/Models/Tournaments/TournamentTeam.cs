// Project by Bauss
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace CandyConquer.WorldApi.Models.Tournaments
{
	/// <summary>
	/// Model for a tournament team.
	/// </summary>
	public sealed class TournamentTeam : Controllers.Tournaments.TournamentTeamController
	{
		/// <summary>
		/// Gets or sets the points of the team.
		/// </summary>
		public int Points { get; set; }
		
		/// <summary>
		/// Gets or sets the kills of the team.
		/// </summary>
		public int Kills { get; set; }
		
		/// <summary>
		/// Gets a collection of the members in the team.
		/// </summary>
		internal ConcurrentDictionary<uint,Models.Entities.Player> Members { get; private set; }
		
		/// <summary>
		/// Gets the tournament associated with the team.
		/// </summary>
		public ITournamentBase Tournament { get; private set; }
		
		/// <summary>
		/// Gets the X coordinate of the team's spawn.
		/// </summary>
		public ushort X { get; private set; }
		
		/// <summary>
		/// Gets the Y coordinate of the team's spawn.
		/// </summary>
		public ushort Y { get; private set; }
		
		/// <summary>
		/// Gets the name of the team.
		/// </summary>
		public string Name { get; private set; }
		
		/// <summary>
		/// Gets the id of the team.
		/// </summary>
		public int Id { get; private set; }
		
		/// <summary>
		/// Creates a new tournament team.
		/// </summary>
		/// <param name="tournament">The tournament team.</param>
		/// <param name="id">The id.</param>
		/// <param name="name">The name.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public TournamentTeam(ITournamentBase tournament, int id, string name, ushort x, ushort y)
		{
			Members = new ConcurrentDictionary<uint,Models.Entities.Player>();
			Tournament = tournament;
			Id = id;
			Name = name;
			X = x;
			Y = y;
			
			Team = this;
		}
	}
}
