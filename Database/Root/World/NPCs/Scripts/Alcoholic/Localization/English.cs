/// <summary>
/// English messages for the alcoholic npc.
/// </summary>
private static string AlcoholicMessageEnglish(string message)
{
	switch (message)
	{
		case "WELCOME_MSG":
			return "I'm really thirsty. If I could have some special wine, that would be perfect. You want something in return? Of course, I will show you a whole new world!";
		case "HERE_IS_A_WINE":
			return "Here's some special wine.";
		case "NO_WINE":
			return "You do not have a wine of my taste.";
		
		default:
			return string.Empty;
	}
}