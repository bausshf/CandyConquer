// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'Players' table.
	/// </summary>
	[DataEntry(Name = "Players", EntryPoint = ConnectionStrings.World)]
	public sealed class DbPlayer : SqlModel<DbPlayer>
	{
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }

		public int AccountId { get; set; }
		
		internal DbAccount _account;
		[DataIgnore()]
		public DbAccount Account
		{
			get
			{
				if (_account == null && AccountId > 0)
				{
					_account = Dal.Accounts.GetAccountById(AccountId);
				}
				
				return _account;
			}
		}
		
		public string Server { get; set; }
		public bool New { get; set; }
		public string Name { get; set; }
		public string AuthKey { get; set; }
		public byte? Level { get; set; }
		public uint? Money { get; set; }
		public uint? CPs { get; set; }
		public uint? BoundCPs { get; set; }
		public string Job { get; set; }
		public ushort? Avatar { get; set; }
		public ushort? Model { get; set; }
		public int? MapId { get; set; }
		public ushort? X { get; set; }
		public ushort? Y { get; set; }
		public int? LastMapId { get; set; }
		public ushort? LastMapX { get; set; }
		public ushort? LastMapY { get; set; }
		public ushort? Strength { get; set; }
		public ushort? Vitality { get; set; }
		public ushort? Agility { get; set; }
		public ushort? Spirit { get; set; }
		public ushort? Hair { get; set; }
		public ulong? Experience { get; set; }
		public ushort? AttributePoints { get; set; }
		public short? PKPoints { get; set; }
		public string Title { get; set; }
		public string Spouse { get; set; }
		public int? HP { get; set; }
		public int? MP { get; set; }
		public byte? Reborns { get; set; }
		public string Permission { get; set; }
		public string Language { get; set; }
		public string StatusFlag { get; set; }
		public uint DonationPoints { get; set; } // Every $25 is 1 point, all donations above $150 is +10 points
		public uint WHMoney { get; set; }
		public string WHPassword { get; set; }
	}
}
