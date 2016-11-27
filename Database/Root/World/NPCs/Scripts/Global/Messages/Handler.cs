/// <summary>
/// Globally used messages
/// </summary>
private static string Message(Player player, string message)
{
	switch (player.Language)
	{
		case "English":
		default:
			return MessageEnglish(message);
	}
}