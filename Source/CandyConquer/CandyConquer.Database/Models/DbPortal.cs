// Project by Bauss
using System;
using Candy;
using CandyConquer.Maps;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'Porals' table.
	/// </summary>
	[DataEntry(Name = "Portals", EntryPoint = ConnectionStrings.World)]
	public sealed class DbPortal : SqlModel<DbPortal>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort StartMapId { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort StartX { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort StartY { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort EndMapId { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort EndX { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort EndY { get; set; }
	}
}
