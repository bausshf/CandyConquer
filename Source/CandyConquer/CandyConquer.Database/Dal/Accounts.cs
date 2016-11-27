// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;
using CandyConquer.Database.Models;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for accounts.
	/// </summary>
	public static class Accounts
	{
		/// <summary>
		/// Gets an account.
		/// </summary>
		/// <param name="pars">The parameters.</param>
		/// <returns>The account if found, null otherwise.</returns>
		private static DbAccount Get(Dictionary<string,object> pars)
		{
			return SqlDalHelper.Get<DbAccount>(ConnectionStrings.Auth,
			                                   string.Format("SELECT * FROM Accounts WHERE {0}", Sql.GetSel(pars)),
			                                   pars);
		}
		
		/// <summary>
		/// Gets a specific account based on an account name and password.
		/// </summary>
		/// <param name="name">The account name.</param>
		/// <param name="password">The password.</param>
		/// <returns>The user if found, null otherwise.</returns>
		public static DbAccount GetAccountByUserNameAndPassword(string name, string password)
		{
			var pars = Sql.GetParsDict();
			pars.Add("Name", name);
			pars.Add("Password", password);
			
			return Get(pars);
		}
		
		/// <summary>
		/// Gets an account by its id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>The account.</returns>
		public static DbAccount GetAccountById(int id)
		{
			var pars = Sql.GetParsDict();
			pars.Add("Id", id);
			
			return Get(pars);
		}
		
		/// <summary>
		/// Bans an account.
		/// </summary>
		/// <param name="account">The account.</param>
		/// <param name="reason">The reason for the ban.</param>
		/// <param name="range">The range of the ban.</param>
		public static bool Ban(DbAccount account, string reason, Models.DbAccount.BanRangeType range)
		{
			account.Banned = true;
			account.BanDescription = reason;
			account.BanDate = DateTime.Now;
			account.BanRange = range;
			
			return account.Update();
		}
	}
}
