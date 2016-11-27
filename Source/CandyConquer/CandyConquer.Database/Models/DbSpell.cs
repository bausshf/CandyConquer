// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'Spells' table.
	/// </summary>
	[DataEntry(Name = "Spells", EntryPoint = ConnectionStrings.World)]
	public sealed class DbSpell : SqlModel<DbSpell>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		public int PlayerId { get; set; }
		public ushort SpellId { get; set; }
		public ushort Level { get; set; }
		public uint Experience { get; set; }
		public string Type { get; set; }
	}
}
