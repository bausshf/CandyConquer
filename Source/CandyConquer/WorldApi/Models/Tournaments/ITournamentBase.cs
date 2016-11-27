// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Models.Tournaments
{
	/// <summary>
	/// Model for the base of a tournament.
	/// </summary>
	public interface ITournamentBase
	{
		/// <summary>
		/// Gets or sets the id of the tournament.
		/// </summary>
		int Id { get; set; }
		
		#region Info
		/// <summary>
		/// Gets the name of the tournament.
		/// </summary>
		string Name { get; }
		
		/// <summary>
		/// Gets the short name of the tournament.
		/// </summary>
		/// <remarks>Ex. TeamDeathMatch would have the short name TDM.</remarks>
		string ShortName { get; }
		
		/// <summary>
		/// Gets the description of the tournament.
		/// </summary>
		string Description { get; }
		#endregion
		
		#region Requirements
		/// <summary>
		/// Gets the required minimum level to join the tournament.
		/// </summary>
		byte RequiredLevel { get; }
		
		/// <summary>
		/// Gets the required job to join the tournament.
		/// Put: Unknown for all.
		/// </summary>
		Enums.Job RequiredJob { get; }
		
		/// <summary>
		/// Gets the required amount of reborns to join the tournament.
		/// </summary>
		byte RequiredReborns { get; }
		
		/// <summary>
		/// Gets a boolean whether players are required to be VIP to join the tournament.
		/// </summary>
		bool RequiredToBeVIP { get; }
		
		/// <summary>
		/// Gets a boolean whether players are required to be female to join the tournament.
		/// </summary>
		bool RequiredToBeFemale { get; }
		
		/// <summary>
		/// Gets a boolean whether players are required to be male to join the tournament.
		/// </summary>
		bool RequiredToBeMale { get; }
		
		/// <summary>
		/// Gets a boolean whether players are required to be in a guild to join the tournament.
		/// </summary>
		bool RequiredGuild { get; }
		
		/// <summary>
		/// Gets a boolean whether players are required to be guild leader to join the tournament.
		/// </summary>
		bool RequiredGuildLeader { get; }
		
		/// <summary>
		/// Gets a boolean whether players are required to be deputy leader to join the tournament.
		/// </summary>
		bool RequiredDeputyLeader { get; }
		
		/// <summary>
		/// Gets the minimum required team members to join the tournament.
		/// </summary>
		/// <remarks>Set to 0 if no team is required.</remarks>
		int MinimumTeamMembers { get; }
		#endregion
		
		#region Config
		/// <summary>
		/// Gets the type of the tournament.
		/// </summary>
		Enums.TournamentType TournamentType { get; }
		
		/// <summary>
		/// Gets the timeout period in milliseconds.
		/// </summary>
		/// <remarks>Set to 0 for no time out.</remarks>
		int Timeout { get; }
		
		/// <summary>
		/// Gets or sets a boolean determining whether players can sign up for this tournament currently.
		/// </summary>
		bool CanSignUp { get; set; }
		
		/// <summary>
		/// Gets or sets a boolean determining whether the tournament has ended or not.
		/// </summary>
		bool Ended { get; set; }
		
		/// <summary>
		/// Gets the map id of the tournament.
		/// </summary>
		int MapId { get; }
		
		/// <summary>
		/// Gets the reward of the tournament.
		/// </summary>
		TournamentReward Reward { get; }
		
		/// <summary>
		/// Gets or sets a boolean determining whether the tournament is weekly or not.
		/// </summary>
		bool IsWeekly { get; }
		
		/// <summary>
		/// If weekly, gets the day of the week the tournament should run.
		/// </summary>
		DayOfWeek DayOfWeek { get; }
		
		/// <summary>
		/// Gets the hour the tournament should run.
		/// </summary>
		int Hour { get; }
		
		/// <summary>
		/// Gets the minute the tournament should run.
		/// </summary>
		int Minute { get; }
		
		/// <summary>
		/// Gets or sets a boolean determining whether the tournament is currently running.
		/// </summary>
		bool Running { get; set; }
		#endregion
		
		#region Functions
		/// <summary>
		/// Handler for players signing up for the tournament.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <returns>The sign up response.</returns>
		TournamentSignUpResponse SignUp(Models.Entities.Player player);
		
		/// <summary>
		/// Handler for when the tournament should open up for sign ups.
		/// </summary>
		void OnSignUp();
		
		/// <summary>
		/// Handler for when signed up players should get sent to the tournament map.
		/// </summary>
		void OnSend();
		
		/// <summary>
		/// Handler for when the tournament times out and has to end.
		/// </summary>
		void OnEndTimeOut();
		
		/// <summary>
		/// Handler for timed events. (Executed every 100 MS)
		/// </summary>
		void OnTime();
		#endregion
	}
}
