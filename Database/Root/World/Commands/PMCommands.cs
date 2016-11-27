private static bool HandlePMCommand(Player player,
                                    string[] commandArgs,
                                    string fullCommand,
                                    string command,
                                    char commandPrefix)
{
	if ((byte)player.Permission < (byte)PlayerPermission.PM)
	{
		return false;
	}
	
	switch (commandArgs[0])
	{
		default:
			return false;
	}
}
