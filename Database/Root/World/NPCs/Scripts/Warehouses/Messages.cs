/// <summary>
/// Messages for warehouse npcs.
/// </summary>
private static string WarehouseMessage(Player player, string message)
{
	switch (player.Language)
	{
		case "English":
		default:
			return WarehouseMessageEnglish(message);
	}
}
