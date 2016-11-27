/// <summary>
/// English messages for the house agent npcs.
/// </summary>
private static string HouseAgentMessageEnglish(string message)
{
	switch (message)
	{
		case "WELCOME_NO_HOUSE_MSG":
			return "I'm the house agent of {0}. Would you like to rent or buy a house?";
		case "WELCOME_NO_GUILD_HOUSE_MSG":
			return "I'm the guild house agent. Would you like to buy a house for your guild?\nThe price is a total of 250,000,000 silvers.";
		case "WELCOME_HOUSE_MSG":
			return "What can I help you with?";
		case "RENT":
			return "Rent.";
		case "BUY":
			return "Buy.";
		case "RENT_MSG":
			return "The price per month for renting an apartment is 1,000,000 silvers.\nThe price per month for renting a house is 10,000,000 silvers.\nDo you still wish to rent an apartment or house?";
		case "BUY_MSG":
			return "The price to buy a house is 100,000,000 silvers. Do you still wish to buy a house?";
		case "RENT_SMALL":
			return "Rent apartment.";
		case "RENT_BIG":
			return "Rent house.";
		case "NOT_ENOUGH_MONEY_RENT":
			return "You don't have enough money to rent this home.";
		case "NOT_ENOUGH_MONEY_BUY":
			return "You don't have enough money to buy this house.";
		case "ENTER":
			return "Enter home.";
		case "ENTER_GUILD_HOUSE":
			return "Enter guild house.";
		case "END_LEASE":
			return "End my lease-contract.";
		case "CONTRACT_ENDED":
			return "Your contract has been revoked. You're no longer renting this home.";
		case "NO_GUILD":
			return "You're not a member of any guilds.";
		case "NO_GUILD_HOUSE":
			return "Your guild does not have a house.";
		case "NOT_ENOUGH_MONEY_GUILD_HOUSE":
			return "You do not have enough money to buy a guild house.";
		case "NOT_ENOUGH_MONEY_PAY_RENT":
			return "You don't have enough money to pay rent.";
		case "RENT_IS_NOT_DUE":
			return "Your rent is not due until {0}.";
		case "RENT_PAID":
			return "Your rent has already been paid!";
		case "PAY_RENT":
			return "Pay rent.";
		case "BUY_WAREHOUSE":
			return "Buy warehouse.";
		case "NOT_ENOUGH_MONEY_PLAYER_WAREHOUSE":
			return "You do not have enough money to buy a warehouse.";
		case "GOT_WAREHOUSE":
			return "Congratulations with your new warehouse.";
			
		default:
			return string.Empty;
	}
}