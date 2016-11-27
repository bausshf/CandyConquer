// Project by Bauss
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CandyConquer.WorldApi.Models.Maps;

namespace CandyConquer.WorldApi.Controllers.Maps
{
	/// <summary>
	/// Controller for map objects.
	/// </summary>
	public class MapObjectController
	{
		/// <summary>
		/// The map object associated with the controller.
		/// </summary>
		public IMapObject MapObject { get; protected set; }
		
		/// <summary>
		/// Collection of current map objects in the screen.
		/// </summary>
		private ConcurrentDictionary<uint, IMapObject> _screenObjects;
		
		/// <summary>
		/// Boolean determining whether the map object has been despawned or not.
		/// </summary>
		private bool _despawned = false;
		
		/// <summary>
		/// Constructor for the controller.
		/// </summary>
		public MapObjectController()
		{
			_screenObjects = new ConcurrentDictionary<uint, IMapObject>();
		}
		
		/// <summary>
		/// Adds a map object to the screen of another map object.
		/// </summary>
		/// <param name="obj">The map object to add.</param>
		/// <param name="player">The player associated to the owner map object, if any.</param>
		/// <param name="spawnPacket">The spawn packet of the owner map object.</param>
		/// <param name="packet">A packet to send to the map object if it's a client.</param>
		private void AddToScreen(IMapObject obj, Models.Entities.Player player, byte[] spawnPacket, byte[] packet, Enums.UpdateScreenFlags flags, bool spawnToOwner)
		{
			if (obj is Models.Entities.Player)
			{
				var objPlayer = (Models.Entities.Player)obj;
				
				if (!objPlayer.LoggedIn)
				{
					return;
				}
				
				objPlayer.ClientSocket.Send(spawnPacket);
				
				if (packet != null)
				{
					if (flags != Enums.UpdateScreenFlags.DeadPlayers ||
					    flags == Enums.UpdateScreenFlags.DeadPlayers &&
					    !objPlayer.Alive)
					{
						objPlayer.ClientSocket.Send(packet);
					}
				}
			}
			
			if (player != null && spawnToOwner)
			{
				player.ClientSocket.Send(obj.GetSpawnPacket());
			}
			
			(obj as MapObjectController)._screenObjects.TryAdd(MapObject.ClientId, MapObject);
			_screenObjects.TryAdd(obj.ClientId, obj);
		}
		
		/// <summary>
		/// Removes a map object from the screen.
		/// </summary>
		/// <param name="obj">The map object to remove.</param>
		/// <param name="player">The player associated to the owner map object.</param>
		/// <param name="removePacket">The remove packet of the owner map object.</param>
		private void RemoveFromScreen(IMapObject obj, Models.Entities.Player player, byte[] removePacket)
		{
			IMapObject robj;
			(obj as MapObjectController)._screenObjects.TryRemove(MapObject.ClientId, out robj);
			_screenObjects.TryRemove(obj.ClientId, out robj);
			if (obj is Models.Entities.Player)
			{
				(obj as Models.Entities.Player).ClientSocket.Send(removePacket);
			}
			if (player != null)
			{
				player.ClientSocket.Send(obj.GetRemoveSpawnPacket());
			}
		}
		
