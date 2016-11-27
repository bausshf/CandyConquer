// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CandyConquer.WorldApi.Models.Guilds;
using CandyConquer.Drivers;

namespace CandyConquer.WorldApi.Controllers.Guilds
{
	/// <summary>
	/// Controller for guilds.
	/// </summary>
	public class GuildController
	{
		/// <summary>
		/// Gets the guild associated with the controller.
		/// </summary>
		public Guild Guild { get; protected set; }
		
		/// <summary>
		/// The allies.
		/// </summary>
		private ConcurrentDictionary<int,Guild> _allies;
		/// <summary>
		/// The enemies.
		/// </summary>
		private ConcurrentDictionary<int,Guild> _enemies;
		/// <summary>
		/// The members.
		/// </summary>
		private ConcurrentDictionary<int,GuildMember> _members;
		
		/// <summary>
		/// The cached members after sorting.
		/// </summary>
		/// <remarks>Setting this to null will force a re-cache.</remarks>
		private ReadOnlyCollection<GuildMember> _cachedSortedMembers;
		
		/// <summary>
		/// The guild house.
		/// </summary>
		private Models.Maps.DynamicMap _house;
		
		/// <summary>
		/// Creates a new guild controller.
		/// </summary>
		public GuildController()
		{
			_allies = new ConcurrentDictionary<int, Guild>();
			_enemies = new ConcurrentDictionary<int, Guild>();
			_members = new ConcurrentDictionary<int, GuildMember>();
		}
		
		/// <summary>
		/// Gets the allie count.
		/// </summary>
		public int AllieCount
		{
			get { return _allies.Count; }
		}
		
		/// <summary>
		/// Gets the enemy count.
		/// </summary>
		public int EnemyCount
		{
			get { return _enemies.Count; }
		}
		
		/// <summary>
		/// Gets the member count.
		/// </summary>
		public int MemberCount
		{
			get { return _members.Count; }
		}
		
		/// <summary>
		/// Loads the guild.
		/// </summary>
		public void Load()
		{
			foreach (var member in Database.Dal.Guilds.GetAllMembers(Guild.Id))
			{
				var guildMember = new GuildMember(member);
				if (guildMember.Rank == Enums.GuildRank.GuildLeader)
				{
					Guild.GuildLeader = guildMember;
				}
				
				_members.TryAdd(member.PlayerId, guildMember);
			}
		}
		
		/// <summary>
		/// Loads the guild associations.
		/// </summary>
		public void LoadAssociations()
		{
			foreach (var allie in Database.Dal.Guilds.GetAllAssociations(Guild.Id, "Allie"))
			{
				Guild allieGuild;
				if (Collections.GuildCollection.TryGetGuild(allie.AssociationGuildId, out allieGuild))
				{
					if (Database.Dal.Guilds.IsAssociation(allieGuild.Id, Guild.Id, "Allie"))
					{
						_allies.TryAdd(allieGuild.Id, allieGuild);
					}
					else
					{
						Database.Dal.Guilds.DeleteAssociation(Guild.Id, allieGuild.Id);
					}
				}
				else
				{
					Database.Dal.Guilds.DeleteAssociation(Guild.Id, allieGuild.Id);
				}
			}
			
			foreach (var enemy in Database.Dal.Guilds.GetAllAssociations(Guild.Id, "Enemy"))
			{
				Guild enemyGuild;
				if (Collections.GuildCollection.TryGetGuild(enemy.AssociationGuildId, out enemyGuild))
				{
					_enemies.TryAdd(enemyGuild.Id, enemyGuild);
				}
				else
				{
					Database.Dal.Guilds.DeleteAssociation(Guild.Id, enemyGuild.Id);
				}
			}
		}
		
		/// <summary>
		/// Sets the announcement of the guild.
		/// </summary>
		/// <param name="announcement">The announcement.</param>
		public void SetAnnouncement(string announcement)
		{
			Guild.DbGuild.Announcement = announcement;
			Guild.DbGuild.AnnouncementDate = Drivers.Time.GetTime(Drivers.Time.TimeType.Day);
			Guild.DbGuild.Update();
			
			foreach (var member in GetMembers())
			{
				if (member.Online)
				{
					member.Player.UpdateClientGuild();
				}
			}
			
			SendMessage("GUILD_ANNOUNCEMENT_UPDATE", Enums.GuildMessageType.Normal);
		}
		
