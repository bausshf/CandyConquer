// Project by Bauss
using System;

namespace Candy
{
	/// <summary>
	/// A data reader.
	/// </summary>
	public abstract class DataReader
	{
		/// <summary>
		/// Creates a new data reader.
		/// </summary>
		public DataReader()
		{
		}
		
		/// <summary>
		/// A method that checks whether there's data to read or not.
		/// </summary>
		/// <returns>True if there's data to read.</returns>
		public abstract bool Read();
		
		/// <summary>
		/// Reads data by a specific type and member.
		/// </summary>
		/// <param name="memberName">The member to read by.</param>
		/// <returns>The value read as an object.</returns>
		public abstract object Get<T>(string memberName);
		
		/// <summary>
		/// Reads data by a specific type, member and a custom reader function.
		/// </summary>
		/// <param name="memberName">The member to read by.</param>
		/// <param name="customReader">The custom reader.</param>
		/// <returns>The value read as an object.</returns>
		public abstract object GetCustom<T>(string memberName, Func<object,T> customReader);
	}
}
