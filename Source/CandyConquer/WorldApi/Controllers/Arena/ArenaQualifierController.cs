// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using CandyConquer.Drivers.Repositories.Collections;
using CandyConquer.WorldApi.Models.Entities;

namespace CandyConquer.WorldApi.Controllers.Arena
{
	/// <summary>
	/// Controller for the arena qualifier.
	/// </summary>
	public static class ArenaQualifierController
	{
		/// <summary>
		/// Collection of matches.
		/// </summary>
		private static ConcurrentList<Models.Arena.ArenaBattle> _matches;
		
		/// <summary>
		/// Collection of players signed up for the arena.
		/// </summary>
		private static ConcurrentDictionary<uint, Models.Entities.Player> _playerQueue;
		
		/// <summary>
		/// Static constructor for the arena qualifier.
		/// </summary>
		static ArenaQualifierController()
		{
			_matches = new ConcurrentList<Models.Arena.ArenaBattle>();
			_playerQueue = new ConcurrentDictionary<uint, Models.Entities.Player>();
		}
		
		/// <summary>
		/// Gets all matches in the arena.
		/// </summary>
		/// <returns>An array of all matches.</returns>
		public static Models.Arena.ArenaBattle[] GetMatches()
		{
			return _matches.Where(match => match.Player1.Player != null && match.Player2.Player != null).ToArray();
		}
		
		/// <summary>
		/// Removes a player from the signed up collection.
		/// </summary>
		/// <param name="player">The player to remove.</param>
		public static void RemovePlayer(Models.Entities.Player player)
		{
			Models.Entities.Player removedPlayer;
			_playerQueue.TryRemove(player.ClientId, out removedPlayer);
		}
		
		/// <summary>
		/// Gets the amount of players signed up for the arena.
		/// </summary>
		public static int PlayerQueueCount
		{
			get { return _playerQueue.Count; }
		}
		
		/// <summary>
		/// Joins the arena.
		/// </summary>
		/// <param name="player">The player.</param>
		public static void Join(Player player)
		{
			if (player.Battle != null ||
			    player.Map.IsDynamic)
			{
				return;
			}
			
			if (_playerQueue.ContainsKey(player.ClientId))
			{
				player.ArenaInfo.Status = Enums.ArenaStatus.NotSignedUp;
				player.ClientSocket.Send(new Models.Packets.Arena.ArenaStatisticPacket
				                         {
				                         	Player = player
				                         });
				
				player.ArenaInfo.Status = Enums.ArenaStatus.WaitingForOpponent;
				player.ClientSocket.Send(new Models.Packets.Arena.ArenaStatisticPacket
				                         {
				                         	Player = player
				                         });
			}
			else
			{
				player.ArenaInfo.Status = Enums.ArenaStatus.WaitingForOpponent;
				player.ClientSocket.Send(new Models.Packets.Arena.ArenaStatisticPacket
				                         {
				                         	Player = player
				                         });
				
				_playerQueue.TryAdd(player.ClientId, player);
			}
			
			if (_matches.ItemCount > 0)
			{
				player.ClientSocket.Send(new Models.Packets.Arena.ArenaBattleInfoPacket());
			}
			
			Task.Run(async() => await HandleMatchupAsync(player));
		}
		
		/// <summary>
		/// Accepts a match.
		/// </summary>
		/// <param name="player">The player.</param>
		public static void Accept(Player player)
		{
			var match = player.Battle as Models.Arena.ArenaBattle;
			if (match == null)
			{
				return;
			}
			
			
			if (player.ClientId == match.Player1.Player.ClientId)
			{
				match.Player1.Accepted = true;
			}
			else
			{
				match.Player2.Accepted = true;
			}
			
			if (match.Player1.Accepted && match.Player2.Accepted)
			{
				match.Begin();
				_matches.TryAdd(match);
			}
		}
		
		/// <summary>
		/// Quits the arena.
		/// </summary>
		/// <param name="player">The player.</param>
		public static void Quit(Player player)
		{
			var match = player.Battle as Models.Arena.ArenaBattle;
			if (match == null ||
			    !match.Started)
			{
				return;
			}
			
			match.Quit(player);
		}
		
