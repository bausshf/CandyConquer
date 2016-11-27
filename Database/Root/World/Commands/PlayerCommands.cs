private static void HandlePlayerCommand(Player player,
                                        string[] commandArgs,
                                        string fullCommand,
                                        string command,
                                        char commandPrefix)
{
	switch (commandArgs[0])
	{
		case "dc":
			HandleDisconnection(player);
			break;
		
		default:
			{
				player.SendFormattedSystemMessage("INVALID_COMMAND", false, fullCommand);
				break;
			}
	}
}

private static void HandleDisconnection(Player player)
{
	player.ClientSocket.Disconnect(CandyConquer.Drivers.Messages.COMMAND_DISCONNECT);
}