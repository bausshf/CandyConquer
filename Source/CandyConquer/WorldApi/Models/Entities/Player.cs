// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CandyConquer.ApiServer;
using CandyConquer.Database.Models;
using CandyConquer.Security.Cryptography.World;
using CandyConquer.WorldApi.Models.Maps;
using CandyConquer.Drivers.Repositories.Collections;

namespace CandyConquer.WorldApi.Models.Entities
{
	/// <summary>
	/// Model for a player.
	/// </summary>
	public class Player : Controllers.Entities.PlayerController, IAttackableEntity, IEntity, Models.Maps.IMapObject
	{
		/// <summary>
		/// Creates a new player.
		/// </summary>
		/// <param name="clientSocket">The socket to associate with it.</param>
		public Player(Client<Player> clientSocket)
			: base()
		{
			// Sets controllers
			Player = this;
			AttackableEntity = this;
			Entity = this;
			MapObject = this;
			
			// Default data ...
			ClientSocket = clientSocket;
			Exchanged = false;
			Action = Enums.PlayerAction.None;
			Direction = Enums.Direction.SouthWest;
			Alive = true;
			PKMode = Enums.PKMode.Capture;
			Inventory = new Collections.Inventory(this);
			Equipments = new Collections.Equipments(this);
			Trade = new Models.Misc.TradeData();
			Spells = new Collections.SpellCollection(this);
			Houses = new Collections.PlayerHouseCollection(this);
			Warehouses = new Collections.WarehouseCollection(this);
			MapChangeEvents = new ConcurrentQueue<Action<Player>>();
			MaskedSkills = new ConcurrentHashSet<ushort>();
			CanAttack = true;
		}
		
		#region Internal System
		/// <summary>
		/// Gets the client socket associated with the player.
		/// </summary>
		public Client<Player> ClientSocket { get; private set; }
		/// <summary>
		/// Gets or sets a value indicating whether the keys have been exchanged or not.
		/// </summary>
		public bool Exchanged { get; set; }
		/// <summary>
		/// Gets or sets the key exchange.
		/// </summary>
		public DHKeyExchange KeyExchange { get; set; }
		/// <summary>
		/// Gets or sets the database row for the player.
		/// Do not update any members of DbPlayer directly.
		/// Use the properties available here.
		/// </summary>
		public DbPlayer DbPlayer { get; set; }
		
		/// <summary>
		/// Boolean indicating whether client has logged in or not.
		/// </summary>
		private bool _loggedIn;
		/// <summary>
		/// Gets or sets a value indicating whether the client has logged in or not.
		/// </summary>
		public bool LoggedIn
		{
			get { return _loggedIn; }
			set
			{
				_loggedIn = value;
				DbPlayer.CanWrite = _loggedIn;
			}
		}
		#endregion
		
		#region Player
		public ConcurrentHashSet<ushort> MaskedSkills { get; private set; }
		
		/// <summary>
		/// Gets or sets the tournament team of the player.
		/// </summary>
		public Models.Tournaments.TournamentTeam TournamentTeam { get; set; }
		
		/// <summary>
		/// Gets or sets the arena information of the player.
		/// </summary>
		public Models.Arena.ArenaInfo ArenaInfo { get; set; }
		
		/// <summary>
		/// Gets a queue of events to fire when the player changes map.
		/// </summary>
		public ConcurrentQueue<Action<Player>> MapChangeEvents { get; private set; }
		
		/// <summary>
		/// The static status flag.
		/// </summary>
		private Enums.StatusFlag _staticStatusFlag;
		
		/// <summary>
		/// Gets or sets the static status flag of the player.
		/// </summary>
		public Enums.StatusFlag StaticStatusFlag
		{
			get { return _staticStatusFlag; }
			set
			{
				_staticStatusFlag = value;
				
				DbPlayer.StatusFlag = _staticStatusFlag.ToString();
				DbPlayer.Update();
				
				AddStatusFlag(_staticStatusFlag);
			}
		}
		
