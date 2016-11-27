// Project by Bauss
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Candy
{
	/// <summary>
	/// A class to help handling and executing sql queries.
	/// </summary>
	public static class Sql
	{
		/// <summary>
		/// Gets a dictionary for sql parameters.
		/// </summary>
		/// <returns>A newly created dictionary for sql parameters.</returns>
		public static Dictionary<string,object> GetParsDict()
		{
			return new Dictionary<string, object>();
		}
		
		/// <summary>
		/// Gets a WHERE statement based on parameters.
		/// </summary>
		/// <param name="pars">The parameters.</param>
		/// <returns>The WHERE statement, excluding 'WHERE'</returns>
		public static string GetSel(Dictionary<string,object> pars)
		{
			var builder = new System.Text.StringBuilder();
			foreach (var key in pars.Keys)
			{
				builder.AppendFormat("[{0}] = @{1} AND ", key, key);
			}
			builder.Length -= 5;
			return builder.ToString();
		}
		
		/// <summary>
		/// Gets an INSERT values statement based on parameters.
		/// </summary>
		/// <param name="pars">The parameters.</param>
		/// <returns>The INSERT statement values.</returns>
		public static string GetIns(Dictionary<string,object> pars)
		{
			if (pars.Count == 0)
			{
				return string.Empty;
			}
			
			var keys = pars.Keys.Select(k => string.Format("[{0}]", k));
			var columns = string.Join(", ", keys);
			var values = "@" + string.Join(", @", pars.Keys);
			
			return string.Format("({0}) OUTPUT INSERTED.ID VALUES ({1})", columns, values);
		}
		
		/// <summary>
		/// Gets a set statement based on parameters.
		/// </summary>
		/// <param name="pars">The parameters.</param>
		/// <returns>The set statement.</returns>
		public static string GetSet(Dictionary<string,object> pars)
		{
			var builder = new System.Text.StringBuilder();
			foreach (var key in pars.Keys)
			{
				builder.AppendFormat("[{0}] = @{1}, ", key, key);
			}
			builder.Length -= 2;
			return builder.ToString();
		}
		
		/// <summary>
		/// Adds parameters to a sql command.
		/// This serves as a translator from Dictionary to SqlParameterCollection
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="pars">The parameters.</param>
		private static void AddParameters(SqlCommand command, Dictionary<string, object> pars)
		{
			if (pars == null)
			{
				return;
			}
			
			foreach (var par in pars)
			{
				var value = par.Value;
				if (value == null)
				{
					command.Parameters.AddWithValue(par.Key, DBNull.Value);
					continue;
				}
				
				if (value is byte)
				{
					command.Parameters.AddWithValue(par.Key, (int)(byte)value);
				}
				else if (value is ushort)
				{
					command.Parameters.AddWithValue(par.Key, (int)(ushort)value);
				}
				else if (value is uint)
				{
					command.Parameters.AddWithValue(par.Key, (int)(uint)value);
				}
				else if (value is ulong)
				{
					command.Parameters.AddWithValue(par.Key, (long)(ulong)value);
				}
				else
				{
					command.Parameters.AddWithValue(par.Key, value);
				}
			}
		}
		
		/// <summary>
		/// Executes a non query sql statement.
		/// </summary>
		/// <param name="connString">The connection string.</param>
		/// <param name="sql">The sql.</param>
		/// <param name="pars">The parameters.</param>
		/// <returns>The amount of columns updated.</returns>
		public static int ExecuteNonQuery(string connString, string sql, Dictionary<string,object> pars)
		{
			using (var connection = new SqlConnection(connString))
			{
				connection.Open();
				
				using (var command = new SqlCommand(sql, connection))
				{
					AddParameters(command, pars);
					
					return command.ExecuteNonQuery();
				}
			}
		}
		
		/// <summary>
		/// Executes a scalar sql query.
		/// </summary>
		/// <param name="connString">The connection string.</param>
		/// <param name="sql">The sql.</param>
		/// <param name="pars">The parameters.</param>
		/// <returns>The value of the first column in the result.</returns>
		public static object ExecuteScalar(string connString, string sql, Dictionary<string,object> pars)
		{
			using (var connection = new SqlConnection(connString))
			{
				connection.Open();
				
				using (var command = new SqlCommand(sql, connection))
				{
					AddParameters(command, pars);
					
					return command.ExecuteScalar();
				}
			}
		}
		
		/// <summary>
		/// Executes a sql reader.
		/// </summary>
		/// <param name="connString">The connection string.</param>
		/// <param name="sql">The sql.</param>
		/// <param name="pars">The parameters.</param>
		/// <returns>The sql reader.</returns>
		public static SqlDataReader ExecuteReader(string connString, string sql, Dictionary<string,object> pars)
		{
			var connection = new SqlConnection(connString);
			
			try
			{
				connection.Open();
				var command = new SqlCommand(sql, connection);
				
				try
				{
					AddParameters(command, pars);
					
					return new SqlDataReader
					{
						Connection = connection,
						Command = command,
						Reader = command.ExecuteReader()
					};
				}
				catch (Exception e)
				{
					command.Dispose();
					throw e;
				}
			}
			catch (Exception e)
			{
				connection.Dispose();
				throw e;
			}
		}
	}
}
