// Project by Bauss
using System;

namespace CandyConquer.AuthApi.Models.Server
{
	/// <summary>
	/// Server information.
	/// </summary>
	public class ServerInfo
	{
		/// <summary>
		/// The key name.
		/// </summary>
		public string KeyName { get; set; }
		/// <summary>
		/// The server name.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// The server IP address.
		/// </summary>
		public string IPAddress { get; set; }
		/// <summary>
		/// The server port.
		/// </summary>
		public int Port { get; set; }
	}
}
