/// <summary>
/// Messages for tourny master npc.
/// </summary>
private static string TournyMasterMessage(Player player, string message)
{
	switch (player.Language)
	{
		case "English":
		default:
			return TournyMasterMessageEnglish(message);
	}
}
