// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;
using CandyTest.SqlTest.Models;

namespace CandyTest.SqlTest.Dal
{
	/// <summary>
	/// Dal for users.
	/// </summary>
	internal static class Users
	{
		/// <summary>
		/// Gets a specific user based on an organization, username and password.
		/// </summary>
		/// <param name="orgId">The org id.</param>
		/// <param name="userName">The username.</param>
		/// <param name="password">The password.</param>
		/// <returns>The user if found, null otherwise.</returns>
		internal static User GetByUserNameAndPassword(int orgId, string userName, string password)
		{
			var pars = Sql.GetParsDict();
			pars.Add("OrgId", orgId);
			pars.Add("UserName", userName);
			pars.Add("Password", password);
			
			return SqlDalHelper.Get<User>(Helpers.SqlTests.ConnectionString,
			                           string.Format("SELECT * FROM Users WHERE {0}", Sql.GetSel(pars)),
			                           pars);
		}
		
		/// <summary>
		/// Gets all users tied to a specific organization-
		/// </summary>
		/// <param name="orgId">The org id.</param>
		/// <returns>A list of all users tied to the organization.</returns>
		internal static List<User> GetAllUsers(int orgId)
		{
			var pars = Sql.GetParsDict();
			pars.Add("orgId", orgId);
			
			return SqlDalHelper.GetAll<User>(Helpers.SqlTests.ConnectionString,
			                           string.Format("SELECT * FROM Users WHERE {0}", Sql.GetSel(pars)),
			                           pars);
		}
	}
}
