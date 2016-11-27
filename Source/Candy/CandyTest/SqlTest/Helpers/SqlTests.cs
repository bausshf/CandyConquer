// Project by Bauss
using System;
using System.Collections.Generic;
using CandyTest.SqlTest.Models;
using Candy;

namespace CandyTest.SqlTest.Helpers
{
	/// <summary>
	/// Helper for sql tests.
	/// </summary>
	public static class SqlTests
	{
		/// <summary>
		/// The connection string.
		/// </summary>
		public const string ConnectionString = "Server=MOR-HP\\BAUSSSQL;Database=TestDatabase;Trusted_Connection=True";
		
		/// <summary>
		/// Runs the initial test.
		/// </summary>
		public static void RunInitialTest()
		{
			while (true)
			{
				Console.Write("Write an organization id: ");
				int orgId = int.Parse(Console.ReadLine());
				
				var organization = Dal.Organizations.GetOrganizationById(orgId);
				if (organization == null)
				{
					Console.WriteLine("Invalid organization.");
					return;
				}
				
				Console.WriteLine(organization);
				
				Console.Write("Write a username: ");
				string userName = Console.ReadLine();
				
				Console.Write("Write a password: ");
				string password = Console.ReadLine();
				
				var user = Dal.Users.GetByUserNameAndPassword(orgId, userName, password);
				if (user == null)
				{
					Console.WriteLine("Invalid username or password for the chosen organization.");
					return;
				}
				
				Console.WriteLine(user);
				
				var employee = Dal.Employees.GetEmployeeBySocialSecurityNumber(orgId, user.SocialSecurityNumber);
				if (employee == null)
				{
					Console.Write("Write a first name: ");
					string firstName = Console.ReadLine();
					
					Console.Write("Write a middle name (N/A for none): ");
					string middleName = Console.ReadLine();
					if (string.Equals(middleName, "N/A", StringComparison.InvariantCultureIgnoreCase))
					{
						middleName = null;
					}
					Console.Write("Write a last name: ");
					string lastName = Console.ReadLine();
					
					employee = new Employee
					{
						OrgId = orgId,
						FirstName = firstName,
						MiddleName = middleName,
						LastName = lastName,
						SocialSecurityNumber = user.SocialSecurityNumber
					};
					employee.Create();
				}
				else
				{
					employee.Update();
				}
				
				Console.WriteLine(employee);
			}
		}
		
		/// <summary>
		/// Runs an insert test.
		/// </summary>
		public static void RunInsertTest()
		{
			Console.Write("Write an organization id: ");
			int orgId = int.Parse(Console.ReadLine());
			
			var organization = Dal.Organizations.GetOrganizationById(orgId);
			if (organization == null)
			{
				Console.WriteLine("Invalid organization.");
				return;
			}
			
			Console.WriteLine(organization);
			
			Console.Write("Write a username: ");
			string userName = Console.ReadLine();
			
			Console.Write("Write a password: ");
			string password = Console.ReadLine();
			
			var user = new User();
			user.OrgId = organization.Id;
			user.UserName = userName;
			user.Password = password;
			
			user.Create();
			
			Console.WriteLine("Success {0} ...", user.Id);
			Console.ReadLine();
		}
		
		/// <summary>
		/// Runs an update test.
		/// </summary>
		public static void RunUpdateTest()
		{
			Console.Write("Write an organization id: ");
			int orgId = int.Parse(Console.ReadLine());
			
			var organization = Dal.Organizations.GetOrganizationById(orgId);
			if (organization == null)
			{
				Console.WriteLine("Invalid organization.");
				return;
			}
			
			Console.WriteLine(organization);
			
			Console.Write("Write a username: ");
			string userName = Console.ReadLine();
			
			Console.Write("Write a password: ");
			string password = Console.ReadLine();
			
			var user = Dal.Users.GetByUserNameAndPassword(orgId, userName, password);
			if (user == null)
			{
				Console.WriteLine("Invalid username or password for the chosen organization.");
				return;
			}
			
			Console.WriteLine(user);
			
			var employee = Dal.Employees.GetEmployeeBySocialSecurityNumber(orgId, user.SocialSecurityNumber);
			if (employee == null)
			{
				Console.WriteLine("The user is not an active employee.");
				return;
			}
			
			Console.WriteLine(employee);
			
			while (true)
			{
				Console.Write("Write a salary: ");
				decimal salary = decimal.Parse(Console.ReadLine());
				employee.Salary = salary;
				if (employee.Update())
				{
					Console.WriteLine(employee);
				}
			}
		}
		
		public static void RunInsertManyTest()
		{
			Console.WriteLine("Initial insert ...");
			var collection = new List<Something>();
			for (int i = 0; i < 1000; i++)
			{
				int j = i;
				collection.Add(new Something
				               {
				               	SomeNumber = j
				               });
			}
			
			Console.WriteLine("Created {0} entries ...", collection.Create());
			Console.ReadLine();
		}
		
		public static void RunSelectManyTest()
		{
			Console.WriteLine("Begin select ...");
			
			var collection = Dal.SomeTable.GetAll(1000);
			
			Console.WriteLine("Selected {0} ...", collection.Count);
			Console.ReadLine();
		}
	}
}
