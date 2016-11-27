[ScriptEntry(Entry = "HandleCommands")]
public static void HandleCommands(Player player,
                                  string fullCommand,
                                  string command,
                                  char commandPrefix)
{
	var commandArgs = command.Split(' ');
	
	if (!HandlePMCommand(player, commandArgs, fullCommand, command, commandPrefix))
	{
		if (!HandleGMCommand(player, commandArgs, fullCommand, command, commandPrefix))
		{
			if (!HandleModeratorCommand(player, commandArgs, fullCommand, command, commandPrefix))
			{
				if (!HandleSupportCommand(player, commandArgs, fullCommand, command, commandPrefix))
				{
					if (!HandleVIPCommand(player, commandArgs, fullCommand, command, commandPrefix))
					{
						HandlePlayerCommand(player, commandArgs, fullCommand, command, commandPrefix);
					}
				}
			}
		}
	}
}