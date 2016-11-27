// Project by Bauss
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace CandyConquer.WorldApi.Controllers.Tournaments
{
	/// <summary>
	/// Controller for tournament teams.
	/// </summary>
	public class TournamentTeamController
	{
		/// <summary>
		/// The tournament team.
		/// </summary>
		public Models.Tournaments.TournamentTeam Team { get; protected set; }
		
		/// <summary>
		/// Creates a new tournament team controller.
		/// </summary>
		protected TournamentTeamController()
		{
		}
		
		/// <summary>
		/// Handles the enter for all team members.
		/// </summary>
		public void Enter()
		{
			foreach (var member in Team.Members.Values)
			{
				member.Teleport(Team.Tournament.MapId, Team.X, Team.Y);
			}
		}
		
		/// <summary>
		/// Handles the respawn of a team member.
		/// </summary>
		/// <param name="player">The player.</param>
		public void Respawn(Models.Entities.Player player)
		{
			player.Teleport(Team.Tournament.MapId, Team.X, Team.Y);
		}
		
		/// <summary>
		/// Handles the leave for all team members.
		/// </summary>
		/// <param name="unmask">Boolean determining whether equipments should be unmasked or not.</param>
		public void Leave(bool unmask)
		{
			foreach (var member in Team.Members.Values)
			{
				member.TournamentTeam = null;
				member.Battle = null;
				member.RemoveStatusFlag(Enums.StatusFlag.IceBlock);
				
				foreach (var spellId in member.MaskedSkills.GetHashes())
				{
					member.ClientSocket.Send(new CandyConquer.WorldApi.Models.Packets.Client.DataExchangePacket
					                         {
					                         	ExchangeType = Enums.ExchangeType.DropMagic,
					                         	ClientId = member.ClientId,
					                         	Data1Low = spellId
					                         });
					if (member.Spells.ContainsSkill(spellId))
					{
						member.Spells.GetOrCreateSkill(spellId).SendToClient();
					}
				}
				
				member.MaskedSkills.Clear();
				
				if (unmask)
				{
					member.Equipments.UnmaskAll();
				}
				
				member.Teleport(1002, 400, 400);
			}
		}
		
		/// <summary>
		/// Checks whether or not a player is in the spawn area of the tournament team.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <returns>True if the player is in the area.</returns>
		public bool InArea(Models.Entities.Player player)
		{
			return Tools.RangeTools.ValidDistance(player.X, player.Y, Team.X, Team.Y);
		}
		
		/// <summary>
		/// Adds a player to the team.
		/// </summary>
		/// <param name="player">The player.</param>
		public void Add(Models.Entities.Player player)
		{
			player.TournamentTeam = Team;
			
			Team.Members.TryAdd(player.ClientId, player);
		}
		
		/// <summary>
		/// Removes a player from the team.
		/// </summary>
		/// <param name="player">The player.</param>
		public void Remove(Models.Entities.Player player)
		{
			Models.Entities.Player removedPlayer;
			Team.Members.TryRemove(player.ClientId, out removedPlayer);
		}
		
		/// <summary>
		/// Rewards the team.
		/// </summary>
		/// <param name="reward">The reward.</param>
		public void Reward(Models.Tournaments.TournamentReward reward)
		{
			foreach (var member in Team.Members.Values)
			{
				member.Money += reward.Money;
				member.CPs += reward.CPs;
				member.BoundCPs += reward.BoundCPs;
				
				foreach (var item in reward.Items)
				{
					member.Inventory.Add(item.Copy());
				}
			}
		}
		
		/// <summary>
		/// Clears the team.
		/// </summary>
		public void Clear()
		{
			Team.Kills = 0;
			Team.Points = 0;
			Team.Members.Clear();
		}
		
		/// <summary>
		/// Gets the amount of members in the team.
		/// </summary>
		public int Count
		{
			get { return Team.Members.Count; }
		}
		
		/// <summary>
		/// Broadcasts the scores of a tournament.
		/// </summary>
		/// <param name="scores">The scores.</param>
		public void BroadcastScore(IEnumerable<string> scores)
		{
			foreach (var score in scores)
			{
				var msg = Controllers.Packets.Message.MessageController.Create(Enums.MessageType.Right,
				                                                               "SYSTEM", "ALLUSERS",
				                                                               score);
				
				foreach (var member in Team.Members.Values)
				{
					member.ClientSocket.Send(msg);
				}
			}
		}
		
		/// <summary>
		/// Clears the scores.
		/// </summary>
		public void ClearScore()
		{
			var msg = Controllers.Packets.Message.MessageController.Create(Enums.MessageType.BeginRight,
			                                                               "SYSTEM", "ALLUSERS",
			                                                               string.Empty);
			foreach (var member in Team.Members.Values)
			{
				member.ClientSocket.Send(msg);
			}
		}
	}
}
