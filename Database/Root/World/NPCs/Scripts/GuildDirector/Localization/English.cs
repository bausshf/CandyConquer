/// <summary>
/// English messages for the guild director npc.
/// </summary>
private static string GuildDirectorMessageEnglish(string message)
{
	switch (message)
	{
		case "WELCOME_MSG":
			return "Hello, I'm the Guild Director. An expert in the knowledge of guilds.\nHow may I help you?";
		case "ABOUT_GUILDS":
			return "Tell me about guilds.";
		case "ABOUT":
			return "Guilds are unions of players, that ties a strong bonds between them all,\nfighting side by side helping each other; They're the type of family that isn't\nbound by your own blood, but by your enemies.";
		case "MANAGE_GUILD":
			return "Manage guild.";
		case "MANAGE_NEW_GUILD":
			return "Create guild.";
		case "CREATE_GUILD_MSG":
			return "Do you wish to create a guild? Just let me know what you want to call it.";
		case "LEVEL_TOO_LOW":
			return "Your level is not high enough.";
		case "MONEY_TOO_LOW":
			return "You don't have enough silvers to create a guild.";
		case "SUCCESS":
			return "Congratulations on your new guild!";
		case "FAIL":
			return "Failed to create guild. The name was most likely taken.";
		case "MANAGE_GUILD_LEADER_MSG":
		case "MANAGE_DEPUTY_LEADER_MSG":
			return "What do you want to do?";
		case "DISBAN":
			return "Disban.";
		case "DISBAN_MSG":
			return "Your guild has been disbanned. Sad to see it go, but oh well.";
		case "KICK":
			return "Kick.";
		case "KICK_MSG":
			return "Tell me the name of the member you want to kick.";
		case "ADD_ALLIE":
			return "Add Ally.";
		case "REMOVE_ALLIE":
			return "Remove Allie.";
		case "ADD_ENEMY":
			return "Add Enemy.";
		case "REMOVE_ENEMY":
			return "Remove Enemy.";
		case "TOO_MANY_ALLIES":
			return "You already have 5 allies or the guild you're attempting to create an alliance with has 5 allies.";
		case "TOO_MANY_ENEMIES":
			return "You already have 5 enemies.";
		case "ADD_ALLIE_MSG":
			return "Tell me the name of the guild you want to have an alliance with.";
		case "REMOVE_ALLIE_MSG":
			return "Tell me the name of the guild you want to remove your alliance with.";
		case "ADD_ENEMY_MSG":
			return "Tell me the name of the guild you want to be your enemy.";
		case "REMOVE_ENEMY_MSG":
			return "Tell me the name of the guild you no longer are enemies with.";
		case "GUILD_NOT_FOUND":
			return "The guild was not found.";
		case "PENDING_ALLIE_MSG":
			return "You have a pending alliance request from {0}.\nWould you like to create an alliance with them?";
		case "OWN_GUILD_ALLIE":
			return "You cannot create an alliance with yourself.";
		case "OWN_GUILD_ENEMY":
			return "You cannot become an enemy with yourself.";
		case "ALREADY_ALLIE":
			return "This guild is already your allie.";
		case "ALREADY_ENEMY":
			return "This guild is already your enemy.";
		case "NOT_ALLIE":
			return "This guild is not your allie.";
		case "NOT_ENEMY":
			return "This guild is not your enemy.";
		case "DEPUTY":
			return "Add Deputy.";
		case "REMOVE_DEPUTY":
			return "Remove Deputy";
		case "DEPUTY_MSG":
			return "Tell me the name of the member you want to promote to a deputy leader.";
		case "PROMOTE_SELF":
			return "You cannot promote yourself.";
		case "MEMBER_NOT_FOUND":
			return "The member was not found.";
		case "NOT_DEPUTY":
			return "The member is not a deputy leader.";
		case "REMOVE_DEPUTY_MSG":
			return "Tell me the name of the deputy leader you want to unpromote.";
			
		default:
			return string.Empty;
	}
}