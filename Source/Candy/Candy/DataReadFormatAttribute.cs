// Project by Bauss
using System;
using System.Collections.Generic;

namespace Candy
{
	/// <summary>
	/// A data reading format attribute.
	/// This attribute is used to create a custom reading format for a specific member.
	/// This can be used to avoid the default reading method, in case you want to handle the read value in a specific way.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class DataReadFormatAttribute : Attribute
	{
		/// <summary>
		/// The reading format.
		/// </summary>
		private string _readFormat;
		
		/// <summary>
		/// Gets or sets the reading format.
		/// </summary>
		public string ReadFormat
		{
			get { return _readFormat; }
			set
			{
				if (!value.Contains("@value"))
				{
					throw new FormatException("Could not find a place in the context to format the read value.");
				}
				
				_readFormat = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the associated namespaces, in case the format utilizes in special namespaces other than Candy and System.
		/// </summary>
		public string[] AssociatedNamespaces { get; set; }
	}
}
