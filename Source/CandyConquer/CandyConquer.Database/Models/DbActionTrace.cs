// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'ActionTraces' table.
	/// </summary>
	[DataEntry(Name = "ActionTraces", EntryPoint = ConnectionStrings.Log)]
	public sealed class DbActionTrace : SqlModel<DbActionTrace>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		public int? MapId { get; set; }
		public int OwnerId { get; set; }
		public string AuthKey { get; set; }
		public uint ClientId { get; set; }
		public string ActionName { get; set; }
		public string ActionDescription { get; set; }
		
		[DataSpecialType(DataType = SpecialDataType.Timestamp)]
		public DateTime Timestamp { get; set; }
	}
}
