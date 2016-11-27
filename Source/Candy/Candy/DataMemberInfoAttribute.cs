// Project by Bauss
using System;

namespace Candy
{
	/// <summary>
	/// A data member infor attribute.
	/// This attribute can be use an alternative name for the property during reads / writes.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class DataMemberInfoAttribute : Attribute
	{
		/// <summary>
		/// The alternative name.
		/// </summary>
		public string Name { get; set; }
	}
}
