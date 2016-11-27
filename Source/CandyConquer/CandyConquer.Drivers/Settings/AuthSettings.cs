// Project by Bauss
using System;

namespace CandyConquer.Drivers.Settings
{
	/// <summary>
	/// The auth settings.
	/// </summary>
	public static class AuthSettings
	{
		/// <summary>
		/// Gets the IP address of the auth server.
		/// </summary>
		public static string IPAddress { get; private set; }
		
		/// <summary>
		/// Loads the auth settings.
		/// </summary>
		public static void Load()
		{
			Console.WriteLine(Messages.LOADING_AUTH_SETTINGS);
			var settings = new Repositories.IO.IniFile(DatabaseSettings.AuthFlatDatabase + "\\Config.ini");
			if (settings.Exists())
			{
				settings.Open();
				
				IPAddress = settings.GlobalSection.GetValue("IPAddress");
			}
		}
	}
}
