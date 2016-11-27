// Project by Bauss
using System;
using Candy;
using CandyTest.SqlTest.Models;

namespace CandyTest.StoreTest.Stores
{
	/// <summary>
	/// Description of EmployeeStore.
	/// </summary>
	public class EmployeeStore : DataStore<Employee>
	{
		public EmployeeStore()
			: base()
		{
			Proxy = new Proxies.EmployeeProxy();
			Proxy.Parameters.AddOrUpdate("Url", "API/GetAllEmployees");
			Proxy.Parameters.AddOrUpdate("OrgId", 0);
		}
	}
}
