// Project by Bauss
using System;
using CandyTest.SqlTest.Helpers;

namespace CandyTest.SqlTest.Controllers
{
	/// <summary>
	/// Test controller for sql.
	/// </summary>
	public static class SqlTestController
	{
		/// <summary>
		/// Runs an initial test.
		/// </summary>
		public static void RunInitialTest()
		{
			SqlTests.RunInitialTest();
		}
		
		/// <summary>
		/// Runs an insert test.
		/// </summary>
		public static void RunInsertTest()
		{
			SqlTests.RunInsertTest();
		}
		
		/// <summary>
		/// Runs a desert test.
		/// </summary>
		public static void RunUpdateTest()
		{
			SqlTests.RunUpdateTest();
		}
		
		/// <summary>
		/// Runs an insert many test.
		/// </summary>
		public static void RunInsertManyTest()
		{
			SqlTests.RunInsertManyTest();
		}
		
		/// <summary>
		/// Runs a select many test.
		/// </summary>
		public static void RunSelectManyTest()
		{
			SqlTests.RunSelectManyTest();
		}
	}
}