		/// <summary>
		/// Gives up.
		/// </summary>
		/// <param name="player">The player.</param>
		public static void GiveUp(Player player)
		{
			var match = player.Battle as Models.Arena.ArenaBattle;
			if (match == null ||
			    !match.Started)
			{
				return;
			}
			
			match.GiveUp(player);
		}
		
		/// <summary>
		/// Quits waiting.
		/// </summary>
		/// <param name="player">The player.</param>
		public static void QuitWait(Player player)
		{
			if (player.Battle != null ||
			    player.Map.IsDynamic)
			{
				return;
			}
			
			player.ArenaInfo.Status = Enums.ArenaStatus.NotSignedUp;
			player.ClientSocket.Send(new Models.Packets.Arena.ArenaStatisticPacket
			                         {
			                         	Player = player
			                         });
			
			Player removedPlayer;
			_playerQueue.TryRemove(player.ClientId, out removedPlayer);
		}
		
		/// <summary>
		/// Handles player match up asynchronously.
		/// </summary>
		/// <param name="player">The player to match up with.</param>
		private static async Task HandleMatchupAsync(Player player)
		{
			if (player == null || !_playerQueue.ContainsKey(player.ClientId))
			{
				return;
			}
			
			int delayTime = 10000;
			
			try
			{
				foreach (var opponent in _playerQueue.Values
				         .Where(o => o != null && o.ClientId != player.ClientId && o.Battle == null))
				{
					int levelDifference = player.Level - opponent.Level;
					
					if (levelDifference >= -25 && levelDifference <= 25)
					{
						int highestPlus = Math.Max(player.Equipments.NumberOfPlus, opponent.Equipments.NumberOfPlus) / 2;
						
						if (player.Equipments.NumberOfPlus >= highestPlus &&
						    opponent.Equipments.NumberOfPlus >= highestPlus)
						{
							var match = new Models.Arena.ArenaBattle();
							match.Player1.Player = player;
							match.Player2.Player = opponent;
							
							player.Battle = match;
							opponent.Battle = match;
							
							Models.Entities.Player removedPlayer;
							_playerQueue.TryRemove(player.ClientId, out removedPlayer);
							_playerQueue.TryRemove(opponent.ClientId, out removedPlayer);
							
							match.BeginTimeOut();
							
							player.ArenaInfo.Status = Enums.ArenaStatus.WaitingInactive;
							opponent.ArenaInfo.Status = Enums.ArenaStatus.WaitingInactive;
							
							player.ClientSocket.Send(new Models.Packets.Arena.ArenaStatisticPacket
							                         {
							                         	Player = player
							                         });
							
							opponent.ClientSocket.Send(new Models.Packets.Arena.ArenaStatisticPacket
							                           {
							                           	Player = opponent
							                           });
							
							player.ClientSocket.Send(new Models.Packets.Arena.ArenaActionPacket
							                         {
							                         	Name = player.Name,
							                         	Level = player.Level,
							                         	ArenaPoints = player.ArenaInfo.DbPlayerArenaQualifier.ArenaPoints,
							                         	Job = player.Job,
							                         	Dialog = Enums.ArenaDialog.StartCountDown,
							                         	Rank = player.ArenaInfo.Ranking,
							                         	ClientId = player.ClientId,
							                         	Unknown = 1
							                         });
							
							opponent.ClientSocket.Send(new Models.Packets.Arena.ArenaActionPacket
							                           {
							                           	Name = opponent.Name,
							                           	Level = opponent.Level,
							                           	ArenaPoints = opponent.ArenaInfo.DbPlayerArenaQualifier.ArenaPoints,
							                           	Job = opponent.Job,
							                           	Dialog = Enums.ArenaDialog.StartCountDown,
							                           	Rank = opponent.ArenaInfo.Ranking,
							                           	ClientId = opponent.ClientId,
							                           	Unknown = 1
							                           });
							
							delayTime = 0;
						}
					}
				}
			}
			catch (Exception e)
			{
				Drivers.Global.RaiseException(e);
				
				if (player != null)
				{
					player.ClientSocket.LastException = e;
					player.ClientSocket.Disconnect(Drivers.Messages.Errors.FATAL_ERROR_TITLE);
					return;
				}
			}
			
			await Task.Delay(delayTime);
			if (delayTime > 0)
			{
				await HandleMatchupAsync(player);
			}
		}
	}
}
