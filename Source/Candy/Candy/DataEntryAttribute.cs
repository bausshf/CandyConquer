// Project by Bauss
using System;

namespace Candy
{
	/// <summary>
	/// A data entry attribute.
	/// This attribute is used to identify an entry.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class DataEntryAttribute : Attribute
	{
		/// <summary>
		/// The entry name.
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// The entry point.
		/// </summary>
		public string EntryPoint { get; set; }
	}
}
