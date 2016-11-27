// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;
using CandyTest.SqlTest.Models;

namespace CandyTest.SqlTest.Dal
{
	/// <summary>
	/// Dal for something.
	/// </summary>
	internal static class SomeTable
	{
		/// <summary>
		/// Gets a list of all something.
		/// </summary>
		/// <param name="count">The amount to get.</param>
		/// <returns>A list of all something.</returns>
		internal static List<Something> GetAll(int count)
		{
			return SqlDalHelper.GetAll<Something>(Helpers.SqlTests.ConnectionString,
			                           string.Format("SELECT TOP {0} * FROM SomeTable ", count),
			                           null);
		}
	}
}
