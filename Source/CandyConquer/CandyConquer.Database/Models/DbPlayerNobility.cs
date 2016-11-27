// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'PlayerNobility' table.
	/// </summary>
	[DataEntry(Name = "PlayerNobility", EntryPoint = ConnectionStrings.World)]
	public sealed class DbPlayerNobility : SqlModel<DbPlayerNobility>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		public int PlayerId { get; set; }
		public string PlayerName { get; set; }
		public long Donation { get; set; }
		public string Server { get; set; }
	}
}