		/// <summary>
		/// Updates the screen for a map object.
		/// </summary>
		/// <param name="clear">Set to true if the screen should be cleared before the update.</param>
		/// <param name="packet">A packet to send to clients within the screen.</param>
		/// <param name="flags">Flags determining the type of update.</param>
		public void UpdateScreen(bool clear, byte[] packet = null, Enums.UpdateScreenFlags flags = Enums.UpdateScreenFlags.None)
		{
			if (!_despawned)
			{
				if (!MapObject.CanUpdateScreen || MapObject.Map == null)
				{
					return;
				}
			}
			else
			{
				_despawned = false;
			}
			
			if (clear)
			{
				ClearScreen();
			}
			
			var removePacket = MapObject.GetRemoveSpawnPacket();
			var spawnPacket = MapObject.GetSpawnPacket();
			var player = (MapObject as Models.Entities.Player);
			
			foreach (var obj in _screenObjects.Values)
			{
				if (obj.Map == null ||MapObject.Map == null || obj.MapId != MapObject.MapId || !InRange(obj))
				{
					RemoveFromScreen(obj, player, removePacket);
				}
				else
				{
					if (flags == Enums.UpdateScreenFlags.Idle)
					{
						obj.Idle = false;
					}
					
					if (packet != null)
					{
						var objPlayer = obj as Models.Entities.Player;
						if (objPlayer != null)
						{
							var match = objPlayer.Battle as Models.Arena.ArenaBattle;
							
							if (match != null && player != null)
							{
								if (match.Watchers.ContainsKey(player.ClientId))
								{
									continue;
								}
							}
							
							if (flags != Enums.UpdateScreenFlags.DeadPlayers ||
							    flags == Enums.UpdateScreenFlags.DeadPlayers &&
							    !objPlayer.Alive)
							{
								objPlayer.ClientSocket.Send(packet);
							}
						}
					}
				}
			}
			
			if (MapObject.Map != null)
			{
				foreach (var obj in MapObject.Map.MapObjects.Values
				         .Where(x => x.ClientId != MapObject.ClientId && !_screenObjects.ContainsKey(x.ClientId) && InRange(x)))
				{
					if (flags == Enums.UpdateScreenFlags.Idle)
					{
						obj.Idle = false;
					}
					
					if (player != null)
					{
						var match = player.Battle as Models.Arena.ArenaBattle;
						bool spawnToOwner = true;
						byte[] sendPacket = packet;
						
						if (match != null)
						{
							if (match.Watchers.ContainsKey(obj.ClientId))
							{
								spawnToOwner = false;
							}
						}
						else
						{
							var objPlayer = obj as Models.Entities.Player;
							if (objPlayer != null)
							{
								match = objPlayer.Battle as Models.Arena.ArenaBattle;
								
								if (match != null)
								{
									if (match.Watchers.ContainsKey(player.ClientId))
									{
										sendPacket = null;
										spawnPacket = null;
									}
								}
							}
						}
						
						AddToScreen(obj, player, spawnPacket, sendPacket, flags, spawnToOwner);
					}
					else
					{
						AddToScreen(obj, player, spawnPacket, packet, flags, true);
					}
				}
			}
		}
		
		/// <summary>
		/// Clears the screen.
		/// </summary>
		/// <remarks>Asynchronous experimental, if proven to not work properly then removed.</remarks>
		public void ClearScreen()
		{
			var removePacket = MapObject.GetRemoveSpawnPacket();
			var player = (MapObject as Models.Entities.Player);
			
			foreach (var obj in _screenObjects.Values)
			{
				RemoveFromScreen(obj, player, removePacket);
			}
			
			_screenObjects.Clear();
		}
		
		/// <summary>
		/// Teleports the map object to its last map.
		/// </summary>
		public void TeleportToLastMap()
		{
			Teleport(MapObject.LastMapId, MapObject.LastMapX, MapObject.LastMapY);
		}
		
		/// <summary>
		/// Teleports the map object to a dynamic map.
		/// </summary>
		/// <param name="dynamicId">The dynamic map id.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public void TeleportDynamic(int dynamicId, ushort x, ushort y)
		{
			DynamicMap map;
			if (Collections.MapCollection.TryGetDynamicMap(dynamicId, out map))
			{
				TeleportDynamic(map, x, y);
			}
		}
		
