// Project by Bauss
using System;
using CandyConquer.Drivers;
using CandyConquer.Drivers.Repositories.Collections;
using CandyConquer.ApiServer;
using CandyConquer.Database.Models;
using CandyConquer.Security.Cryptography.World;
using CandyConquer.WorldApi.Models.Maps;

namespace CandyConquer.WorldApi.Models.Entities
{
	/// <summary>
	/// Model for a monster.
	/// </summary>
	public class Monster : Controllers.Entities.MonsterController, IAttackableEntity, IEntity, Models.Maps.IMapObject
	{
		/// <summary>
		/// The database monster.
		/// </summary>
		private Database.Models.DbMonster _dbMonster;
		
		/// <summary>
		/// Creates a new Monster.
		/// </summary>
		/// <param name="dbMonster">The database monster to associate with it.</param>
		public Monster(Database.Models.DbMonster dbMonster)
			: base()
		{
			// Sets controllers
			Monster = this;
			AttackableEntity = this;
			Entity = this;
			MapObject = this;
			
			// Default data ...
			Direction = Enums.Direction.SouthWest;
			Alive = true;
			_dbMonster = dbMonster;
			_clientId = Drivers.Repositories.Safe.IdentityGenerator.GetMonsterId();
			Behaviour = _dbMonster.Behaviour.ToEnum<Enums.MonsterBehaviour>();
			ExtraExperience = (ulong)_dbMonster.ExtraExperience;
			Boss = _dbMonster.Boss != 0;
			Controllers.Threads.MonsterThreadController.HandleMonster(this);
			CanAttack = true;
			Direction = (Enums.Direction)Drivers.Repositories.Safe.Random.NextEnum(typeof(Enums.Direction));
			HP = MaxHP;
			MP = MaxMP;
			Dodge = Math.Min(99, _dbMonster.Dodge);
			IsGuard = ((int)Behaviour) >= 3;
			IsWalking = true;
			Idle = !IsGuard;
		}
		
		#region Monster
		/// <summary>
		/// Gets the id of the monster.
		/// </summary>
		public int Id { get { return _dbMonster.MobId; } }
		
		/// <summary>
		/// Gets the mesh of the monster.
		/// </summary>
		public ushort Mesh { get { return _dbMonster.Mesh; } }
		
		/// <summary>
		/// Gets the minimum attack of the monster.
		/// </summary>
		public int MinAttack { get { return _dbMonster.MinAttack; } }
		
		/// <summary>
		/// Gets the max attack of the monster.
		/// </summary>
		public int MaxAttack { get { return _dbMonster.MaxAttack; } }
		
		/// <summary>
		/// Gets the defense of the monster.
		/// </summary>
		public int Defense { get { return _dbMonster.Defense; } }
		
		/// <summary>
		/// Gets the dexterity of the monster.
		/// </summary>
		public int Dexterity { get { return _dbMonster.Dexterity; } }
		
		/// <summary>
		/// Gets the dodge of the monster.
		/// </summary>
		public int Dodge { get; private set; }
		
		/// <summary>
		/// Gets the attack range of the monster.
		/// </summary>
		public int AttackRange { get { return _dbMonster.AttackRange; } }
		
		/// <summary>
		/// Gets the view range of the monster.
		/// </summary>
		public int ViewRange { get { return _dbMonster.ViewRange; } }
		
		/// <summary>
		/// Gets the attack speed of the monster.
		/// </summary>
		public int AttackSpeed { get { return _dbMonster.AttackSpeed; } }
		
		/// <summary>
		/// Gets the move speed of the monster.
		/// </summary>
		public int MoveSpeed { get { return _dbMonster.MoveSpeed; } }
		
		/// <summary>
		/// Gets the attack type of the monster.
		/// </summary>
		public int AttackType { get { return _dbMonster.AttackType; } }
		
		/// <summary>
		/// Gets the behaviour of the monster.
		/// </summary>
		public Enums.MonsterBehaviour Behaviour { get; private set; }
		
		/// <summary>
		/// Gets the magic type of the monster.
		/// </summary>
		public int MagicType { get { return _dbMonster.MagicType; } }
		
		/// <summary>
		/// Gets the magic defense of the monster.
		/// </summary>
		public int MagicDefense { get { return _dbMonster.MagicDefense; } }
		
		/// <summary>
		/// Gets the magic hit rate of the monster.
		/// </summary>
		public int MagicHitRate { get { return _dbMonster.MagicHitRate; } }
		
		/// <summary>
		/// Gets the extra experience of the monster.
		/// </summary>
		public ulong ExtraExperience { get; private set; }
		
		/// <summary>
		/// Gets the extra damage of the monster.
		/// </summary>
		public int ExtraDamage { get { return _dbMonster.ExtraDamage; } }
		
		/// <summary>
		/// Gets a boolean indicating whether the monster is a boss or not.
		/// </summary>
		public bool Boss { get; private set; }
		
