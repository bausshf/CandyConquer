// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections.Generic;
using CandyConquer.WorldApi.Models.Entities;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A collection of players.
	/// </summary>
	public static class PlayerCollection
	{
		/// <summary>
		/// The player collection.
		/// </summary>
		private static ConcurrentDictionary<uint,Player> _players;
		
		/// <summary>
		/// Static constructor for PlayerCollection.
		/// </summary>
		static PlayerCollection()
		{
			_players = new ConcurrentDictionary<uint, Player>();
		}
		
		/// <summary>
		/// Checks whether the collection contains a player by their client id.
		/// </summary>
		/// <param name="clientId">The client id.</param>
		/// <returns>True if the player is within the collection, false otherwise.</returns>
		public static bool ContainsClientId(uint clientId)
		{
			return _players.ContainsKey(clientId);
		}
		
		/// <summary>
		/// Checks whether the collection contains a player by their database id.
		/// </summary>
		/// <param name="playerId">The database id.</param>
		/// <returns>True if the player is within the collection, false otherwise.</returns>
		public static bool ContainsPlayerId(int playerId)
		{
			return _players
				.Count(p => p.Value.DbPlayer != null && p.Value.DbPlayer.Id == playerId) > 0;
		}
		
		/// <summary>
		/// Attempts to add a player to the collection.
		/// </summary>
		/// <param name="player">The player to add.</param>
		/// <returns>True if the player was added.</returns>
		public static bool TryAdd(Player player)
		{
			return _players.TryAdd(player.ClientId, player);
		}
		
		/// <summary>
		/// Removes a player from the collection.
		/// </summary>
		/// <param name="player">The player to remove.</param>
		public static void Remove(Player player)
		{
			Player removedPlayer;
			_players.TryRemove(player.ClientId, out removedPlayer);
		}
		
		/// <summary>
		/// Attempts to get a player by their client id.
		/// </summary>
		/// <param name="clientId">The client id.</param>
		/// <returns>The player if found, null otherwise.</returns>
		public static Player GetPlayerByClientId(uint clientId)
		{
			Player player;
			_players.TryGetValue(clientId, out player);
			return player;
		}
		
		/// <summary>
		/// Attempts to find all players tied to a specific player id.
		/// </summary>
		/// <param name="playerId">The player id.</param>
		/// <returns>IEnumerable of the players.</returns>
		public static IEnumerable<Player> GetPlayerById(int playerId)
		{
			return _players
				.Where(player => player.Value.DbPlayer != null && player.Value.DbPlayer.Id == playerId)
				.Select(player => player.Value);
		}
		
		/// <summary>
		/// Attempts to find a player by their name.
		/// </summary>
		/// <param name="playerName">The player name.</param>
		/// <returns>The player.</returns>
		public static Player GetPlayerByName(string playerName)
		{
			return _players.Values
				.Where(player => player.Name == playerName)
				.FirstOrDefault();
		}
		
		/// <summary>
		/// Performs an iteration of all players.
		/// </summary>
		/// <param name="iterate">The iteration action.</param>
		public static void ForEach(Action<Player> iterate)
		{
			if (iterate != null)
			{
				foreach (var player in _players.Values)
				{
					iterate(player);
				}
			}
		}
		
		/// <summary>
		/// Broadcasts a packet.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		public static void Broadcast(byte[] buffer)
		{
			foreach (var player in _players.Values)
			{
				player.ClientSocket.Send(buffer);
			}
		}
		
		/// <summary>
		/// Broadcasts a message to all players.
		/// </summary>
		/// <param name="message">The message to broadcast.</param>
		public static void BroadcastMessage(string message)
		{
			foreach (var player in _players.Values)
			{
				var msg = Data.Localization.Language.GetMessage(player.Language, message);
				
				player.ClientSocket.Send(Controllers.Packets.Message.MessageController.CreateCenter(msg));
			}
		}
		
		/// <summary>
		/// Broadcasts a formatted message to all players.
		/// </summary>
		/// <param name="message">The message to broadcast.</param>
		/// <param name="formattedArgs">Formatted message arguments.</param>
		public static void BroadcastFormattedMessage(string message, params object[] formattedArgs)
		{
			foreach (var player in _players.Values)
			{
				var msg = string.Format(Data.Localization.Language.GetMessage(player.Language, message), formattedArgs);
				
				player.ClientSocket.Send(Controllers.Packets.Message.MessageController.CreateCenter(msg));
			}
		}
		
		/// <summary>
		/// Broadcasts a message to all players.
		/// </summary>
		/// <param name="message">The message to broadcast.</param>
		public static void BroadcastSystemMessage(string message)
		{
			foreach (var player in _players.Values)
			{
				var msg = Data.Localization.Language.GetMessage(player.Language, message);
				
				player.ClientSocket.Send(Controllers.Packets.Message.MessageController.CreateSystemTopLeft(msg, player.Name));
			}
		}
		
		/// <summary>
		/// Broadcasts a formatted message to all players.
		/// </summary>
		/// <param name="message">The message to broadcast.</param>
		/// <param name="formattedArgs">Formatted message arguments.</param>
		public static void BroadcastFormattedSystemMessage(string message, params object[] formattedArgs)
		{
			foreach (var player in _players.Values)
			{
				var msg = string.Format(Data.Localization.Language.GetMessage(player.Language, message), formattedArgs);
				
				player.ClientSocket.Send(Controllers.Packets.Message.MessageController.CreateSystemTopLeft(msg, player.Name));
			}
		}
	}
}
