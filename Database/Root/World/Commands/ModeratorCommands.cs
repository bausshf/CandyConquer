private static bool HandleModeratorCommand(Player player,
                                           string[] commandArgs,
                                           string fullCommand,
                                           string command,
                                           char commandPrefix)
{
	if ((byte)player.Permission < (byte)PlayerPermission.Moderator)
	{
		return false;
	}
	
	switch (commandArgs[0])
	{
		case "tele":
			HandleTeleport(player, commandArgs);
			return true;
		
		default:
			return false;
	}
}

private static void HandleTeleport(Player player, string[] commandArgs)
{
	if (commandArgs.Length != 4)
	{
		return;
	}
	
	int mapId;
	ushort x;
	ushort y;
	if (int.TryParse(commandArgs[1], out mapId) && ushort.TryParse(commandArgs[2], out x) && ushort.TryParse(commandArgs[3], out y))
	{
		player.Teleport(mapId, x, y);
	}
}