		/// <summary>
		/// Gets the action of the monster.
		/// </summary>
		public int Action { get { return _dbMonster.Action; } }
		
		/// <summary>
		/// Gets or sets the next movement time of the monster.
		/// </summary>
		public DateTime NextMoveTime { get; set; }
		
		/// <summary>
		/// Gets or sets the next attack time of the monster.
		/// </summary>
		public DateTime NextAttackTime { get; set; }
		
		/// <summary>
		/// Gets the original map id of the monster.
		/// </summary>
		public int OriginalMapId { get; set; }
		
		/// <summary>
		/// Gets the original x coordinate of the monster.
		/// </summary>
		public ushort OriginalX { get; set; }
		
		/// <summary>
		/// Gets the original y coordinate of the monster.
		/// </summary>
		public ushort OriginalY { get; set; }
		
		/// <summary>
		/// Gets the original range size of the monster.
		/// </summary>
		public int OriginalRangeSize { get; set; }
		
		/// <summary>
		/// Gets or sets a boolean whether the monster is a guard or not.
		/// </summary>
		public bool IsGuard { get; private set; }
		
		/// <summary>
		/// Gets or sets a boolean whether the monster can walk or not.
		/// </summary>
		public bool IsWalking { get; set; }
		#endregion
		
		#region IEntity
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
				throw new InvalidOperationException("Do not set this property. It's there for interface compatibility only.");
			}
		}
		
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name
		{
			get { return _dbMonster.Name; }
			set
			{
				throw new InvalidOperationException("Do not set this property. It's there for interface compatibility only.");
			}
		}
		
		
		/// <summary>
		/// Gets or sets the level.
		/// </summary>
		public byte Level
		{
			get { return _dbMonster.Level; }
			set
			{
				throw new InvalidOperationException("Do not set this property. It's there for interface compatibility only.");
			}
		}
		#endregion
		
		#region IAttackableEntity
		// Level is under IEntity ...
		
		/// <summary>
		/// Gets or sets the poison effect of the monster.
		/// </summary>
		public int PoisonEffect { get; set; }
		
		/// <summary>
		/// Gets or sets the next poison time.
		/// </summary>
		public DateTime NextPoison { get; set; }
		
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		public IAttackableEntity Target { get; set; }
		
		/// <summary>
		/// Gets or sets the reborns of the monster.
		/// </summary>
		public byte Reborns { get; set; }
		
		/// <summary>
		/// Gets or sets a boolean determining whether the player can attack or not.
		/// </summary>
		public bool CanAttack { get; set; }
		
		/// <summary>
		/// Gets or sets the strength.
		/// </summary>
		public ushort Strength { get; set; }
		
		/// <summary>
		/// Gets or sets the vitality.
		/// </summary>
		public ushort Vitality { get; set; }
		
		/// <summary>
		/// Gets or sets the agility.
		/// </summary>
		public ushort Agility { get; set; }
		
		/// <summary>
		/// Gets or sets the spirit.
		/// </summary>
		public ushort Spirit { get; set; }
		
		/// <summary>
		/// Gets the max hp of the monster.
		/// </summary>
		public int MaxHP
		{
			get { return _dbMonster.HP; }
			set
			{
				throw new InvalidOperationException("Do not set this property. It's there for interface compatibility only.");
			}
		}
		
		/// <summary>
		/// The hp of the monster.
		/// </summary>
		private int _hp;
		
		/// <summary>
		/// Gets or sets the hp.
		/// </summary>
		public int HP
		{
			get { return _hp; }
			set
			{
				value = Math.Min(MaxHP, value);
				_hp = value;
			}
		}
		
		/// <summary>
		/// Gets the max mp of the monster.
		/// </summary>
		public int MaxMP
		{
			get { return _dbMonster.MP; }
			set
			{
				throw new InvalidOperationException("Do not set this property. It's there for interface compatibility only.");
			}
		}
		
		/// <summary>
		/// The mp of the monster.
		/// </summary>
		private int _mp;
		
		/// <summary>
		/// Gets or sets the mp.
		/// </summary>
		public int MP
		{
			get { return _mp; }
			set
			{
				value = Math.Min(MaxMP, value);
				
				_mp = value;
			}
		}
		
		/// <summary>
		/// Gets or sets a value indicating whether the monster is alive or not.
		/// </summary>
		public bool Alive
		{
			get; set;
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
		/// Gets the spawn packet.
		/// </summary>
		/// <returns>The spawn packet.</returns>
		public byte[] GetSpawnPacket()
		{
			return CreateSpawnPacket();
		}
		
		/// <summary>
		/// Gets the removal spawn packet.
		/// </summary>
		/// <returns>The removal spawn packet.</returns>
		public byte[] GetRemoveSpawnPacket()
		{
			return CreateRemoveSpawnPacket();
		}
		
		/// <summary>
		/// Gets a boolean determining whether the item can update its screen.
		/// </summary>
		public bool CanUpdateScreen
		{
			get { return ClientId != 0; }
		}
		#endregion
	}
}
