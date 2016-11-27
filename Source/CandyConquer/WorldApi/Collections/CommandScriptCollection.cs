// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Reflection;
using CandyConquer.Drivers;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A collection of command scripts.
	/// </summary>
	public sealed class CommandScriptCollection : Compiler.ScriptingEngine.ScriptEngine
	{
		/// <summary>
		/// The script engine declaration.
		/// </summary>
		private static CommandScriptCollection Scripts { get; set; }
		
		/// <summary>
		/// Static constructor for CommandScriptCollection.
		/// </summary>
		static CommandScriptCollection()
		{
			Scripts = new CommandScriptCollection();
			
			ScriptNamespaces.AddNamespaces(Scripts);
		}
		
		/// <summary>
		/// The collection of scripts.
		/// </summary>
		private ConcurrentDictionary<string, MethodInfo> _scripts;
		
		/// <summary>
		/// Creates a new command script collection.
		/// </summary>
		private CommandScriptCollection()
			: base(Drivers.Settings.DatabaseSettings.WorldFlatDatabase + "\\Commands")
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
		}
		
		/// <summary>
		/// Invokes a script.
		/// </summary>
		/// <param name="name">The name of the method to invoke.</param>
		public static void Invoke(Models.Entities.Player player,
		                          string fullCommand,
		                          string command,
		                          char commandPrefix)
		{
			Scripts._scripts["HandleCommands"].Invoke(null, new object[] { player, fullCommand, command, commandPrefix });
		}
		
		/// <summary>
		/// Loads all the command scripts.
		/// </summary>
		public static void Load()
		{
			Scripts.Compile();
		}
	}
}
