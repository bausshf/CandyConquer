// Project by Bauss
using System;
using CandyConquer.Database.Models;
using CandyConquer.WorldApi.Models.Maps;

namespace CandyConquer.WorldApi.Models.Items
{
	/// <summary>
	/// Model for an item.
	/// </summary>
	public sealed class Item : Controllers.Items.ItemController, IMapObject
	{
		/// <summary>
		/// Gets the database item.
		/// </summary>
		public DbItem DbItem { get; private set; }
		/// <summary>
		/// Gets or sets the database owner item.
		/// </summary>
		public DbOwnerItem DbOwnerItem { get; set; }
		
		/// <summary>
		/// Creates a new item.
		/// </summary>
		public Item()
			: base()
		{
			Item = this;
			MapObject = this;
		}
		
		/// <summary>
		/// Creates a new item.
		/// </summary>
		/// <param name="dbItem">The database item tied to it.</param>
		public Item(DbItem dbItem)
			: this()
		{
			DbItem = dbItem;
		}
		
		#region Item
		/// <summary>
		/// The client id.
		/// </summary>
		private uint _clientId;
		/// <summary>
		/// Gets the client id.
		/// </summary>
		public uint ClientId
		{
			get { return _clientId; }
			set
			{
				throw new InvalidOperationException("Do not set the client id. The set property is there for interface compatibility only.");
			}
		}
		
		/// <summary>
		/// Gets a boolean indicating whether the item is female or not.
		/// </summary>
		public bool IsFemale
		{
			get { return DbItem.Sex == "Female"; }
		}
		
		/// <summary>
		/// Gets the item quality.
		/// </summary>
		public Enums.ItemQuality Quality
		{
			get { return (Enums.ItemQuality)(DbItem.Id % 10); }
		}
		
		/// <summary>
		/// Gets the item addition.
		/// </summary>
		public ItemAddition Addition
		{
			get
			{
				if (DbOwnerItem == null || DbOwnerItem.Plus == 0)
				{
					return null;
				}
				
				int type = DbItem.Id;
				if (type / 100000 == 4 || type / 100000 == 5)
				{
					type = (type / 100000) * 111000 + (type / 10 * 10 % 1000);
				}
				else
				{
					type = type / 10 * 10;
				}
				
				return Collections.ItemCollection.GetItemAddition((uint)type, DbOwnerItem.Plus);
			}
		}
		
		/// <summary>
		/// The position of the item.
		/// </summary>
		private Enums.ItemPosition _position;
		/// <summary>
		/// Gets or sets position of the item.
		/// </summary>
		public Enums.ItemPosition Position
		{
			get { return _position; }
			set
			{
				_position = value;
				
				if (DbOwnerItem != null)
				{
					DbOwnerItem.Position = Position.ToString();
				}
			}
		}
		
		/// <summary>
		/// The first socket.
		/// </summary>
		private Enums.Gem _gem1;
		/// <summary>
		/// Gets or sets the first socket.
		/// </summary>
		public Enums.Gem Gem1
		{
			get { return _gem1; }
			set
			{
				_gem1 = value;
				DbOwnerItem.Gem1 = _gem1.ToString();
			}
		}
		
		/// <summary>
		/// The second socket.
		/// </summary>
		private Enums.Gem _gem2;
		/// <summary>
		/// Gets or sets the second socket.
		/// </summary>
		public Enums.Gem Gem2
		{
			get { return _gem2; }
			set
			{
				_gem2 = value;
				DbOwnerItem.Gem2 = _gem2.ToString();
			}
		}
		
		/// <summary>
		/// The item color.
		/// </summary>
		private Enums.ItemColor _itemColor = Enums.ItemColor.Orange;
		/// <summary>
		/// Gets or sets the item color.
		/// </summary>
		public Enums.ItemColor Color
		{
			get { return (IsArmor || IsHead || IsShield) ? _itemColor : (Enums.ItemColor)0; }
			set
			{
				_itemColor = value;
				DbOwnerItem.Color = _itemColor.ToString();
			}
		}
		
		/// <summary>
		/// Gets a boolean determining whether the item is discardable or not.
		/// </summary>
		public bool Discardable
		{
			get
			{
				if (DbOwnerItem != null)
				{
					return !DbOwnerItem.Free &&
						!DbOwnerItem.Suspicious &&
						!DbOwnerItem.Locked;
				}
				
				return true;
			}
		}
		
