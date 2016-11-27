// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle
{
	/// <summary>
	/// Handler for physical attacks.
	/// </summary>
	public static class Physical
	{
		/// <summary>
		/// Handles a base physical attack.
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
					if (!ProcessPlayer(player, packet))
					{
						return;
					}
				}
				
				if ((attacker.AttackableEntity.Level + 15) < target.AttackableEntity.Level)
				{
					if (!attacker.ContainsStatusFlag(Enums.StatusFlag.StarOfAccuracy) &&
					    Tools.CalculationTools.ChanceSuccess(10))
					{
						return;
					}
					else if (Tools.CalculationTools.ChanceSuccess(5))
					{
						return;
					}
				}
				
				uint damage = Calculations.PhysicalCalculations.GetDamage(attacker.AttackableEntity, target.AttackableEntity);
				Damage.Process(attacker, target, ref damage, true);
				
				var targetMonster = target as Monster;
				
				if (damage > 0 && player != null && targetMonster != null)
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
					
					if (packet.WeaponTypeLeft > 0)
					{
						var prof = player.Spells.GetOrCreateProficiency(packet.WeaponTypeLeft);
						if (prof != null)
						{
							prof.Raise(newExperience);
						}
					}
				}
				
				packet.Data = damage;
				attacker.UpdateScreen(false, packet);
				if (player != null)
				{
					player.ClientSocket.Send(packet);
				}
			}
		}
		
		/// <summary>
		/// Processes a player doing a physical attack.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		/// <returns>True if the player can use the physical attack.</returns>
		private static bool ProcessPlayer(Player player, Models.Packets.Entities.InteractionPacket packet)
		{
			var rightHand = player.Equipments.Get(Enums.ItemPosition.WeaponR, false);
			var leftHand = player.Equipments.Get(Enums.ItemPosition.WeaponL, false);
			
			if (rightHand != null)
			{
				ushort subtype = rightHand.DbItem.BaseId;
				packet.WeaponTypeRight = subtype;
				
				if (subtype == 421)
				{
					subtype--;
				}
				
				if (Collections.SpellInfoCollection.ContainsWeaponSpell(subtype))
				{
					subtype = Collections.SpellInfoCollection.GetWeaponSpell(subtype);
				}
				
				if (ProcessWeaponSkill(subtype, player, packet))
				{
					return false;
				}
			}
			
			if (leftHand != null)
			{
				ushort subtype = leftHand.DbItem.BaseId;
				packet.WeaponTypeLeft = subtype;
				
				if (subtype == 421)
				{
					subtype--;
				}
				
				if (Collections.SpellInfoCollection.ContainsWeaponSpell(subtype))
				{
					subtype = Collections.SpellInfoCollection.GetWeaponSpell(subtype);
				}
				
				if (ProcessWeaponSkill(subtype, player, packet))
				{
					return false;
				}
			}
			
			if (player.Battle != null)
			{
				return player.Battle.HandleBeginHit_Physical(player);
			}
			
			return true;
		}

		/// <summary>
		/// Processes a weapon skill.
		/// </summary>
		/// <param name="subtype">The sub type.</param>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		/// <returns>True if the player can process the weapon skill.</returns>
		private static bool ProcessWeaponSkill(ushort subtype, Player player, Models.Packets.Entities.InteractionPacket packet)
		{
			if (player.Spells.ContainsSkill(subtype) && player.Battle == null)
			{
				var skill = player.Spells.GetOrCreateSkill(subtype);
				var spellInfo = Collections.SpellInfoCollection.GetSpellInfo(skill.Id, (byte)skill.Level);
				
				if (spellInfo != null)
				{
					if (Tools.CalculationTools.ChanceSuccess((int)spellInfo.DbSpellInfo.SpellPercent))
					{
						packet.MagicType = spellInfo.Id;
						packet.MagicLevel = spellInfo.Level;
						packet.Action = Enums.InteractionAction.MagicAttack;
						Magic.Handle(player, packet);
						return true;
					}
				}
			}
			
			return false;
		}
	}
}
