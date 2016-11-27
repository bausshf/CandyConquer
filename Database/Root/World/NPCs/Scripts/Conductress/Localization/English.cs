/// <summary>
/// English messages for the conductress npc.
/// </summary>
private static string ConductressMessageEnglish(string message)
{
	switch (message)
	{
		case "STANDARD_MSG":
			return "Hello Adventure, where are you heading?";
		
		case "MARKET_MSG":
			return "Do you wish to go back?";
		case "MARKET_LEAVE":
			return "Yes please.";
		case "MARKET_STAY":
			return "Let me stay a little longer.";
		
		case "TRAVEL_INVALID_MONEY":
			return "You need at least {0} silvers to travel.";
			
		default:
			return string.Empty;
	}
}