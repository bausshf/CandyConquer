// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;
using CandyTest.SqlTest.Models;

namespace CandyTest.SqlTest.Dal
{
	/// <summary>
	/// Dal for organizations.
	/// </summary>
	internal static class Organizations
	{
		/// <summary>
		/// Gets an organization by id.
		/// </summary>
		/// <param name="orgId">The org id.</param>
		/// <returns>The organization.</returns>
		internal static Organization GetOrganizationById(int orgId)
		{
			var pars = Sql.GetParsDict();
			pars.Add("Id", orgId);
			
			return SqlDalHelper.Get<Organization>(Helpers.SqlTests.ConnectionString,
			                           string.Format("SELECT * FROM Organizations WHERE {0}", Sql.GetSel(pars)),
			                           pars);
		}
		
		/// <summary>
		/// Gets a list of all organization.
		/// </summary>
		/// <returns>The list of organizations.</returns>
		internal static List<Organization> GetAllOrganizations()
		{
			return SqlDalHelper.GetAll<Organization>(Helpers.SqlTests.ConnectionString,
			                           "SELECT * FROM Organizations",
			                           null);
		}
	}
}
