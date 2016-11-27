// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for portals.
	/// </summary>
	public static class Portals
	{
		/// <summary>
		/// Gets a list of all portals.
		/// </summary>
		/// <returns>A list of all portals.</returns>
		public static List<Models.DbPortal> GetAll()
		{
			return SqlDalHelper.GetAll<Models.DbPortal>(ConnectionStrings.World,
			                                            "SELECT * FROM Portals",
			                                            null);
		}
	}
}
