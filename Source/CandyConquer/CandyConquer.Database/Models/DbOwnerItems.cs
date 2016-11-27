// Project by Bauss
using System;
using Candy;

namespace CandyConquer.Database.Models
{
	/// <summary>
	/// Database model for owner items.
	/// When updating use "Update(string,bool)"
	/// When creating use "Create(string,bool)"
	/// When deleting use "Delete(string,bool)"
	/// </summary>
	[DataEntry(Name = "N/A", EntryPoint = ConnectionStrings.World)]
	public sealed class DbOwnerItem : SqlModel<DbOwnerItem>
	{
		public const string Inventories = "Inventories";
		public const string Equipments = "Equipments";
		public const string Warehouse = "PlayerWarehouses";
		public const string GuildWarehouse = "GuildWarehouses";
		
		public DbOwnerItem()
			: base()
		{
			ComposedBy = new StringList<string>();
			OwnedBy = new StringList<string>();
			
			DeleteLog = true;
		}
		
		[DataIgnore(IgnoreType = DataIgnoreType.Write)]
		[DataSpecialType(DataType = SpecialDataType.Id)]
		public int Id { get; set; }
		
		public int PlayerId { get; set; } // For guild warehouse this is the guild id.
		public uint? LocationId { get; set; }
		public uint ItemId { get; set; }
		public string Position { get; set; }
		public string Gem1 { get; set; }
		public string Gem2 { get; set; }
		public byte Plus { get; set; }
		public byte Bless { get; set; }
		public byte Enchant { get; set; }
		private int _currentDura;
		public int CurrentDura
		{
			get { return _currentDura; }
			set
			{
				value = Math.Max(0, value);
				if (MaxDura > 0)
				{
					value = Math.Min(MaxDura, value);
				}
				
				_currentDura = value;
			}
		}
		
		private int _maxDura;
		public int MaxDura
		{
			get { return _maxDura; }
			set
			{
				_maxDura = Math.Max(0, value);
				_maxDura = Math.Min(15000, value);
				
				if (CurrentDura > 0)
				{
					CurrentDura = CurrentDura;
				}
			}
		}
		
		public string Color { get; set; }
		public ushort RebornEffect { get; set; }
		public bool Free {  get; set; }
		public bool Suspicious { get; set; }
		public bool Locked { get; set; }
		public uint Composition { get; set; }
		public ushort Amount { get; set; }
		public string MadeBy { get; set; }
		
		[DataSpecialType(DataType = SpecialDataType.AsString)]
		[DataReadFormat(ReadFormat = "new StringList<string>((@value as string) != null ? (@value as string).Split(',') : new string[0])",
		                AssociatedNamespaces = new string[]
		                {
		                	"System.Collections.Generic",
		                	"CandyConquer.Database.Models"
		                })]
		public StringList<string> ComposedBy { get; set; }
		
		[DataSpecialType(DataType = SpecialDataType.AsString)]
		[DataReadFormat(ReadFormat = "new StringList<string>((@value as string) != null ? (@value as string).Split(',') : new string[0])",
		                AssociatedNamespaces = new string[]
		                {
		                	"System.Collections.Generic",
		                	"CandyConquer.Database.Models"
		                })]
		public StringList<string> OwnedBy { get; set; }
		
		public uint SocketRGB { get; set; }
	}
}