		/// <summary>
		/// Gets the repair price of the item.
		/// </summary>
		public uint RepairPrice
		{
			get
			{
				if (DbOwnerItem.CurrentDura == DbOwnerItem.MaxDura)
				{
					return 0;
				}
				
				int recoverDurability = Math.Max(0, (DbOwnerItem.MaxDura - DbOwnerItem.CurrentDura));
				int repairCost = 0;
				
				if (DbOwnerItem.MaxDura > 0)
				{
					repairCost = (int)(DbItem.Price * recoverDurability / DbOwnerItem.MaxDura / 2);
				}
				
				return (uint)Math.Max(1, repairCost);
			}
		}
		
		/// <summary>
		/// Gets or sets the drop time of the item.
		/// </summary>
		public DateTime DropTime { get; set; }
		
		/// <summary>
		/// Gets or sets a boolean determining whether item was dropped by a player or not.
		/// </summary>
		public bool PlayerDrop { get; set; }
		
		/// <summary>
		/// Gets or sets the drop client id.
		/// </summary>
		public uint DropClientId { get; set; }
		
		/// <summary>
		/// Gets or sets the amount of money tied to the drop.
		/// </summary>
		public uint DropMoney { get; set; }
		#endregion
		
		#region Is ?
		/// <summary>
		/// Checks whether the item is a shield.
		/// </summary>
		/// <returns>Returns true if the item is a shield.</returns>
		public bool IsShield
		{
			get { return (DbItem.Id >= 900000 && DbItem.Id <= 900999); }
		}
		
		/// <summary>
		/// Checks whether the item is an armor.
		/// </summary>
		/// <returns>Returns true if the item is an armor.</returns>
		public bool IsArmor
		{
			get { return (DbItem.Id >= 130000 && DbItem.Id <= 136999); }
		}
		
		/// <summary>
		/// Checks whether the item is a headgear.
		/// </summary>
		/// <returns>Returns true if the item is a headgear.</returns>
		public bool IsHead
		{
			get { return ((DbItem.Id >= 111000 && DbItem.Id <= 118999) || (DbItem.Id >= 123000 && DbItem.Id <= 123999) || (DbItem.Id >= 141999 && DbItem.Id <= 143999)); }
		}
		
		/// <summary>
		/// Checks whether the item is an one hander.
		/// </summary>
		/// <returns>Returns true if the item is an one hander.</returns>
		public bool IsOneHand
		{
			get { return (DbItem.Id >= 410000 && DbItem.Id <= 499999 || DbItem.Id >= 601000 && DbItem.Id <= 601999 || DbItem.Id >= 610000 && DbItem.Id <= 610999); }
		}
		
		/// <summary>
		/// Checks whether the item is a backsword.
		/// </summary>
		/// <returns>Returns true if the item is a backsword.</returns>
		public bool IsBacksword
		{
			get { return (DbItem.Id >= 421000 && DbItem.Id <= 421999); }
		}
		
		/// <summary>
		/// Checks whether the item is a sword or blade.
		/// </summary>
		/// <returns>Returns true if the item is a sword or blade.</returns>
		public bool IsSwordOrBlade
		{
			get { return (DbItem.Id >= 410000 && DbItem.Id <= 421999); }
		}
		
		/// <summary>
		/// Checks whether the item is a two hander.
		/// </summary>
		/// <returns>Returns true if the item is a two hander.</returns>
		public bool IsTwoHand
		{
			get
			{
				return (DbItem.Id > 510000 && DbItem.Id < 599999) || IsBow;
			}
		}
		
		/// <summary>
		/// Checks whether the item is an arrow.
		/// </summary>
		/// <returns>Returns true if the item is an arrow.</returns>
		public bool IsArrow
		{
			get { return (DbItem.Id >= 1050000 && DbItem.Id <= 1051000); }
		}
		
		/// <summary>
		/// Checks whether the item is a bow.
		/// </summary>
		/// <returns>Returns true if the item is a bow.</returns>
		public bool IsBow
		{
			get { return (DbItem.Id >= 500000 && DbItem.Id < 510000); }
		}
		
		/// <summary>
		/// Checks whether the item is a necklace.
		/// </summary>
		/// <returns>Returns true if the item is a necklace.</returns>
		public bool IsNecklace
		{
			get { return (DbItem.Id >= 120000 && DbItem.Id <= 121999); }
		}
		
