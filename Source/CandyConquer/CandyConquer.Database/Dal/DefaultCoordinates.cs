// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;
using CandyConquer.Database.Models;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for default coordinates.
	/// </summary>
	internal static class DefaultCoordinates
	{
		/// <summary>
		/// Gets all default coordinates.
		/// </summary>
		/// <returns>A list of all the default coordinates.</returns>
		internal static List<DbDefaultCoordinate> GetAll()
		{
			return SqlDalHelper.GetAll<DbDefaultCoordinate>(ConnectionStrings.World,
			                                   "SELECT * FROM DefaultCoordinates",
			                                   null);
		}
	}
}
