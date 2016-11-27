// Project by Bauss
using System;
using System.Threading.Tasks;
using CandyConquer.WorldApi.Models.Entities;

namespace CandyConquer.WorldApi.Controllers.Arena
{
	/// <summary>
	/// Controller for arena battles.
	/// </summary>
	public class ArenaBattleController : Battle.BattleController
	{
		/// <summary>
		/// Gets the arena battle.
		/// </summary>
		public Models.Arena.ArenaBattle ArenaBattle { get; protected set; }
		
		/// <summary>
		/// The dynamic map associated with the battle.
		/// </summary>
		private Models.Maps.DynamicMap _dynamicMap;
		
		/// <summary>
		/// Creates a new arena battle controller.
		/// </summary>
		public ArenaBattleController()
			: base()
		{
		}
		
		#region ArenaBattle
		/// <summary>
		/// Begins the time out.
		/// </summary>
		public void BeginTimeOut()
		{
			Task.Run(async() => await EndTimeOutAsync(60000, true));
		}
		
		/// <summary>
		/// Begins the battle.
		/// </summary>
		public void Begin()
		{
			_dynamicMap = Collections.MapCollection.GetDynamicMap(700);
			_dynamicMap.Show();
			
			Task.Run(async() => await EndTimeOutAsync(90000, false));
			
			ArenaBattle.MatchStartTime = DateTime.UtcNow.AddMilliseconds(10000);
			ArenaBattle.Started = true;
			
			var player1 = ArenaBattle.Player1.Player;
			var player2 = ArenaBattle.Player2.Player;
			
			player1.Revive(true);
			player2.Revive(true);
			
			player1.TransformModel = 0;
			player2.TransformModel = 0;
			
			player1.RemoveStatusFlag(Enums.StatusFlag.Ghost);
			player1.RemoveStatusFlag(Enums.StatusFlag.Dead);
			player1.RemoveStatusFlag(Enums.StatusFlag.Riding);
			
			player2.RemoveStatusFlag(Enums.StatusFlag.Ghost);
			player2.RemoveStatusFlag(Enums.StatusFlag.Dead);
			player2.RemoveStatusFlag(Enums.StatusFlag.Riding);
			
			player1.MapChangeEvents
				.Enqueue((p) =>
				         {
				         	p.ClientSocket.Send(GetMatch());
				         	p.ClientSocket.Send(GetCountDown(player2, player1));
				         	p.ClientSocket.Send(GetPacket());
				         });
			
			player2.MapChangeEvents
				.Enqueue((p) =>
				         {
				         	p.ClientSocket.Send(GetMatch());
				         	p.ClientSocket.Send(GetCountDown(player1, player2));
				         	p.ClientSocket.Send(GetPacket());
				         });
			
			foreach (var watcher in ArenaBattle.Watchers.Values)
			{
				watcher.MapChangeEvents
					.Enqueue((p) =>
					         {
					         	p.ClientSocket.Send(GetPacket());
					         });
			}
			
			var player1Location = GenerateRandomLocation();
			player1.TeleportDynamic(_dynamicMap.Id, player1Location.X, player1Location.Y);
			
			var player2Location = GenerateRandomLocation();
			player2.TeleportDynamic(_dynamicMap.Id, player2Location.X, player2Location.Y);
		}
		
		/// <summary>
		/// Quits the battle.
		/// </summary>
		/// <param name="player">The player quitting.</param>
		public void Quit(Models.Entities.Player player)
		{
			if (player.ClientId == ArenaBattle.Player1.Player.ClientId)
			{
				ArenaBattle.Player1.Damage = 0;
				ArenaBattle.Player2.Damage = 1;
			}
			else
			{
				ArenaBattle.Player1.Damage = 1;
				ArenaBattle.Player2.Damage = 0;
			}
			
			ArenaBattle.Started = true;
			End();
		}
		
		/// <summary>
		/// Generates the honor.
		/// </summary>
		/// <param name="loserHonor">The losers honor.</param>
		/// <returns>An array of the honor.</returns>
		private static int[] GenerateHonor(int loserHonor)
		{
			int loseHonor = Drivers.Repositories.Safe.Random.Next(32, Math.Min(1000, Math.Max(33, Math.Min(113, (int)loserHonor))));
			int winHonor = Drivers.Repositories.Safe.Random.Next(loseHonor / 3, loseHonor / 2);
			
			return new int[] { loseHonor, winHonor };
		}
		
