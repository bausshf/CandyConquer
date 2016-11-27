// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;
using CandyConquer.Database.Models;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for items.
	/// </summary>
	public static class Items
	{
		/// <summary>
		/// Gets a list of all items.
		/// </summary>
		/// <returns>A list of all items.</returns>
		public static List<DbItem> GetAll()
		{
			return SqlDalHelper.GetAll<DbItem>(ConnectionStrings.World,
			                                   "SELECT * FROM Items",
			                                   null);
		}
	}
}
