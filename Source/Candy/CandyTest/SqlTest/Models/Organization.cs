// Project by Bauss
using System;
using Candy;

namespace CandyTest.SqlTest.Models
{
	/// <summary>
	/// Organization model.
	/// </summary>
	[DataEntry(Name = "Organizations", EntryPoint = Helpers.SqlTests.ConnectionString)]
	public class Organization : SqlModel<Organization>
	{
		/// <summary>
		/// The id.
		/// </summary>
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		/// <summary>
		/// The name.
		/// </summary>
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string Name { get; set; }
		
		/// <summary>
		/// Creates a string based on the model.
		/// </summary>
		/// <returns>The string based on the model.</returns>
		public override string ToString()
		{
			return string.Format("[Organization Id={0}, Name={1}]", Id, Name);
		}
	}
}
