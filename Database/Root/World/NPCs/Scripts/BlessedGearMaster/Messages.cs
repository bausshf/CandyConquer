/// <summary>
/// Messages for the blessed gear master npc.
/// </summary>
private static string BlessedGearMasterMessage(Player player, string message)
{
	switch (player.Language)
	{
		case "English":
		default:
			return BlessedGearMasterMessageEnglish(message);
	}
}