		/// <summary>
		/// Checks whether the item is a ring.
		/// </summary>
		/// <returns>Returns true if the item is a ring.</returns>
		public bool IsRing
		{
			get { return (DbItem.Id >= 150000 && DbItem.Id <= 159999); }
		}
		
		/// <summary>
		/// Checks whether the item is boots.
		/// </summary>
		/// <returns>Returns true if the item is boots.</returns>
		public bool IsBoots
		{
			get { return (DbItem.Id >= 160000 && DbItem.Id <= 160999); }
		}
		
		/// <summary>
		/// Checks whether the item is a garment.
		/// </summary>
		/// <returns>Returns true if the item is a garment.</returns>
		public bool IsGarment
		{
			get
			{
				if (DbItem.Id == 134155 || DbItem.Id == 131155 || DbItem.Id == 133155 || DbItem.Id == 130155)
				{
					return true;
				}
				
				return (DbItem.Id >= 181000 && DbItem.Id <= 194999);
			}
		}
		
		/// <summary>
		/// Checks whether the item is a fan.
		/// </summary>
		/// <returns>Returns true if the item is a fan.</returns>
		public bool IsFan
		{
			get { return (DbItem.Id >= 201000 && DbItem.Id <= 201999); }
		}
		
		/// <summary>
		/// Checks whether the item is a tower.
		/// </summary>
		/// <returns>Returns true if the item is a tower.</returns>
		public bool IsTower
		{
			get { return (DbItem.Id >= 202000 && DbItem.Id <= 202999); }
		}
		
		/// <summary>
		/// Checks whether the item is a steed.
		/// </summary>
		/// <returns>Returns true if the item is a steed.</returns>
		public bool IsSteed
		{
			get { return (DbItem.Id == 300000); }
		}
		
		/// <summary>
		/// Checks whether the item is a bottle.
		/// </summary>
		/// <returns>Returns true if the item is a bottle.</returns>
		public bool IsBottle
		{
			get { return (DbItem.Id >= 2100025 && DbItem.Id <= 2100095); }
		}
		
		/// <summary>
		/// Checks whether the is a mount armor.
		/// </summary>
		/// <returns>Returns true if the item is a mount armor.</returns>
		public bool IsMountArmor
		{
			get { return (DbItem.Id >= 200000 && DbItem.Id <= 200420); }
		}
		
		/// <summary>
		/// Checks whether the item is a misc item.
		/// </summary>
		/// <returns>Returns true if the item is a misc item.</returns>
		public bool IsMisc
		{
			get { return (!IsHead && !IsArmor &&
			              !IsShield && !IsOneHand &&
			              !IsTwoHand && !IsNecklace &&
			              !IsRing && !IsArrow &&
			              !IsBow && !IsBoots &&
			              !IsGarment && !IsFan &&
			              !IsTower && !IsBottle &&
			              !IsSteed && !IsMountArmor); }
		}
		#endregion
		
		#region IMapObject
		/// <summary>
		/// Gets or sets the idle state of the monster.
		/// </summary>
		public bool Idle { get; set; }
		/// <summary>
		/// The map.
		/// </summary>
		private Map _map;
		/// <summary>
		/// Gets or sets the map.
		/// </summary>
		public Map Map
		{
			get
			{
				return _map;
			}
			set
			{
				if (_map != null && !_map.IsDynamic)
				{
					if (_map.MapType != DbMap.DbMapType.NoSkillsNoLogin &&
					    _map.MapType != DbMap.DbMapType.NoLogin &&
					    _map.MapType != DbMap.DbMapType.Tournament)
					{
						LastMap = _map;
					}
				}
				
				_map = value;
			}
		}
		
		/// <summary>
		/// Gets the map id.
		/// </summary>
		public int MapId
		{
			get { return Map.Id; }
		}
		
		/// <summary>
		/// The x coordinate.
		/// </summary>
		private ushort _x;
		
		/// <summary>
		/// Gets or sets the x coordinate.
		/// </summary>
		public ushort X
		{
			get { return _x; }
			set
			{
				_lastX = _x;
				_x = value;
			}
		}
		
		/// <summary>
		/// The y coordinate.
		/// </summary>
		private ushort _y;
		/// <summary>
		/// Gets or sets the y coordinate.
		/// </summary>
		public ushort Y
		{
			get { return _y; }
			set
			{
				_lastY = _y;
				_y = value;
			}
		}
		
		/// <summary>
		/// The last map.
		/// </summary>
		private Map _lastMap;
		
