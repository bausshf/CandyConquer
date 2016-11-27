// Project by Bauss
using System;

namespace CandyTest
{
	class Program
	{
		public static void Main(string[] args)
		{
			#region Candy.Sql tests - View SqlTest for implementations
			// An initial tests that tests the aspects of Candy.Sql
			// SqlTest.Controllers.SqlTestController.RunInitialTest();
			
			// A test that tests sql inserts with Candy.Sql
			// SqlTest.Controllers.SqlTestController.RunInsertTest();
			
			// A test that tests sql updates with Candy.Sql
			// SqlTest.Controllers.SqlTestController.RunUpdateTest();
			
			// A test that tests sql inserts with Candy.Sql where there's a lot of entries to insert.
			// SqlTest.Controllers.SqlTestController.RunInsertManyTest();
			
			// A test that tests sql selects with Candy.Sql where it selects a lot of entries.
			// SqlTest.Controllers.SqlTestController.RunSelectManyTest();
			#endregion
			
			#region Candy tests
			
			#region Store tests - View StoreTest for implementations
			// An initial test ...
			// StoreTest.StoreTests.InitialTest();
			
			// A test that tests store association removal by models.
			StoreTest.StoreTests.TestAssociationSync();
			#endregion
			
			#endregion
		}
	}
}