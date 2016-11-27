// Project by Bauss
using System;
using System.Collections.Concurrent;
using CandyConquer.WorldApi.Controllers.Entities;
using CandyConquer.WorldApi.Models.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle
{
	/// <summary>
	/// Handler for magic attacks.
	/// </summary>
	public static class Magic
	{
		/// <summary>
		/// Handling a base magic attack.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="packet">The packet.</param>
		public static void Handle(Controllers.Entities.AttackableEntityController attacker, Models.Packets.Entities.InteractionPacket packet)
		{
			if (packet == null)
			{
				return;
			}
			
			Controllers.Entities.AttackableEntityController target;
			var canAttack = ValidateMagicAttack(attacker, packet, out target);
			if (canAttack == 0)
			{
				var spellPacket = new Models.Packets.Spells.SpellPacket
				{
					ClientId = attacker.AttackableEntity.ClientId,
					SpellId = packet.MagicType,
					SpellX = packet.X,
					SpellY = packet.Y,
					SpellLevel = 0
				};
				
				Models.Spells.SpellInfo spellInfo = null;
				var player = attacker as Player;
				var monster = attacker as Monster;

				if (player != null)
				{
					if (!ProcessPlayer(player, packet, spellPacket, out spellInfo))
					{
						return;
					}
					// TODO: AI BOT ...
				}
				else if (monster != null)
				{
					if (Collections.SpellInfoCollection.ContainsSpell(packet.MagicType))
					{
						spellInfo = Collections.SpellInfoCollection.GetHighestSpell(packet.MagicType);
					}
				}
				
				if (spellInfo != null)
				{
					if (packet.ClientId != attacker.AttackableEntity.ClientId && target != null)
					{
						spellPacket.SpellX = target.MapObject.X;
						spellPacket.SpellY = target.MapObject.Y;
					}
					
					if (spellInfo.DbSpellInfo.UseEP > 0)
					{
						if (player != null)
						{
							// TODO: IF NOT AI BOT
							if (spellInfo.Id != 7001 && player.Stamina < spellInfo.DbSpellInfo.UseEP ||
							    spellInfo.Id == 7001 &&
							    player.ContainsStatusFlag(Enums.StatusFlag.Riding) &&
							    player.Stamina < spellInfo.DbSpellInfo.UseEP)
							{
								return;
							}
						}
					}
					
					if (spellInfo.DbSpellInfo.UseMP > 0)
					{
						if (monster != null)
						{
							if (monster.MP < spellInfo.DbSpellInfo.UseMP && !monster.IsGuard)
							{
								return;
							}
						}
						else if (attacker.AttackableEntity.MP < spellInfo.DbSpellInfo.UseMP)
						{
							return;
						}
					}
					
					if (attacker.MapObject.Map == null)
					{
						return;
					}
					
					bool success = false;
					
					switch (packet.MagicType)
					{
							#region Line Skills
						case 1045:
						case 1046:
						case 11005:
						case 11000:
							success = Skills.LineSkills.Handle(attacker, packet, spellPacket, spellInfo);
							break;
							#endregion
							
							#region SectorSkills
							
							#region Magic
						case 1165:
						case 7014:
							success = Skills.SectorSkills.Handle(attacker, packet, spellPacket, spellInfo, true);
							break;
							#endregion
							
							#region Physical
						case 1250:
						case 5050:
						case 5020:
						case 1300:
							success = Skills.SectorSkills.Handle(attacker, packet, spellPacket, spellInfo, false);
							break;
							#endregion
							
							#endregion
							
							#region Single
							
							#region Magic
						case 10310:
						case 1000:
						case 1001:
						case 1002:
						case 1150:
						case 1160:
						case 1180:
							success = Skills.SingleSkills.Handle(attacker, target, packet, spellPacket, spellInfo, true);
							break;
							#endregion
							
							#region Physical
						case 1290:
						case 5030:
						case 5040:
						case 7000:
						case 7010:
						case 7030:
						case 7040:
							success = Skills.SingleSkills.Handle(attacker, target, packet, spellPacket, spellInfo, false);
							break;
							#endregion
							
							#endregion
							
							#region Circle
							
							#region Magic
						case 1010://lightning tao
						case 1120://fc
						case 1125://volc
						case 3090://pervade
						case 5001://speed
						case 8030://arrows
						case 7013://flame shower
						case 30011://small ice circle
						case 30012://large ice circle
						case 10360:
						case 10361:
						case 10392:
						case 10308:
							success = Skills.CircleSkills.Handle(attacker, packet, spellPacket, spellInfo, true);
							break;
							#endregion
							
							#region Physical
						case 5010:
						case 7020:
						case 1115://herc
							success = Skills.CircleSkills.Handle(attacker, packet, spellPacket, spellInfo, false);
							break;
							#endregion
							
							#endregion
							
							#region MountSkill
						case 7001:
							if (player != null)
							{
								success = Skills.MountSkill.Handle(player, spellPacket);
							}
							else
							{
								success = false;
							}
							break;
							#endregion
							
							#region Buff
						case 1075:
						case 1085:
						case 1090:
						case 1095:
							success = Skills.BuffCurseSkills.Handle(attacker, target, packet, spellPacket, spellInfo);
							break;
							#endregion
							
							#region Revive
						case 1050:
						case 1100:
							success = Skills.ReviveSkills.Handle(attacker, target, packet, spellPacket);
							break;
							#endregion
							
							#region Fly
						case 8002:
						case 8003:
							success = Skills.FlySkills.Handle(attacker);
							break;
							#endregion
							
							#region Scatter
						case 8001:
							success = Skills.ScatterSkill.Handle(attacker, packet, spellPacket, spellInfo);
							break;
							#endregion
							
							#region Cure
							
							#region Self
						case 1190:
						case 1195:
						case 7016:
							success = Skills.CureSkills.HandleSelf(attacker, target, packet, spellPacket, spellInfo);
							break;
							#endregion
							
							#region Surroundings
						case 1005:
						case 1055:
						case 1170:
						case 1175:
							success = Skills.CureSkills.HandleSurroundings(attacker, target, packet, spellPacket, spellInfo);
							break;
							#endregion
							
							#endregion
							
							#region Archer
						case 10313:
						case 8000:
						case 9991:
						case 7012:
						case 7015:
						case 7017:
						case 1320:
							success = Skills.SingleSkills.Handle(attacker, target, packet, spellPacket, spellInfo, false, true);
							break;
							#endregion
							
							#region Ninja
							
							#region Toxic Fog
						case 6001:
							success = Skills.SectorSkills.Handle(attacker, packet, spellPacket, spellInfo, false, true);
							break;
							#endregion
							
							#region TwoFold
						case 6000:
							success = Skills.SingleSkills.Handle(attacker, target, packet, spellPacket, spellInfo, false, false);
							break;
							#endregion
							
							#region PoisonStar
						case 6002:
							success = Skills.BuffCurseSkills.Handle(attacker, target, packet, spellPacket, spellInfo, true);
							break;
							#endregion
							
							#region ArcherBane
						case 6004:
							success = Skills.BuffCurseSkills.Handle(attacker, target, packet, spellPacket, spellInfo, false, true);
							break;
							#endregion
							
							#endregion
							
						default:
							{
								if (player != null)
								{
									player.SendFormattedSystemMessage("INVALID_SKILL", true, spellInfo.Id);
								}
								success = false;
								break;
							}
					}
					
					if (success)
					{
						if (spellInfo.DbSpellInfo.UseEP > 0)
						{
							if (player != null)
							{
								player.Stamina = (byte)Math.Max(0, (((int)player.Stamina) - spellInfo.DbSpellInfo.UseEP));
							}
						}
						
						if (spellInfo.DbSpellInfo.UseMP > 0)
						{
							attacker.AttackableEntity.MP -= spellInfo.DbSpellInfo.UseMP;
						}
						
						attacker.UpdateScreen(false, spellPacket);
						
						// Not a single skill and skill is not safe, do damage here ...
						if (spellPacket.Targets.Count > 0 && !spellPacket.Safe && spellPacket.Process)
						{
							foreach (var spellTarget in spellPacket.Targets)
							{
								if (spellTarget.AssociatedEntity != null)
								{
									Damage.Hit(attacker, spellTarget.AssociatedEntity, spellTarget.Damage);
								}
							}
						}
						
						if (player != null)
						{
							player.ClientSocket.Send(spellPacket);
							
							if (spellPacket.SpellId >= 1000 && spellPacket.SpellId <= 1002)
							{
								packet.ActivationType = 0;
								packet.ActivationValue = 0;
								player.AttackPacket = packet;
								player.UseAutoAttack(packet);
							}
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Validates a magic attack.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="packet">The packet.</param>
		/// <param name="target">The target.</param>
		/// <returns>The status of the validation. (0 = success.)</returns>
		private static int ValidateMagicAttack(AttackableEntityController attacker, Models.Packets.Entities.InteractionPacket packet, out AttackableEntityController target)
		{
			target = null;
			if (packet == null)
			{
				return 1;
			}
			
			if (!attacker.AttackableEntity.Alive)
			{
				return 2;
			}
			
			if (!Tools.RangeTools.ValidDistance(attacker.MapObject.X, attacker.MapObject.Y, packet.X, packet.Y))
			{
				return 3;
			}
			
			Models.Maps.IMapObject targetMapObject = null;
			bool requiresTarget = false;
			if (packet.TargetClientId > 0)
			{
				if (!attacker.GetFromScreen(packet.TargetClientId, out targetMapObject) &&
				    packet.TargetClientId != attacker.AttackableEntity.ClientId)
				{
					return 4;
				}
				else
				{
					requiresTarget = packet.TargetClientId != attacker.AttackableEntity.ClientId;
				}
			}
			
			target = targetMapObject as AttackableEntityController;
			if (target != null)
			{
				var targetPlayer = target as Player;
				if (targetPlayer != null)
				{
					if (!targetPlayer.LoggedIn)
					{
						return 5;
					}
					
					if (DateTime.UtcNow < targetPlayer.LoginProtectionEndTime)
					{
						return 5;
					}
					
					if (DateTime.UtcNow < targetPlayer.ReviveProtectionEndTime)
					{
						return 6;
					}
				}
				
				if (!Tools.RangeTools.ValidDistance(attacker.MapObject.X, attacker.MapObject.Y, target.MapObject.X, target.MapObject.Y))
				{
					return 7;
				}
				
				if (!target.AttackableEntity.Alive &&
				    packet.MagicType != 1100 && packet.MagicType != 1050)
				{
					return 8;
				}
			}
			else if (requiresTarget)
			{
				return 10;
			}
			
			return 0;
		}
		
		/// <summary>
		/// Processes a player using a magic attack.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		/// <param name="spellPacket">The spell packet.</param>
		/// <param name="spellInfo">The spell info.</param>
		/// <returns>True if the player can handle the magic attack.</returns>
		private static bool ProcessPlayer(Player player, Models.Packets.Entities.InteractionPacket packet, Models.Packets.Spells.SpellPacket spellPacket, out Models.Spells.SpellInfo spellInfo)
		{
			spellInfo = null;
			ushort spellId = packet.MagicType;
			
			if (player.MaskedSkills.Contains(spellId))
			{
				spellInfo = Collections.SpellInfoCollection.GetHighestSpell(spellId);
			}
			else
			{
				if (spellId >= 3090 && spellId <= 3306)
				{
					spellId = 3090;
				}
				
				if (spellId == 6012)
				{
					spellInfo = Collections.SpellInfoCollection.GetSpellInfo(6010, 0);
					packet.X = player.X;
					packet.Y = player.Y;
				}
				else
				{
					byte choseLevel = 0;
					if (spellId == packet.MagicType)
					{
						if (!player.Spells.ContainsSkill(spellId))
						{
							return false;
						}
						
						choseLevel = (byte)player.Spells.GetOrCreateSkill(spellId).Level;
					}
					if (!Collections.SpellInfoCollection.ContainsSpell(spellId, choseLevel))
					{
						spellInfo = Collections.SpellInfoCollection.GetHighestSpell(spellId);
					}
					else
					{
						spellInfo = Collections.SpellInfoCollection.GetSpellInfo(spellId, choseLevel);
					}
				}
			}
			
			if (spellInfo == null)
			{
				return false;
			}
			
			spellPacket.SpellId = spellInfo.Id;
			spellPacket.SpellLevel = spellInfo.Level;
			
			if (player.Battle != null)
			{
				return player.Battle.HandleBeginHit_Magic(player, spellPacket);
			}
			
			return true;
		}
	}
}
