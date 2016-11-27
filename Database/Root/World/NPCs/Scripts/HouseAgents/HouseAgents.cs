/*
 * Player House Agents
 */
/// <summary>
/// TCHouseAgent
/// </summary>
[ScriptEntry(Entry = 111001)]
public static void TCHouseAgent(Player player, byte option)
{
	HandleHouseAgent(player, option);
}

/// <summary>
/// PCHouseAgent
/// </summary>
[ScriptEntry(Entry = 111002)]
public static void PCHouseAgent(Player player, byte option)
{
	HandleHouseAgent(player, option);
}

/// <summary>
/// ACHouseAgent
/// </summary>
[ScriptEntry(Entry = 111003)]
public static void ACHouseAgent(Player player, byte option)
{
	HandleHouseAgent(player, option);
}

/// <summary>
/// DCHouseAgent
/// </summary>
[ScriptEntry(Entry = 111004)]
public static void DCHouseAgent(Player player, byte option)
{
	HandleHouseAgent(player, option);
}

/// <summary>
/// BIHouseAgent
/// </summary>
[ScriptEntry(Entry = 111005)]
public static void BIHouseAgent(Player player, byte option)
{
	HandleHouseAgent(player, option);
}


private static void HandleHouseAgent(Player player, byte option)
{
	PlayerHouse house;
	player.Houses.TryGetHouse(player.MapId, out house);
	switch (option)
	{
		case 0:
			{
				if (house != null)
				{
					Dialog.SendDialog(player, HouseAgentMessage(player, "WELCOME_HOUSE_MSG"));
					Dialog.SendOption(player,  HouseAgentMessage(player, "ENTER"), 4);
					if (house.DbPlayerHouse.IsLeasing)
					{
						Dialog.SendOption(player,  HouseAgentMessage(player, "END_LEASE"), 5);
						if (DateTime.UtcNow >= house.DbPlayerHouse.NextRentDate)
						{
							Dialog.SendOption(player,  HouseAgentMessage(player, "PAY_RENT"), 7);
						}
					}
					
					if (!house.DbPlayerHouse.Warehouse)
					{
						Dialog.SendOption(player,  HouseAgentMessage(player, "BUY_WAREHOUSE"), 8);
					}
				}
				else
				{
					Dialog.SendDialog(player, string.Format(HouseAgentMessage(player, "WELCOME_NO_HOUSE_MSG"), player.Map.Name));
					Dialog.SendOption(player,  HouseAgentMessage(player, "RENT"), 1);
					Dialog.SendOption(player,  HouseAgentMessage(player, "BUY"), 6);
				}
				
				Dialog.SendOption(player,  Message(player, "NOTHING"), 255);
				Dialog.Finish(player);
				break;
			}
			
		case 1:
			{
				if (house != null) return;
				
				Dialog.SendDialog(player, HouseAgentMessage(player, "RENT_MSG"));
				Dialog.SendOption(player,  HouseAgentMessage(player, "RENT_SMALL"), 2);
				Dialog.SendOption(player,  HouseAgentMessage(player, "RENT_BIG"), 3);
				Dialog.SendOption(player,  Message(player, "NEVERMIND"), 255);
				Dialog.Finish(player);
				break;
			}
			
		case 2:
			if (house != null) return;
			RentHome(player, false);
			break;
			
		case 3:
			if (house != null) return;
			RentHome(player, true);
			break;
			
		case 4:
			if (house == null) return;
			house.EnterHouse(player);
			break;
			
		case 5:
			if (house == null || !house.DbPlayerHouse.IsLeasing) return;
			EndLease(house, player);
			break;
			
		case 6:
			if (house != null) return;
			BuyHouse(player);
			break;
			
		case 7:
			if (house == null || !house.DbPlayerHouse.IsLeasing) return;
			PayRent(house, player);
			break;
			
		case 8:
			if (house == null || house.DbPlayerHouse.Warehouse) return;
			BuyPlayerWarehouse(house, player);
			break;
	}
}

