/// <summary>
/// Messages for the alcoholic npc.
/// </summary>
private static string AlcoholicMessage(Player player, string message)
{
	switch (player.Language)
	{
		case "English":
		default:
			return AlcoholicMessageEnglish(message);
	}
}
