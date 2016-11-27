// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for weathers.
	/// </summary>
	public static class Weathers
	{
		/// <summary>
		/// Gets a list of all weathers.
		/// </summary>
		/// <returns>The list of weathers.</returns>
		public static List<Models.DbWeather> GetAll()
		{
			return SqlDalHelper.GetAll<Models.DbWeather>(ConnectionStrings.World,
			                           "SELECT * FROM Weathers",
			                           null);
		}
	}
}
