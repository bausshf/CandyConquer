// Project by Bauss
using System;
using Candy;

namespace CandyTest.SqlTest.Models
{
	/// <summary>
	/// Something.
	/// </summary>
	[DataEntry(Name = "SomeTable", EntryPoint = Helpers.SqlTests.ConnectionString)]
	public class Something : SqlModel<Something>
	{
		/// <summary>
		/// The id property.
		/// </summary>
		[DataSpecialType(DataType = SpecialDataType.Id)]
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int Id { get; set; }
		
		/// <summary>
		/// Some number.
		/// </summary>
		public int SomeNumber { get; set; }
	}
}
