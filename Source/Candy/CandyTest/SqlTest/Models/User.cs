// Project by Bauss
using System;
using Candy;

namespace CandyTest.SqlTest.Models
{
	/// <summary>
	/// User model.
	/// </summary>
	[DataEntry(Name = "Users", EntryPoint = Helpers.SqlTests.ConnectionString)]
	public class User : SqlModel<User>
	{
		/// <summary>
		/// The id.
		/// </summary>
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		/// <summary>
		/// The org id.
		/// </summary>
		public int OrgId { get; set; }
		
		/// <summary>
		/// The username.
		/// </summary>
		public string UserName { get; set; }
		
		/// <summary>
		/// The password.
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// The email.
		/// </summary>
		public string Email { get; set; }
		
		/// <summary>
		/// The social security number.
		/// </summary>
		[DataMemberInfo(Name = "SSN")]
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string SocialSecurityNumber { get; set; }
		
		/// <summary>
		/// A property to ignore.
		/// (Only here for demonstration ...)
		/// </summary>
		[DataIgnore()]
		public int IgnoreProperty { get; set; }
		
		/// <summary>
		/// Creates a string based on the model.
		/// </summary>
		/// <returns>The string based on the model.</returns>
		public override string ToString()
		{
			return string.Format("[User Id={0}, OrgId={1}, UserName={2}, Email={3}, SocialSecurityNumber={4}, IgnoreProperty={5}]", Id, OrgId, UserName, Email, SocialSecurityNumber, IgnoreProperty);
		}
	}
}
