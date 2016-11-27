// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'GuildRanks' table.
	/// </summary>
	[DataEntry(Name = "GuildRanks", EntryPoint = ConnectionStrings.World)]
	public sealed class DbGuildRank : SqlModel<DbGuildRank>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		public int GuildId { get; set; }
		public int PlayerId { get; set; }
		
		[DataIgnore()]
		public string PlayerName { get; set; }
		
		[DataIgnore()]
		public byte PlayerLevel { get; set; }
		
		public string GuildRank { get; set; }
		public uint SilverDonation { get; set; }
		public uint CPDonation { get; set; }
		public uint JoinDate { get; set; }
	}
}