		/// <summary>
		/// Disbans the guild.
		/// </summary>
		public void Disban()
		{
			if (Collections.GuildCollection.RemoveGuild(Guild))
			{
				SendMessage("GUILD_DISBAN", Enums.GuildMessageType.Red);
				
				foreach (var member in GetMembers())
				{
					member.DbGuildRank.Delete();
					
					if (member.Online)
					{
						member.Player.Guild = null;
						member.Player.GuildMember = null;
						
						member.Player.UpdateClientGuild();
					}
				}
				
				_members.Clear();
				
				foreach (var allie in _allies.Values)
				{
					allie.RemoveAllie(Guild);
				}
				
				foreach (var enemy in _enemies.Values)
				{
					enemy.RemoveAllie(Guild);
				}
				
				Database.Dal.Guilds.DeleteAllAssociations(Guild.Id);
				Guild.DbGuild.Delete();
			}
		}
		
		/// <summary>
		/// Removes an allie from the guild.
		/// </summary>
		/// <param name="guild">The guild to remove.</param>
		/// <param name="associationsDeleted">A boolean determining whether the association has been deleted.</param>
		public void RemoveAllie(Guild guild, bool associationsDeleted = false)
		{
			Guild removedAllie;
			if (_allies.TryRemove(guild.Id, out removedAllie))
			{
				if (!associationsDeleted)
				{
					Database.Dal.Guilds.DeleteAssociation(Guild.Id, guild.Id);
					Database.Dal.Guilds.DeleteAssociation(guild.Id, Guild.Id);
					
					guild.RemoveAllie(Guild, true);
				}
				
				var clearAllyPacket = new Models.Packets.Guilds.GuildPacket
				{
					Data = (uint)guild.Id,
					Action = Enums.GuildAction.ClearAlly
				};
				
				foreach (var member in GetMembers())
				{
					if (member.Online)
					{
						member.Player.ClientSocket.Send(clearAllyPacket);
					}
				}
			}
		}
		
		/// <summary>
		/// Removes an enemy.
		/// </summary>
		/// <param name="guild">The guild to remove.</param>
		public void RemoveEnemy(Guild guild)
		{
			Guild removedEnemy;
			if (_enemies.TryRemove(guild.Id, out removedEnemy))
			{
				Database.Dal.Guilds.DeleteAssociation(Guild.Id, guild.Id);
				Database.Dal.Guilds.DeleteAssociation(guild.Id, Guild.Id);
				
				var clearEnemyPacket = new Models.Packets.Guilds.GuildPacket
				{
					Data = (uint)guild.Id,
					Action = Enums.GuildAction.ClearEnemy
				};
				
				foreach (var member in GetMembers())
				{
					if (member.Online)
					{
						member.Player.ClientSocket.Send(clearEnemyPacket);
					}
				}
			}
		}
		
		/// <summary>
		/// Gets all allies.
		/// </summary>
		/// <returns>ICollection.</returns>
		public ICollection<Guild> GetAllies()
		{
			return _allies.Values;
		}
		
		/// <summary>
		/// Gets all enemies.
		/// </summary>
		/// <returns>ICollection.</returns>
		public ICollection<Guild> GetEnemies()
		{
			return _enemies.Values;
		}
		
		/// <summary>
		/// Gets all members.
		/// </summary>
		/// <returns>ICollection.</returns>
		public ICollection<GuildMember> GetMembers()
		{
			return _members.Values;
		}
		
		/// <summary>
		/// Gets all members paged.
		/// </summary>
		/// <param name="index">The index to retrieve from.</param>
		/// <returns>A read only collection of the members.</returns>
		/// <remarks>The sorting goes by rank, donation and then level.</remarks>
		public ReadOnlyCollection<GuildMember> GetPagedMembers(int index)
		{
			if (_cachedSortedMembers != null)
			{
				return _cachedSortedMembers;
			}
			
			if (_members.Count <= index)
			{
				_cachedSortedMembers = _members.Values.ToList().AsReadOnly();
				return _cachedSortedMembers;
			}
			else
			{
				_cachedSortedMembers = _members.Values
					.OrderByDescending(member => member.Rank)
					.ThenByDescending(member => member.DbGuildRank.CPDonation)
					.ThenByDescending(member => member.DbGuildRank.SilverDonation)
					.ThenByDescending(member => member.DbGuildRank.PlayerLevel)
					.Skip(index)
					.Take(Math.Min(_members.Count - index, 12))
					.ToList().AsReadOnly();
				return _cachedSortedMembers;
			}
		}
		