		/// <summary>
		/// Gets or sets the last map.
		/// </summary>
		public  Map LastMap
		{
			get { return _lastMap; }
			set
			{
				if (value == null)
				{
					return;
				}
				_lastMap = value;
				LastMapX = X;
				LastMapY = Y;
			}
		}
		
		/// <summary>
		/// Gets the last map id.
		/// </summary>
		public  int LastMapId
		{
			get { return _lastMap.Id; }
		}
		
		/// <summary>
		/// Gets or sets the last map x.
		/// </summary>
		public  ushort LastMapX
		{
			get; set;
		}
		
		/// <summary>
		/// Gets or sets the last map y.
		/// </summary>
		public  ushort LastMapY
		{
			get; set;
		}
		
		/// <summary>
		/// The last x coordinate.
		/// </summary>
		private ushort _lastX;
		
		/// <summary>
		/// Gets the last x coordinate.
		/// </summary>
		public  ushort LastX
		{
			get { return _lastX; }
		}
		
		/// <summary>
		/// The last y coordinate.
		/// </summary>
		private ushort _lastY;
		
		/// <summary>
		/// Gets the last y coordinate.
		/// </summary>
		public  ushort LastY
		{
			get { return _lastY; }
		}
		
		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		public Enums.Direction Direction
		{
			get; set;
		}
		
		/// <summary>
		/// The status flag holder.
		/// </summary>
		private ulong _statusflag;
		
		/// <summary>
		/// Gets or sets the status flag.
		/// Please use AddStatusFlag() or RemoveStatusFlag() instead of modifying this directly.
		/// </summary>
		public ulong StatusFlag
		{
			get { return _statusflag; }
			set
			{
				_statusflag = value;
				
				UpdateScreen(true);
			}
		}
		
		/// <summary>
		/// Do not use.
		/// </summary>
		[Obsolete("Do not use.")]
		public bool Alive { get; set; }
		
		/// <summary>
		/// Gets the spawn packet.
		/// </summary>
		/// <returns>The spawn packet.</returns>
		public byte[] GetSpawnPacket()
		{
			return new Models.Packets.Items.GroundItemPacket
			{
				ClientId = this.ClientId,
				ItemId = (uint)DbItem.Id,
				X = this.X,
				Y = this.Y,
				Remove = false
			};
		}
		
		/// <summary>
		/// Gets the removal spawn packet.
		/// </summary>
		/// <returns>The removal spawn packet.</returns>
		public byte[] GetRemoveSpawnPacket()
		{
			return new Models.Packets.Items.GroundItemPacket
			{
				ClientId = this.ClientId,
				ItemId = (uint)DbItem.Id,
				X = 0,
				Y = 0,
				Remove = true
			};
		}
		
		/// <summary>
		/// Gets a boolean determining whether the item can update its screen.
		/// </summary>
		public bool CanUpdateScreen
		{
			get { return ClientId != 0; }
		}
		#endregion
		
		/// <summary>
		/// Copies the item into a new item.
		/// </summary>
		/// <returns>A copy of the item.</returns>
		public Item Copy()
		{
			return new Item
			{
				_clientId = Drivers.Repositories.Safe.IdentityGenerator.GetItemId(),
				DbItem = this.DbItem,
				_gem1 = this._gem1,
				_gem2 = this._gem2,
				_itemColor = this._itemColor
			};
		}
		
		/// <summary>
		/// Upgrades the level of the item.
		/// </summary>
		/// <param name="owner">The owner of the item.</param>
		/// <returns>True if the item was upgraded, false if the item has no more levels.</returns>
		public bool UpgradeLevel(Models.Entities.Player owner)
		{
			if (IsSteed || IsGarment || IsArrow || IsBottle || IsFan || IsTower || IsMountArmor || IsMisc)
			{
				return false;
			}
			
			int id = DbItem.Id;
			while (id % 10 < 5)
			{
				id++;
			}
			
			if (DbItem.Level == 1 && (IsOneHand || IsBow))
			{
				id = ((DbItem.BaseId * 1000) - 10) + 5;
			}
			
			uint newItemId = (uint)(id + 10);
			int loop = 4;
			while (!Collections.ItemCollection.Contains(newItemId))
			{
				newItemId += 10;
				loop--;
				if (loop <= 0)
					break;
			}
			
			if (!Collections.ItemCollection.Contains(newItemId))
			{
				return false;
			}
			
			var newItem = Collections.ItemCollection.CreateItemById(newItemId);
			if (newItem == null)
			{
				return false;
			}
			
			if (newItem.DbItem.Level <= DbItem.Level || DbItem.Level > 1 && newItem.DbItem.TypeName != DbItem.TypeName)
			{
				return false;
			}
			
			DbItem = newItem.DbItem;
			if (DbOwnerItem == null)
			{
				DbOwnerItem = new Database.Models.DbOwnerItem();
				DbOwnerItem.MaxDura = DbItem.Amount;
				DbOwnerItem.CurrentDura = DbOwnerItem.MaxDura;
				Gem1 = Enums.Gem.NoSocket;
				Gem2 = Enums.Gem.NoSocket;
			}
			DbOwnerItem.ItemId = (uint)DbItem.Id;
			
			if (owner != null)
			{
				UpdateClient(owner, Enums.ItemUpdateAction.Update);
				owner.UpdateBaseStats();
			}
			
			return true;
		}
		
