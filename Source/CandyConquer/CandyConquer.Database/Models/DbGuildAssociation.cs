// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'GuildAssociations' table.
	/// </summary>
	[DataEntry(Name = "GuildAssociations", EntryPoint = ConnectionStrings.World)]
	public sealed class DbGuildAssociation : SqlModel<DbGuildAssociation>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		public int GuildId { get; set; }
		public int AssociationGuildId { get; set; }
		public string AssociationType { get; set; }
	}
}