		/// <summary>
		/// Adds a member to the guild.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="rank">The starting rank.</param>
		/// <param name="update">A boolean determining whether the client of the player should be updated.</param>
		public void AddMember(Models.Entities.Player player, Enums.GuildRank rank, bool update = true)
		{
			var guildMember = new GuildMember(new Database.Models.DbGuildRank
			                                  {
			                                  	GuildId = Guild.Id,
			                                  	PlayerId = player.DbPlayer.Id,
			                                  	PlayerName = player.Name,
			                                  	PlayerLevel = player.Level,
			                                  	SilverDonation = 0,
			                                  	CPDonation = 0,
			                                  	GuildRank = rank.ToString(),
			                                  	JoinDate = Drivers.Time.GetTime(Drivers.Time.TimeType.Day)
			                                  });

			if (guildMember.DbGuildRank.Create() && _members.TryAdd(player.DbPlayer.Id, guildMember))
			{
				player.AddActionLog("JoinGuild", Guild.Id);
				
				guildMember.Player = player;
				player.Guild = Guild;
				player.GuildMember = guildMember;
				
				if (rank == Enums.GuildRank.GuildLeader)
				{
					Guild.GuildLeader = guildMember;
				}
				
				if (update)
				{
					player.UpdateClientGuild();
					player.UpdateScreen(true);
				}
			}
			
			_cachedSortedMembers = null;
		}
		
