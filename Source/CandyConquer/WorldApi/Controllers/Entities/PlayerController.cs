// Project by Bauss
using System;
using System.Threading.Tasks;

namespace CandyConquer.WorldApi.Controllers.Entities
{
	/// <summary>
	/// Controller for players.
	/// </summary>
	public class PlayerController : AttackableEntityController
	{
		/// <summary>
		/// Gets the player associated with the controller.
		/// </summary>
		public Models.Entities.Player Player { get; protected set; }
		
		/// <summary>
		/// Constructor for the controller.
		/// </summary>
		public PlayerController()
			: base()
		{
		}
		
		/// <summary>
		/// Updates the basic stats such as strength, vitality, agility and spirit.
		/// </summary>
		/// <param name="stats">Forced stats. (Set to null to get stats from database.)</param>
		/// <remarks>Calls UpdateHealthAndMana().</remarks>
		public void UpdateStats(ushort[] stats = null)
		{
			if (stats == null)
			{
				if (Player.Reborns > 0)
				{
					return;
				}
				
				if (Player.Level > 120)
				{
					return;
				}
				
				stats = Collections.StatCollection.Get(Player.Job, Player.Level);
			}
			
			Player.Strength = stats[0];
			Player.Vitality = stats[1];
			Player.Agility = stats[2];
			Player.Spirit = stats[3];
			
			UpdateBaseStats();
		}
		
		/// <summary>
		/// Updates the base stats.
		/// </summary>
		public void UpdateBaseStats()
		{
			UpdateBattleStats();
			
			double maxHp = (double)(Player.Strength * 3);
			maxHp += (double)(Player.Agility * 3);
			maxHp += (double)(Player.Vitality * 24);
			maxHp += (double)(Player.Spirit * 3);
			maxHp += (double)Player.BonusHP;
			
			switch (Player.Job)
			{
				case Enums.Job.Trojan:
					maxHp *= 1.05;
					break;
				case Enums.Job.VeteranTrojan:
					maxHp *= 1.08;
					break;
				case Enums.Job.TigerTrojan:
					maxHp *= 1.10;
					break;
				case Enums.Job.DragonTrojan:
					maxHp *= 1.12;
					break;
				case Enums.Job.TrojanMaster:
					maxHp *= 1.15;
					break;
			}
			
			Player.MaxHP = (int)maxHp;
			
			int maxMp = (Player.Spirit * 5);
			maxMp += Player.BonusMP;
			
			Player.MaxMP = maxMp;
			
			if (Player.LoggedIn)
			{
				Player.ClientSocket.Send(new Models.Packets.Entities.PlayerStatsPacket
				                         {
				                         	Player = this.Player
				                         });
			}
		}
		
		/// <summary>
		/// Updates the client with an unsigned value.
		/// </summary>
		/// <param name="updateType">The update type.</param>
		/// <param name="value">The value.</param>
		public void UpdateClient(Enums.UpdateClientType updateType, ulong value)
		{
			if (!Player.LoggedIn)
			{
				return;
			}
			
			Player.ClientSocket.Send(new Models.Packets.Client.UpdateClientPacket
			                         {
			                         	ClientId = Player.ClientId,
			                         	UpdateType = updateType,
			                         	Value = value
			                         });
		}
		
		/// <summary>
		/// Updates the client with a signed value.
		/// </summary>
		/// <param name="updateType">The update type.</param>
		/// <param name="value">The value.</param>
		public void UpdateClient(Enums.UpdateClientType updateType, long value)
		{
			UpdateClient(updateType, (ulong)value);
		}
		
