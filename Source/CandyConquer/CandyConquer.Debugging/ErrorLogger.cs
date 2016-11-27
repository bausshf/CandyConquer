// Project by Bauss
using System;

namespace CandyConquer.Debugging
{
	/// <summary>
	/// Error logging utilities.
	/// </summary>
	public static class ErrorLogger
	{
		/// <summary>
		/// Logs an error.
		/// </summary>
		/// <param name="title">The error title.</param>
		/// <param name="message">The error message.</param>
		public static void Log(string title, string message)
		{
			Console.WriteLine("[Error - {0}]", title);
			Console.WriteLine("[{0}]", DateTime.Now);
			Console.WriteLine(message);
		}
		
		/// <summary>
		/// Logs an error.
		/// </summary>
		/// <param name="title">The error title.</param>
		/// <param name="e">The error exception.</param>
		public static void Log(string title, Exception e)
		{
			string message = e.ToString();
			if (e.InnerException != null)
			{
				message += "\r\n" + e.InnerException;
			}
			Log(title, message);
		}
		
		/// <summary>
		/// Logs an error.
		/// </summary>
		/// <param name="title">The error title.</param>
		/// <param name="message">The error message.</param>
		/// <param name="args">The errror format arguments.</param>
		public static void Log(string title, string message, params object[] args)
		{
			Log(title, string.Format(message, args));
		}
		
		/// <summary>
		/// Logs an error.
		/// </summary>
		/// <param name="title">The error title.</param>
		/// <param name="message">The error message.</param>
		/// <param name="arg">The error format arguments.</param>
		public static void Log(string title, string message, object arg)
		{
			Log(title, string.Format(message, arg));
		}
	}
}