		/// <summary>
		/// Removes a member from the guild.
		/// </summary>
		/// <param name="name">The name of the member to remove.</param>
		/// <param name="kick">A boolean determining whether the removal is a kick or not.</param>
		/// <returns>True if the member was removed, false otherwise.</returns>
		public bool RemoveMember(string name, bool kick)
		{
			var member = _members.Values.Where(m => m.DbGuildRank.PlayerName == name).FirstOrDefault();
			if (member != null && member.Rank == Enums.GuildRank.Member)
			{
				if (kick)
				{
					SendMessageFormat("KICK_MEMBER", Enums.GuildMessageType.Red, name);
				}
				
				GuildMember removedMember;
				if (member.DbGuildRank.Delete() && _members.TryRemove(member.DbGuildRank.PlayerId, out removedMember))
				{
					member.Player.Guild = null;
					member.Player.GuildMember = null;
					
					if (member.Player != null)
					{
						member.Player.UpdateClientGuild();
						member.Player.UpdateScreen(true);
						
						member.Player = null;
					}
				}
				
				_cachedSortedMembers = null;
				return true;
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>
		/// Gets a member by their name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>The member if found, null otherwise.</returns>
		public GuildMember GetMemberByName(string name)
		{
			return GetMembers().Where(member => member.DbGuildRank.PlayerName == name).FirstOrDefault();
		}
		
		/// <summary>
		/// Updates the client of a player with the guild's associations.
		/// </summary>
		/// <param name="player">The player.</param>
		public void UpdateClientGuildAssociation(Models.Entities.Player player)
		{
			foreach (var allie in _allies.Values)
			{
				var alliePacket = new Models.Packets.Guilds.GuildPacket
				{
					Data = (uint)allie.Id,
					Action = Enums.GuildAction.SetAlly
				};
				
				player.ClientSocket.Send(alliePacket);
			}
			
			foreach (var enemy in _enemies.Values)
			{
				var enemyPacket = new Models.Packets.Guilds.GuildPacket
				{
					Data = (uint)enemy.Id,
					Action = Enums.GuildAction.SetEnemy
				};
				
				player.ClientSocket.Send(enemyPacket);
			}
		}
		
		/// <summary>
		/// Attempts to get a member by their id.
		/// </summary>
		/// <param name="playerId">The player id.</param>
		/// <param name="member">The member.</param>
		/// <returns>True if the member was retrieved.</returns>
		public bool TryGetMember(int playerId, out GuildMember member)
		{
			return _members.TryGetValue(playerId, out member);
		}
		
		/// <summary>
		/// Donates silvers.
		/// </summary>
		/// <param name="player">The player who's donating.</param>
		/// <param name="amount">The amount to donate.</param>
		/// <returns>True if the donation was a success.</returns>
		public bool DonateSilvers(Models.Entities.Player player, uint amount)
		{
			if (player.Money < amount)
			{
				return false;
			}
			
			player.AddActionLog("DonateSilversGuild", amount);
			player.Money -= amount;
			player.GuildMember.DbGuildRank.SilverDonation += amount;
			player.GuildMember.DbGuildRank.Update();
			Guild.DbGuild.Fund += amount;
			Guild.DbGuild.Update();
			
			player.UpdateClientGuild();
			player.ClientSocket.Send(new Models.Packets.Guilds.GuildDonationPacket
			                         {
			                         	Flags = Enums.GuildDonationFlags.AllDonations,
			                         	Money = player.GuildMember.DbGuildRank.SilverDonation,
			                         	CPs = player.GuildMember.DbGuildRank.CPDonation
			                         });
			
			_cachedSortedMembers = null;
			
			SendMessageFormat("GUILD_DONATE", Enums.GuildMessageType.Green, player.Name, amount, "Silvers");
			return true;
		}
		
		/// <summary>
		/// Donates cps.
		/// </summary>
		/// <param name="player">The player donating.</param>
		/// <param name="amount">The amount to donate.</param>
		/// <returns>True if the donation was a success.</returns>
		public bool DonateCPs(Models.Entities.Player player, uint amount)
		{
			if (player.CPs < amount)
			{
				return false;
			}
			
			player.AddActionLog("DonateCpsGuild", amount);
			player.CPs -= amount;
			player.GuildMember.DbGuildRank.CPDonation += amount;
			player.GuildMember.DbGuildRank.Update();
			Guild.DbGuild.CPsFund += amount;
			Guild.DbGuild.Update();
			
			player.UpdateClientGuild();
			player.ClientSocket.Send(new Models.Packets.Guilds.GuildDonationPacket
			                         {
			                         	Flags = Enums.GuildDonationFlags.AllDonations,
			                         	Money = player.GuildMember.DbGuildRank.SilverDonation,
			                         	CPs = player.GuildMember.DbGuildRank.CPDonation
			                         });
			
			_cachedSortedMembers = null;
			
			SendMessageFormat("GUILD_DONATE", Enums.GuildMessageType.Green, player.Name, amount, "CPs");
			return true;
		}
		
		/// <summary>
		/// Sends a message to the guild by formatting.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="messageType">The message type.</param>
		/// <param name="formatArgs">The formatting arguments.</param>
		public void SendMessageFormat(string message, Enums.GuildMessageType messageType, params object[] formatArgs)
		{
			foreach (var member in GetMembers())
			{
				if (member.Online)
				{
					uint msgColor = 0;
					switch (messageType) {
						case CandyConquer.WorldApi.Enums.GuildMessageType.Green:
							msgColor = 0x0000ff00;
							break;
						case CandyConquer.WorldApi.Enums.GuildMessageType.Red:
							msgColor = 0x00ff0000;
							break;
						case CandyConquer.WorldApi.Enums.GuildMessageType.Normal:
						default:
							msgColor = 0;
							break;
					}
					var msg = string.Format(Data.Localization.Language.GetMessage(member.Player.Language, message), formatArgs);
					member.Player.ClientSocket.Send(
						Controllers.Packets.Message.MessageController.Create(Enums.MessageType.Guild,
						                                                     Guild.DbGuild.Name, member.Player.Name,
						                                                     msg, color: msgColor)
					);
				}
			}
		}
		
		/// <summary>
		/// Sends a message to the guild.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="messagType">The message type.</param>
		public void SendMessage(string message, Enums.GuildMessageType messagType)
		{
			foreach (var member in GetMembers())
			{
				if (member.Online)
				{
					uint msgColor = 0;
					switch (messagType) {
						case CandyConquer.WorldApi.Enums.GuildMessageType.Green:
							msgColor = 0x00ff0000;
							break;
						case CandyConquer.WorldApi.Enums.GuildMessageType.Red:
							msgColor = 0xff000000;
							break;
						case CandyConquer.WorldApi.Enums.GuildMessageType.Normal:
						default:
							msgColor = 0;
							break;
					}
					var msg = Data.Localization.Language.GetMessage(member.Player.Language, message);
					member.Player.ClientSocket.Send(
						Controllers.Packets.Message.MessageController.Create(Enums.MessageType.Guild,
						                                                     Guild.DbGuild.Name, member.Player.Name,
						                                                     msg, color: msgColor)
					);
				}
			}
		}
		
		/// <summary>
		/// Broadcasts a message packet.
		/// </summary>
		/// <param name="message">The message packet.</param>
		public void BroadcastMessage(Models.Packets.Message.MessagePacket message)
		{
			foreach (var member in GetMembers())
			{
				if (member.Online)
				{
					member.Player.ClientSocket.Send(message);
				}
			}
		}
		
		/// <summary>
		/// Promotes/Unpromotes a member.
		/// </summary>
		/// <param name="member">The member.</param>
		/// <param name="promotionRank">The promotion rank. Member for unpromote.</param>
		/// <returns>True if the promotion or unpromotion was a success.</returns>
		public bool Promote(GuildMember member, Enums.GuildRank promotionRank)
		{
			if (promotionRank == Enums.GuildRank.None)
			{
				return false;
			}
			
			if (promotionRank == Enums.GuildRank.DeputyLeader && _members.Values.Count(m => m.Rank == Enums.GuildRank.DeputyLeader) >= 5)
			{
				return false;
			}
			
			var oldRank = member.Rank;
			member.Rank = promotionRank;
			member.DbGuildRank.Update();
			
			if (member.Online)
			{
				member.Player.UpdateClientGuild();
				member.Player.UpdateScreen(true);
			}
			
			if (promotionRank == Enums.GuildRank.Member)
			{
				if (member.Player != null)
				{
					if (oldRank == Enums.GuildRank.DeputyLeader)
					{
						SendMessageFormat("REMOVED_DEPUTY_LEADER", Enums.GuildMessageType.Red, member.Player.Name);
					}
					else
					{
						SendMessageFormat("UNPROMOTED_MEMBER", Enums.GuildMessageType.Red, member.Player.Name, oldRank.ToString());
					}
				}
			}
			else
			{
				if (member.Player != null)
				{
					if (promotionRank == Enums.GuildRank.DeputyLeader)
					{
						SendMessageFormat("NEW_DEPUTY_LEADER", Enums.GuildMessageType.Green, member.Player.Name);
					}
					else
					{
						SendMessageFormat("PROMOTED_MEMBER", Enums.GuildMessageType.Green, member.Player.Name, promotionRank.ToString());
					}
				}
			}
			
			_cachedSortedMembers = null;
			return true;
		}
		
		/// <summary>
		/// Adds an allie to the guild.
		/// </summary>
		/// <param name="guild">The guild to allie.</param>
		/// <param name="isAllie">A boolean determining whether the guild is an allie.</param>
		/// <returns>True if the allie was added.</returns>
		public bool AddAllie(Guild guild, bool isAllie = false)
		{
			if (_allies.Count >= 5)
			{
				return false;
			}
			
			var association = new Database.Models.DbGuildAssociation
			{
				GuildId = Guild.Id,
				AssociationGuildId = guild.Id,
				AssociationType = "Allie"
			};
			
			if (!isAllie && guild.AddAllie(Guild, true) && association.Create() && _allies.TryAdd(guild.Id, guild) ||
			    isAllie && association.Create() && _allies.TryAdd(guild.Id, guild))
			{
				var alliePacket = new Models.Packets.Guilds.GuildPacket
				{
					Data = (uint)guild.Id,
					Action = Enums.GuildAction.SetAlly
				};
				
				SendMessageFormat("NEW_ALLIE", Enums.GuildMessageType.Green, guild.DbGuild.Name);
				
				foreach (var member in GetMembers())
				{
					if (member.Online)
					{
						member.Player.ClientSocket.Send(alliePacket);
					}
				}
			}
			
			return true;
		}
		
		/// <summary>
		/// Adds an enemy guild.
		/// </summary>
		/// <param name="guild">The guild to add as an enemy.</param>
		/// <returns>True if the enemy was added.</returns>
		public bool AddEnemy(Guild guild)
		{
			if (_enemies.Count >= 5)
			{
				return false;
			}
			
			var association = new Database.Models.DbGuildAssociation
			{
				GuildId = Guild.Id,
				AssociationGuildId = guild.Id,
				AssociationType = "Enemy"
			};
			
			if (association.Create() && _enemies.TryAdd(guild.Id, guild))
			{
				var enemyPacket = new Models.Packets.Guilds.GuildPacket
				{
					Data = (uint)guild.Id,
					Action = Enums.GuildAction.SetEnemy
				};
				
				SendMessageFormat("NEW_ENEMY", Enums.GuildMessageType.Red, guild.DbGuild.Name);
				
				foreach (var member in GetMembers())
				{
					if (member.Online)
					{
						member.Player.ClientSocket.Send(enemyPacket);
					}
				}
			}
			
			return true;
		}
		
		/// <summary>
		/// Checks whether the guild is an allie.
		/// </summary>
		/// <param name="guild">The guild.</param>
		/// <returns>True if the guild is an allie.</returns>
		public bool IsAllie(Guild guild)
		{
			return _allies.ContainsKey(guild.Id);
		}
		
		/// <summary>
		/// Checks whether the guild is an enemy.
		/// </summary>
		/// <param name="guild">The guild.</param>
		/// <returns>True if the guild is an enemy.</returns>
		public bool IsEnemy(Guild guild)
		{
			return _enemies.ContainsKey(guild.Id);
		}
		
		/// <summary>
		/// Changes the leadership of the guild.
		/// </summary>
		/// <param name="newLeader">The new leader.</param>
		/// <param name="changeTime">The change time.</param>
		public void ChangeLeader(GuildMember newLeader, Enums.GuildLeaderChangeTime changeTime)
		{
			// TODO: this ...
			// note: if not permanent then the original leader can always claim the guild back.
			// note: the new leader must be a deputy
			// note: a permanent guild leader change cost 50% of the price of a new guild
			
			// _cachedSortedMembers = null;
		}
		
		/// <summary>
		/// Creates a house.
		/// </summary>
		public void CreateHouse()
		{
			Guild.DbGuild.HasHouse = true;
			
			if (_house == null)
			{
				_house = Collections.MapCollection.GetDynamicMap(1099);
				
				_house.Show();
				
				Guild.Warehouse = new CandyConquer.WorldApi.Collections.GuildWarehouse(Guild);
				
				foreach (var dbOwnerItem in Database.Dal.Warehouses.GetGuildWarehouseItems(Guild.Id))
				{
					var item = Collections.ItemCollection.CreateItemById((uint)dbOwnerItem.ItemId);
					item.DbOwnerItem = dbOwnerItem;
					if (!string.IsNullOrWhiteSpace(dbOwnerItem.Color))
					{
						item.Color = dbOwnerItem.Color.ToEnum<Enums.ItemColor>();
					}
					item.Gem1 = dbOwnerItem.Gem1.ToEnum<Enums.Gem>();
					item.Gem2 = dbOwnerItem.Gem2.ToEnum<Enums.Gem>();
					
					Guild.Warehouse.Add(item, null, true);
				}
				
				var dbNpc = new Database.Models.DbNpc
				{
					Id = 200000,
					NpcId = 200000,
					Type = "Normal",
					Name = "GuildWarehouse",
					MapId = _house.Id,
					X = 44,
					Y = 23,
					Flag = 2,
					Mesh = 5280,
					Avatar = 0,
					Server = Drivers.Settings.WorldSettings.Server
				};
				var npc = new Models.Entities.Npc(dbNpc);
				npc.TeleportDynamic(_house.Id, npc.X, npc.Y);
			}
			
			Guild.DbGuild.Update();
		}
		
		/// <summary>
		/// Let's a player enter the house.
		/// </summary>
		/// <param name="player">The player that should enter the house.</param>
		public void EnterHouse(Models.Entities.Player player)
		{
			if (player.Guild != null && player.Guild.Id == Guild.Id && _house != null)
			{
				player.AddActionLog("EnterGuildHouse", Guild.Id);
				player.TeleportDynamic(_house.Id, 53, 83);
			}
		}
		
		/// <summary>
		/// Checks whether a player is in the house or not.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <returns>True if the player is in the house.</returns>
		public bool InHouse(Models.Entities.Player player)
		{
			return _house != null && player.MapId == _house.Id;
		}
	}
}
