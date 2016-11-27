// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A team collection.
	/// </summary>
	public sealed class TeamCollection
	{
		/// <summary>
		/// The team leader id.
		/// </summary>
		private uint _teamLeaderId;
		
		/// <summary>
		/// The collection of team members.
		/// </summary>
		private ConcurrentDictionary<uint, Models.Entities.Player> _members;
		
		/// <summary>
		/// The pending join id for the team.
		/// </summary>
		public uint PendingJoin { get; set; }
		
		/// <summary>
		/// The pending invite id for the team.
		/// </summary>
		public uint PendingInvite { get; set; }
		
		/// <summary>
		/// Creates a new team collection.
		/// </summary>
		/// <param name="teamLeaderId">The team leader id.</param>
		private TeamCollection(uint teamLeaderId)
		{
			_members = new ConcurrentDictionary<uint, CandyConquer.WorldApi.Models.Entities.Player>();
			_teamLeaderId = teamLeaderId;
		}
		
		/// <summary>
		/// Checks whether a specific player is the leader of the team.
		/// </summary>
		/// <param name="player">The player to validate.</param>
		/// <returns>True if the player is the leader.</returns>
		public bool IsLeader(Models.Entities.Player player)
		{
			return player.ClientId == _teamLeaderId;
		}
		
		/// <summary>
		/// Removes a specific team member.
		/// </summary>
		/// <param name="clientId">The team member's client id.</param>
		/// <param name="removedPlayer">The removed team member.</param>
		/// <returns>True if the team member was removed.</returns>
		public bool Remove(uint clientId, out Models.Entities.Player removedPlayer)
		{
			return _members.TryRemove(clientId, out removedPlayer);
		}
		
		/// <summary>
		/// Deletes the team.
		/// </summary>
		/// <returns>An IEnumerable of all the members that were in the team.</returns>
		public IEnumerable<Models.Entities.Player> Delete()
		{
			var membersArray = new Models.Entities.Player[_members.Count];
			_members.Values.CopyTo(membersArray, 0);
			
			_teamLeaderId = 0;
			_members.Clear();
			
			return membersArray;
		}
		
		/// <summary>
		/// Adds a new player to the team.
		/// </summary>
		/// <param name="player">The player to add.</param>
		/// <returns>True if the player was added, false otherwise.</returns>
		public bool Add(Models.Entities.Player player)
		{
			if (_members.Count >= 5)
			{
				return false;
			}
			
			if (_members.TryAdd(player.ClientId, player))
			{
				player.Team = this;
				return true;
			}
			
			return false;
		}
		
		/// <summary>
		/// Gets all members in the team.
		/// </summary>
		/// <returns>The collection of members.</returns>
		public ICollection<Models.Entities.Player> GetMembers()
		{
			return _members.Values;
		}
		
		/// <summary>
		/// Creates a new team.
		/// </summary>
		/// <param name="player">The team leader.</param>
		/// <returns>True if the team was created, false otherwise.</returns>
		public static bool Create(Models.Entities.Player player)
		{
			player.Team = new TeamCollection(player.ClientId);
			if (!player.Team.Add(player))
			{
				player.Team = null;
				return false;
			}
			
			return true;
		}
	}
}