		/// <summary>
		/// Updates the stamina.
		/// </summary>
		public void UpdateStamina()
		{
			if (Player.Stamina < 100)
			{
				if (Player.Action == Enums.PlayerAction.Lie &&
				    DateTime.UtcNow >= Player.LastStaminaUpdateTime.AddMilliseconds(Data.Constants.Time.StaminaLieTime))
				{
					Player.LastStaminaUpdateTime = DateTime.UtcNow;
					
					int stamina = Math.Min(100, Player.Stamina + 15);
					Player.Stamina = (byte)stamina;
				}
				else if (Player.Action == Enums.PlayerAction.Sit)
				{
					Player.LastStaminaUpdateTime = DateTime.UtcNow;
					
					int stamina = Math.Min(100, Player.Stamina + 10);
					Player.Stamina = (byte)stamina;
				}
			}
		}
		
		/// <summary>
		/// Updates the battle stats.
		/// </summary>
		private void UpdateBattleStats()
		{
			#region Reset
			Player.DragonGemPercentage = 0;
			Player.PhoenixGemPercentage = 0;
			Player.RainbowGemPercentage = 0;
			Player.TortoiseGemPercentage = 0;
			Player.VioletGemPercentage = 0;
			Player.MoonGemPercentage = 0;
			
			Player.BonusHP = 0;
			Player.BonusMP = 0;
			
			Player.MinAttack = 0;
			Player.MaxAttack = 0;
			Player.Defense = 0;
			Player.MagicAttack = 0;
			Player.MagicDefense = 0;
			
			Player.Dodge = 0;
			Player.Accuracy = 0;
			Player.MagicDefensePercentage = 0;
			Player.Bless = 0;
			
			Player.FinalPhysicalDamage = 0;
			Player.FinalMagicDamage = 0;
			Player.FinalPhysicalDefense = 0;
			Player.FinalMagicDefense = 0;
			
			Player.CriticalStrike = 0;
			Player.Block = 0;
			Player.BreakThrough = 0;
			Player.CounterAction = 0;
			Player.SkillCriticalStrike = 0;
			Player.Immunity = 0;
			Player.Penetration = 0;
			Player.Detoxication = 0;
			
			Player.MetalDefense = 0;
			Player.WaterDefense = 0;
			Player.FireDefense = 0;
			Player.EarthDefense = 0;
			Player.WoodDefense = 0;
			#endregion
			
			foreach (var item in Player.Equipments.GetAll())
			{
				if (item.DbOwnerItem.CurrentDura <= 0)
					continue;
				
				if (item.Position == Enums.ItemPosition.WeaponL)
				{
					Player.MinAttack += item.DbItem.MinAttack / 2;
					Player.MaxAttack += item.DbItem.MaxAttack / 2;
				}
				else
				{
					Player.MinAttack += item.DbItem.MinAttack;
					Player.MaxAttack += item.DbItem.MaxAttack;
				}
				
				Player.MagicAttack += item.DbItem.MagicAttack;
				Player.Defense += item.DbItem.Defense;
				
				Player.Bless += (item.DbOwnerItem.Bless * 0.01);
				
				AppendGemPercentage(item.Gem1);
				AppendGemPercentage(item.Gem2);
				
				var addition = item.Addition;
				if (addition != null)
				{
					Player.BonusHP += addition.HP;
					Player.MinAttack += (int)addition.MinAttack;
					Player.MaxAttack += (int)addition.MaxAttack;
					Player.Defense += addition.Defense;
					Player.MagicAttack += addition.MagicAttack;
					Player.MagicDefense += addition.MagicDefense;
					Player.Dodge += addition.Dodge;
				}
				
				Player.MagicDefensePercentage += (item.DbItem.MagicDefense * 0.01);
				Player.Dodge += item.DbItem.Dodge;
				Player.BonusHP += (item.DbOwnerItem.Enchant + item.DbItem.HP);
				Player.BonusMP += item.DbItem.MP;
			}
		}
		
