/// <summary>
/// Messages for the gw guardian npc.
/// </summary>
private static string GWGuardianMessage(Player player, string message)
{
	switch (player.Language)
	{
		case "English":
		default:
			return GWGuardianMessageEnglish(message);
	}
}
