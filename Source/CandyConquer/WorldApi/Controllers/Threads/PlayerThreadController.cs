// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Controllers.Threads
{
	/// <summary>
	/// Controller for the player thread.
	/// </summary>
	public static class PlayerThreadController
	{
		/// <summary>
		/// Handles the thread.
		/// </summary>
		public static void Handle()
		{
			Collections.PlayerCollection
				.ForEach(player =>
				         {
				         	try
				         	{
				         		#region Stamina
				         		player.UpdateStamina();
				         		#endregion
				         		#region Poison
				         		if (player.ContainsStatusFlag(Enums.StatusFlag.Poisoned))
				         		{
				         			if (DateTime.UtcNow >= player.NextPoison)
				         			{
				         				player.NextPoison = DateTime.UtcNow.AddMilliseconds(3000);
				         				
				         				if (player.PoisonEffect > 0)
				         				{
				         					uint damage = (uint)Math.Max(1, (player.HP / 100) * player.PoisonEffect);
				         					
				         					if (player.HP > damage)
				         					{
				         						Helpers.Packets.Interaction.Battle.Damage.Hit(null, player, damage);
				         						
				         						var poisonPacket = new Models.Packets.Entities.InteractionPacket
				         						{
				         							Action = Enums.InteractionAction.Attack,
				         							ClientId = player.ClientId,
				         							TargetClientId = player.ClientId,
				         							X = player.X,
				         							Y = player.Y,
				         							Data = damage
				         						};
				         						
				         						player.UpdateScreen(false, poisonPacket);
				         						player.ClientSocket.Send(poisonPacket);
				         					}
				         					else
				         					{
				         						player.PoisonEffect = 0;
				         						player.RemoveStatusFlag(Enums.StatusFlag.Poisoned);
				         					}
				         				}
				         				else
				         				{
				         					player.RemoveStatusFlag(Enums.StatusFlag.Poisoned);
				         				}
				         			}
				         		}
				         		#endregion
				         		#region PKPoints
				         		if (player.PKPoints > 0)
				         		{
				         			if (DateTime.UtcNow > player.NextPKPointRemoval)
				         			{
				         				player.NextPKPointRemoval = DateTime.UtcNow.AddMilliseconds(Data.Constants.Time.PKPointsRemovalTime);
				         				
				         				player.PKPoints--;
				         			}
				         		}
				         		#endregion
				         	}
				         	catch { player.ClientSocket.Disconnect("Thread Failure."); }
				         });
		}
	}
}
