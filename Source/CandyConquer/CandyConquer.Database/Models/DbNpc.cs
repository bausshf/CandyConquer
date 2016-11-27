// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'NPCs' table.
	/// </summary>
	[DataEntry(Name = "NPCs", EntryPoint = ConnectionStrings.World)]
	public sealed class DbNpc : SqlModel<DbNpc>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint NpcId { get; set; }
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string Type { get; set; }
		
		public string Name { get; set; }
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public int MapId { get; set; }
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort X { get; set; }
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort Y { get; set; }
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public uint Flag { get; set; }
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public ushort Mesh { get; set; }
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public byte Avatar { get; set; }
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		public string Server { get; set; }
	}
}
