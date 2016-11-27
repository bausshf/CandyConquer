/// <summary>
/// English messages for the love stone npc.
/// </summary>
private static string LoveStoneMessageEnglish(string message)
{
	switch (message)
	{
		case "WELCOME_MSG":
			return "I'm the magical stone of love. I can bring eternal love between two people.\nHowever are you found not to be soul mates, then my power is strong\nenough to split you apart again.";
		case "MARRY":
			return "I want to get married.";
		case "DIVORCE":
			return "I Want to get divorced.";
		
		default:
			return string.Empty;
	}
}