		/// <summary>
		/// Upgrades the quality of the item.
		/// </summary>
		/// <param name="owner">The owner of the item.</param>
		/// <returns>True if the item's quality was upgraded, false if the quality is super.</returns>
		public bool UpgradeQuality(Models.Entities.Player owner)
		{
			if (IsSteed || IsGarment || IsArrow || IsBottle || IsMountArmor || IsMisc ||
			    Quality == Enums.ItemQuality.Super)
			{
				return false;
			}
			
			uint newItemId = (uint)DbItem.Id;
			if (((byte)Quality) >= 5)
				newItemId++;
			else
			{
				while ((newItemId % 10) < 6)
				{
					newItemId++;
				}
			}
			
			if (!Collections.ItemCollection.Contains(newItemId))
			{
				return false;
			}
			
			var newItem = Collections.ItemCollection.CreateItemById(newItemId);
			if (newItem == null)
			{
				return false;
			}
			
			DbItem = newItem.DbItem;
			if (DbOwnerItem == null)
			{
				DbOwnerItem = new Database.Models.DbOwnerItem();
				DbOwnerItem.MaxDura = DbItem.Amount;
				DbOwnerItem.CurrentDura = DbOwnerItem.MaxDura;
				Gem1 = Enums.Gem.NoSocket;
				Gem2 = Enums.Gem.NoSocket;
			}
			DbOwnerItem.ItemId = (uint)DbItem.Id;
			
			if (owner != null)
			{
				UpdateClient(owner, Enums.ItemUpdateAction.Update);
				owner.UpdateBaseStats();
			}
			
			return true;
		}
		
		/// <summary>
		/// Sets the quality of an item.
		/// </summary>
		/// <param name="owner">The owner of the item.</param>
		/// <param name="quality">The quality to set.</param>
		/// <returns>True if the quality was set, false if the quality is below the current quality.</returns>
		public bool SetQuality(Models.Entities.Player owner, Enums.ItemQuality quality)
		{
			byte qualityValue = ((byte)quality);
			if (qualityValue < ((byte)Quality))
			{
				return false;
			}
			
			if (IsSteed || IsGarment || IsArrow || IsBottle || IsMountArmor || IsMisc)
			{
				return false;
			}
			
			uint newItemId = (uint)DbItem.Id;
			
			while ((newItemId % 10) > 5)
			{
				newItemId--;
			}
			
			newItemId += (uint)(qualityValue - 5);
			
			if (!Collections.ItemCollection.Contains(newItemId))
			{
				return false;
			}
			
			var newItem = Collections.ItemCollection.CreateItemById(newItemId);
			if (newItem == null)
			{
				return false;
			}
			
			if (newItem.DbItem.Name != DbItem.Name)
			{
				return false;
			}
			
			DbItem = newItem.DbItem;
			if (DbOwnerItem == null)
			{
				DbOwnerItem = new Database.Models.DbOwnerItem();
				DbOwnerItem.MaxDura = DbItem.Amount;
				DbOwnerItem.CurrentDura = DbOwnerItem.MaxDura;
				Gem1 = Enums.Gem.NoSocket;
				Gem2 = Enums.Gem.NoSocket;
			}
			DbOwnerItem.ItemId = (uint)DbItem.Id;
			
			if (owner != null)
			{
				UpdateClient(owner, Enums.ItemUpdateAction.Update);
				owner.UpdateBaseStats();
			}
			
			return true;
		}
	}
}
