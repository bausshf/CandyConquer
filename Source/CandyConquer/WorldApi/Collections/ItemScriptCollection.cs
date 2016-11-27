// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Reflection;
using CandyConquer.Drivers;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A collection of item scripts.
	/// </summary>
	public sealed class ItemScriptCollection : Compiler.ScriptingEngine.ScriptEngine
	{
		/// <summary>
		/// The script engine declaration.
		/// </summary>
		private static ItemScriptCollection Scripts { get; set; }
		
		/// <summary>
		/// Static constructor for ItemScriptCollection.
		/// </summary>
		static ItemScriptCollection()
		{
			Scripts = new ItemScriptCollection();
			
			ScriptNamespaces.AddNamespaces(Scripts);
		}
		
		/// <summary>
		/// The collection of scripts.
		/// </summary>
		private ConcurrentDictionary<int, MethodInfo> _scripts;
		
		/// <summary>
		/// Creates a new item script collection.
		/// </summary>
		private ItemScriptCollection()
			: base(Drivers.Settings.DatabaseSettings.WorldFlatDatabase + "\\Items\\Scripts")
		{
			_scripts = new ConcurrentDictionary<int, MethodInfo>();
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
					_scripts.TryAdd(Convert.ToInt32(entryAttribute.Entry), method);
				}
			}
		}
		
		/// <summary>
		/// Invokes a specific item script.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="npcId">The item id.</param>
		/// <param name="option">The option.</param>
		/// <returns>True if the script was invoked, false otherwise.</returns>
		public static bool Invoke(Models.Entities.Player player, int itemId)
		{
			if (Scripts._scripts.Count == 0)
			{
				return false;
			}
			
			MethodInfo script;
			if (Scripts._scripts.TryGetValue(itemId, out script))
			{
				script.Invoke(null, new object[] { player });
				return true;
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>
		/// Loads all the item scripts.
		/// </summary>
		public static void Load()
		{
			Scripts.Compile();
		}
	}
}
