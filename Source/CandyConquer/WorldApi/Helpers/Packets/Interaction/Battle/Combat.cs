// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle
{
	/// <summary>
	/// Handler for combat.
	/// This is a shared handler for the Attack, Shoot and Magic subtype of the interact packet.
	/// </summary>
	public static class Combat
	{
		/// <summary>
		/// Handling the combat of the interact packet.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="interact">The interact packet.</param>
		public static void Handle(Player player, Models.Packets.Entities.InteractionPacket packet)
		{
			if (packet == null)
			{
				player.SendSystemMessage("REST");
				return;
			}
			
			if (!player.Alive)
			{
				player.SendSystemMessage("REST");
				return;
			}
			
			if (!player.CanAttack)
			{
				player.SendSystemMessage("REST");
				return;
			}
			
			if (DateTime.UtcNow < player.LoginProtectionEndTime)
			{
				player.SendSystemMessage("REST");
				return;
			}
			
			if (DateTime.UtcNow < player.ReviveProtectionEndTime)
			{
				player.SendSystemMessage("REST");
				return;
			}
			
			if (DateTime.UtcNow > player.NextAllowedAttackTime.AddMilliseconds(Data.Constants.Time.AttackInterval))
			{
				player.AttackPacket = null;
			}
			
			if (DateTime.UtcNow < player.NextAllowedAttackTime && (player.AttackPacket == null || packet.Action == Enums.InteractionAction.Attack))
			{
				player.SendSystemMessage("REST");
				return;
			}
			
			if (player.Battle != null)
			{
				if (!player.Battle.HandleBeginAttack(player))
				{
					return;
				}
			}
			
			player.NextAllowedAttackTime = DateTime.UtcNow.AddMilliseconds(Data.Constants.Time.AttackInterval);
			
			if (player.ContainsStatusFlag(Enums.StatusFlag.Riding))
			{
				if (packet.Action != Enums.InteractionAction.MagicAttack ||
				    packet.MagicType != 7001)
				{
					player.Stamina = 0;
					player.RemoveStatusFlag(Enums.StatusFlag.Riding);
				}
			}

			switch (packet.Action)
			{
				case Enums.InteractionAction.MagicAttack:
					{
						Battle.Magic.Handle(player, packet);
						break;
					}
					
				case Enums.InteractionAction.Attack:
					{
						Battle.Physical.Handle(player, packet);
						break;
					}
					
				case Enums.InteractionAction.Shoot:
					{
						Battle.Ranged.Handle(player, packet);
						break;
					}
			}
		}
	}
}
