// Project by Bauss
using System;

namespace CandyConquer.Compiler.ScriptingEngine
{
	/// <summary>
	/// A script entry attribute.
	/// Must be declared by all entries that should be used outside of the engine itself.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class ScriptEntryAttribute : Attribute
	{
		/// <summary>
		/// The entry point.
		/// </summary>
		public object Entry { get; set; }
	}
}
