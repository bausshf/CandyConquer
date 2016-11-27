// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'PacketTraces' table.
	/// </summary>
	[DataEntry(Name = "PacketTraces", EntryPoint = ConnectionStrings.Log)]
	public sealed class DbPacketTrace : SqlModel<DbPacketTrace>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		public int PacketId { get; set; }
		public string PacketSubObject { get; set; }
		public int OwnerId { get; set; }
		public string TraceId { get; set; }
		public int Size { get; set; }
		public int VirtualSize { get; set; }
		public byte[] Buffer { get; set; }
		
		[DataSpecialType(DataType = SpecialDataType.Timestamp)]
		public DateTime Timestamp { get; set; }
	}
}
