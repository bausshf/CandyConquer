// Project by Bauss
using System;

namespace Candy
{
	/// <summary>
	/// Data ignoring types.
	/// These types specifies when to ignore a specific property.
	/// </summary>
	public enum DataIgnoreType
	{
		/// <summary>
		/// Always ignore the property. (Default)
		/// </summary>
		Both,
		/// <summary>
		/// Ignore the property during reads.
		/// </summary>
		Read,
		/// <summary>
		/// Ignore the property during writes.
		/// </summary>
		Write
	}
	
	/// <summary>
	/// A data ignoring attribute.
	/// This attribute should be declared for properties that needs to be ignored during specific situations.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class DataIgnoreAttribute : Attribute
	{
		/// <summary>
		/// The ignore type.
		/// </summary>
		public DataIgnoreType IgnoreType { get; set; }
	}
}
