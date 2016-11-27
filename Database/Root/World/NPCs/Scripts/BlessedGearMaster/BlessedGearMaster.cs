[ScriptEntry(Entry = 35015)]
public static void BlessedGearMaster(Player player, byte option)
{
	switch (option)
	{
		case 0:
			{
				Dialog.SendDialog(player, BlessedGearMasterMessage(player, "WELCOME_MSG"));
				Dialog.SendOption(player,  BlessedGearMasterMessage(player, "BLESS"), 1);
				Dialog.SendOption(player,  Message(player, "NO"), 255);
				Dialog.Finish(player);
				break;
			}
		case 1:
			{
				Dialog.OpenWindow(player, 426);
				break;
			}
	}
}