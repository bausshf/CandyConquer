// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'MonsterSpawns' table.
	/// </summary>
	[DataEntry(Name = "MonsterSpawns", EntryPoint = ConnectionStrings.World)]
	public class DbMonsterSpawn : SqlModel<DbMonsterSpawn>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		public int MapId { get; set; }
		public int MonsterId { get; set; }
		public int RangeSize { get; set; }
		public ushort X { get; set; }
		public ushort Y { get; set; }
		public int Amount { get; set; }
		public string Server { get; set; }
	}
}
