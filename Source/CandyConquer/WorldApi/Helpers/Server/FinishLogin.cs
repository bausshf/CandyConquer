// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Helpers.Server
{
	/// <summary>
	/// Description of FinishLogin.
	/// </summary>
	public static class FinishLogin
	{
		public static void Handle(Models.Entities.Player player)
		{
			player.AddActionLog("Login");
			
			if (player.HP == 0)
			{
				player.Revive(false);
			}
			
			player.SendFormattedSystemMessage("MOTD", true, player.Name);
			Console.WriteLine(Drivers.Messages.LOGIN_SUCCESS, player.Name);
		}
	}
}
