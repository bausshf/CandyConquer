// Project by Bauss
using System;
using CandyConquer.Debugging;

namespace CandyConquer.Drivers
{
	/// <summary>
	/// The global driver.
	/// </summary>
	public static class Global
	{
		/// <summary>
		/// Initializes all global data.
		/// </summary>
		public static void Initialize()
		{
			Console.WriteLine("Initializing the global driver.");
			Settings.DatabaseSettings.Load();
			Settings.GlobalSettings.Load();
			Settings.AuthSettings.Load();
			Settings.WorldSettings.Load();
		}
		
		/// <summary>
		/// Writes a global message to the associated server.
		/// </summary>
		/// <param name="message">The message.</param>
		public static void Message(string message)
		{
			Console.WriteLine(message);
		}
		
		/// <summary>
		/// Writes a global message to the associated server.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="arg">Format argument.</param>
		public static void Message(string message, object arg)
		{
			Message(string.Format(message, arg));
		}
		
		/// <summary>
		/// Writes a global message to the associated server.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="args">Format arguments.</param>
		public static void Message(string message, params object[] args)
		{
			Message(string.Format(message, args));
		}
		
		/// <summary>
		/// Raises an exception to the logger.
		/// </summary>
		/// <param name="e">The exception.</param>
		public static void RaiseException(Exception e)
		{
			#if DEBUG
			#if TRACE
			ErrorLogger.Log(StackTracing.GetCurrentMethod().Name, e);
			#else
			ErrorLogger.Log(Messages.Errors.FATAL_ERROR_TITLE, e);
			#endif
			#else
			Message(e.ToString());
			#endif
		}
	}
}
