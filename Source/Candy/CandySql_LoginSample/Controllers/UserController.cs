// Project by Bauss
using System;
using CandySql_LoginSample.Models;

namespace CandySql_LoginSample.Controllers
{
	/// <summary>
	/// Description of UserController.
	/// </summary>
	public static class UserController
	{
		public static Response CreateUser(string userName, string password)
		{
			if (string.IsNullOrWhiteSpace(userName))
			{
				return new Response
				{
					Success = false,
					Message = "Please enter a username."
				};
			}
			
			if (string.IsNullOrWhiteSpace(password))
			{
				return new Response
				{
					Success = false,
					Message = "Please enter a password."
				};
			}
			
			var user = new User();
			user.UserName = userName;
			user.Password = password;
			user.RegisterDate = DateTime.UtcNow;
			
			var success = user.Create();
			
			return new Response
			{
				Success = success,
				Message = success ? string.Empty : "Failed to create the user."
			};
		}
		
		public static Response Login(string userName, string password)
		{
			var user = Dal.Users.GetUserByUserNameAndPassword(userName, password);
			Program.LoggedUser = user;
			
			return new Response
			{
				Success = user != null,
				Message = user != null ? string.Empty : "Invalid username or password."
			};
		}
	}
}
