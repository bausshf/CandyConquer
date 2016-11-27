/// <summary>
/// GWGuardian - TwinCity
/// </summary>
[ScriptEntry(Entry = 128012)]
public static void GWGuardian_TwinCity(Player player, byte option)
{
	switch (option)
	{
		case 0:
			{
				Dialog.SendDialog(player, GWGuardianMessage(player, "WELCOME_MSG"));
				Dialog.SendOption(player,  Message(player, "YES"), 1);
				Dialog.SendOption(player,  Message(player, "NO"), 255);
				Dialog.Finish(player);
				break;
			}
		case 1:
			{
				int startX = 345;
				int startY = 327;
				int x = CandyConquer.Drivers.Repositories.Safe.Random.Next(startX - 10, startX + 10);
				int y = CandyConquer.Drivers.Repositories.Safe.Random.Next(startY - 10, startY + 10);
				player.Teleport(1038, (ushort)x, (ushort)y);
				break;
			}
	}
}

/// <summary>
/// GWGuardian - GWMap
/// </summary>
[ScriptEntry(Entry = 128013)]
public static void GWGuardian_GWMap(Player player, byte option)
{
	switch (option)
	{
		case 0:
			{
				Dialog.SendDialog(player, GWGuardianMessage(player, "BACK_MSG"));
				Dialog.SendOption(player,  Message(player, "YES"), 1);
				Dialog.SendOption(player,  Message(player, "NO"), 255);
				Dialog.Finish(player);
				break;
			}
		case 1:
			{
				player.Teleport(1002);
				break;
			}
	}
}