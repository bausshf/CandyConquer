private static bool HandleGMCommand(Player player,
                                    string[] commandArgs,
                                    string fullCommand,
                                    string command,
                                    char commandPrefix)
{
	if ((byte)player.Permission < (byte)PlayerPermission.GM)
	{
		return false;
	}
	
	switch (commandArgs[0])
	{
		default:
			return false;
	}
}
