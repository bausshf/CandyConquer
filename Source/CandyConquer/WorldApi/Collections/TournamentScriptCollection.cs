// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Reflection;
using CandyConquer.Drivers;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A collection of tournament scripts.
	/// </summary>
	public sealed class TournamentScriptCollection : Compiler.ScriptingEngine.ScriptEngine
	{
		/// <summary>
		/// The script engine declaration.
		/// </summary>
		private static TournamentScriptCollection Scripts { get; set; }
		
		/// <summary>
		/// Static constructor for TournamentScriptCollection.
		/// </summary>
		static TournamentScriptCollection()
		{
			Scripts = new TournamentScriptCollection();
			
			ScriptNamespaces.AddNamespaces(Scripts);
		}
		
		/// <summary>
		/// The collection of scripts.
		/// </summary>
		private ConcurrentDictionary<string, MethodInfo> _scripts;
		
		/// <summary>
		/// Creates a new tournament script collection.
		/// </summary>
		private TournamentScriptCollection()
			: base(Drivers.Settings.DatabaseSettings.WorldFlatDatabase + "\\Tournaments")
		{
			_scripts = new ConcurrentDictionary<string, MethodInfo>();
		}
		
		/// <summary>
		/// Method executed when the scripts has been compiled successfully.
		/// </summary>
		/// <param name="methods">The methods compiled.</param>
		protected override void Compiled(MethodInfo[] methods)
		{
			_scripts.Clear();
			
			foreach (var method in methods)
			{
				var entryAttribute = method.GetCustomTypeAttribute<Compiler.ScriptingEngine.ScriptEntryAttribute>();
				if (entryAttribute != null)
				{
					_scripts.TryAdd(Convert.ToString(entryAttribute.Entry), method);
				}
			}
			
			Controllers.Threads.TournamentQueueThread.ShouldAssemble = true;
			
			TournamentCollection.Clear();
		}
		
		/// <summary>
		/// Invokes a script.
		/// </summary>
		/// <param name="name">The name of the method to invoke.</param>
		private static void Invoke(string name)
		{
			Scripts._scripts[name].Invoke(null, null);
		}
		
		/// <summary>
		/// Loads all the tournament scripts.
		/// </summary>
		public static void Load()
		{
			Scripts.Compile();
		}
		
		/// <summary>
		/// Assembles all tournaments.
		/// </summary>
		public static void Assemble()
		{
			Invoke("CreateTournamentQueue");
		}
	}
}
