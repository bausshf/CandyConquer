/// <summary>
/// English messages for the tourny master npc.
/// </summary>
private static string TournyMasterMessageEnglish(string message)
{
	switch (message)
	{
		case "WELCOME_MSG":
			return "I'm the tournament master. I can tell you about tournaments or sign you up\nfor running ones.";
		case "SIGN_UP":
			return "Sign up.";
		case "GUIDE":
			return "Guide.";
		case "NO_TOURNAMENTS_SIGN_UP":
			return "There are no tournaments open for sign up at the moment.";
		case "SIGN_UP_MSG":
			return "Which tournament would you like to sign up for?";
		case "NO_TOURNAMENTS":
			return "There are no tournaments in this server.";
		case "GUIDE_MSG":
			return "Which tournament would you like to hear about?";
		case "ALREADY_BATTLE":
			return "Please exit your current battle environment to hear about tournaments.";
			
		default:
			return string.Empty;
	}
}