		/// <summary>
		/// Teleports the map object to a dynamic map.
		/// </summary>
		/// <param name="map">The dynamic map.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public void TeleportDynamic(DynamicMap map, ushort x, ushort y)
		{
			if (MapObject.Map != null)
			{
				if (map.Id == MapObject.MapId && x == MapObject.X && y == MapObject.Y)
				{
					return;
				}
				else if (!MapObject.Map.RemoveFromMap(MapObject))
				{
					return;
				}
			}
			
			if (map == null)
			{
				return;
			}
			
			if (!map.AddToMap(MapObject))
			{
				return;
			}
			
			MapObject.X = x;
			MapObject.Y = y;
			
			if (MapObject is Models.Entities.Player)
			{
				var player = (Models.Entities.Player)MapObject;
				if (player.LoggedIn)
				{
					player.AttackPacket = null;
					uint sendMap = (uint)player.Map.ClientMapId;
					player.ClientSocket.Send(new Models.Packets.Client.DataExchangePacket
					                         {
					                         	ClientId = player.ClientId,
					                         	Data1 = sendMap,
					                         	Timestamp = Drivers.Time.GetSystemTime(),
					                         	ExchangeType = Enums.ExchangeType.Teleport,
					                         	Data3Low = x,
					                         	Data3High = y
					                         });
					player.ClientSocket.Send(new Models.Packets.Client.DataExchangePacket
					                         {
					                         	ClientId = player.ClientId,
					                         	Data1 = sendMap,
					                         	Timestamp = Drivers.Time.GetSystemTime(),
					                         	ExchangeType = Enums.ExchangeType.ChangeMap,
					                         	Data3Low = x,
					                         	Data3High = y
					                         });
					player.ClientSocket.Send(new Models.Packets.Location.MapInfoPacket
					                         {
					                         	Map = player.Map
					                         });
				}
				
				player.DbPlayer.Update();
			}
			
			UpdateScreen(true);
		}
		
		/// <summary>
		/// Teleports to a specific map id.
		/// </summary>
		/// <param name="mapId">The map id.</param>
		public void Teleport(int mapId)
		{
			Models.Maps.Map map = Collections.MapCollection.GetMap(mapId);
			if (map != null)
			{
				Teleport(map);
			}
		}
		
		/// <summary>
		/// Teleports to a specific map's default (revive) point.
		/// </summary>
		/// <param name="map">The map.</param>
		public void Teleport(Map map)
		{
			if (map != null)
			{
				CandyConquer.Database.Models.DbDefaultCoordinate coord;
				if (map.DefaultCoordinates.TryGetValue("Revive", out coord))
				{
					Teleport(coord.TargetMapId, coord.X, coord.Y);
				}
			}
		}
		
		/// <summary>
		/// Teleports to a specific map and coordination.
		/// </summary>
		/// <param name="mapId">The map id.</param>
		/// <param name="x">The x coord.</param>
		/// <param name="y">The y coord.</param>
		/// <param name="pullback">Boolean indicating whether the teleport is a pullback or not.</param>
		public void Teleport(int mapId, ushort x, ushort y, bool pullback = false)
		{
			Models.Maps.Map newMap = null;
			
			if (!pullback)
			{
				if (MapObject.Map != null)
				{
					if (mapId == MapObject.MapId && x == MapObject.X && y == MapObject.Y)
					{
						return;
					}
					else if (!MapObject.Map.RemoveFromMap(MapObject))
					{
						return;
					}
				}
				
				if (mapId != 0)
				{
					newMap = Collections.MapCollection.GetMap(mapId);
					if (newMap == null )
					{
						return;
					}
					
					if (!newMap.AddToMap(MapObject))
					{
						return;
					}
				}
			}
			
			var player = MapObject as Models.Entities.Player;
			if (player != null && player.LoggedIn)
			{
				player.AttackPacket = null;
				uint sendMap = (uint)player.Map.ClientMapId;
				player.ClientSocket.Send(new Models.Packets.Client.DataExchangePacket
				                         {
				                         	ClientId = player.ClientId,
				                         	Data1 = sendMap,
				                         	Timestamp = Drivers.Time.GetSystemTime(),
				                         	ExchangeType = Enums.ExchangeType.Teleport,
				                         	Data3Low = x,
				                         	Data3High = y
				                         });
				player.ClientSocket.Send(new Models.Packets.Client.DataExchangePacket
				                         {
				                         	ClientId = player.ClientId,
				                         	Data1 = sendMap,
				                         	Timestamp = Drivers.Time.GetSystemTime(),
				                         	ExchangeType = Enums.ExchangeType.ChangeMap,
				                         	Data3Low = x,
				                         	Data3High = y
				                         });
				player.ClientSocket.Send(new Models.Packets.Location.MapInfoPacket
				                         {
				                         	Map = player.Map
				                         });
			}
			
			MapObject.X = x;
			MapObject.Y = y;
			
			UpdateScreen(true);
		}
		
