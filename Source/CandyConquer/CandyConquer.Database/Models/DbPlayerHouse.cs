// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'PlayerHouses' table.
	/// </summary>
	[DataEntry(Name = "PlayerHouses", EntryPoint = ConnectionStrings.World)]
	public sealed class DbPlayerHouse : SqlModel<DbPlayerHouse>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		public int PlayerId { get; set; }
		public int MapId { get; set; }
		public bool IsBig { get; set; }
		public bool Warehouse { get; set; }
		public bool IsLeasing { get; set; }
		public DateTime? NextRentDate { get; set; }
		public uint InvestedMoney { get; set; }
	}
}
