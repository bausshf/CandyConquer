private static bool AuthenticateWarehouse(Player player, byte option)
{
	if (string.IsNullOrWhiteSpace(player.WarehousePassword))
	{
		return true;
	}
	
	if (option == 101)
	{
		player.WarehouseAuthenticated = player.WarehousePassword == player.NpcInputData;
		
		if (!player.WarehouseAuthenticated)
		{
			Dialog.SendDialog(player, WarehouseMessage(player, "INVALID_PASSWORD"));
			Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
			Dialog.Finish(player);
			return false;
		}
	}
	else if (!player.WarehouseAuthenticated)
	{
		Dialog.SendDialog(player, WarehouseMessage(player, "ENTER_PASSWORD"));
		Dialog.SendInput(player, 101);
		Dialog.SendOption(player,  Message(player, "NEVERMIND"), 255);
		Dialog.Finish(player);
	}
	
	return player.WarehouseAuthenticated;
}

[ScriptEntry(Entry = 8)]
public static void TCWarehouse(Player player, byte option)
{
	if (AuthenticateWarehouse(player, option))
	{
		Dialog.OpenWindow(player, 4);
	}
}

[ScriptEntry(Entry = 10012)]
public static void MarketWarehouse(Player player, byte option)
{
	if (AuthenticateWarehouse(player, option))
	{
		Dialog.OpenWindow(player, 4);
	}
}

[ScriptEntry(Entry = 10028)]
public static void PCWarehouse(Player player, byte option)
{
	if (AuthenticateWarehouse(player, option))
	{
		Dialog.OpenWindow(player, 4);
	}
}

[ScriptEntry(Entry = 10011)]
public static void ACWarehouse(Player player, byte option)
{
	if (AuthenticateWarehouse(player, option))
	{
		Dialog.OpenWindow(player, 4);
	}
}

[ScriptEntry(Entry = 1027)]
public static void DCWarehouse(Player player, byte option)
{
	if (AuthenticateWarehouse(player, option))
	{
		Dialog.OpenWindow(player, 4);
	}
}

[ScriptEntry(Entry = 4101)]
public static void BIWarehouse(Player player, byte option)
{
	if (AuthenticateWarehouse(player, option))
	{
		Dialog.OpenWindow(player, 4);
	}
}

//[ScriptEntry(Entry = 44)]
//public static void SCWarehouse(Player player, byte option)
//{
//	Dialog.OpenWindow(player, 4);
//}

[ScriptEntry(Entry = 100000)]
public static void PlayerWarehouse(Player player, byte option)
{
	if (AuthenticateWarehouse(player, option))
	{
		Dialog.OpenWindow(player, 4);
	}
}

[ScriptEntry(Entry = 200000)]
public static void GuildWarehouse(Player player, byte option)
{
	if (AuthenticateWarehouse(player, option))
	{
		Dialog.OpenWindow(player, 4);
	}
}

[ScriptEntry(Entry = 128011)]
public static void WHGuardian(Player player, byte option)
{
	if (!AuthenticateWarehouse(player, option))
	{
		return;
	}
	
	if (option != 0 && option != 1)
	{
		option = 0;
	}
	
	switch (option)
	{
		case 0:
			{
				if (string.IsNullOrWhiteSpace(player.WarehousePassword))
				{
					Dialog.SendDialog(player, WarehouseMessage(player, "ENTER_NEW_PASSWORD"));
				}
				else
				{
					Dialog.SendDialog(player, WarehouseMessage(player, "UPDATE_PASSWORD"));
				}
				
				Dialog.SendInput(player, 1);
				Dialog.SendOption(player,  Message(player, "CANCEL"), 255);
				Dialog.Finish(player);
				break;
			}
			
		case 1:
			{
				if (player.NpcInputData.Length < 4 || player.NpcInputData.Length > 8)
				{
					Dialog.SendDialog(player, WarehouseMessage(player, "INVALID_PASSWORD_LENGTH"));
					Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
					Dialog.Finish(player);
					return;
				}
				
				player.WarehousePassword = player.NpcInputData;
				player.WarehouseAuthenticated = false;
				
				Dialog.SendDialog(player, WarehouseMessage(player, "PASSWORD_SET"));
				Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
				Dialog.Finish(player);
				break;
			}
	}
}