		/// <summary>
		/// Checks if a map object is in range with the owner map object.
		/// </summary>
		/// <param name="obj">The map object to compare range with.</param>
		/// <returns>True if the map object is in range.</returns>
		public bool InRange(IMapObject obj)
		{
			return Tools.RangeTools.ValidDistance(obj.X, obj.Y, MapObject.X, MapObject.Y);
		}
		
		/// <summary>
		/// Gets a map object from the screen.
		/// </summary>
		/// <param name="clientId">The client id associated with the map object.</param>
		/// <param name="obj">The map object from the screen.</param>
		/// <returns>True if the map object was found in the screen.</returns>
		public bool GetFromScreen(uint clientId, out IMapObject obj)
		{
			return _screenObjects.TryGetValue(clientId, out obj);
		}
		
		/// <summary>
		/// Checks whether the map object has a specific map object in the screen.
		/// </summary>
		/// <param name="clientId">The client id of the map object to validate.</param>
		/// <returns>True if the map object is in the screen.</returns>
		public bool ContainsInScreen(uint clientId)
		{
			return _screenObjects.ContainsKey(clientId);
		}
		
		/// <summary>
		/// Pulls the current map object back.
		/// </summary>
		public void Pullback()
		{
			Teleport(MapObject.MapId, MapObject.X, MapObject.Y, true);
		}
		
		/// <summary>
		/// Resets the local of a map object asynchronous.
		/// </summary>
		/// <remarks>Asynchronous experimental, if proven to not work properly then removed.</remarks>
		public async Task ResetLocationAsync(bool instant = false, Action callback = null, Action<Exception> error = null)
		{
			if (!instant)
			{
				await Task.Delay(Data.Constants.Time.DroppedItemRemovalTime);
			}
			
			try
			{
				var item = (MapObject as Models.Items.Item);
				
				if (item != null)
				{
					if (item.DbOwnerItem == null || item.DbOwnerItem.Id == 0)
					{
						MapObject.X = 0;
						MapObject.Y = 0;
						if (MapObject.Map != null)
						{
							UpdateScreen(true);
							MapObject.Map.RemoveFromMap(MapObject);
						}
					}
				}
				
				if (callback != null)
				{
					callback();
				}
			}
			catch (Exception e)
			{
				Drivers.Global.RaiseException(e);
				
				if (error != null)
				{
					error(e);
				}
				else
				{
					var mapObjectPlayer = MapObject as Models.Entities.Player;
					if (mapObjectPlayer != null)
					{
						mapObjectPlayer.ClientSocket.LastException = e;
						mapObjectPlayer.ClientSocket.Disconnect(Drivers.Messages.Errors.FATAL_ERROR_TITLE);
					}
				}
			}
		}
		
		/// <summary>
		/// Despawns the map object.
		/// </summary>
		public void Despawn()
		{
			_despawned = true;
			
			Teleport(0, 0, 0);
		}
		
		
		/// <summary>
		/// Adds a status flag.
		/// </summary>
		/// <param name="effect">The effect.</param>
		/// <param name="time">Milliseconds to wait before remove. 0 = never remove.</param>
		public void AddStatusFlag(Enums.StatusFlag effect, int time = 0)
		{
			if (!ContainsStatusFlag(effect))
			{
				if (!ContainsStatusFlag(Enums.StatusFlag.Dead) && !ContainsStatusFlag(Enums.StatusFlag.Ghost)
				    || effect == Enums.StatusFlag.TeamLeader)
				{
					MapObject.StatusFlag |= (ulong)effect;
					
					if (time > 0)
					{
						Task.Run(async() => await RemoveStatusFlagAsync(time, effect));
					}
				}
			}
		}
		
