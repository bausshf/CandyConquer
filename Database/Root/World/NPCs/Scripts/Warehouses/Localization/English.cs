/// <summary>
/// English messages for the warehouse npcs.
/// </summary>
private static string WarehouseMessageEnglish(string message)
{
	switch (message)
	{
		case "ENTER_PASSWORD":
			return "Enter your warehouse password please.";
		case "INVALID_PASSWORD":
			return "Invalid password.";
		case "ENTER_NEW_PASSWORD":
			return "Enter a warehouse password you'd like. It must be between 4 and 8 characters long.";
		case "UPDATE_PASSWORD":
			return "Let me know the new warehouse password you want.";
		case "INVALID_PASSWORD_LENGTH":
			return "Your password has an invalid length. Make sure it's between 4 and 8 characters long.";
		case "PASSWORD_SET":
			return "Congratulations your password has been set.";
			
		default:
			return string.Empty;
	}
}