// Project by Bauss
using System;

namespace Candy
{
	/// <summary>
	/// A sql data reader based on Candy.DataReader
	/// </summary>
	public class SqlDataReader : DataReader, IDisposable
	{
		/// <summary>
		/// The associated connection.
		/// </summary>
		internal System.Data.SqlClient.SqlConnection Connection { get; set; }
		/// <summary>
		/// The associated command.
		/// </summary>
		internal System.Data.SqlClient.SqlCommand Command { get; set; }
		/// <summary>
		/// The associated reader.
		/// </summary>
		internal System.Data.SqlClient.SqlDataReader Reader { get; set; }
		
		/// <summary>
		/// Creates a new sql data reader.
		/// </summary>
		internal SqlDataReader() { }
		
		/// <summary>
		/// Checks whether there's any rows to read.
		/// </summary>
		/// <returns>True if there's any rows.</returns>
		public override bool Read()
		{
			return Reader.Read();
		}
		
		/// <summary>
		/// Gets the value of a specific column based on a name.
		/// </summary>
		/// <param name="columnName">The name of the column.</param>
		/// <returns>The value read as an object.</returns>
		public override object Get<T>(string columnName)
		{
			var index = Reader.GetOrdinal(columnName);
			var isNull = Reader.IsDBNull(index);

			if (isNull)
			{
				return null;
			}
			else if (typeof(T) == typeof(byte) || typeof(T) == typeof(byte?))
			{
				return (byte)Reader.GetInt32(index);
			}
			else if (typeof(T) == typeof(ushort) || typeof(T) == typeof(ushort?))
			{
				return (ushort)Reader.GetInt32(index);
			}
			else if (typeof(T) == typeof(uint) || typeof(T) == typeof(uint?))
			{
				return (uint)Reader.GetInt32(index);
			}
			else if (typeof(T) == typeof(ulong) || typeof(T) == typeof(ulong?))
			{
				return (ulong)Reader.GetInt64(index);
			}
			else
			{
				return Reader[index];
			}
		}
		
		/// <summary>
		/// Gets the value of a specific column based on a name and a custom reader.
		/// </summary>
		/// <param name="columnName">The name of the column.</param>
		/// <param name="customReader">The custom data reader.</param>
		/// <returns>The value read as an object.</returns>
		public override object GetCustom<T>(string memberName, Func<object,T> customReader)
		{
			if (customReader == null)
			{
				throw new ArgumentNullException("customReader", "Consider creating a custom reader or calling Get<T>()");
			}
			
			var index = Reader.GetOrdinal(memberName);
			var isNull = Reader.IsDBNull(index);

			if (isNull)
			{
				return null;
			}
			
			var value = Reader[index];
			return customReader.Invoke(value);
		}
		
		/// <summary>
		/// Disposing the reader.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		/// <summary>
		/// Disposing the reader.
		/// </summary>
		/// <param name="disposing">Set to true if disposing.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (Reader != null)
					Reader.Dispose();
				if (Command != null)
					Command.Dispose();
				if (Connection != null)
					Connection.Close();
			}
		}
	}
}