		/// <summary>
		/// Appends the gem percentage stats.
		/// </summary>
		/// <param name="gem">The gem to append the stats of.</param>
		private void AppendGemPercentage(Enums.Gem gem)
		{
			switch (gem)
			{
				case Enums.Gem.NormalPhoenixGem:
				case Enums.Gem.RefinedPhoenixGem:
				case Enums.Gem.SuperPhoenixGem:
					Player.PhoenixGemPercentage += (0.05 * ((byte)gem));
					break;
				case Enums.Gem.NormalDragonGem:
				case Enums.Gem.RefinedDragonGem:
				case Enums.Gem.SuperDragonGem:
					Player.DragonGemPercentage += (0.05 * (((byte)gem) - 10));
					break;
				case Enums.Gem.NormalRainbowGem:
					Player.RainbowGemPercentage += 0.1;
					break;
				case Enums.Gem.RefinedRainbowGem:
					Player.RainbowGemPercentage += 0.15;
					break;
				case Enums.Gem.SuperRainbowGem:
					Player.RainbowGemPercentage += 0.25;
					break;
				case Enums.Gem.NormalVioletGem:
					Player.VioletGemPercentage += 0.3;
					break;
				case Enums.Gem.RefinedVioletGem:
					Player.VioletGemPercentage += 0.5;
					break;
				case Enums.Gem.SuperVioletGem:
					Player.VioletGemPercentage += 1.0;
					break;
					// fury
					// kylin
				case Enums.Gem.NormalMoonGem:
					Player.MoonGemPercentage += 0.15;
					break;
				case Enums.Gem.RefinedMoonGem:
					Player.MoonGemPercentage += 0.3;
					break;
				case Enums.Gem.SuperMoonGem:
					Player.MoonGemPercentage += 0.5;
					break;
				case Enums.Gem.NormalTortoiseGem:
				case Enums.Gem.RefinedTortoiseGem:
				case Enums.Gem.SuperTortoiseGem:
					Player.TortoiseGemPercentage += (0.02 * (((byte)gem) - 70));
					break;
			}
		}
		
		/// <summary>
		/// Updates the client with the player's guild.
		/// </summary>
		public void UpdateClientGuild()
		{
			var guild = Player.Guild;
			
			if (guild != null)
			{
				Player.ClientSocket.Send(new Models.Packets.Guilds.GuildAttributePacket
				                         {
				                         	GuildId = (uint)guild.Id,
				                         	Rank = Player.GuildMember.Rank,
				                         	EnrollmentDate = Player.GuildMember.DbGuildRank.JoinDate,
				                         	SilverFund = guild.DbGuild.Fund,
				                         	CPsFund = guild.DbGuild.CPsFund,
				                         	Amount = guild.MemberCount,
				                         	GuildLeader = guild.GuildLeader.DbGuildRank.PlayerName,
				                         	RequiredLevel = 0,
				                         	RequiredMetempsychosis = 0,
				                         	RequiredProfession = 0,
				                         	Level = guild.DbGuild.Level
				                         });
			}
			else
			{
				Player.ClientSocket.Send(new Models.Packets.Guilds.GuildAttributePacket
				                         {
				                         	GuildId = 0,
				                         	Rank = Enums.GuildRank.None
				                         });
			}
			
			UpdateClientGuildAnnouncement();
			
			UpdateClientGuildAssociation();
			
			UpdateScreen(true);
		}
		
		/// <summary>
		/// Updates the client with the player's guild's associations.
		/// </summary>
		private void UpdateClientGuildAssociation()
		{
			if (Player.Guild == null)
			{
				return;
			}
			
			Player.Guild.UpdateClientGuildAssociation(Player);
		}
		
		/// <summary>
		/// Updates the client with the player's guild's announcement.
		/// </summary>
		private void UpdateClientGuildAnnouncement()
		{
			if (Player.Guild == null)
			{
				Player.ClientSocket.Send(new Models.Packets.Guilds.GuildPacket
				                         {
				                         	Action = Enums.GuildAction.SetAnnounce,
				                         	Data = Drivers.Time.GetTime(Drivers.Time.TimeType.Day),
				                         	Announcement = Drivers.Messages.SAMPLE_GUILD_ANNOUNCEMENT
				                         });
				return;
			}
			
			Player.ClientSocket.Send(new Models.Packets.Guilds.GuildPacket
			                         {
			                         	Action = Enums.GuildAction.SetAnnounce,
			                         	Data = Player.Guild.DbGuild.AnnouncementDate,
			                         	Announcement = Player.Guild.DbGuild.Announcement
			                         });
		}
		
