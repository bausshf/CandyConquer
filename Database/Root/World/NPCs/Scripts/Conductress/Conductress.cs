/// <summary>
/// The amount of money it costs to travel.
/// </summary>
private const int TravelMoney = 100;

/// <summary>
/// Handles invalid amount of money for players who travel.
/// </summary>
private static bool HandleInvalidTravelMoney(Player player, byte option)
{
	if (option != 0)
	{
		if (player.Money < TravelMoney)
		{
			Dialog.SendDialog(player,
			                  string.Format(ConductressMessage(player, "TRAVEL_INVALID_MONEY"),
			                                TravelMoney));
			Dialog.SendOption(player, Message(player, "I_SEE"), 255);
			Dialog.Finish(player);
			return false;
		}
		else
			player.Money -= TravelMoney;
	}
	
	return true;
}

/// <summary>
/// Conductress in TwinCity.
/// </summary>
[ScriptEntry(Entry = 1070)]
public static void ConductressTwinCity(Player player, byte option)
{
	if (!HandleInvalidTravelMoney(player, option))
	{
		return;
	}
	
	switch (option)
	{
		case 0:
			{
				Dialog.SendDialog(player, ConductressMessage(player, "STANDARD_MSG"));
				Dialog.SendOption(player, "PhoenixCastle.", 1);
				Dialog.SendOption(player, "ApeCity.", 2);
				Dialog.SendOption(player, "DesertCity.", 3);
				Dialog.SendOption(player, "BirdIsland.", 4);
				Dialog.SendOption(player, "MineCave.", 5);
				Dialog.SendOption(player, "Market.", 6);
				Dialog.Finish(player);
				break;
			}
		case 1:
			player.Teleport(1002, 960, 556);
			break;
		case 2:
			player.Teleport(1002, 556, 958);
			break;
		case 3:
			player.Teleport(1002, 64, 465);
			break;
		case 4:
			player.Teleport(1002, 228, 197);
			break;
		case 5:
			player.Teleport(1002, 57, 406);
			break;
		case 6:
			player.Teleport(1036, 218, 198);
			break;
	}
}

/// <summary>
/// Conductress in PhoenixCastle.
/// </summary>
[ScriptEntry(Entry = 1071)]
public static void ConductressPhoenixCastle(Player player, byte option)
{
	if (!HandleInvalidTravelMoney(player, option))
	{
		return;
	}
	
	switch (option)
	{
		case 0:
		{
			Dialog.SendDialog(player, ConductressMessage(player, "STANDARD_MSG"));
			Dialog.SendOption(player, "TwinCity.", 1);
			Dialog.SendOption(player, "Market.", 2);
			Dialog.Finish(player);
			break;
		}
		case 1:
			player.Teleport(1011, 11, 379);
			break;
		case 2:
			player.Teleport(1036, 218, 198);
			break;
	}
}

/// <summary>
/// Conductress in ApeCity.
/// </summary>
[ScriptEntry(Entry = 1072)]
public static void ConductressApeCity(Player player, byte option)
{
	if (!HandleInvalidTravelMoney(player, option))
	{
		return;
	}
	
	switch (option)
	{
		case 0:
		{
			Dialog.SendDialog(player, ConductressMessage(player, "STANDARD_MSG"));
			Dialog.SendOption(player, "TwinCity.", 1);
			Dialog.SendOption(player, "Market.", 2);
			Dialog.Finish(player);
			break;
		}
		case 1:
			player.Teleport(1020, 381, 14);
			break;
		case 2:
			player.Teleport(1036, 218, 198);
			break;
	}
}

/// <summary>
/// Conductress in DesertCity.
/// </summary>
[ScriptEntry(Entry = 1073)]
public static void ConductressDesertCity(Player player, byte option)
{
	if (!HandleInvalidTravelMoney(player, option))
	{
		return;
	}
	
	switch (option)
	{
		case 0:
		{
			Dialog.SendDialog(player, ConductressMessage(player, "STANDARD_MSG"));
			Dialog.SendOption(player, "TwinCity.", 1);
			Dialog.SendOption(player, "MysticCastle.", 2);
			Dialog.SendOption(player, "Market.", 3);
			Dialog.Finish(player);
			break;
		}
		case 1:
			player.Teleport(1000, 970, 667);
			break;
		case 2:
			player.Teleport(1000, 85, 323);
			break;
		case 3:
			player.Teleport(1036, 218, 198);
			break;
	}
}

/// <summary>
/// Conductress in BirdIsland.
/// </summary>
[ScriptEntry(Entry = 1074)]
public static void ConductressBirdIsland(Player player, byte option)
{
	if (!HandleInvalidTravelMoney(player, option))
	{
		return;
	}
	
	switch (option)
	{
		case 0:
		{
			Dialog.SendDialog(player, ConductressMessage(player, "STANDARD_MSG"));
			Dialog.SendOption(player, "TwinCity.", 1);
			Dialog.SendOption(player, "Market.", 2);
			Dialog.Finish(player);
			break;
		}
		case 1:
			player.Teleport(1015, 1014, 707);
			break;
		case 2:
			player.Teleport(1036, 218, 198);
			break;
	}
}

/// <summary>
/// Conductress at the market.
/// </summary>
[ScriptEntry(Entry = 1075)]
public static void ConductressMarket(Player player, byte option)
{
	switch (option)
	{
		case 0:
		{
			Dialog.SendDialog(player, ConductressMessage(player, "MARKET_MSG"));
			Dialog.SendOption(player, ConductressMessage(player, "MARKET_LEAVE"), 1);
			Dialog.SendOption(player, ConductressMessage(player, "MARKET_STAY"), 255);
			Dialog.Finish(player);
			break;
		}
		case 1:
			player.TeleportToLastMap();
			break;
	}
}