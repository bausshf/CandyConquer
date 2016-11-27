// Project by Bauss
using System;
using Candy;
using CandySql_LoginSample.Models;

namespace CandySql_LoginSample.Dal
{
	/// <summary>
	/// Description of Users.
	/// </summary>
	public static class Users
	{
		public static User GetUserByUserNameAndPassword(string userName, string password)
		{
			var pars = Sql.GetParsDict();
			pars.Add("userName", userName);
			pars.Add("password", password);
			
			return SqlDalHelper.Get<User>(Helpers.SqlHelper.ConnectionString,
			                              string.Format("SELECT * FROM Users WHERE {0}", Sql.GetSel(pars)),
			                              pars);
		}
	}
}
