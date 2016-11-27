/// <summary>
/// Messages for the conductress npc.
/// </summary>
private static string ConductressMessage(Player player, string message)
{
	switch (player.Language)
	{
		case "English":
		default:
			return ConductressMessageEnglish(message);
	}
}