		/// <summary>
		/// Adds a specific amount of experience to the player.
		/// </summary>
		/// <param name="newExperience">The new amount of experience.</param>
		public void AddExperience(ulong newExperience)
		{
			if (Player.Level >= Data.Constants.GameMode.MaxLevel)
			{
				return;
			}
			
			ulong requiredExperience = Data.Constants.Level.GetLevelExperience(Player.Level);
			
			newExperience *= Data.Constants.GameMode.PlayerExperienceRate;
			newExperience += (ulong)(newExperience * Player.RainbowGemPercentage);
			
			Player.Experience += newExperience;
			
			if (Player.Experience >= requiredExperience)
			{
				long nextExperience = (long)Player.Experience;
				Player.Experience = 0;
				byte addLevels = 1;
				nextExperience -= (long)requiredExperience;
				
				while (nextExperience >= (long)requiredExperience && addLevels <= 12 &&
				       (Player.Level + addLevels) < Data.Constants.GameMode.MaxLevel)
				{
					addLevels++;
					requiredExperience = Data.Constants.Level.GetLevelExperience((byte)(Player.Level + addLevels));
					nextExperience -= (long)requiredExperience;
				}
				
				Player.Level += addLevels;
				
				if (Player.Reborns > 0)
				{
					Player.AttributePoints += (ushort)(addLevels * 3);
				}
				
				if (addLevels >= 12)
				{
					Player.Experience = 0;
				}
				else
				{
					Player.Experience = (ulong)nextExperience;
				}
				
				var levelUpEffect = new Models.Packets.Misc.StringPacket
				{
					Action = Enums.StringAction.RoleEffect,
					Data = Player.ClientId,
					String = "LevelUp"
				};
				
				Player.UpdateScreen(false, levelUpEffect);
				Player.ClientSocket.Send(levelUpEffect);
			}
			else if (Player.LoggedIn)
			{
				Player.SendFormattedSystemMessage("GAIN_EXP", true, newExperience);
			}
		}
		
		/// <summary>
		/// Loses dura when the player attacks.
		/// </summary>
		/// <param name="damage">The damage of the attack.</param>
		public void LoseAttackDura(uint damage)
		{
			if (Player.Battle != null)
			{
				return;
			}

			if (damage <= 1 && Tools.CalculationTools.ChanceSuccessBig(10) ||
			    Tools.CalculationTools.ChanceSuccessBig(25))
			{
				var duraItem = Tools.CalculationTools.ChanceSuccess(75) ?
					Player.Equipments.Get(Enums.ItemPosition.WeaponR, false) :
					Player.Equipments.Get(Enums.ItemPosition.WeaponL, false);

				if (duraItem != null && !duraItem.IsArrow && duraItem.DbOwnerItem.CurrentDura > 0)
				{
					duraItem.DbOwnerItem.CurrentDura -= 100;
					duraItem.UpdateDatabase();
					duraItem.UpdateClient(Player, Enums.ItemUpdateAction.Update);
					Player.UpdateBaseStats();
				}
			}
		}
		
