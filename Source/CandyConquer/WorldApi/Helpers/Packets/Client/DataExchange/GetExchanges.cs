// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Packets.Client;
using CandyConquer.Drivers;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Client.DataExchange
{
	/// <summary>
	/// Helper for the get exchange login sub types of the data exchange packet.
	/// </summary>
	[ApiController()]
	public static class GetExchanges
	{
		/// <summary>
		/// Handles the get surroundings.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.GetSurroundings)]
		public static bool GetSurroundings(Models.Entities.Player player, DataExchangePacket packet)
		{
			if (player.Map != null)
			{
				player.Map.DisplayWeather(player);
			}
			
			player.UpdateScreen(true);
			
			if (player.MapChangeEvents.Count > 0)
			{
				Action<Models.Entities.Player> mapChangeEvent;
				while (player.MapChangeEvents.TryDequeue(out mapChangeEvent))
				{
					mapChangeEvent.Invoke(player);
				}
			}
			return true;
		}
		
		/// <summary>
		/// Handles the get item set.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.GetItemSet)]
		public static bool GetItemSet(Models.Entities.Player player, DataExchangePacket packet)
		{
			player.ClientSocket.Send(packet);
			player.LoggedIn = true;
			
			foreach (var dbOwnerItem in Database.Dal.Inventories.GetInventory(player.DbPlayer.Id))
			{
				var item = Collections.ItemCollection.CreateItemById((uint)dbOwnerItem.ItemId);
				item.DbOwnerItem = dbOwnerItem;
				if (!string.IsNullOrWhiteSpace(dbOwnerItem.Color))
				{
					item.Color = dbOwnerItem.Color.ToEnum<Enums.ItemColor>();
				}
				item.Gem1 = dbOwnerItem.Gem1.ToEnum<Enums.Gem>();
				item.Gem2 = dbOwnerItem.Gem2.ToEnum<Enums.Gem>();
				
				if (!player.Inventory.Add(item))
				{
					player.ClientSocket.Disconnect(Drivers.Messages.Errors.FAILED_TO_LOAD_INVENTORY);
					return false;
				}
			}
			
			foreach (var dbOwnerItem in Database.Dal.Equipments.GetEquipments(player.DbPlayer.Id))
			{
				var item = Collections.ItemCollection.CreateItemById((uint)dbOwnerItem.ItemId);
				item.DbOwnerItem = dbOwnerItem;
				item.Position = dbOwnerItem.Position.ToEnum<Enums.ItemPosition>();
				if (!string.IsNullOrWhiteSpace(dbOwnerItem.Color))
				{
					item.Color = dbOwnerItem.Color.ToEnum<Enums.ItemColor>();
				}
				item.Gem1 = dbOwnerItem.Gem1.ToEnum<Enums.Gem>();
				item.Gem2 = dbOwnerItem.Gem2.ToEnum<Enums.Gem>();
				
				if (!player.Equipments.Equip(item, item.Position, false, false, true))
				{
					player.ClientSocket.Disconnect(Drivers.Messages.Errors.FAILED_TO_LOAD_EQUIPMENTS);
					return false;
				}
			}

			player.UpdateScreen(false);
			return true;
		}
		
		/// <summary>
		/// Handles the get association.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.GetAssociation)]
		public static bool GetAssociation(Models.Entities.Player player, DataExchangePacket packet)
		{
			// load friends
			// load enemies
			
			player.ClientSocket.Send(packet);
			return true;
		}
		
		/// <summary>
		/// Handles the get weapon skill set.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.GetWeaponSkillSet)]
		public static bool GetWeaponSkillSet(Models.Entities.Player player, DataExchangePacket packet)
		{
			var proficiencies = Database.Dal.Spells.GetAllProficiencies(player.DbPlayer.Id);
			foreach (var dbSpell in proficiencies)
			{
				player.Spells.TryAddProficiency(dbSpell);
			}
			
			player.Spells.SendAllProficiencies();
			player.ClientSocket.Send(packet);
			return true;
		}
		
		/// <summary>
		/// Handles the get magic set.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.GetMagicSet)]
		public static bool GetMagicSet(Models.Entities.Player player, DataExchangePacket packet)
		{
			var spells = Database.Dal.Spells.GetAllSkills(player.DbPlayer.Id);
			foreach (var dbSpell in spells)
			{
				player.Spells.TryAddSkill(dbSpell);
			}
			
			player.Spells.SendAllSkills();
			player.ClientSocket.Send(packet);
			return true;
		}
		
		/// <summary>
		/// Handles the get syn attr.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.GetSynAttr)]
		public static bool GetSynAttr(Models.Entities.Player player, DataExchangePacket packet)
		{
			int guildId;
			if (Database.Dal.Guilds.HasGuild(player.DbPlayer.Id, out guildId))
			{
				Models.Guilds.Guild guild;
				if (Collections.GuildCollection.TryGetGuild(guildId, out guild))
				{
					Models.Guilds.GuildMember member;
					if (guild.TryGetMember(player.DbPlayer.Id, out member))
					{
						player.Guild = guild;
						player.GuildMember = member;
						member.Player = player;
						
						player.UpdateClientGuild();
					}
				}
			}
			
			player.ClientSocket.Send(packet);
			return true;
		}
		
		/// <summary>
		/// Handles the get custom player data sub type.
		/// </summary>
		/// <param name="player">The player.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.GetCustomPlayerData)]
		public static bool GetCustomPlayerData(Models.Entities.Player player, Models.Packets.Client.DataExchangePacket packet)
		{
			#region Basic stats
			// To update it client side ...
			player.BoundCPs = player.BoundCPs;
			
			player.ValidateLoginMap();
			
			player.UpdateStats();
			
			player.Stamina = Data.Constants.GameMode.StartStamina;
			#endregion
			
			#region Status Flags
			if (!string.IsNullOrWhiteSpace(player.DbPlayer.StatusFlag))
			{
				player.StaticStatusFlag = player.DbPlayer.StatusFlag.ToEnum<Enums.StatusFlag>();
			}
			
			// Idk why, but for some reason you can't update the client any other way with the pk points and status flag ...
			var pkp = player.PKPoints;
			player.PKPoints = 0;
			player.PKPoints = pkp;
			#endregion
			
			#region Messages
			if (Collections.BroadcastQueue.LastMessage != null)
			{
				player.ClientSocket.Send(Collections.BroadcastQueue.LastMessage);
			}
			
			foreach (var dbWhisper in Database.Dal.WhisperCache.GetAllByRecipent(player.Name, Drivers.Settings.WorldSettings.Server))
			{
				player.ClientSocket.Send(
					Controllers.Packets.Message.MessageController.CreateWhisper(string.Concat("[Sent offline]", dbWhisper.Message), dbWhisper.From, dbWhisper.To, dbWhisper.Mesh, player.Mesh)
				);
				
				dbWhisper.Delete();
			}
			#endregion
			
			#region Houses
			var houses = Database.Dal.PlayerHouses.GetAll(player.DbPlayer.Id);
			if (houses.Count > 0)
			{
				bool anyRent = false;
				
				foreach (var dbPlayerHouse in houses)
				{
					if (!anyRent && dbPlayerHouse.IsLeasing && dbPlayerHouse.NextRentDate.HasValue &&
					    DateTime.UtcNow >= dbPlayerHouse.NextRentDate.Value)
					{
						anyRent = true;
					}
					
					var playerHouse = new Models.Maps.PlayerHouse(player, dbPlayerHouse);
					player.Houses.Add(playerHouse);
					
					if (dbPlayerHouse.Warehouse)
					{
						player.Warehouses.CreateWarehouse((uint)(dbPlayerHouse.MapId + 100000));
						playerHouse.CreateWarehouse();
					}
				}
				
				if (anyRent)
				{
					player.SendSystemMessage("RENT_DUE");
				}
			}
			#endregion
			
			#region Warehouse
			foreach (var dbOwnerItem in Database.Dal.Warehouses.GetWarehouseItems(player.DbPlayer.Id))
			{
				var item = Collections.ItemCollection.CreateItemById((uint)dbOwnerItem.ItemId);
				item.DbOwnerItem = dbOwnerItem;
				if (!string.IsNullOrWhiteSpace(dbOwnerItem.Color))
				{
					item.Color = dbOwnerItem.Color.ToEnum<Enums.ItemColor>();
				}
				item.Gem1 = dbOwnerItem.Gem1.ToEnum<Enums.Gem>();
				item.Gem2 = dbOwnerItem.Gem2.ToEnum<Enums.Gem>();
				
				Collections.Warehouse warehouse;
				if (player.Warehouses.TryGetWarehouse(dbOwnerItem.LocationId.Value, out warehouse))
				{
					if (!warehouse.Add(item, true))
					{
						player.ClientSocket.Disconnect(Drivers.Messages.Errors.FAILED_TO_LOAD_WAREHOUSE);
						return false;
					}
				}
			}
			#endregion
			
			#region Nobility
			Models.Nobility.NobilityDonation nobility;
			if (Collections.NobilityBoard.TryGetNobility(player.DbPlayer.Id, out nobility))
			{
				nobility.Player = player;
				player.Nobility = nobility;
			}
			
			player.UpdateClientNobility();
			#endregion
			
			#region VIP
			byte vipLevel = 0;
			if (player.DbPlayer.DonationPoints >= 6)
			{
				vipLevel = 6;
			}
			else if (player.DbPlayer.DonationPoints > 0)
			{
				vipLevel = (byte)player.DbPlayer.DonationPoints;
			}
			player.VIPLevel = vipLevel;
			#endregion
			
			#region Arena
			Models.Arena.ArenaInfo arenaInfo;
			if (!Collections.ArenaQualifierCollection.TryGetArenaInfo(player.DbPlayer.Id, out arenaInfo))
			{
				arenaInfo = new Models.Arena.ArenaInfo(null)
				{
					Player = player
				};
				
				if (!Collections.ArenaQualifierCollection.TryAddArenaInfo(player.DbPlayer.Id, arenaInfo))
				{
					player.ClientSocket.Disconnect(Drivers.Messages.Errors.FAILED_TO_INITIALIZE_ARENA_INFO);
				}
			}
			else
			{
				arenaInfo.Player = player;
			}
			
			player.ArenaInfo = arenaInfo;
			#endregion
			
			// TODO: Load subclass
			
			// TODO: Load heaven blessing
			
			#region Merchant
			player.UpdateClient(Enums.UpdateClientType.Merchant, 255UL);
			#endregion
			
			Server.FinishLogin.Handle(player);
			return true;
		}
	}
}
