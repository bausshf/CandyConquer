// Project by Bauss
using System;
using System.Collections.Generic;
using CandyConquer.Drivers.Repositories.Api.Attributes;

namespace CandyConquer.Compiler.Api
{
	/// <summary>
	/// An api controller method information.
	/// </summary>
	public class ApiControllerMethodInfo
	{
		/// <summary>
		/// The call attribute for the method call.
		/// </summary>
		public ApiCallAttribute CallAttribute { get; set; }
		
		/// <summary>
		/// The full name of the method call.
		/// </summary>
		public string Call { get; set; }
	}
}
