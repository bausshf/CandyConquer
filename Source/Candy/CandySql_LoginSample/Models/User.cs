// Project by Bauss
using System;
using Candy;

namespace CandySql_LoginSample.Models
{
	/// <summary>
	/// Description of User.
	/// </summary>
	[DataEntry(Name = "Users", EntryPoint = Helpers.SqlHelper.ConnectionString)]
	public class User : SqlModel<User>
	{
		[DataSpecialType(DataType = SpecialDataType.Id)]
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int Id { get; set; }
		
		public string UserName { get; set; }
		public string Password { get; set; }
		
		public DateTime? RegisterDate { get; set; }
		public DateTime? LastLoginDate { get; set; }
		
		[DataSpecialType(DataType = SpecialDataType.Timestamp)]
		public DateTime? LastUpdateTime { get; set; }
	}
}
