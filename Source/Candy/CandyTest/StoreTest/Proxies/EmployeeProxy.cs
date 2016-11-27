// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;
using CandyTest.SqlTest.Models;

namespace CandyTest.StoreTest.Proxies
{
	/// <summary>
	/// Description of EmployeeProxy.
	/// </summary>
	public class EmployeeProxy : DataProxy<Employee>
	{
		public override List<Employee> RequestData(out bool success)
		{
			switch (Parameters["Url"].ToString())
			{
				case "API/GetAllEmployees":
					{
						int orgId = (int)Parameters["OrgId"];
						success = true;
						return SqlTest.Dal.Employees.GetAllEmployees(orgId);
					}
					
					default:
					{
						success = false;
						return new List<Employee>();
					}
			}
		}
	}
}
