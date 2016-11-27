// Project by Bauss
using System;
using System.Linq;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for the 'Accounts' table.
	/// </summary>
	[DataEntry(Name = "Accounts", EntryPoint = ConnectionStrings.Auth)]
	public sealed class DbAccount : SqlModel<DbAccount>
	{
		/// <summary>
		/// Ban range type.
		/// </summary>
		public enum BanRangeType
		{
			OneDay,
			ThreeDays,
			OneWeek,
			OneMonth,
			ThreeMonths,
			SixMonths,
			OneYear,
			Perm
		}
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		public string Name { get; set; }
		public string Password { get; set; }
		
		public string Email { get; set; }
		public string SecondaryEmail { get; set; }
		
		public string SecurityQuestion { get; set; }
		public string SecurityAnswer { get; set; }
		
		public string RegistrationIP { get; set; }
		public string LastIP { get; set; }
		public string FirstLoginIP { get; set; }
		public string RegistrationIPRegion { get; set; }
		public string FirstLoginIPRegion { get; set; }
		public string LastLoginIPRegion { get; set; }
		
		public string Country { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public byte? Age { get; set; }
		
		public string LastAuthKey { get; set; }
		public string LastServer { get; set; }
		
		public bool Banned { get; set; }
		public string BanDescription { get; set; }
		public DateTime? BanDate { get; set; }
		[DataSpecialType(DataType = SpecialDataType.AsString)]
		[DataReadFormat(ReadFormat = "EnumExtensions.ToEnum<DbAccount.BanRangeType>(@value as string)",
		                AssociatedNamespaces = new string[]
		                {
		                	"CandyConquer.Drivers",
		                	"CandyConquer.Database.Models"
		                })]
		public BanRangeType BanRange { get; set; }
		
		public DateTime? RegistrationDate { get; set; }
		public DateTime? FirstLoginDate { get; set; }
		public DateTime? LastLoginDate { get; set; }
	}
}
