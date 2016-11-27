private static bool HandleVIPCommand(Player player,
                                     string[] commandArgs,
                                     string fullCommand,
                                     string command,
                                     char commandPrefix)
{
	if (player.VIPLevel == 0)
	{
		return false;
	}
	
	switch (commandArgs[0])
	{
		default:
			return false;
	}
}
