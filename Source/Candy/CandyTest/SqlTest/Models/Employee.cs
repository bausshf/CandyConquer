// Project by Bauss
using System;
using System.Configuration;
using Candy;

namespace CandyTest.SqlTest.Models
{
	/// <summary>
	/// Employee model.
	/// </summary>
	[DataEntry(Name = "Employees", EntryPoint = Helpers.SqlTests.ConnectionString)]
	public class Employee : SqlModel<Employee>
	{
		/// <summary>
		/// The id.
		/// </summary>
		[DataSpecialType(DataType = SpecialDataType.Id)]
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int Id { get; set; }
		
		public int OrgId { get; set; }
		
		/// <summary>
		/// The first name.
		/// </summary>
		public string FirstName { get; set; }
		
		/// <summary>
		/// The middle name.
		/// </summary>
		public string MiddleName { get; set; }
		
		/// <summary>
		/// The last name.
		/// </summary>
		public string LastName { get; set; }
		
		/// <summary>
		/// The social security number.
		/// </summary>
		[DataMemberInfo(Name = "SSN")]
		public string SocialSecurityNumber { get; set; }
		
		/// <summary>
		/// The salary.
		/// </summary>
		public decimal? Salary { get; set; }
		
		/// <summary>
		/// The timestamp.
		/// </summary>
		[DataSpecialType(DataType = SpecialDataType.Timestamp)]
		public DateTime? Timestamp { get; set; }
		
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
			return string.Format("[Employee Id={0}, OrgId={1}, FirstName={2}, MiddleName={3}, LastName={4}, SocialSecurityNumber={5}, Salary={6}, Timestamp={7}, IgnoreProperty={8}]", Id, OrgId, FirstName, MiddleName, LastName, SocialSecurityNumber, Salary, Timestamp, IgnoreProperty);
		}
	}
}
