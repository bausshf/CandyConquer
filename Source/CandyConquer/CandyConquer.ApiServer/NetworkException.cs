// Project by Bauss
using System;
using System.Runtime.Serialization;

namespace CandyConquer.ApiServer
{
	/// <summary>
	/// An exception thrown by network errors.
	/// </summary>
	public class NetworkException : Exception, ISerializable
	{
		public NetworkException()
		{
		}

	 	public NetworkException(string message) : base(message)
		{
		}

		public NetworkException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// This constructor is needed for serialization.
		protected NetworkException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}