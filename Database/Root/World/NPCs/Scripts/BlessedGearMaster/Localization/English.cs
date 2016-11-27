/// <summary>
/// English messages for the blessed gear npc.
/// </summary>
private static string BlessedGearMasterMessageEnglish(string message)
{
	switch (message)
	{
		case "WELCOME_MSG":
			return "I'm an expert in item blessing and enchantment.\nOnce I bless your gear you'll receive an amount of enchantment too!\nAre you interested in a magical blessing?";
		case "BLESS":
			return "Bless my gear.";
		
		default:
			return string.Empty;
	}
}