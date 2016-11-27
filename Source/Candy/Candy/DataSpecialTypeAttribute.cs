// Project by Bauss
using System;

namespace Candy
{
	/// <summary>
	/// The special data types.
	/// </summary>
	public enum SpecialDataType
	{
		/// <summary>
		/// A type that indicates its an identifier.
		/// Usually a primary key of some sort.
		/// The value of an id property is cached and may only be set once.
		/// A change of an id property can cause invalid reads and writes.
		/// If the value has to be changed make sure to call UpdateIdProperty().
		/// </summary>
		Id,
		/// <summary>
		/// A type that indicates its a timestamp and those always will update with the current UtcNow time.
		/// </summary>
		Timestamp,
		/// <summary>
		/// A type that should always be converted to a string when writing.
		/// </summary>
		AsString
	}
	
	/// <summary>
	/// A special data type attribute.
	/// This can be used to declare a property as a special kind of data.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class DataSpecialTypeAttribute : Attribute
	{
		/// <summary>
		/// The special data type.
		/// </summary>
		public SpecialDataType DataType { get; set; }
	}
}