		/// <summary>
		/// Loses dura when the player is attacked.
		/// </summary>
		/// <param name="damage">The damage.</param>
		public void LoseDefenseDura(uint damage)
		{
			if (Player.Battle != null)
			{
				return;
			}

			if (damage <= 1 && Tools.CalculationTools.ChanceSuccessBig(10) ||
			    damage >= (Player.MaxHP / 2) && Tools.CalculationTools.ChanceSuccess(90) ||
			    Tools.CalculationTools.ChanceSuccessBig(15))
			{
				Models.Items.Item duraItem = null;
				if (Tools.CalculationTools.ChanceSuccess(75))
				{
					duraItem = Player.Equipments.Get(Enums.ItemPosition.Armor, false);
				}

				if (duraItem == null && Tools.CalculationTools.ChanceSuccess(75))
				{
					duraItem = Player.Equipments.Get(Enums.ItemPosition.Head, false);
				}

				if (duraItem == null && Tools.CalculationTools.ChanceSuccess(75))
				{
					duraItem = Player.Equipments.Get(Enums.ItemPosition.Necklace, false);
				}

				if (duraItem == null && Tools.CalculationTools.ChanceSuccess(75))
				{
					duraItem = Player.Equipments.Get(Enums.ItemPosition.Ring, false);
				}

				if (duraItem == null && Tools.CalculationTools.ChanceSuccess(75))
				{
					duraItem = Player.Equipments.Get(Enums.ItemPosition.Boots, false);
				}

				if (Data.Constants.GameMode.AllowTower && duraItem == null && Tools.CalculationTools.ChanceSuccess(75))
				{
					duraItem = Player.Equipments.Get(Enums.ItemPosition.Tower, false);
				}

				if (Data.Constants.GameMode.AllowFan && duraItem == null && Tools.CalculationTools.ChanceSuccess(75))
				{
					duraItem = Player.Equipments.Get(Enums.ItemPosition.Fan, false);
				}

				if (duraItem != null && duraItem.DbOwnerItem.CurrentDura > 0)
				{
					duraItem.DbOwnerItem.CurrentDura -= 100;
					duraItem.UpdateDatabase();
					duraItem.UpdateClient(Player, Enums.ItemUpdateAction.Update);
					Player.UpdateBaseStats();
				}
			}
		}
		
		/// <summary>
		/// Handler for when the player has been killed.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="damage">The damage.</param>
		protected override void OnKill(AttackableEntityController attacker, uint damage)
		{
			if (attacker != null)
			{
				AddActionLog("Die", attacker.AttackableEntity.ClientId + " : " + attacker.Entity.Name);
			}
			else
			{
				AddActionLog("Die");
			}
			
			Player.DeadTime = DateTime.UtcNow.AddMilliseconds(2000); // To fix a jump glitch ...
			RemoveStatusFlag(Enums.StatusFlag.Fly);
			RemoveStatusFlag(Enums.StatusFlag.PartiallyInvisible);
			
			var attackerPlayer = attacker as Models.Entities.Player;
			if (attackerPlayer != null && attackerPlayer.ClientId != Player.ClientId)
			{
				if (attackerPlayer.Battle != null)
				{
					if (!attackerPlayer.Battle.HandleDeath(attackerPlayer, Player))
					{
						return;
					}
				}
				else if (!Player.Map.SafePK && !ContainsStatusFlag(Enums.StatusFlag.BlueName) && Player.PKPoints < 30)
				{
					if (Player.Guild != null && attackerPlayer.Guild != null)
					{
						attackerPlayer.NextPKPointRemoval = DateTime.UtcNow.AddMilliseconds(Data.Constants.Time.PKPointsRemovalTime);
						
						if (attackerPlayer.Guild.IsEnemy(Player.Guild))
						{
							attackerPlayer.PKPoints += 3;
						}
						else
						{
							attackerPlayer.PKPoints += 10;
						}
					}
					else
					{
						attackerPlayer.NextPKPointRemoval = DateTime.UtcNow.AddMilliseconds(Data.Constants.Time.PKPointsRemovalTime);
						attackerPlayer.PKPoints += 10;
					}
				}
			}
			
			Player.AttackPacket = null;
			Player.ReviveTime = DateTime.UtcNow.AddSeconds(20);
			AddStatusFlag(Enums.StatusFlag.Dead);
			AddStatusFlag(Enums.StatusFlag.Ghost);
			RemoveStatusFlag(Enums.StatusFlag.BlueName);
			Player.Stamina = 0;
			
			Player.TransformModel = Tools.CalculationTools.GetGhostTransform(Player.Model);
		}
		
