/// <summary>
/// Messages for the guild director npc.
/// </summary>
private static string GuildDirectorMessage(Player player, string message)
{
	switch (player.Language)
	{
		case "English":
		default:
			return GuildDirectorMessageEnglish(message);
	}
}
