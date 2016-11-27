// Project by Bauss
using System;
using System.Collections.Generic;

namespace CandyConquer.Compiler.Internal
{
	/// <summary>
	/// A helper class for compilation.
	/// </summary>
	internal sealed class CompilerHelper
	{
		/// <summary>
		/// The return type.
		/// </summary>
		internal string ReturnType { get; set; }
		/// <summary>
		/// The parameters.
		/// </summary>
		internal List<string> Parameters { get; set; }
		/// <summary>
		/// The namespaces.
		/// </summary>
		internal List<string> Namespaces { get; set; }
		/// <summary>
		/// The code (Method body.)
		/// </summary>
		internal string Code { get; set; }
		
		/// <summary>
		/// Creates a new compiler helper.
		/// </summary>
		public CompilerHelper()
		{
			Parameters = new List<string>();
			Namespaces = new List<string>();
		}
	}
}