		/// <summary>
		/// Handler for when the player revives.
		/// </summary>
		/// <param name="reviveHere">Boolean determining whether it should revive on its spot or not.</param>
		protected override void OnRevive(bool reviveHere)
		{
			AddActionLog("Revive");
			
			bool shouldRevive = Player.Battle == null ||
				Player.Battle.HandleRevive(Player);
			
			if (!reviveHere && shouldRevive)
			{
				Player.Teleport(Player.Map);
			}
			
			Player.Alive = true;
			Player.AttackPacket = null;
			Player.TransformModel = 0;
			RemoveStatusFlag(Enums.StatusFlag.Dead);
			RemoveStatusFlag(Enums.StatusFlag.Ghost);
			RemoveStatusFlag(Enums.StatusFlag.BlueName);
			
			Player.HP = Player.MaxHP;
			if (Player.MaxMP > 50)
			{
				if (Player.MP < (Player.MaxMP / 2))
					Player.MP = (Player.MaxMP / 2);
			}
			
			Player.Stamina = Data.Constants.GameMode.StartStamina;
			Player.ReviveProtectionEndTime = DateTime.UtcNow.AddSeconds(Data.Constants.Time.ReviveProtectionTime);
		}
		
		/// <summary>
		/// Validates a pk target for the player.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <returns>The status of the validation. (0 = success)</returns>
		public int ValidPkTarget(Models.Entities.Player target)
		{
			if (Player.Map.MapType == Database.Models.DbMap.DbMapType.NoPK)
			{
				return 1;
			}
			
			switch (Player.PKMode)
			{
				case Enums.PKMode.PK:
					return 0;
					
				case Enums.PKMode.Capture:
					return target.ContainsStatusFlag(Enums.StatusFlag.BlueName) ? 0 : 2;
					
				case Enums.PKMode.Team:
					{
						if (Player.Team != null && target.Team != null)
						{
							if (Player.Team.GetMembers().Contains(target))
							{
								return 3;
							}
						}
						
						if (Player.Guild != null && target.Guild != null)
						{
							if (Player.Guild.Id == target.Guild.Id)
							{
								return 4;
							}
							
							if (Player.Guild.IsAllie(target.Guild))
							{
								return 5;
							}
						}
						
						return 0;
					}
					
				default:
					return 6;
			}
		}
		
		/// <summary>
		/// Checks whether the player can use auto attack or not.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>True if the player can use auto attack.</returns>
		public bool CanUseAutoAttack(Models.Packets.Entities.InteractionPacket packet)
		{
			return Player.AttackPacket != null &&
				Player.AttackPacket.Packetstamp == packet.Packetstamp;
		}
		
		/// <summary>
		/// Uses an auto attack.
		/// </summary>
		/// <param name="packet">The packet.</param>
		public void UseAutoAttack(Models.Packets.Entities.InteractionPacket packet)
		{
			if (CanUseAutoAttack(packet))
			{
				Task.Run(async() => await UseAutoAttackAsync(packet));
			}
		}
		
		/// <summary>
		/// Uses an auto attack asynchronous with a delay.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private async Task UseAutoAttackAsync(Models.Packets.Entities.InteractionPacket packet)
		{
			await Task.Delay(Data.Constants.Time.AttackInterval);
			
			if (CanUseAutoAttack(packet))
			{
				Helpers.Packets.Interaction.Battle.Combat.Handle(Player, packet);
			}
		}
		
