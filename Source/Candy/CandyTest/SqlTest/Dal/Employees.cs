// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;
using CandyTest.SqlTest.Models;

namespace CandyTest.SqlTest.Dal
{
	/// <summary>
	/// Dal for employyes.
	/// </summary>
	internal static class Employees
	{
		/// <summary>
		/// Gets an employee based on a social security number, tied to a specific organization.
		/// </summary>
		/// <param name="orgId">The org id of the employee.</param>
		/// <param name="socialSecurityNumber">The social security number of the employee.</param>
		/// <returns>The employee if existing, null otherwise.</returns>
		internal static Employee GetEmployeeBySocialSecurityNumber(int orgId, string socialSecurityNumber)
		{
			var pars = Sql.GetParsDict();
			pars.Add("OrgId", orgId);
			pars.Add("SSN", socialSecurityNumber);
			
			return SqlDalHelper.Get<Employee>(Helpers.SqlTests.ConnectionString,
			                           string.Format("SELECT * FROM Employees WHERE {0}", Sql.GetSel(pars)),
			                           pars);
		}
		
		/// <summary>
		/// Gets a list of all employees, tied to a specific organization.
		/// </summary>
		/// <param name="orgId">The org id.</param>
		/// <returns>A list of all employees tied to the specific organization.</returns>
		internal static List<Employee> GetAllEmployees(int orgId)
		{
			var pars = Sql.GetParsDict();
			pars.Add("orgId", orgId);
			
			return SqlDalHelper.GetAll<Employee>(Helpers.SqlTests.ConnectionString,
			                           string.Format("SELECT * FROM Employees WHERE {0}", Sql.GetSel(pars)),
			                           pars);
		}
	}
}
