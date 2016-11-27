private static bool HandleSupportCommand(Player player,
                                         string[] commandArgs,
                                         string fullCommand,
                                         string command,
                                         char commandPrefix)
{
	if ((byte)player.Permission < (byte)PlayerPermission.Support)
	{
		return false;
	}
	
	switch (commandArgs[0])
	{
		default:
			return false;
	}
}
