// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'DisconnectTraces' table.
	/// </summary>
	[DataEntry(Name = "DisconnectTraces", EntryPoint = ConnectionStrings.Log)]
	public sealed class DbDisconnectTrace : SqlModel<DbDisconnectTrace>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		public string TraceId { get; set; }
		public int OwnerId { get; set; }
		public string DisconnectReason { get; set; }
		
		[DataSpecialType(DataType = SpecialDataType.Timestamp)]
		public DateTime Timestamp { get; set; }
	}
}
