// Project by Bauss
using System;
using System.Runtime.Serialization;

namespace CandyConquer.Compiler.Internal
{
	/// <summary>
	/// An exception thrown by the compiler.
	/// </summary>
	public class CompilerException : Exception, ISerializable
	{
		public CompilerException()
		{
		}

	 	public CompilerException(string message) : base(message)
		{
		}

		public CompilerException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// This constructor is needed for serialization.
		protected CompilerException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}