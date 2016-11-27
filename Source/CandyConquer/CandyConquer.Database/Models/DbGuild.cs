// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'Guilds' table.
	/// </summary>
	[DataEntry(Name = "Guilds", EntryPoint = ConnectionStrings.World)]
	public sealed class DbGuild : SqlModel<DbGuild>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		public string Name { get; set; }
		public string Announcement { get; set; }
		public uint AnnouncementDate { get; set; }
		public ulong Fund { get; set; }
		public uint CPsFund { get; set; }
		public byte Level { get; set; }
		public string Server { get; set; }
		public bool HasHouse { get; set; }
		public uint WHMoney { get; set; }
	}
}
