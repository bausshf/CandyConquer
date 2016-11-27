/// <summary>
/// The win items id.
/// </summary>
private const uint WineItemId = 711328;

/// <summary>
/// Alcoholic
/// </summary>
[ScriptEntry(Entry = 130002)]
public static void Alcoholic(Player player, byte option)
{
	switch (option)
	{
		case 0:
			{
				Dialog.SendDialog(player, AlcoholicMessage(player, "WELCOME_MSG"));
				Dialog.SendOption(player,  AlcoholicMessage(player, "HERE_IS_A_WINE"), 1);
				Dialog.SendOption(player,  Message(player, "NEVERMIND"), 255);
				Dialog.Finish(player);
				break;
			}
		case 1:
			{
				if (player.Inventory.RemoveById(WineItemId))
				{
					player.Teleport(1023, 312, 646);
				}
				else
				{
					Dialog.SendDialog(player, AlcoholicMessage(player, "NO_WINE"));
					Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
					Dialog.Finish(player);
				}
				break;
			}
	}
}
