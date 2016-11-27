// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'WhisperCache' table.
	/// </summary>
	[DataEntry(Name = "WhisperCache", EntryPoint = ConnectionStrings.World)]
	public sealed class DbWhisper : SqlModel<DbWhisper>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		public string To { get; set; }
		public string From { get; set; }
		public string Message { get; set; }
		public uint Mesh { get; set; }
		public string Server { get; set; }
		
		[DataSpecialType(DataType = SpecialDataType.Timestamp)]
		public DateTime Timestamp { get; set; }
	}
}