private static void RentHome(Player player, bool isBig)
{
	if (player.Money < (isBig ? 10000000 : 1000000))
	{
		Dialog.SendDialog(player, HouseAgentMessage(player, "NOT_ENOUGH_MONEY_RENT"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	var dbPlayerHouse = new CandyConquer.Database.Models.DbPlayerHouse
	{
		PlayerId = player.DbPlayer.Id,
		MapId = player.MapId,
		IsBig = isBig,
		Warehouse = false,
		IsLeasing = true,
		NextRentDate = DateTime.UtcNow,
		InvestedMoney = 0
	};
	if (dbPlayerHouse.Create())
	{
		var playerHouse = new PlayerHouse(player, dbPlayerHouse);
		if (playerHouse.PayRent() != RentStatus.Success)
		{
			dbPlayerHouse.Delete();
		}
		else
		{
			player.Houses.Add(playerHouse);
			playerHouse.EnterHouse(player);
		}
	}
}

private static void BuyHouse(Player player)
{
	if (player.Money < 100000000)
	{
		Dialog.SendDialog(player, HouseAgentMessage(player, "NOT_ENOUGH_MONEY_BUY"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	var dbPlayerHouse = new CandyConquer.Database.Models.DbPlayerHouse
	{
		PlayerId = player.DbPlayer.Id,
		MapId = player.MapId,
		IsBig = true,
		Warehouse = false,
		IsLeasing = false,
		InvestedMoney = 100000000
	};
	if (dbPlayerHouse.Create())
	{
		var playerHouse = new PlayerHouse(player, dbPlayerHouse);
		player.Houses.Add(playerHouse);
		playerHouse.EnterHouse(player);
	}
}

private static void EndLease(PlayerHouse house, Player player)
{
	if (house.DbPlayerHouse.Delete())
	{
		player.Houses.Remove(house);
		
		Dialog.SendDialog(player, HouseAgentMessage(player, "CONTRACT_ENDED"));
		Dialog.SendOption(player,  Message(player, "THANKS"), 255);
		Dialog.Finish(player);
	}
}

private static void PayRent(PlayerHouse house, Player player)
{
	switch (house.PayRent())
	{
		case RentStatus.NotEnoughMoney:
			{
				Dialog.SendDialog(player, HouseAgentMessage(player, "NOT_ENOUGH_MONEY_PAY_RENT"));
				Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
				Dialog.Finish(player);
				break;
			}
			
		case RentStatus.RentIsNotDue:
			{
				Dialog.SendDialog(player, string.Format(HouseAgentMessage(player, "RENT_IS_NOT_DUE"), house.DbPlayerHouse.NextRentDate));
				Dialog.SendOption(player,  Message(player, "THANKS"), 255);
				Dialog.Finish(player);
				break;
			}
			
		case RentStatus.Success:
			{
				Dialog.SendDialog(player, HouseAgentMessage(player, "RENT_PAID"));
				Dialog.SendOption(player,  Message(player, "THANKS"), 255);
				Dialog.Finish(player);
				break;
			}
			
		default:
			{
				player.ClientSocket.Disconnect("Failed to pay rent.");
				break;
			}
	}
}


private static void BuyPlayerWarehouse(PlayerHouse house, Player player)
{
	if (player.Money < 500000)
	{
		Dialog.SendDialog(player, HouseAgentMessage(player, "NOT_ENOUGH_MONEY_PLAYER_WAREHOUSE"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	player.Money -= 500000;
	house.DbPlayerHouse.Warehouse = true;
	house.DbPlayerHouse.Update();
	
	player.Warehouses.CreateWarehouse((uint)(house.DbPlayerHouse.MapId + 100000));
	house.CreateWarehouse();
	
	Dialog.SendDialog(player, HouseAgentMessage(player, "GOT_WAREHOUSE"));
	Dialog.SendOption(player,  Message(player, "THANKS"), 255);
	Dialog.Finish(player);
}

/*
 * Guild House Agents
 */
/// <summary>
/// TC GuildHouseAgent
/// </summary>
[ScriptEntry(Entry = 111006)]
public static void TCGuildHouseAgent(Player player, byte option)
{
	HandleGuildHouseAgent(player, option);
}

/// <summary>
/// PC GuildHouseAgent
/// </summary>
[ScriptEntry(Entry = 111007)]
public static void PCGuildHouseAgent(Player player, byte option)
{
	HandleGuildHouseAgent(player, option);
}

/// <summary>
/// AC GuildHouseAgent
/// </summary>
[ScriptEntry(Entry = 111008)]
public static void ACGuildHouseAgent(Player player, byte option)
{
	HandleGuildHouseAgent(player, option);
}

/// <summary>
/// DC GuildHouseAgent
/// </summary>
[ScriptEntry(Entry = 111009)]
public static void DCGuildHouseAgent(Player player, byte option)
{
	HandleGuildHouseAgent(player, option);
}

/// <summary>
/// BI GuildHouseAgent
/// </summary>
[ScriptEntry(Entry = 111010)]
public static void BIGuildHouseAgent(Player player, byte option)
{
	HandleGuildHouseAgent(player, option);
}

public static void HandleGuildHouseAgent(Player player, byte option)
{
	if (player.Guild == null)
	{
		Dialog.SendDialog(player, HouseAgentMessage(player, "NO_GUILD"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	if (!player.Guild.DbGuild.HasHouse && player.GuildMember.Rank != GuildRank.GuildLeader)
	{
		Dialog.SendDialog(player, HouseAgentMessage(player, "NO_GUILD_HOUSE"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	switch (option)
	{
		case 0:
			{
				if (player.Guild.DbGuild.HasHouse)
				{
					Dialog.SendDialog(player, HouseAgentMessage(player, "WELCOME_HOUSE_MSG"));
					Dialog.SendOption(player,  HouseAgentMessage(player, "ENTER_GUILD_HOUSE"), 2);
				}
				else
				{
					Dialog.SendDialog(player, string.Format(HouseAgentMessage(player, "WELCOME_NO_GUILD_HOUSE_MSG"), player.Map.Name));
					Dialog.SendOption(player,  HouseAgentMessage(player, "BUY"), 1);
				}
				
				Dialog.SendOption(player,  Message(player, "NOTHING"), 255);
				Dialog.Finish(player);
				break;
			}
			
		case 1:
			{
				if (player.Guild.DbGuild.HasHouse || player.GuildMember.Rank != GuildRank.GuildLeader) return;
				
				BuyGuildHouse(player);
				break;
			}
			
		case 2:
			{
				player.Guild.EnterHouse(player);
				break;
			}
	}
}

private static void BuyGuildHouse(Player player)
{
	if (player.Money < 250000000)
	{
		Dialog.SendDialog(player, HouseAgentMessage(player, "NOT_ENOUGH_MONEY_GUILD_HOUSE"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	player.Money -= 250000000;
	player.Guild.CreateHouse();
	player.Guild.EnterHouse(player);
}