		/// <summary>
		/// Sets a status flag.
		/// </summary>
		/// <param name="effect">The effect.</param>
		/// <param name="time">Milliseconds to wait before remove. 0 = never remove.</param>
		public void SetStatusFlag(Enums.StatusFlag effect, int time = 0)
		{
			MapObject.StatusFlag = (ulong)effect;
			var player = MapObject as Models.Entities.Player;
			if (player != null && player.StaticStatusFlag != Enums.StatusFlag.None)
			{
				AddStatusFlag(player.StaticStatusFlag);
			}
			
			if (time > 0)
			{
				Task.Run(async() => await RemoveStatusFlagAsync(time, effect));
			}
		}
		
		/// <summary>
		/// Checks whether the status flag already contains an effect..
		/// </summary>
		/// <param name="effect">The effect</param>
		/// <returns>Returns true if the status flag contains the effect.</returns>
		public bool ContainsStatusFlag(Enums.StatusFlag effect)
		{
			ulong aux = MapObject.StatusFlag;
			aux &= ~(ulong)effect;
			return !(aux == MapObject.StatusFlag);
		}
		
		/// <summary>
		/// Removes a status flag from the client.
		/// </summary>
		/// <param name="effect">The effect.</param>
		public void RemoveStatusFlag(Enums.StatusFlag effect)
		{
			if (ContainsStatusFlag(effect))
			{
				MapObject.StatusFlag &= ~(ulong)effect;
			}
		}
		
		/// <summary>
		/// Removes a status flag asynchronously.
		/// </summary>
		/// <param name="time">The time to wait before removing the flag.</param>
		/// <param name="effect">The effect to remove.</param>
		private async Task RemoveStatusFlagAsync(int time, Enums.StatusFlag effect)
		{
			await Task.Delay(time);
			
			try
			{
				RemoveStatusFlag(effect);
			}
			catch (Exception e)
			{
				Drivers.Global.RaiseException(e);
				
				var player = MapObject as Models.Entities.Player;
				if (player != null)
				{
					player.ClientSocket.LastException = e;
					player.ClientSocket.Disconnect(Drivers.Messages.Errors.FATAL_ERROR_TITLE);
				}
			}
		}
		
		/// <summary>
		/// Validates a move coordinate for a specific map+ object.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>True if the coordinate is valid.</returns>
		public bool ValidMoveCoord<T>(ushort x, ushort y)
			where T : MapObjectController
		{
			return MapObject.Map.ValidCoord(x, y) &&
				_screenObjects.Values.Count(mapobject => (mapobject as T) != null && mapobject.X == x && mapobject.Y == y) == 0;
		}
		
		/// <summary>
		/// Finds the closest map object by a specific map object type.
		/// </summary>
		/// <returns>The map object that's closes. Null if none.</returns>
		public T FindClosest<T>()
			where T : MapObjectController
		{
			return _screenObjects.Values
				.Where(mapobject => (mapobject as T) != null)
				.FirstOrDefault() as T;
		}
		
		/// <summary>
		/// Attempts to find a map object in the screen by a predicate.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <returns>The map object if found, null otherwise.</returns>
		public T FindInScreen<T>(Func<T,bool> predicate)
			where T : MapObjectController
		{
			return _screenObjects.Values.Where(m => predicate(m as T)).FirstOrDefault() as T;
		}
		
		/// <summary>
		/// Gets all map objects in the screen.
		/// </summary>
		/// <returns></returns>
		public ICollection<IMapObject> GetAllInScreen()
		{
			return _screenObjects.Values;
		}
		
		/// <summary>
		/// Gets a coordinate nearby the map object.
		/// </summary>
		/// <returns>The coordinate nearby.</returns>
		public Coordinate NearBy()
		{
			for (int x = -1; x < 3; x++)
			{
				for (int y = -1; y < 3; y++)
				{
					if (x != 0 && y != 0)
					{
						var newX = (ushort)Math.Max(0, (MapObject.X + x));
						var newY = (ushort)Math.Max(0, (MapObject.Y + y));
						
						if (MapObject.Map.ValidCoord(newX, newY))
						{
							return new Coordinate(newX, newY);
						}
					}
				}
			}
			
			return new Coordinate();
		}
	}
}