		/// <summary>
		/// Sends a system message to the player.
		/// </summary>
		/// <param name="message">The message name. View Data.Localization for more info.</param>
		/// <param name="topLeft">Boolean determining whether the message should be displayed top left or not.</param>
		public void SendSystemMessage(string message, bool topLeft = true)
		{
			var msg = Data.Localization.Language.GetMessage(Player.Language, message);
			Player.ClientSocket.Send(topLeft ?
			                         Controllers.Packets.Message.MessageController.CreateSystemTopLeft(msg, Player.Name) :
			                         Controllers.Packets.Message.MessageController.CreateSystemTalk(msg, Player.Name));
		}
		
		/// <summary>
		/// Sends a formatted system message to the player.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="topLeft">Boolean determining whether the message should be displayed top left or not.</param>
		/// <param name="formattingArgs">The formatting arguments.</param>
		public void SendFormattedSystemMessage(string message, bool topLeft, params object[] formattingArgs)
		{
			var msg = string.Format(Data.Localization.Language.GetMessage(Player.Language, message),
			                        formattingArgs);
			Player.ClientSocket.Send(topLeft ?
			                         Controllers.Packets.Message.MessageController.CreateSystemTopLeft(msg, Player.Name) :
			                         Controllers.Packets.Message.MessageController.CreateSystemTalk(msg, Player.Name));
		}
		
		/// <summary>
		/// Validates the login map.
		/// </summary>
		public void ValidateLoginMap()
		{
			var map = Collections.MapCollection.GetValidLoginMap(Player.Map, Player.LastMap);
			
			if (map == null || map.MapType == Database.Models.DbMap.DbMapType.NoLogin ||
			    map.MapType == Database.Models.DbMap.DbMapType.NoSkillsNoLogin)
			{
				Player.Teleport(1002, 400, 400);
			}
			else if (Player.Map == null || map.Id != Player.MapId)
			{
				Player.Teleport(map);
			}
			
			if (!Player.Map.ValidCoord(Player.X, Player.Y))
			{
				if (Player.Map.DefaultCoordinates.ContainsKey("Revive"))
				{
					Player.Teleport(Player.Map);
					
					Player.SendSystemMessage("STUCK_MAP");
				}
				else
				{
					Player.Teleport(1002);
					
					Player.SendSystemMessage("STUCK_MAP_NO_REVIVE");
				}
			}
		}
		
		/// <summary>
		/// Updates the client with nobility information.
		/// </summary>
		public void UpdateClientNobility()
		{
			var nobilityInfo = new Models.Nobility.NobilityInfoString
			{
				ClientId = Player.ClientId
			};
			var nobility = Player.Nobility;
			
			if (nobility != null)
			{
				nobilityInfo.Donation = nobility.Donation;
				nobilityInfo.Rank = nobility.Rank;
				nobilityInfo.Ranking = nobility.Ranking;
			}
			else
			{
				nobilityInfo.Rank = Enums.NobilityRank.Serf;
				nobilityInfo.Ranking = -1;
			}
			
			var nobilityPacket = new Models.Packets.Nobility.NobilityPacket(47)
			{
				Action = Enums.NobilityAction.Info
			};
			nobilityPacket.Offset = 8;
			nobilityPacket.WriteUInt32(Player.ClientId);
			nobilityPacket.Offset = 32;
			nobilityPacket.WriteStrings(nobilityInfo.ToString());
			
			Player.ClientSocket.Send(nobilityPacket);
		}
		
		/// <summary>
		/// Adds an action log of the player.
		/// </summary>
		/// <param name="actionName">The action name.</param>
		/// <param name="actionDescription">The action description.</param>
		public void AddActionLog(string actionName, object actionDescription = null)
		{
			(new Database.Models.DbActionTrace
			 {
			 	MapId = Player.Map != null ? (int?)Player.Map.Id : null,
			 	OwnerId = Player.DbPlayer.Id,
			 	AuthKey = Player.DbPlayer.Account.LastAuthKey,
			 	ClientId = Player.ClientId,
			 	ActionName = actionName,
			 	ActionDescription = actionDescription != null ? actionDescription.ToString() : null
			 }).Create();
		}
	}
}
