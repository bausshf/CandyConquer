// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A collection of namespaces for scripting.
	/// </summary>
	public static class ScriptNamespaces
	{
		/// <summary>
		/// Adds all namespaces to a specific script engine.
		/// </summary>
		/// <param name="scriptEngine">The script engine.</param>
		public static void AddNamespaces(Compiler.ScriptingEngine.ScriptEngine scriptEngine)
		{
			scriptEngine.AddNamespace("CandyConquer.WorldApi.Collections");
			scriptEngine.AddNamespace("CandyConquer.WorldApi.Controllers.Packets.Message");
			scriptEngine.AddNamespace("CandyConquer.WorldApi.Controllers.Threads");
			scriptEngine.AddNamespace("CandyConquer.WorldApi.Data.Constants");
			scriptEngine.AddNamespace("CandyConquer.WorldApi.Data.Localization");
			scriptEngine.AddNamespace("CandyConquer.WorldApi.Enums");
			scriptEngine.AddNamespace("CandyConquer.WorldApi.Helpers.Packets.Npc");
			scriptEngine.AddNamespace("CandyConquer.WorldApi.Models.Entities");
			scriptEngine.AddNamespace("CandyConquer.WorldApi.Models.Items");
			scriptEngine.AddNamespace("CandyConquer.WorldApi.Models.Maps");
			scriptEngine.AddNamespace("CandyConquer.WorldApi.Models.Guilds");
			scriptEngine.AddNamespace("CandyConquer.WorldApi.Models.Tournaments");
			scriptEngine.AddNamespace("CandyConquer.WorldApi.Tools");
		}
	}
}
