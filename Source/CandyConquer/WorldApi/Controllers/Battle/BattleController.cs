// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Entities;

namespace CandyConquer.WorldApi.Controllers.Battle
{
	/// <summary>
	/// A battle controller base implementation.
	/// </summary>
	public abstract class BattleController
	{
		/// <summary>
		/// Creates a new battle controller.
		/// </summary>
		public BattleController()
		{
			
		}
		
		/// <summary>
		/// Handles begin attack.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <returns>True if the attack can begin.</returns>
		public abstract bool HandleBeginAttack(Player attacker);
		/// <summary>
		/// Handles the attack.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked.</param>
		/// <param name="damage">The damage.</param>
		/// <returns>True if the attack can be handled.</returns>
		public abstract bool HandleAttack(Player attacker, Player attacked, ref uint damage);
		/// <summary>
		/// Handles the begin of a physical hit.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <returns>True if the physical attack can begin.</returns>
		public abstract bool HandleBeginHit_Physical(Player attacker);
		/// <summary>
		/// Handles the begin of a ranged attack.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <returns>True if the ranged attack can begin.</returns>
		public abstract bool HandleBeginHit_Ranged(Player attacker);
		/// <summary>
		/// Handles the begin of a magic attack.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="packet">The spell packet.</param>
		/// <returns>True if the magic attack can begin.</returns>
		public abstract bool HandleBeginHit_Magic(Player attacker, Models.Packets.Spells.SpellPacket packet);
		/// <summary>
		/// Handles death.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="attacked">The attacked.</param>
		/// <returns>True if the death can be handled.</returns>
		public abstract bool HandleDeath(Player attacker, Player attacked);
		/// <summary>
		/// Handles revive.
		/// </summary>
		/// <param name="killed">The killed player.</param>
		/// <returns>True if the revive can be handled.</returns>
		public abstract bool HandleRevive(Player killed);
		/// <summary>
		/// Handles the enter of an area.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <returns>True if the player enters.</returns>
		public abstract bool EnterArea(Player player);
		/// <summary>
		/// Handles the leave of an area.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <returns>True if the player leaves.</returns>
		public abstract bool LeaveArea(Player player);
		/// <summary>
		/// Handles the kill of a monster.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="monster">The monster.</param>
		public abstract void KillMonster(Player player, Monster monster);
		
		/// <summary>
		/// Handles the disconnect of a player.
		/// </summary>
		/// <param name="player">The player.</param>
		public abstract void HandleDisconnect(Player player);
	}
}
