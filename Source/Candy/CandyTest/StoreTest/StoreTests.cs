// Project by Bauss
using System;
using CandyTest.SqlTest.Models;

namespace CandyTest.StoreTest
{
	/// <summary>
	/// Description of StoreTests.
	/// </summary>
	public class StoreTests
	{
		public static void InitialTest()
		{
			Console.Write("Write an organization id: ");
			int orgId = int.Parse(Console.ReadLine());
			
			var employeeStore = new Stores.EmployeeStore();
			employeeStore.Proxy.Parameters.AddOrUpdate("OrgId", orgId);
			
			while (true)
			{
				Console.WriteLine("Loading ...");
				employeeStore.Load(true);
				
				foreach (Employee employee in employeeStore)
				{
					Console.WriteLine(employee);
				}
				
				Console.WriteLine("Finished loading ...");
				Console.ReadLine();
			}
		}
		
		public static void TestAssociationSync()
		{
			Console.Write("Write an organization id: ");
			int orgId = int.Parse(Console.ReadLine());
			
			var employeeStore = new Stores.EmployeeStore();
			employeeStore.Proxy.Parameters.AddOrUpdate("OrgId", orgId);
			
			while (true)
			{
				Console.WriteLine("Loading ...");
				employeeStore.Load(true);
				
				Employee firstEmployee = null;
				foreach (Employee employee in employeeStore)
				{
					if (firstEmployee == null)
					{
						firstEmployee = employee;
					}
					
					Console.WriteLine(employee);
				}
				
				// Could call firstEmployee.Delete() too
				// However when I ran this test case I didn't want to delete my model from the database
				firstEmployee.RemoveStoreAssociation();
				
				foreach (Employee employee in employeeStore)
				{
					Console.WriteLine(employee);
				}
				
				Console.WriteLine("Finished loading ...");
				Console.ReadLine();
			}
		}
	}
}