		/// <summary>
		/// Gets or sets the current battle controller for the player.
		/// </summary>
		public Controllers.Battle.BattleController Battle { get; set; }
		
		/// <summary>
		/// Gets or sets the team of the player.
		/// </summary>
		public Collections.TeamCollection Team { get; set; }
		
		/// <summary>
		/// Gets or sets the alliance guild name.
		/// </summary>
		public string AllianceGuildName { get; set; }
		
		/// <summary>
		/// Gets or sets the apply guild member client id.
		/// </summary>
		public uint ApplyGuildMemberClientId { get; set; }
		
		/// <summary>
		/// Gets or sets the npc input data.
		/// </summary>
		public string NpcInputData { get; set; }
		
		/// <summary>
		/// Gets or sets the current npc.
		/// </summary>
		public Models.Entities.Npc CurrentNpc { get; set; }
		
		/// <summary>
		/// Gets the spells of the player.
		/// </summary>
		public Collections.SpellCollection Spells { get; private set; }
		
		/// <summary>
		/// Gets the trade data.
		/// </summary>
		public Models.Misc.TradeData Trade { get; private set; }
		
		/// <summary>
		/// Gets the player's warehouses.
		/// </summary>
		public Collections.WarehouseCollection Warehouses { get; private set; }
		
		/// <summary>
		/// Gets or sets the guild.
		/// </summary>
		public Models.Guilds.Guild Guild { get; set; }
		
		/// <summary>
		/// Gets or sets the guild member info.
		/// </summary>
		public Models.Guilds.GuildMember GuildMember { get; set; }
		
		/// <summary>
		/// Gets or sets the nobility of the player.
		/// </summary>
		public Models.Nobility.NobilityDonation Nobility { get; set; }
		
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
				
