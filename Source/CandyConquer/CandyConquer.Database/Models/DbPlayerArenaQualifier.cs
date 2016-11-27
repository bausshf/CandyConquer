// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'PlayerArenaQualifiers' table.
	/// </summary>
	[DataEntry(Name = "PlayerArenaQualifiers", EntryPoint = ConnectionStrings.World)]
	public sealed class DbPlayerArenaQualifier : SqlModel<DbPlayerArenaQualifier>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		public int PlayerId { get; set; }
		public uint TotalWins { get; set; }
		public uint TotalWinsToday { get; set; }
		public uint TotalWinsSeason { get; set; }
		public uint TotalLoss { get; set; }
		public uint TotalLossToday { get; set; }
		public uint TotalLossSeason { get; set; }
		public uint ArenaPoints { get; set; }
		public uint HonorPoints { get; set; }
		public uint Mesh { get; set; }
		public string Name { get; set; }
		public string Job { get; set; }
		public byte Level { get; set; }
		public string Server { get; set; }
		
		[DataSpecialType(DataType = SpecialDataType.Timestamp)]
		public DateTime Timestamp { get; set; }
	}
}
