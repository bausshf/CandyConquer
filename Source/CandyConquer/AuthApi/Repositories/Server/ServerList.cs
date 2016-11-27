// Project by Bauss
using System;
using System.Collections.Generic;
using System.Linq;
using CandyConquer.Drivers.Repositories.IO;
using CandyConquer.AuthApi.Models.Server;

namespace CandyConquer.AuthApi.Repositories.Server
{
	/// <summary>
	/// Server list of available world servers.
	/// </summary>
	public static class ServerList
	{
		/// <summary>
		/// Collection of all servers.
		/// </summary>
		private static Dictionary<string, ServerInfo> ServerInfos;
		
		/// <summary>
		/// Loads the server list.
		/// </summary>
		public static void Load()
		{
			var serversFile = new IniFile(Drivers.Settings.DatabaseSettings.AuthFlatDatabase + "\\ServerList.ini");
			if (serversFile.Exists())
			{
				serversFile.Open();
				
				ServerInfos = serversFile.Sections
					.Select(section =>
					        {
					        	IniFile.IniFileSection infoSection = section;
					        	if (section.HasValue("Link"))
					        	{
					        		infoSection = serversFile.GetSection(section.GetValue("Link"));
					        	}
					        	
					        	string ip = infoSection.GetValue("IPAddress");
					        	int port = int.Parse(infoSection.GetValue("Port"));
					        	
					        	return new ServerInfo
					        	{
					        		Name = infoSection.Name,
					        		KeyName = section.Name,
					        		IPAddress = ip,
					        		Port = port
					        	};
					        }).ToDictionary(x => x.KeyName, x => x);
			}
		}
		
		/// <summary>
		/// Gets server info based on the server's name.
		/// </summary>
		/// <param name="serverName">The server name.</param>
		/// <returns>The server info.</returns>
		public static ServerInfo GetServerInfo(string serverName)
		{
			lock (Drivers.Locks.GlobalLock)
			{
				ServerInfo info;
				if (!ServerInfos.TryGetValue(serverName, out info) && serverName != "Default")
				{
					return GetServerInfo("Default");
				}
				return info;
			}
		}
	}
}