		/// <summary>
		/// Gives up.
		/// </summary>
		/// <param name="player">The player giving up.</param>
		public void GiveUp(Models.Entities.Player player)
		{
			lock (Drivers.Locks.GlobalLock)
			{
				if (ArenaBattle.EndedAlready)
				{
					return;
				}
				
				ArenaBattle.EndedAlready = true;
			}
			
			var player1 =  ArenaBattle.Player1;
			var player2 =  ArenaBattle.Player2;
			int loserHonor = 0;
			
			if (player.ClientId == player1.Player.ClientId)
			{
				player2.Player.ClientSocket.Send(GetGiveUp());
				player2.Player.ArenaInfo.Win();
				player1.Player.ArenaInfo.Lose();
				
				loserHonor = player1.Player.ArenaInfo.HonorPoints;
				
				player1.Player.ArenaInfo.Status = Enums.ArenaStatus.WaitingInactive;
				player1.Player.ClientSocket.Send(new Models.Packets.Arena.ArenaStatisticPacket
				                                 {
				                                 	Player = player1.Player
				                                 });
				
				player1.Player.ArenaInfo.Status = Enums.ArenaStatus.WaitingForOpponent;
				player1.Player.ClientSocket.Send(new Models.Packets.Arena.ArenaStatisticPacket
				                                 {
				                                 	Player = player1.Player
				                                 });
			}
			else
			{
				player1.Player.ClientSocket.Send(GetGiveUp());
				player1.Player.ArenaInfo.Win();
				player2.Player.ArenaInfo.Lose();
				
				loserHonor = player2.Player.ArenaInfo.HonorPoints;
				
				player1.Player.ArenaInfo.Status = Enums.ArenaStatus.WaitingInactive;
				player1.Player.ClientSocket.Send(new Models.Packets.Arena.ArenaStatisticPacket
				                                 {
				                                 	Player = player1.Player
				                                 });
				
				player1.Player.ArenaInfo.Status = Enums.ArenaStatus.WaitingForOpponent;
				player1.Player.ClientSocket.Send(new Models.Packets.Arena.ArenaStatisticPacket
				                                 {
				                                 	Player = player1.Player
				                                 });
			}
			
			player1.Player.Battle = null;
			player2.Player.Battle = null;
			
			var honorPoints = GenerateHonor(loserHonor);
			
			if (player.ClientId == player1.Player.ClientId)
			{
				player2.Player.ArenaInfo.HonorPoints += honorPoints[1];
				player1.Player.ArenaInfo.HonorPoints -= honorPoints[0];
			}
			else
			{
				player1.Player.ArenaInfo.HonorPoints += honorPoints[1];
				player2.Player.ArenaInfo.HonorPoints -= honorPoints[0];
			}
			
			Collections.ArenaQualifierCollection.GetTop10();
		}
		
		/// <summary>
		/// Ends the battle.
		/// </summary>
		public void End()
		{
			lock (Drivers.Locks.GlobalLock)
			{
				if (ArenaBattle.EndedAlready)
				{
					return;
				}
				
				ArenaBattle.EndedAlready = true;
			}
			
			var player1 = ArenaBattle.Player1;
			var player2 = ArenaBattle.Player2;
			
			var endMatchPacket = GetEnd();
			player1.Player.ClientSocket.Send(endMatchPacket);
			player2.Player.ClientSocket.Send(endMatchPacket);
			
			int loserHonor = 0;
			
			if (player1.Damage > player2.Damage)
			{
				player1.Player.ArenaInfo.Win();
				player2.Player.ArenaInfo.Lose();
				loserHonor = player2.Player.ArenaInfo.HonorPoints;
				
				Collections.PlayerCollection.BroadcastFormattedSystemMessage("ARENA_WIN", player1.Player.Name, player2.Player.Name);
			}
			else if (player2.Damage > player1.Damage)
			{
				player2.Player.ArenaInfo.Win();
				player1.Player.ArenaInfo.Lose();
				loserHonor = player1.Player.ArenaInfo.HonorPoints;
				
				Collections.PlayerCollection.BroadcastFormattedSystemMessage("ARENA_WIN", player2.Player.Name, player1.Player.Name);
			}
			else
			{
				player1.Player.ArenaInfo.Lose();
				player2.Player.ArenaInfo.Lose();
				
				Collections.PlayerCollection.BroadcastFormattedSystemMessage("ARENA_DRAW", player1.Player.Name, player2.Player.Name);
			}
			
			var honorPoints = GenerateHonor(loserHonor);
			player1.Player.Battle = null;
			player2.Player.Battle = null;
			
			if (player1.Damage > player2.Damage)
			{
				player1.Player.ArenaInfo.HonorPoints += honorPoints[1];
				player2.Player.ArenaInfo.HonorPoints -= honorPoints[0];
			}
			else if (player2.Damage > player1.Damage)
			{
				player2.Player.ArenaInfo.HonorPoints += honorPoints[1];
				player1.Player.ArenaInfo.HonorPoints -= honorPoints[0];
			}
			else
			{
				player1.Player.ArenaInfo.HonorPoints -= honorPoints[0];
				player2.Player.ArenaInfo.HonorPoints -= honorPoints[0];
			}
			
			Collections.ArenaQualifierCollection.GetTop10();
			
			player1.Player.ArenaInfo.Status = Enums.ArenaStatus.NotSignedUp;
			player1.Player.ClientSocket.Send(new Models.Packets.Arena.ArenaStatisticPacket
			                                 {
			                                 	Player = player1.Player
			                                 });
			player2.Player.ArenaInfo.Status = Enums.ArenaStatus.NotSignedUp;
			player2.Player.ClientSocket.Send(new Models.Packets.Arena.ArenaStatisticPacket
			                                 {
			                                 	Player = player2.Player
			                                 });
			
			player1.Player.Revive(true);
			player2.Player.Revive(true);
			
			_dynamicMap.Hide();
			ArenaBattle.Player1.Player = null;
			ArenaBattle.Player2.Player = null;
			
			foreach (var watcher in ArenaBattle.Watchers.Values)
			{
				watcher.ArenaInfo.Status = Enums.ArenaStatus.NotSignedUp;
				watcher.ClientSocket.Send(new Models.Packets.Arena.ArenaStatisticPacket
				                          {
				                          	Player = watcher
				                          });
			}
			
			ArenaBattle.Watchers.Clear();
		}
		
