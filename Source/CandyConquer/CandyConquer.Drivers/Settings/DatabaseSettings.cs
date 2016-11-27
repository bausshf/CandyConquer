// Project by Bauss
using System;

namespace CandyConquer.Drivers.Settings
{
	/// <summary>
	/// Database settings.
	/// </summary>
	public static class DatabaseSettings
	{
		/// <summary>
		/// Gets the auth flatfile database.
		/// </summary>
		public static string AuthFlatDatabase { get; private set; }
		/// <summary>
		/// Gets the world flatfile database.
		/// </summary>
		public static string WorldFlatDatabase { get; private set; }
		/// <summary>
		/// Gets the shared flatfile database.
		/// </summary>
		public static string SharedFlatDatabase { get; private set; }
		
		/// <summary>
		/// Loads the database settings.
		/// </summary>
		public static void Load()
		{
			Console.WriteLine(Messages.LOADING_DATABASE_SETTINGS);
			
			SharedFlatDatabase = @"C:\CandyConquer\Database\Root\Shared";
			
			var dbSettings = new Repositories.IO.IniFile(SharedFlatDatabase + "\\DbConfig.ini");
			if (dbSettings.Exists())
			{
				dbSettings.Open();
				
				AuthFlatDatabase = @"C:\CandyConquer\Database\Root\" + dbSettings.GlobalSection.GetValue("AuthFlatDatabase");
				WorldFlatDatabase = @"C:\CandyConquer\Database\Root\" + dbSettings.GlobalSection.GetValue("WorldFlatDatabase");
			}
		}
	}
}
