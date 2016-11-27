// Project by Bauss
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Candy
{
	/// <summary>
	/// An exception thrown durin compilation.
	/// </summary>
	public class DataCompilerException : Exception, ISerializable
	{
		public IEnumerable<string> Errors { get; private set; }
		
		public DataCompilerException()
		{
		}

	 	public DataCompilerException(string message) : base(message)
		{
		}
	 	
	 	public DataCompilerException(string message, IEnumerable<string> errors)
	 		: this(message)
	 	{
	 		Errors = errors;
	 	}

		public DataCompilerException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// This constructor is needed for serialization.
		protected DataCompilerException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}