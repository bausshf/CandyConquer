// Project by Bauss
using System;

namespace CandyConquer.Database
{
	/// <summary>
	/// Connection strings associated with the database.
	/// </summary>
	internal static class ConnectionStrings
	{
		/// <summary>
		/// The auth connection string.
		/// </summary>
		internal const string Auth = "Server=MOR-HP\\BAUSSSQL;Database=FCQ_AUTH_DB;Trusted_Connection=True";
		/// <summary>
		/// The world connection string.
		/// </summary>
		internal const string World = "Server=MOR-HP\\BAUSSSQL;Database=FCQ_WORLD_DB;Trusted_Connection=True";
		/// <summary>
		/// The log connection string.
		/// </summary>
		internal const string Log = "Server=MOR-HP\\BAUSSSQL;Database=FCQ_LOG_DB;Trusted_Connection=True";
	}
}
