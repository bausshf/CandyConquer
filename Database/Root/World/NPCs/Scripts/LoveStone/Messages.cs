/// <summary>
/// Messages for the gw guardian npc.
/// </summary>
private static string LoveStoneMessage(Player player, string message)
{
	switch (player.Language)
	{
		case "English":
		default:
			return LoveStoneMessageEnglish(message);
	}
}
