// Project by Bauss
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace CandyConquer.Drivers.Repositories.IO
{
	/// <summary>
	/// Simple IniFile wrapper.
	/// </summary>
	public sealed class IniFile
	{
		/// <summary>
		/// IniFile section.
		/// </summary>
		public sealed class IniFileSection
		{
			/// <summary>
			/// Entries of the section.
			/// </summary>
			private Dictionary<string,string> entries;
			/// <summary>
			/// Gets the name of the section.
			/// </summary>
			public string Name { get; private set; }
			
			/// <summary>
			/// Creates a new section.
			/// </summary>
			/// <param name="name">The name.</param>
			internal IniFileSection(string name)
			{
				entries = new Dictionary<string, string>();
				Name = name;
			}
			
			/// <summary>
			/// Gets a value based on an entry key.
			/// </summary>
			/// <param name="key">The key for the entry.</param>
			/// <returns>The value.</returns>
			public string GetValue(string key)
			{
				return entries[key];
			}
			
			/// <summary>
			/// Sets an entry value. If the entry does not exist then it's created.
			/// </summary>
			/// <param name="key">The key.</param>
			/// <param name="value">The value.</param>
			public void SetValue<T>(string key, T value)
			{
				if (entries.ContainsKey(key))
				{
					entries[key] = value.ToString();
				}
				else
				{
					entries.Add(key, value.ToString());
				}
			}
			
			/// <summary>
			/// Checks if the section has a specific entry.
			/// </summary>
			/// <param name="key">The key of the entry.</param>
			/// <returns>True if the entry exists, false otherwise</returns>
			public bool HasValue(string key)
			{
				return entries.ContainsKey(key);
			}
			
			/// <summary>
			/// Updates entry formats.
			/// </summary>
			/// <param name="globalSection">The global inifile section to format with.</param>
			internal void UpdateFormats(IniFileSection globalSection)
			{
				foreach (var key in entries.Keys.ToArray())
				{
					var value = entries[key];
					
					foreach (var innerEntry in entries)
					{
						value = value.Replace(string.Concat("{",innerEntry.Key,"}"), innerEntry.Value);
					}
					
					if (globalSection != null)
					{
						foreach (var innerEntry in globalSection.entries)
						{
							value = value.Replace(string.Concat("{",innerEntry.Key,"}"), innerEntry.Value);
						}
					}
					
					entries[key] = value;
				}
			}
			
			/// <summary>
			/// Serializes the section to an ini string.
			/// </summary>
			/// <returns>A serialized string of the section.</returns>
			public override string ToString()
			{
				var builder = new StringBuilder();
				
				if (!string.IsNullOrWhiteSpace(Name))
				{
					builder.AppendFormat("[{0}]", Name).AppendLine();
				}
				
				foreach (var entry in entries)
				{
					builder.AppendFormat("{0}={1}", entry.Key, entry.Value).AppendLine();
				}
				
				return builder.ToString();
			}
		}
		
		/// <summary>
		/// Collection of sections.
		/// </summary>
		private Dictionary<string, IniFileSection> sections;
		
		/// <summary>
		/// Gets the file name of the ini file.
		/// </summary>
		public string FileName { get; private set; }
		/// <summary>
		/// Gets the global section associated with the ini file.
		/// </summary>
		public IniFileSection GlobalSection { get; private set; }
		
		/// <summary>
		/// Gets an array of all sections stored in the ini file.
		/// </summary>
		public IniFileSection[] Sections
		{
			get { return sections.Values.ToArray(); }
		}
		
		/// <summary>
		/// Creates a new ini file.
		/// </summary>
		/// <param name="fileName">The file name of the ini file.</param>
		public IniFile(string fileName)
		{
			FileName = fileName;
			sections = new Dictionary<string, IniFile.IniFileSection>();
		}
		
		/// <summary>
		/// Checks whether the ini file exists.
		/// </summary>
		/// <returns></returns>
		public bool Exists()
		{
			return File.Exists(FileName);
		}
		
		/// <summary>
		/// Opens the ini file.
		/// </summary>
		public void Open()
		{
			var lines = File.ReadAllLines(FileName);
			GlobalSection = new IniFile.IniFileSection(string.Empty);
			IniFileSection CurrentSection = GlobalSection;
			
			foreach (var line in lines)
			{
				if (!string.IsNullOrWhiteSpace(line))
				{
					if (line.Contains("="))
					{
						var split = line.IndexOf('=');
						var key = line.Substring(0, split);
						var value = line.Substring(split + 1);
						
						CurrentSection.SetValue<string>(key, value);
					}
					else if (line.StartsWith("[") && line.EndsWith("]"))
					{
						var name = line.Substring(1, line.Length - 2);
						
						if (string.IsNullOrWhiteSpace(name))
						{
							continue;
						}
						
						CurrentSection = new IniFile.IniFileSection(name);
						sections.Add(CurrentSection.Name, CurrentSection);
					}
				}
			}
			
			foreach (var section in Sections)
			{
				section.UpdateFormats(GlobalSection);
			}
			
			GlobalSection.UpdateFormats(null);
		}
		
		/// <summary>
		/// Gets a specific section of the ini file.
		/// </summary>
		/// <param name="sectionName">The section name.</param>
		/// <returns>The associated section.</returns>
		public IniFileSection GetSection(string sectionName)
		{
			return sections[sectionName];
		}
		
		/// <summary>
		/// Adds a new section to the inifile.
		/// </summary>
		/// <param name="sectionName">The section name.</param>
		/// <returns>The newly created section.</returns>
		public IniFile.IniFileSection AddSection(string sectionName)
		{
			var section = new IniFile.IniFileSection(sectionName);
			sections.Add(section.Name, section);
			
			return section;
		}
		
		/// <summary>
		/// Closes the inifile and writes all changes to it.
		/// </summary>
		public void Close()
		{
			var builder = new StringBuilder();
			builder.Append(GlobalSection.ToString());
			
			foreach (var section in sections.Values)
			{
				builder.Append(sections.ToString());
			}
			
			File.WriteAllText(FileName, builder.ToString());
		}
	}
}
