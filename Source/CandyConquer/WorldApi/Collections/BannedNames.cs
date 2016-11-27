// Project by Bauss
using System;
using System.Linq;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A collection of banned names.
	/// </summary>
	public class BannedNames
	{
		/// <summary>
		/// Array of banned names.
		/// Keep all names lower-case.
		/// </summary>
		private static readonly string[] Names = new string[]
		{
			"fuck", "allah", "bauss",
			"gamemaster", "moderator", "admin"
		};
		
		/// <summary>
		/// Checks whether a name is within the banned names collection.
		/// </summary>
		/// <param name="name">The name to check.</param>
		/// <returns>True if the name is banned.</returns>
		public static bool Contains(string name)
		{
			return Names.Contains(name.ToLower());
		}
	}
}