		/// <summary>
		/// Ends the battle on time out asynchronously.
		/// </summary>
		/// <param name="timeOut">The time out.</param>
		/// <param name="startCheck">Boolean determining whether it should check if the battle has started or not.</param>
		private async Task EndTimeOutAsync(int timeOut, bool startCheck)
		{
			await Task.Delay(timeOut);
			
			if (!startCheck || startCheck && !ArenaBattle.Started)
			{
				End();
			}
		}
		
		/// <summary>
		/// Generates a random location within the arena.
		/// </summary>
		/// <returns>The random location.</returns>
		private Models.Maps.Coordinate GenerateRandomLocation()
		{
			ushort x = (ushort)Drivers.Repositories.Safe.Random.Next(35, 70);
			ushort y = (ushort)Drivers.Repositories.Safe.Random.Next(35, 70);
			
			return new Models.Maps.Coordinate(x, y);
		}
		
		/// <summary>
		/// Joins the battle as a watcher.
		/// </summary>
		/// <param name="player">The player joining.</param>
		public void JoinAsWatcher(Player player)
		{
			if (_dynamicMap != null && ArenaBattle.Watchers.TryAdd(player.ClientId, player))
			{
				player.CanAttack = false;
				player.MapChangeEvents.Enqueue((p) =>
				                               {
				                               	p.ClientSocket.Send(GetPacket());
				                               });
				var location = GenerateRandomLocation();
				
				player.TeleportDynamic(_dynamicMap.Id, location.X, location.Y);
			}
		}
		
		/// <summary>
		/// Leaves the battle as a watcher.
		/// </summary>
		/// <param name="player">The player.</param>
		public void LeaveAsWatcher(Player player)
		{
			player.TeleportToLastMap();
			player.CanAttack = true;
			
			Models.Entities.Player removedPlayer;
			ArenaBattle.Watchers.TryRemove(player.ClientId, out removedPlayer);
			
			player.ArenaInfo.Status = Enums.ArenaStatus.NotSignedUp;
			player.ClientSocket.Send(new Models.Packets.Arena.ArenaStatisticPacket
			                         {
			                         	Player = player
			                         });
		}
		#endregion
		
		#region Packets
		/// <summary>
		/// Gets the give up packet.
		/// </summary>
		/// <returns>The packet.</returns>
		private byte[] GetGiveUp()
		{
			return new Models.Packets.Arena.ArenaActionPacket
			{
				Dialog = Enums.ArenaDialog.OpponentGaveUp
			};
		}
		
		/// <summary>
		/// Gets the end packet.
		/// </summary>
		/// <returns>The packet.</returns>
		private byte[] GetEnd()
		{
			return new Models.Packets.Arena.ArenaActionPacket
			{
				Dialog = Enums.ArenaDialog.ArenaIconOff
			};
		}
		
		/// <summary>
		/// Gets the match packet.
		/// </summary>
		/// <returns>The packet.</returns>
		private byte[] GetMatch()
		{
			return new Models.Packets.Arena.ArenaActionPacket
			{
				Dialog = Enums.ArenaDialog.Match,
				Option = Enums.ArenaOption.MatchOn
			};
		}
		
