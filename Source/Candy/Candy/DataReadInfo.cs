// Project by Bauss
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Candy
{
	/// <summary>
	/// Data reading info for the data formatter.
	/// </summary>
	public class DataReadInfo
	{
		/// <summary>
		/// The type name.
		/// </summary>
		public string TypeName { get; set; }
		/// <summary>
		/// The member associated with the data.
		/// </summary>
		public PropertyInfo Member { get; set; }
		/// <summary>
		/// The read name.
		/// </summary>
		public string ReadName { get; set; }
		
		/// <summary>
		/// Creates a new data read info.
		/// </summary>
		public DataReadInfo()
		{
		}
	}
	
	/// <summary>
	/// A custom data read info.
	/// This should be used for properties with the read format attribute.
	/// </summary>
	public class DataReadCustomInfo : DataReadInfo
	{
		/// <summary>
		/// The read format.
		/// </summary>
		public string ReadFormat { get; set; }
		/// <summary>
		/// THe associated namespaces.
		/// </summary>
		public IEnumerable<string> AssociatedNamespaces { get; set; }
		
		/// <summary>
		/// Creates an new custom read data.
		/// </summary>
		public DataReadCustomInfo()
			: base()
		{
			
		}
	}
}
