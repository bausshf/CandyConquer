// Project by Bauss
using System;
using CandyConquer.Database.Models;
using CandyConquer.WorldApi.Models.Maps;

namespace CandyConquer.WorldApi.Models.Entities
{
	/// <summary>
	/// Model for a npc.
	/// </summary>
	public class Npc : Controllers.Entities.NpcController, IEntity, Models.Maps.IMapObject
	{
		/// <summary>
		/// The database model for the npc.
		/// </summary>
		private DbNpc _dbNpc;
		
		/// <summary>
		/// Creates a new npc.
		/// </summary>
		/// <param name="dbNpc">The database model tied to it.</param>
		public Npc(DbNpc dbNpc)
		{
			_dbNpc = dbNpc;
			
			_clientId = _dbNpc.NpcId;
			_x = _dbNpc.X;
			_y = _dbNpc.Y;
			
			// Sets controllers
			Npc = this;
			MapObject = this;
			Entity = this;
		}
		
		#region Npc
		/// <summary>
		/// The mesh of the npc.
		/// </summary>
		public ushort Mesh
		{
			get { return _dbNpc.Mesh; }
		}
		
		/// <summary>
		/// The flag of the npc.
		/// </summary>
		public uint Flag
		{
			get { return _dbNpc.Flag; }
		}
		
		/// <summary>
		/// The avatar of the npc.
		/// </summary>
		public byte Avatar
		{
			get { return _dbNpc.Avatar; }
		}
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
				throw new InvalidOperationException("Do not set the client id. The set property is there for interface compatibility only.");
			}
		}
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name
		{
			get { return _dbNpc.Name; }
			set
			{
				_dbNpc.Name = value;
				_dbNpc.Update();
				UpdateScreen(true);
			}
		}
		
		/// <summary>
		/// Gets or sets the level.
		/// </summary>
		public byte Level
		{
			get { return byte.MaxValue; }
			set
			{
				throw new InvalidOperationException("Do not set the level. The set property is there for interface compatibility only.");
			}
		}
		#endregion
		
		#region IMapObject
		/// <summary>
		/// Gets or sets the idle state of the npc.
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
			get { return Map == null ? 0 : Map.Id; }
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
		/// Gets or sets a boolean whether the npc is alive or not.
		/// This should not be used in general and is there for interface compatibility only.
		/// </summary>
		public bool Alive { get; set; }
		
		/// <summary>
		/// Gets the spawn packet.
		/// </summary>
		/// <returns>The spawn packet.</returns>
		public byte[] GetSpawnPacket()
		{
			return GetNpcSpawnPacket();
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
		/// Gets a boolean determining whether the npc can update its screen.
		/// </summary>
		public bool CanUpdateScreen
		{
			get { return ClientId != 0; }
		}
		#endregion
	}
}
