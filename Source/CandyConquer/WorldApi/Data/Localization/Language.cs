// Project by Bauss
using System;
using System.IO;
using System.Collections.Concurrent;

namespace CandyConquer.WorldApi.Data.Localization
{
	/// <summary>
	/// Language localization.
	/// </summary>
	public static class Language
	{
		/// <summary>
		/// Localization information.
		/// </summary>
		private class LocalizationInformation
		{
			/// <summary>
			/// Collection of messages.
			/// </summary>
			public ConcurrentDictionary<string,string> Messages { get; private set; }
			
			/// <summary>
			/// Creates a new localization information.
			/// </summary>
			public LocalizationInformation()
			{
				Messages = new ConcurrentDictionary<string, string>();
			}
		}
		
		/// <summary>
		/// Collection of language's with localization.
		/// </summary>
		private static ConcurrentDictionary<string, LocalizationInformation> _languages { get; set; }
		
		/// <summary>
		/// Initializes the localization.
		/// </summary>
		public static void Initialize()
		{
			_languages = new ConcurrentDictionary<string, Language.LocalizationInformation>();
			
			foreach (var languageFile in 
			         Directory.GetFiles(Drivers.Settings.DatabaseSettings.WorldFlatDatabase + "\\Localization", "*.ini"))
			{
				var languageName = Path.GetFileNameWithoutExtension(languageFile);
				var language = new LocalizationInformation();
				
				foreach (var message in File.ReadLines(languageFile))
				{
					if (!string.IsNullOrWhiteSpace(message) && !message.StartsWith(";"))
					{
						var messageInfo = message.Split('=');
						if (messageInfo.Length == 2)
						{
							language.Messages.TryAdd(messageInfo[0], messageInfo[1]);
						}
					}
				}
				
				if (language.Messages.Count > 0)
				{
					_languages.TryAdd(languageName, language);
				}
			}
		}
		
		/// <summary>
		/// Gets a message based on a specific language.
		/// </summary>
		/// <param name="language">The language.</param>
		/// <param name="message">The message.</param>
		/// <returns>The message if found, empty string otherwise.</returns>
		public static string GetMessage(string language, string message)
		{
			language = string.IsNullOrWhiteSpace(language) || !_languages.ContainsKey(language) ?
				"English" : language;
			
			LocalizationInformation localization;
			if (_languages.TryGetValue(language, out localization))
			{
				string msg;
				if (localization.Messages.TryGetValue(message, out msg))
				{
					return msg;
				}
			}
			
			return language != "English" ? GetMessage("English", message) : string.Format("[N/A : {0} : {1}]", language, message);
		}
	}
}
