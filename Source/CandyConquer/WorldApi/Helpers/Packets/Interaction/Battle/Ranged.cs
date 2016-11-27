// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle
{
	/// <summary>
	/// Handler for ranged attacks.
	/// </summary>
	public static class Ranged
	{
		/// <summary>
		/// Handling a base ranged attack.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="packet">The packet.</param>
		public static void Handle(Controllers.Entities.AttackableEntityController attacker, Models.Packets.Entities.InteractionPacket packet)
		{
			Controllers.Entities.AttackableEntityController target;
			var canAttack = attacker.ValidateAttack(packet, out target);
			
			if (canAttack == 0 && target != null)
			{
				var player = attacker as Player;
				
				if (player != null)
				{
					if (!ProcessPlayer(player, packet, 1))
					{
						return;
					}
				}
				
				uint damage = Calculations.RangedCalculations.GetDamage(attacker as IAttackableEntity, target as IAttackableEntity);
				Damage.Process(attacker, target, ref damage, true);

				if (damage > 0 && player != null)
				{
					var targetMonster = target as Monster;
					
					if (targetMonster != null)
					{
						uint newExperience = Calculations.Experience.GetProficiencyExperience(player, targetMonster, damage);

						if (packet.WeaponTypeRight > 0)
						{
							var prof = player.Spells.GetOrCreateProficiency(packet.WeaponTypeRight);
							if (prof != null)
							{
								prof.Raise(newExperience);
							}
						}
					}
					
					DecreaseArrows(player, 1);
				}

				packet.Data = damage;
				attacker.UpdateScreen(false, packet);
				if (player != null)
				{
					player.ClientSocket.Send(packet);
					
					packet.ActivationType = 0;
					packet.ActivationValue = 0;
					player.AttackPacket = packet;
					player.UseAutoAttack(packet);
				}
			}
		}
		
		/// <summary>
		/// Processes a player attempting to use a ranged attack.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		/// <param name="requiredArrows">The required arrows of the attack.</param>
		/// <returns>True if the player can use the ranged attack.</returns>
		public static bool ProcessPlayer(Player player, Models.Packets.Entities.InteractionPacket packet, int requiredArrows)
		{
			var bow = player.Equipments.Get(Enums.ItemPosition.WeaponR, false);
			if (bow == null || !bow.IsBow)
			{
				return false;
			}
			
			var arrow = player.Equipments.Get(Enums.ItemPosition.WeaponL, false);
			if (arrow == null || !arrow.IsArrow)
			{
				return false;
			}
			
			if (arrow.DbOwnerItem.CurrentDura < requiredArrows)
			{
				if (player.Inventory.ContainsById((uint)arrow.DbItem.Id))
				{
					arrow = player.Inventory.Find(i => i.DbOwnerItem.CurrentDura > 0);
					if (arrow != null)
					{
						if (!player.Equipments.Equip(arrow, Enums.ItemPosition.WeaponL, true))
						{
							player.SendSystemMessage("AUTO_RELOAD_ARROW_FAIL");
							return false;
						}
					}
					else if (!player.Equipments.Unequip(Enums.ItemPosition.WeaponL))
					{
						// send message EMPTY_ARROWS
						return false;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			
			packet.WeaponTypeRight = 500;
			
			if (player.Battle != null)
			{
				return player.Battle.HandleBeginHit_Ranged(player);
			}
			
			return true;
		}
		
		/// <summary>
		/// Decreases a player's amount of equipped arrows.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="amount">The amount to decrease.</param>
		public static void DecreaseArrows(Player player, int amount = 1)
		{
			var arrow = player.Equipments.Get(Enums.ItemPosition.WeaponL);
			arrow.DbOwnerItem.CurrentDura -= Math.Max(1, amount);
			arrow.UpdateDatabase();
			arrow.UpdateClient(player, Enums.ItemUpdateAction.Update);
		}
	}
}