				UpdateClient(Enums.UpdateClientType.StatusEffect, _statusflag);
				UpdateScreen(true);
			}
		}
		
		/// <summary>
		/// Gets a boolean determining whether the player is female or not.
		/// </summary>
		public bool IsFemale
		{
			get { return Model == 2001 || Model == 2002; }
		}
		
		/// <summary>
		/// Gets a boolean determining whether the player is male or not.
		/// </summary>
		public bool IsMale
		{
			get { return Model == 1003 || Model == 1004; }
		}
		
		/// <summary>
		/// The mesh.
		/// </summary>
		private uint _mesh;
		/// <summary>
		/// Gets the mesh.
		/// </summary>
		public uint Mesh
		{
			get { return _mesh; }
			private set
			{
				_mesh = value;
				
				if (ArenaInfo != null)
				{
					ArenaInfo.DbPlayerArenaQualifier.Mesh = value;
					ArenaInfo.DbPlayerArenaQualifier.Update();
				}
				
				UpdateClient(Enums.UpdateClientType.Mesh, _mesh);
				UpdateScreen(true);
			}
		}
		/// <summary>
		/// Gets or sets the model.
		/// </summary>
		public ushort Model
		{
			get { return DbPlayer.Model.Value; }
			set
			{
				DbPlayer.Model = value;
				DbPlayer.Update();
				
				Mesh = (uint)(TransformModel * 10000000 + Avatar * 10000 + DbPlayer.Model);
			}
		}
		/// <summary>
		/// The transform model.
		/// </summary>
		private ushort _transformModel;
		/// <summary>
		/// Gets or sets the transform model.
		/// </summary>
		public ushort TransformModel
		{
			get { return _transformModel; }
			set
			{
				_transformModel = value;
				Mesh = (uint)(_transformModel * 10000000 + Avatar * 10000 + Model);
			}
		}
		/// <summary>
		/// Gets or sets the avatar.
		/// </summary>
		public ushort Avatar
		{
			get { return DbPlayer.Avatar.Value; }
			set
			{
				DbPlayer.Avatar = value;
				DbPlayer.Update();
				
				Mesh = (uint)(TransformModel * 10000000 + DbPlayer.Avatar * 10000 + Model);
			}
		}
		/// <summary>
		/// Gets or sets the hair.
		/// </summary>
		public ushort Hair
		{
			get { return DbPlayer.Hair.Value; }
			set
			{
				DbPlayer.Hair = value;
				DbPlayer.Update();
				UpdateClient(Enums.UpdateClientType.Hair, DbPlayer.Hair.Value);
				UpdateScreen(true);
			}
		}
		/// <summary>
		/// Gets or sets the money.
		/// </summary>
		public uint Money
		{
			get { return DbPlayer.Money.Value; }
			set
			{
				value = Math.Min(Data.Constants.GameMode.MaxMoney, value);
				
				DbPlayer.Money = value;
				DbPlayer.Update();
				UpdateClient(Enums.UpdateClientType.Money, DbPlayer.Money.Value);
			}
		}
		
		/// <summary>
		/// Gets or sets amount of money in the warehouse.
		/// </summary>
		public uint WarehouseMoney
		{
			get { return DbPlayer.WHMoney; }
			set
			{
				value = Math.Min(Data.Constants.GameMode.MaxMoney, value);
				
				DbPlayer.WHMoney = value;
				DbPlayer.Update();
				
				UpdateClient(Enums.UpdateClientType.WarehouseMoney, DbPlayer.WHMoney);
			}
		}
		
		/// <summary>
		/// Gets or sets the CPs.
		/// </summary>
		public uint CPs
		{
			get { return DbPlayer.CPs.Value; }
			set
			{
				value = Math.Min(Data.Constants.GameMode.MaxMoney, value);
				
				DbPlayer.CPs = value;
				DbPlayer.Update();
				UpdateClient(Enums.UpdateClientType.CPs, DbPlayer.CPs.Value);
			}
		}
		/// <summary>
		/// Gets or sets the experience.
		/// </summary>
		public ulong Experience
		{
			get { return DbPlayer.Experience.Value; }
			set
			{
				DbPlayer.Experience = value;
				DbPlayer.Update();
				UpdateClient(Enums.UpdateClientType.Experience, DbPlayer.Experience.Value);
			}
		}
		/// <summary>
		/// Gets or sets the attribute points.
		/// </summary>
		public ushort AttributePoints
		{
			get { return DbPlayer.AttributePoints.Value; }
			set
			{
				value = Math.Min(Data.Constants.GameMode.MaxAttributePoints, value);
				
				DbPlayer.AttributePoints = value;
				DbPlayer.Update();
				UpdateClient(Enums.UpdateClientType.AttributePoints, DbPlayer.AttributePoints.Value);
			}
		}
		/// <summary>
		/// Gets or sets the pk points.
		/// </summary>
		public short PKPoints
		{
			get { return DbPlayer.PKPoints.Value; }
			set
			{
				value = Math.Min(Data.Constants.GameMode.MaxPkPoints, value);
				value = Math.Max((short)0, value);
				
				DbPlayer.PKPoints = value;
				DbPlayer.Update();
				UpdateClient(Enums.UpdateClientType.PKPoints, DbPlayer.PKPoints.Value);
				
				if (value >= 100)
				{
					AddStatusFlag(Enums.StatusFlag.BlackName);
				}
				else if (value >= 30)
				{
					AddStatusFlag(Enums.StatusFlag.RedName);
				}
				else
				{
					RemoveStatusFlag(Enums.StatusFlag.BlackName);
					RemoveStatusFlag(Enums.StatusFlag.RedName);
				}
			}
		}
		
		/// <summary>
		/// The job.
		/// </summary>
		private Enums.Job _job;
		/// <summary>
		/// Gets or sets the job.
		/// </summary>
		public Enums.Job Job
		{
			get { return _job; }
			set
			{
				_job = value;
				DbPlayer.Job = _job.ToString();
				DbPlayer.Update();
				
				if (ArenaInfo != null)
				{
					ArenaInfo.DbPlayerArenaQualifier.Job = _job.ToString();
					ArenaInfo.DbPlayerArenaQualifier.Update();
				}
				
				UpdateClient(Enums.UpdateClientType.Job,(byte)_job);
			}
		}
		/// <summary>
		/// The title.
		/// </summary>
		private Enums.PlayerTitle _title;
		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		public Enums.PlayerTitle Title
		{
			get { return _title; }
			set
			{
				_title = value;
				DbPlayer.Title = _title.ToString();
				DbPlayer.Update();
			}
		}
		/// <summary>
		/// Gets or sets the spouse.
		/// </summary>
		public string Spouse
		{
			get { return DbPlayer.Spouse; }
			set
			{
				DbPlayer.Spouse = value;
				DbPlayer.Update();
			}
		}
		
		/// <summary>
		/// Gets or sets the pending spouse.
		/// </summary>
		public uint PendingSpouse { get; set; }
		
		/// <summary>
		/// Gets or sets the reborns.
		/// </summary>
		public byte Reborns
		{
			get { return (byte)(DbPlayer.Reborns.HasValue ? DbPlayer.Reborns.Value : 0); }
			set
			{
				value = Math.Min(Data.Constants.GameMode.MaxReborns, value);
				
				DbPlayer.Reborns = value;
				DbPlayer.Update();
				UpdateClient(Enums.UpdateClientType.Reborn, DbPlayer.Reborns.Value);
				UpdateBaseStats();
			}
		}
		
		/// <summary>
		/// Gets or sets the current action of the player.
		/// </summary>
		public Enums.PlayerAction Action
		{
			get; set;
		}
		
		/// <summary>
		/// Gets or sets the last movement mode of the player.
		/// </summary>
		public Enums.WalkMode LastMovementMode
		{
			get; set;
		}
		
		/// <summary>
		/// Gets or sets the last movement time of the player.
		/// </summary>
		public DateTime LastMovementTime
		{
			get; set;
		}
		
		/// <summary>
		/// Gets or sets the dead time.
		/// </summary>
		public DateTime DeadTime { get; set; }
		
		/// <summary>
		/// Gets or sets the amount of speed hack checks done in a row.
		/// </summary>
		public int SpeedHackChecks
		{
			get; set;
		}
		
		/// <summary>
		/// Gets or sets the last stamina update time.
		/// </summary>
		public DateTime LastStaminaUpdateTime
		{
			get; set;
		}
		
		/// <summary>
		/// Gets or sets the bound cps associated with the player.
		/// </summary>
		public uint BoundCPs
		{
			get { return DbPlayer.BoundCPs.Value; }
			set
			{
				value = Math.Min(Data.Constants.GameMode.MaxMoney, value);
				
				DbPlayer.BoundCPs = value;
				DbPlayer.Update();
				UpdateClient(Enums.UpdateClientType.BoundCPs, DbPlayer.BoundCPs.Value);
			}
		}
		
		/// <summary>
		/// The permission of the player.
		/// </summary>
		private Enums.PlayerPermission _permission;
		/// <summary>
		/// Gets or sets the permission of the player.
		/// </summary>
		public Enums.PlayerPermission Permission
		{
			get { return _permission; }
			set
			{
				_permission = value;
				
				if (_permission == Enums.PlayerPermission.Admin ||
				    _permission == Enums.PlayerPermission.PM)
				{
					if (!Name.EndsWith("[PM]"))
					{
						Name += "[PM]";
					}
				}
				else if (_permission == Enums.PlayerPermission.GM)
				{
					if (!Name.EndsWith("[GM]"))
					{
						Name += "[GM]";
					}
				}
				else if (Name.EndsWith("[PM]") || Name.EndsWith("[GM]"))
				{
					Name = Name.Substring(0, Name.Length - 4);
				}
				
				DbPlayer.Permission = _permission.ToString();
				DbPlayer.Update();
			}
		}
		
		/// <summary>
		/// The vip level.
		/// </summary>
		private byte _vipLevel;
		
		/// <summary>
		/// Gets or sets the vip level of the player.
		/// </summary>
		public byte VIPLevel
		{
			get { return _vipLevel; }
			set
			{
				value = (byte)Math.Max(0, (int)value);
				value = (byte)Math.Min(6, (int)value);
				
				_vipLevel = value;
				UpdateClient(Enums.UpdateClientType.VIPLevel, _vipLevel);
			}
		}
		
		/// <summary>
		/// Gets or sets the language of the player.
		/// </summary>
		public string Language
		{
			get { return DbPlayer.Language; }
			set
			{
				DbPlayer.Language = value;
				DbPlayer.Update();
			}
		}
		
		/// <summary>
		/// Gets or sets the pk mode of the player.
		/// </summary>
		public Enums.PKMode PKMode
		{
			get; set;
		}
		
		/// <summary>
		/// The stamina of the player.
		/// </summary>
		private byte _stamina;
		/// <summary>
		/// Gets or sets the stamina of the player.
		/// </summary>
		public byte Stamina
		{
			get { return _stamina; }
			set
			{
				_stamina = value;
				UpdateClient(Enums.UpdateClientType.Stamina, _stamina);
			}
		}
		
		/// <summary>
		/// Gets the inventory.
		/// </summary>
		public Collections.Inventory Inventory
		{
			get; private set;
		}
		
		/// <summary>
		/// Gets the equipments.
		/// </summary>
		public Collections.Equipments Equipments
		{
			get; private set;
		}
		
		/// <summary>
		/// Gets or sets the time when the login protection ends.
		/// </summary>
		public DateTime LoginProtectionEndTime { get; set; }
		
		/// <summary>
		/// Gets or sets the time when the revival protection ends.
		/// </summary>
		public DateTime ReviveProtectionEndTime { get; set; }
		
		/// <summary>
		/// Gets or sets the next allowed time for an attack.
		/// </summary>
		public DateTime NextAllowedAttackTime { get; set; }
		
		/// <summary>
		/// Gets or sets the time allowed for revival.
		/// </summary>
		public DateTime ReviveTime { get; set; }
		
		/// <summary>
		/// Gets or sets the attack packet of the player. (Used for auto attacks.)
		/// </summary>
		public Models.Packets.Entities.InteractionPacket AttackPacket { get; set; }
		
		/// <summary>
		/// Gets or sets the next time a small long skill is allowed.
		/// </summary>
		public DateTime NextSmallLongSkill { get; set; }
		
		/// <summary>
		/// Gets or sets the next time a long skill is allowed.
		/// </summary>
		public DateTime NextLongSkill { get; set; }
		
		/// <summary>
		/// Gets or sets the next time pk points should be removed.
		/// </summary>
		public DateTime NextPKPointRemoval { get; set; }
		
		/// <summary>
		/// Gets the player houses.
		/// </summary>
		public Collections.PlayerHouseCollection Houses { get; private set; }
		
		/// <summary>
		/// Gets or sets the warehouse password of the player.
		/// </summary>
		public string WarehousePassword
		{
			get { return DbPlayer.WHPassword; }
			set
			{
				DbPlayer.WHPassword = value;
				DbPlayer.Update();
			}
		}
		
		/// <summary>
		/// Gets or sets a boolean whether the warehouse password has been authenticated or not.
		/// </summary>
		public bool WarehouseAuthenticated { get; set; }
		#endregion
		
		#region IEntity
		/// <summary>
		/// Gets or sets the client id.
		/// </summary>
		public uint ClientId { get; set; }
		
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name
		{
			get { return DbPlayer.Name; }
			set
			{
				DbPlayer.Name = value;
				DbPlayer.Update();
				
				if (GuildMember != null)
				{
					GuildMember.DbGuildRank.PlayerName = DbPlayer.Name;
				}
				
				if (ArenaInfo != null)
				{
					ArenaInfo.DbPlayerArenaQualifier.Name = value;
					ArenaInfo.DbPlayerArenaQualifier.Update();
				}
				
				UpdateScreen(true);
			}
		}
		
		/// <summary>
		/// Gets or sets the level.
		/// </summary>
		public byte Level
		{
			get { return (byte)(DbPlayer.Level.HasValue ? DbPlayer.Level.Value : 1); }
			set
			{
				if (DbPlayer.Level == Data.Constants.GameMode.MaxLevel)
				{
					return;
				}
				
				value = Math.Min(Data.Constants.GameMode.MaxLevel, value);
				
				DbPlayer.Level = value;
				DbPlayer.Update();
				
				UpdateStats();
				UpdateClient(Enums.UpdateClientType.Level, DbPlayer.Level.Value);
				
				if (GuildMember != null)
				{
					GuildMember.DbGuildRank.PlayerLevel = DbPlayer.Level.Value;
				}
				
				if (ArenaInfo != null)
				{
					ArenaInfo.DbPlayerArenaQualifier.Level = value;
					ArenaInfo.DbPlayerArenaQualifier.Update();
				}
				
				UpdateScreen(true); // levelup packet ...
			}
		}
		#endregion
		
		#region IAttackableEntity
		// Level is under IEntity ...
		
		/// <summary>
		/// Gets or sets the poison effect of the player.
		/// </summary>
		public int PoisonEffect { get; set; }
		
		/// <summary>
		/// Gets or sets the next poison time.
		/// </summary>
		public DateTime NextPoison { get; set; }
		
		/// <summary>
		/// Gets or sets the target of the player.
		/// </summary>
		public IAttackableEntity Target { get; set; }
		
		/// <summary>
		/// Gets or sets a boolean determining whether the player can attack or not.
		/// </summary>
		public bool CanAttack { get; set; }
		
		/// <summary>
		/// Gets or sets the strength.
		/// </summary>
		public ushort Strength
		{
			get { return DbPlayer.Strength.Value; }
			set
			{
				value = Math.Min(Data.Constants.GameMode.MaxAttributePoints, value);
				
				DbPlayer.Strength = value;
				DbPlayer.Update();
				
				UpdateBaseStats();
				UpdateClient(Enums.UpdateClientType.Strength, DbPlayer.Strength.Value);
			}
		}
		
		/// <summary>
		/// Gets or sets the vitality.
		/// </summary>
		public ushort Vitality
		{
			get { return DbPlayer.Vitality.Value; }
			set
			{
				value = Math.Min(Data.Constants.GameMode.MaxAttributePoints, value);
				
				DbPlayer.Vitality = value;
				DbPlayer.Update();
				
				UpdateBaseStats();
				UpdateClient(Enums.UpdateClientType.Vitality, DbPlayer.Vitality.Value);
			}
		}
		
		/// <summary>
		/// Gets or sets the agility.
		/// </summary>
		public ushort Agility
		{
			get { return DbPlayer.Agility.Value; }
			set
			{
				value = Math.Min(Data.Constants.GameMode.MaxAttributePoints, value);
				
				DbPlayer.Agility = value;
				DbPlayer.Update();
				
				UpdateBaseStats();
				UpdateClient(Enums.UpdateClientType.Agility, DbPlayer.Agility.Value);
			}
		}
		
		/// <summary>
		/// Gets or sets the spirit.
		/// </summary>
		public ushort Spirit
		{
			get { return DbPlayer.Spirit.Value; }
			set
			{
				value = Math.Min(Data.Constants.GameMode.MaxAttributePoints, value);
				
				DbPlayer.Spirit = value;
				DbPlayer.Update();
				
				UpdateBaseStats();
				UpdateClient(Enums.UpdateClientType.Spirit, DbPlayer.Spirit.Value);
			}
		}
		
		/// <summary>
		/// The max hp.
		/// </summary>
		private int _maxHP;
		/// <summary>
		/// Gets or sets the max hp.
		/// </summary>
		public int MaxHP
		{
			get { return _maxHP; }
			set
			{
				value = Math.Max(0, value);
				
				_maxHP = value;
				UpdateClient(Enums.UpdateClientType.MaxHP, _maxHP);
			}
		}
		
		/// <summary>
		/// Gets or sets the hp.
		/// </summary>
		public int HP
		{
			get { return DbPlayer.HP.Value; }
			set
			{
				value = Math.Max(0, value);
				value = Math.Min(MaxHP, value);
				
				DbPlayer.HP = value;
				DbPlayer.Update();
				UpdateClient(Enums.UpdateClientType.HP, DbPlayer.HP.Value);
			}
		}
		
		/// <summary>
		/// The max mp.
		/// </summary>
		private int _maxMP;
		/// <summary>
		/// Gets or sets the max mp.
		/// </summary>
		public int MaxMP
		{
			get { return _maxMP; }
			set
			{
				value = Math.Max(0, value);
				
				_maxMP = value;
				UpdateClient(Enums.UpdateClientType.MaxMP, _maxMP);
			}
		}
		
		/// <summary>
		/// Gets or sets the mp.
		/// </summary>
		public int MP
		{
			get { return DbPlayer.MP.Value; }
			set
			{
				value = Math.Max(0, value);
				value = Math.Min(MaxMP, value);
				
				DbPlayer.MP = value;
				DbPlayer.Update();
				UpdateClient(Enums.UpdateClientType.MP, DbPlayer.MP.Value);
			}
		}
		
		/// <summary>
		/// Gets or sets a value indicating whether the player is alive or not.
		/// </summary>
		public bool Alive
		{
			get; set;
		}
		#endregion
		
		#region IMapObject
		/// <summary>
		/// Gets or sets the idle state of the player.
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

				if (_map == null)
				{
					return;
				}

				DbPlayer.MapId = _map.Id;

				if (!_map.IsDynamic)
				{
					DbPlayer.Update();
				}
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
		/// Gets or sets the x coordinate,
		/// </summary>
		public ushort X
		{
			get { return DbPlayer.X.Value; }
			set
			{
				_lastX = DbPlayer.X.Value;
				DbPlayer.X = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the y coordinate.
		/// </summary>
		public ushort Y
		{
			get { return DbPlayer.Y.Value; }
			set
			{
				_lastY = DbPlayer.Y.Value;
				DbPlayer.Y = value;
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
				DbPlayer.LastMapId = _lastMap.Id;

				DbPlayer.Update();
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
		/// Gets or sets the last map x coordinate.
		/// </summary>
		public  ushort LastMapX
		{
			get { return DbPlayer.LastMapX.Value; }
			set
			{
				DbPlayer.LastMapX = value;
				DbPlayer.Update();
			}
		}
		
		/// <summary>
		/// Gets or sets the last map y coordinate.
		/// </summary>
		public  ushort LastMapY
		{
			get { return DbPlayer.LastMapY.Value; }
			set
			{
				DbPlayer.LastMapY = value;
				DbPlayer.Update();
			}
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
		/// Gets a boolean determining whether the screen of the player can be updated.
		/// </summary>
		public bool CanUpdateScreen
		{
			get { return LoggedIn; }
		}
		#endregion
		
		#region Combat calculations properties
		/// <summary>
		/// Gets or sets the bonus mp.
		/// </summary>
		public int BonusMP { get; set; }
		
		/// <summary>
		/// Gets or sets the hp
		/// </summary>
		public int BonusHP { get; set; }
		
		/// <summary>
		/// Gets or sets the minimum attack.
		/// </summary>
		public int MinAttack { get; set; }
		
		/// <summary>
		/// Gets or sets the maximum attack.
		/// </summary>
		public int MaxAttack { get; set; }
		
		/// <summary>
		/// Gets or sets the defense.
		/// </summary>
		public int Defense { get; set; }
		
		/// <summary>
		/// Gets or sets the magic attack.
		/// </summary>
		public int MagicAttack { get; set; }
		
		/// <summary>
		/// Gets or sets the magic defense.
		/// </summary>
		public int MagicDefense { get; set; }
		
		/// <summary>
		/// The dodge.
		/// </summary>
		private int _dodge;
		/// <summary>
		/// Gets or sets the dodge.
		/// </summary>
		public int Dodge
		{
			get { return _dodge; }
			set
			{
				value = Math.Min(99, value);
				_dodge = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the accuracy.
		/// </summary>
		public int Accuracy { get; set; }
		
		/// <summary>
		/// Gets or sets the magic defense percentage.
		/// </summary>
		public double MagicDefensePercentage { get; set; }
		
		/// <summary>
		/// Gets or sets the bless.
		/// </summary>
		public double Bless { get; set; }
		
		/// <summary>
		/// Gets or sets the final physical damage.
		/// </summary>
		public int FinalPhysicalDamage { get; set; }
		
		/// <summary>
		/// Gets or sets the final magic damage.
		/// </summary>
		public int FinalMagicDamage { get; set; }
		
		/// <summary>
		/// Gets or sets the final physical defense.
		/// </summary>
		public int FinalPhysicalDefense { get; set; }
		
		/// <summary>
		/// Gets or sets the final magic defense.
		/// </summary>
		public int FinalMagicDefense { get; set; }
		
		/// <summary>
		/// Gets or sets the critical strike.
		/// </summary>
		public int CriticalStrike { get; set; }
		
		/// <summary>
		/// Gets or sets the block.
		/// </summary>
		public int Block { get; set; }
		
		/// <summary>
		/// Gets or sets the breakthrough.
		/// </summary>
		public int BreakThrough { get; set; }
		
		/// <summary>
		/// Gets or sets the counter action.
		/// </summary>
		public int CounterAction { get; set; }
		
		/// <summary>
		/// Gets or sets the skill critical strike.
		/// </summary>
		public int SkillCriticalStrike { get; set; }
		
		/// <summary>
		/// Gets or sets the immunity.
		/// </summary>
		public int Immunity { get; set; }
		
		/// <summary>
		/// Gets or sets the penetration.
		/// </summary>
		public int Penetration { get; set; }
		
		/// <summary>
		/// Gets or sets the detoxication.
		/// </summary>
		public int Detoxication { get; set; }
		
		/// <summary>
		/// Gets or sets the metal defense.
		/// </summary>
		public int MetalDefense { get; set; }
		
		/// <summary>
		/// Gets or sets the wood defense.
		/// </summary>
		public int WoodDefense { get; set; }
		
		/// <summary>
		/// Gets or sets the water defense.
		/// </summary>
		public int WaterDefense { get; set; }
		
		/// <summary>
		/// Gets or sets the fire defense.
		/// </summary>
		public int FireDefense { get; set; }
		
		/// <summary>
		/// Gets or sets the earth defense.
		/// </summary>
		public int EarthDefense { get; set; }
		
		/// <summary>
		/// Gets or sets the dragon gem percentage.
		/// </summary>
		public double DragonGemPercentage { get; set; }
		
		/// <summary>
		/// Gets or sets the rainbow gem percentage.
		/// </summary>
		public double RainbowGemPercentage { get; set; }
		
		/// <summary>
		/// Gets or sets the phoenix gem percentage.
		/// </summary>
		public double PhoenixGemPercentage { get; set; }
		
		/// <summary>
		/// Gets or sets the tortoise gem percentage.
		/// </summary>
		public double TortoiseGemPercentage { get; set; }
		
		/// <summary>
		/// Gets or sets the violet gem percentage.
		/// </summary>
		public double VioletGemPercentage { get; set; }
		
		/// <summary>
		/// Gets or sets the moon gem percentage.
		/// </summary>
		public double MoonGemPercentage { get; set; }
		#endregion
		
		/// <summary>
		/// "Frees" any possible "future lingering" memory.
		/// </summary>
		/// <remarks>Do not call this manually. Only called from disconnect.</remarks>
		public void Free()
		{
			Controllers.Arena.ArenaQualifierController.RemovePlayer(this);
			
			Task.Run(async() => await FreeAsync());
		}
		
		/// <summary>
		/// "Frees" any possible "future lingering" memory asynchronously.
		/// </summary>
		private async Task FreeAsync()
		{
			await Task.Delay(2000);
			
			MapChangeEvents = null;
			Inventory = null;
			Equipments = null;
			Trade = null;
			Warehouses = null;
			Target = null;
			AttackPacket = null;
			Player = null;
			AttackableEntity = null;
			Entity = null;
			MapObject = null;
			ClientSocket = null;
		}
	}
}
