// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Reflection;
using CandyConquer.Drivers;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A collection of npc scripts.
	/// </summary>
	public sealed class NPCScriptCollection : Compiler.ScriptingEngine.ScriptEngine
	{
		/// <summary>
		/// The script engine declaration.
		/// </summary>
		private static NPCScriptCollection Scripts { get; set; }
		
		/// <summary>
		/// Static constructor for NPCScriptCollection.
		/// </summary>
		static NPCScriptCollection()
		{
			Scripts = new NPCScriptCollection();
			
			ScriptNamespaces.AddNamespaces(Scripts);
		}
		
		/// <summary>
		/// The collection of scripts.
		/// </summary>
		private ConcurrentDictionary<uint, MethodInfo> _scripts;
		
		/// <summary>
		/// Creates a new npc script collection.
		/// </summary>
		private NPCScriptCollection()
			: base(Drivers.Settings.DatabaseSettings.WorldFlatDatabase + "\\NPCs\\Scripts")
		{
			_scripts = new ConcurrentDictionary<uint, MethodInfo>();
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
					_scripts.TryAdd(Convert.ToUInt32(entryAttribute.Entry), method);
				}
			}
		}
		
		/// <summary>
		/// Invokes a specific npc script.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="npcId">The npc id.</param>
		/// <param name="option">The option.</param>
		/// <returns>True if the script was invoked, false otherwise.</returns>
		public static bool Invoke(Models.Entities.Player player, uint npcId, byte option)
		{
			if (Scripts._scripts.Count == 0)
			{
				return false;
			}
			
			MethodInfo script;
			if (Scripts._scripts.TryGetValue(npcId, out script))
			{
				script.Invoke(null, new object[] { player, option });
				return true;
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>
		/// Loads all the npc scripts.
		/// </summary>
		public static void Load()
		{
			Scripts.Compile();
		}
	}
}