		/// <summary>
		/// Gets the count down packet.
		/// </summary>
		/// <param name="fromPlayer">The player it's from.</param>
		/// <param name="toPlayer">The player it's to.</param>
		/// <returns>The packet.</returns>
		private byte[] GetCountDown(Player fromPlayer, Player toPlayer)
		{
			return new Models.Packets.Arena.ArenaActionPacket
			{
				Dialog = Enums.ArenaDialog.StartTheFight,
				
				Name = fromPlayer.Name,
				Level = fromPlayer.Level,
				ArenaPoints = fromPlayer.ArenaInfo.DbPlayerArenaQualifier.ArenaPoints,
				Job = fromPlayer.Job,
				Rank = fromPlayer.ArenaInfo.Ranking + 1,
				ClientId = fromPlayer.ClientId,
				Unknown = 1
			};
		}
		
		/// <summary>
		/// Gets the arena match packet.
		/// </summary>
		/// <returns>The packet.</returns>
		private byte[] GetPacket()
		{
			return new Models.Packets.Arena.ArenaMatchPacket
			{
				Match = ArenaBattle
			};
		}
		#endregion
		
		#region Battle
		/// <summary>
		/// Handler for when a player enters an area.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <returns>True if the player can enter.</returns>
		public override bool EnterArea(Player player)
		{
			return true;
		}
		
		/// <summary>
		/// Handler for when a player leaves an area.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <returns>True if the player can leave.</returns>
		public override bool LeaveArea(Player player)
		{
			return true;
		}
		
		/// <summary>
		/// Handler for when the player kills a monster.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="monster">The monster.</param>
		public override void KillMonster(Player player, Monster monster)
		{
			// ...
		}
		
		/// <summary>
		/// Handler for beginning an attack.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <returns>True if the attack can begin.</returns>
		public override bool HandleBeginAttack(Player attacker)
		{
			return ArenaBattle.MatchStarted;
		}
		
		/// <summary>
		/// Handler for beginning a physical attack.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <returns>True if the physical attack can be done.</returns>
		public override bool HandleBeginHit_Physical(Player attacker)
		{
			return true;
		}
		
		/// <summary>
		/// Handler for beginning a magic attack.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="packet">The spell packet.</param>
		/// <returns>True if the magic attack can be done.</returns>
		public override bool HandleBeginHit_Magic(Player attacker, Models.Packets.Spells.SpellPacket packet)
		{
			return true;
		}
		
		/// <summary>
		/// Handler for beginning a ranged attack.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <returns>True if the ranged attack can be done.</returns>
		public override bool HandleBeginHit_Ranged(Player attacker)
		{
			return true;
		}
		
		/// <summary>
		/// Handler for attacking a player.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked player.</param>
		/// <param name="damage">The damage.</param>
		/// <returns>True if the attack can be done.</returns>
		public override bool HandleAttack(Player attacker, Player attacked, ref uint damage)
		{
			if (!(attacker.Battle is ArenaBattleController) ||
			    !(attacked.Battle is ArenaBattleController))
			{
				return false;
			}
			
			if (attacker.ClientId != ArenaBattle.Player1.Player.ClientId &&
			    attacker.ClientId != ArenaBattle.Player2.Player.ClientId &&
			    attacked.ClientId != ArenaBattle.Player1.Player.ClientId &&
			    attacked.ClientId != ArenaBattle.Player2.Player.ClientId)
			{
				return false;
			}
			
			if (attacker.ClientId == ArenaBattle.Player1.Player.ClientId)
			{
				ArenaBattle.Player1.Damage += damage;
			}
			else if (attacker.ClientId == ArenaBattle.Player2.Player.ClientId)
			{
				ArenaBattle.Player2.Damage += damage;
			}
			
			var packet = GetPacket();
			attacker.ClientSocket.Send(packet);
			attacked.ClientSocket.Send(packet);
			
			foreach (var watcher in ArenaBattle.Watchers.Values)
			{
				watcher.ClientSocket.Send(packet);
			}
			
			return true;
		}
		
		/// <summary>
		/// Handler for death.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked.</param>
		/// <returns>True if the death should be handled.</returns>
		public override bool HandleDeath(Player attacker, Player attacked)
		{
			End();
			return false;
		}
		
		/// <summary>
		/// Handler for revival.
		/// </summary>
		/// <param name="killed">The killed player.</param>
		/// <returns>True if the revival should be handled.</returns>
		public override bool HandleRevive(Player killed)
		{
			return false;
		}
		
		/// <summary>
		/// Handler for disconnection.
		/// </summary>
		/// <param name="player">The player who disconnected.</param>
		public override void HandleDisconnect(Player player)
		{
			GiveUp(player);
		}
		#endregion
	}
}
