// Project by Bauss
using System;

namespace CandyConquer.Drivers.Settings
{
	/// <summary>
	/// The global settings.
	/// </summary>
	public static class GlobalSettings
	{
		/// <summary>
		/// Gets the server name.
		/// </summary>
		public static string ServerName { get; private set; }
		
		/// <summary>
		/// Loads the global settings.
		/// </summary>
		public static void Load()
		{
			Console.WriteLine(Messages.LOADING_GLOBAL_SETTINGS);
			var settings = new Repositories.IO.IniFile(DatabaseSettings.SharedFlatDatabase + "\\Config.ini");
			if (settings.Exists())
			{
				settings.Open();
				
				ServerName = settings.GlobalSection.GetValue("ServerName");
			}
		}
	}
}
