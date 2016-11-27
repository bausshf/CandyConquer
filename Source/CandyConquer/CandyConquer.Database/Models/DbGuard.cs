// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'Guards' table.
	/// </summary>
	[DataEntry(Name = "Guards", EntryPoint = ConnectionStrings.World)]
	public sealed class DbGuard : SqlModel<DbGuard>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		public int GuardId { get; set; }
		public int MapId { get; set; }
		public ushort X { get; set; }
		public ushort Y { get; set; }
		public bool CanMove { get; set; }
	}
}
