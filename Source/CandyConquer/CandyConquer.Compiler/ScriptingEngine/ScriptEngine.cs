// Project by Bauss
using System;
using System.Text;
using System.IO;
using System.Security.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;

namespace CandyConquer.Compiler.ScriptingEngine
{
	/// <summary>
	/// A script engine for compiling C# during runtime.
	/// </summary>
	public abstract class ScriptEngine
	{
		/// <summary>
		/// The path of the source files.
		/// </summary>
		private string _path;
		/// <summary>
		/// File system watcher.
		/// </summary>
		private FileSystemWatcher _fileSystemWatcher;
		/// <summary>
		/// A list of namespaces to include during compilation.
		/// </summary>
		private List<string> _namespaces;
		
		/// <summary>
		/// The template.
		/// </summary>
		private const string CODE_TEMPLATE = @"using System;
using System.Collections.Generic;
using System.Linq;
using CandyConquer.Compiler.ScriptingEngine;
@Namespaces

namespace CandyConquer.Compiler
{
	public class CompiledClass
	{
		@Data
	}
}";
		
		/// <summary>
		/// Creates a new script engine.
		/// </summary>
		/// <param name="path">The path of the source files.</param>
		public ScriptEngine(string path)
		{
			_path = path;
			_namespaces = new List<string>();
			
			CreateWatcher();
		}
		
		/// <summary>
		/// Creates the file system watcher.
		/// </summary>
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private void CreateWatcher()
		{
			_fileSystemWatcher = new FileSystemWatcher(_path, "*.cs");
			_fileSystemWatcher.IncludeSubdirectories = true;
			_fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
			
			_fileSystemWatcher.Changed += new FileSystemEventHandler(FileSystem_Changed);
			_fileSystemWatcher.Deleted += new FileSystemEventHandler(FileSystem_Changed);
			
			_fileSystemWatcher.EnableRaisingEvents = true;
		}
		
		/// <summary>
		/// Event for the file system watcher.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event args.</param>
		private void FileSystem_Changed(object sender, FileSystemEventArgs e)
		{
			switch (e.ChangeType) {
				case WatcherChangeTypes.Changed:
					Compile();
					break;
				case WatcherChangeTypes.Deleted:
					Compile();
					break;
			}
		}
		
		/// <summary>
		/// Compiles the source.
		/// Normally not necessary, except for once during program start up.
		/// </summary>
		public void Compile()
		{
			var codes = new List<string>();
			
			foreach (var file in Directory.GetFiles(_path, "*.cs", SearchOption.AllDirectories))
			{
				codes.Add(File.ReadAllText(file));
			}
			
			if (codes.Count == 0)
			{
				return;
			}
			
			var code = CODE_TEMPLATE
				.Replace("@Namespaces",
				         string.Join("\r\n", _namespaces
				                     .Select(ns => string.Format("using {0};", ns))
				                    )
				        )
				.Replace("@Data", string.Join("\r\n", codes));
			
			var results = Internal.CompilerProvider.Compile<CSharpCodeProvider>(null, code);
			if (results.Errors != null && results.Errors.Count > 0)
			{
				var lines = code.Split('\n');
				
				foreach (System.CodeDom.Compiler.CompilerError error in results.Errors)
				{
					if (error.Line == 0)
					{
						Console.WriteLine(error);
						continue;
					}
					var errorLine = error.Line - 1;
					if (errorLine < lines.Length)
					{
						int countFrom = Math.Max(0, errorLine - 3);
						int countUntil = Math.Min(lines.Length, errorLine + 3);
						
						var errorCode = new StringBuilder();
						for (int i = countFrom; i < countUntil; i++)
						{
							if (i == errorLine)
							{
								errorCode.Append("error: >>>");
							}
							
							errorCode.AppendFormat("\t{0}\r\n", lines[i]);
						}
						
						Console.WriteLine("Failed to compile:");
						Console.WriteLine(errorCode.ToString());
					}
				}
				
				return;
			}
			
			Compiled(Internal.CompilerProvider.GetAllMethods(results.CompiledAssembly, "CandyConquer.Compiler"));
		}
		
		/// <summary>
		/// Adds a namespace to the script engine.
		/// </summary>
		/// <param name="nameSpace">The namespace to compile with.</param>
		public void AddNamespace(string nameSpace)
		{
			_namespaces.Add(nameSpace);
		}
		
		/// <summary>
		/// Abstract method for when compilation has finished without errors.
		/// </summary>
		/// <param name="methods">The methods compiled.</param>
		protected abstract void Compiled(MethodInfo[] methods);
	}
}
