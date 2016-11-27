/// <summary>
/// English messages for the gw guardian npc.
/// </summary>
private static string GWGuardianMessageEnglish(string message)
{
	switch (message)
	{
		case "WELCOME_MSG":
			return "I'm the protector of the guild wars area. Would you like to enter?";
		case "BACK_MSG":
			return "I can take you back to TwinCity.";
		default:
			return string.Empty;
	}
}