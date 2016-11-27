// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Models.Tournaments
{
	/// <summary>
	/// Model for tournament sign up response.
	/// </summary>
	public sealed class TournamentSignUpResponse
	{
		/// <summary>
		/// Gets the message.
		/// </summary>
		public string Message { get; private set; }
		
		/// <summary>
		/// Gets a boolean determining whether player can sign up.
		/// </summary>
		public bool Success { get; private set; }
		
		/// <summary>
		/// Creates a new tournament sign up response.
		/// </summary>
		private TournamentSignUpResponse() { }
		
		/// <summary>
		/// Handles the sign up validation for a player.
		/// </summary>
		/// <param name="tournament">The tournament.</param>
		/// <param name="player">The player.</param>
		/// <returns>The response.</returns>
		public static TournamentSignUpResponse SignUp(ITournamentBase tournament, Models.Entities.Player player)
		{
			string msg = string.Empty;
			
			if (player.Level < tournament.RequiredLevel)
			{
				msg = "TOURNAMENT_LEVEL_TOO_LOW";
			}
			
			if (tournament.RequiredJob != Enums.Job.Unknown && Tools.JobTools.GetBaseJob(player.Job) != tournament.RequiredJob)
			{
				msg = "TOURNAMENT_INVALID_JOB";
			}
			
			if (player.Reborns < tournament.RequiredReborns)
			{
				msg = "TOURNAMENT_REBORNS_TOO_LOW";
			}
			
			if (tournament.RequiredToBeVIP && player.VIPLevel == 0)
			{
				msg = "TOURNAMENT_NOT_VIP";
			}
			
			if (tournament.RequiredToBeFemale && !player.IsFemale)
			{
				msg = "TOURNAMENT_NOT_FEMALE";
			}
			else if (tournament.RequiredToBeMale && !player.IsMale)
			{
				msg = "TOURNAMENT_NOT_MALE";
			}
			
			if (tournament.RequiredGuild && player.Guild == null)
			{
				msg = "TOURNAMENT_NO_GUILD";
			}
			else if (tournament.RequiredGuild)
			{
				if (tournament.RequiredGuildLeader && tournament.RequiredDeputyLeader &&
				    player.GuildMember.Rank != Enums.GuildRank.GuildLeader &&
				    player.GuildMember.Rank != Enums.GuildRank.DeputyLeader)
				{
					msg = "TOURNAMENT_NOT_GUILD_LEADER_OR_DEPUTY_LEADER";
				}
				else if (tournament.RequiredGuildLeader && player.GuildMember.Rank != Enums.GuildRank.GuildLeader)
				{
					msg = "TOURNAMENT_NOT_GUILD_LEADER";
				}
				else if (tournament.RequiredDeputyLeader && player.GuildMember.Rank != Enums.GuildRank.DeputyLeader)
				{
					msg = "TOURNAMENT_NOT_DEPUTY_LEADER";
				}
			}
			
			if (tournament.MinimumTeamMembers > 0 && (player.Team == null || player.Team.GetMembers().Count < tournament.MinimumTeamMembers))
			{
				msg = "TOURNAMENT_TEAM_TOO_SMALL";
			}
			
			bool success = string.IsNullOrWhiteSpace(msg);
			if (success)
			{
				msg = "TOURNAMENT_SIGNED_UP";
			}
			
			return new TournamentSignUpResponse
			{
				Message = msg,
				Success = success
			};
		}
	}
}
