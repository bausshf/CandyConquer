// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'DefaultCoordinates' table.
	/// </summary>
	[DataEntry(Name = "DefaultCoordinates", EntryPoint = ConnectionStrings.World)]
	public sealed class DbDefaultCoordinate : SqlModel<DbDefaultCoordinate>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		public string Name { get; set; }
		public int MapId { get; set; }
		public int TargetMapId { get; set; }
		public ushort X { get; set; }
		public ushort Y { get; set; }
	}
}
