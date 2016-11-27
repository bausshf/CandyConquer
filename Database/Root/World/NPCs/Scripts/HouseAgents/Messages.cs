/// <summary>
/// Messages for the house agent npcs.
/// </summary>
private static string HouseAgentMessage(Player player, string message)
{
	switch (player.Language)
	{
		case "English":
		default:
			return HouseAgentMessageEnglish(message);
	}
}
