// Project by Bauss
using System;
using System.Linq;
using CandyConquer.WorldApi.Models.Maps;
using CandyConquer.Database.Models;

namespace CandyConquer.WorldApi.Controllers.Maps
{
	/// <summary>
	/// Controller for maps.
	/// </summary>
	public class MapController
	{
		/// <summary>
		/// The map associated with the controller.
		/// </summary>
		public Map Map { get; protected set; }
		
		/// <summary>
		/// Constructor for the controller.
		/// </summary>
		public MapController()
		{
		}
		
		/// <summary>
		/// Validates two coordinates within the map.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>True if the coordinates are valid, false otherwise.</returns>
		public bool ValidCoord(ushort x, ushort y)
		{
			if (x == 0 || y == 0)
			{
				return false;
			}
			
			if (x > 30720 || y > 30720)
			{
				return false;
			}
			
			if (Map.DMap != null)
			{
				return Map.DMap.Check(x, y);
			}
			return true;
		}
		
		/// <summary>
		/// Adds a map object to the map.
		/// </summary>
		/// <param name="obj">The object to add.</param>
		/// <returns>True if the object was added.</returns>
		public bool AddToMap(IMapObject obj)
		{
			var player = obj as Models.Entities.Player;
			if (player != null)
			{
				if (!Map.Players.TryAdd(obj.ClientId, player))
				{
					player.ClientSocket.Disconnect(Drivers.Messages.Errors.FAILED_TO_ADD_TO_MAP);
					return false;
				}
			}
			
			if (Map.MapObjects.TryAdd(obj.ClientId, obj))
			{
				obj.Map = Map;
				return true;
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>
		/// Removes a map object from the map.
		/// </summary>
		/// <param name="obj">The map object to remove.</param>
		/// <returns>True if the map object was removed.</returns>
		public bool RemoveFromMap(IMapObject obj)
		{
			if (Map.MapObjects.ContainsKey(obj.ClientId))
			{
				var player = obj as Models.Entities.Player;
				if (player != null)
				{
					if (!Map.Players.TryRemove(player.ClientId, out player))
					{
						player.ClientSocket.Disconnect(Drivers.Messages.Errors.FAILED_TO_REMOVE_FROM_MAP);
					}
				}
				
				if (Map.MapObjects.TryRemove(obj.ClientId, out obj))
				{
					obj.Map = null;
					return true;
				}
			}
			return false;
		}
		
		/// <summary>
		/// Teleports all map objects in the map to the last map they were at.
		/// </summary>
		public void TeleportToLastMap()
		{
			foreach (var obj in Map.MapObjects.Values)
			{
				(obj as Controllers.Maps.MapObjectController).TeleportToLastMap();
			}
		}
		
		/// <summary>
		/// Gets a valid item coordinate.
		/// </summary>
		/// <param name="xRange">The x coordinate.</param>
		/// <param name="yRange">The y coordinate.</param>
		/// <returns>A valid coordinate based on the x and y coordinate given.</returns>
		public Coordinate GetValidItemCoordinate(ushort xRange, ushort yRange)
		{
			for (int i = 0; i < 36/* (6 * 6) */; i++)
			{
				var minX = Math.Max((xRange - 3), 0);
				var minY = Math.Max((yRange - 3), 0);
				
				ushort x = (ushort)Drivers.Repositories.Safe.Random.Next(minX, xRange + 3);
				ushort y = (ushort)Drivers.Repositories.Safe.Random.Next(minY, yRange + 3);
				
				if (ValidCoord(x, y) && Map.MapObjects.Values
					    .Count(o => (o as Models.Items.Item) != null && o.X == x && o.Y == y) == 0)
					{
						return new Coordinate(x, y);
					}
			}
			
			return new Coordinate();
		}
		
		/// <summary>
		/// Gets a valid monster spawn coordinate.
		/// </summary>
		/// <param name="xRange">The x range.</param>
		/// <param name="yRange">The y range.</param>
		/// <param name="rangeSize">The range size.</param>
		/// <returns>The coordinate.</returns>
		public Coordinate GetValidMonsterCoordinate(ushort xRange, ushort yRange, int rangeSize)
		{
			for (int i = 0; i < rangeSize * rangeSize; i++)
			{
				var minX = Math.Max((xRange - rangeSize), 0);
				var minY = Math.Max((yRange - rangeSize), 0);
				
				ushort x = (ushort)Drivers.Repositories.Safe.Random.Next(minX, xRange + rangeSize);
				ushort y = (ushort)Drivers.Repositories.Safe.Random.Next(minY, yRange + rangeSize);
				
				if (ValidCoord(x, y) && Map.MapObjects.Values
					    .Count(o => (o as Models.Entities.Monster) != null && o.X == x && o.Y == y) == 0)
					{
						return new Coordinate(x, y);
					}
			}
			
			return new Coordinate();
		}
		
		/// <summary>
		/// Displays weather for the map.
		/// </summary>
		/// <param name="player">The player to display the weather for.</param>
		public void DisplayWeather(Models.Entities.Player player)
		{
			if (Map.Weather == null)
			{
				var weatherPacket = new Models.Packets.Location.WeatherPacket
				{
					Clear = true
				};
				
				player.ClientSocket.Send(weatherPacket);
				
				player.ClientSocket.Send(new Models.Packets.Client.DataExchangePacket
				                         {
				                         	ExchangeType = Enums.ExchangeType.MapARGB,
				                         	ClientId = player.ClientId,
				                         	Data1 = 0
				                         });
			}
			else
			{
				var weatherPacket = new Models.Packets.Location.WeatherPacket
				{
					Weather = Map.Weather
				};
				var showDark =
					(Map.Weather.WeatherType == Enums.WeatherType.Rain || Map.Weather.WeatherType == Enums.WeatherType.RainWind);
				
				player.ClientSocket.Send(weatherPacket);
				
				if (showDark)
				{
					player.ClientSocket.Send(new Models.Packets.Client.DataExchangePacket
					                         {
					                         	ExchangeType = Enums.ExchangeType.MapARGB,
					                         	ClientId = player.ClientId,
					                         	Data1 = 5855577
					                         });
				}
				else
				{
					player.ClientSocket.Send(new Models.Packets.Client.DataExchangePacket
					                         {
					                         	ExchangeType = Enums.ExchangeType.MapARGB,
					                         	ClientId = player.ClientId,
					                         	Data1 = 0
					                         });
				}
			}
		}
		
		/// <summary>
		/// Displays weather for all players in the map.
		/// </summary>
		protected void DisplayWeather()
		{
			if (Map.Weather == null)
			{
				var weatherPacket = new Models.Packets.Location.WeatherPacket
				{
					Clear = true
				};
				
				foreach (var mapObject in Map.MapObjects.Values)
				{
					var player = mapObject as Models.Entities.Player;
					if (player != null && player.LoggedIn)
					{
						player.ClientSocket.Send(weatherPacket);
						
						player.ClientSocket.Send(new Models.Packets.Client.DataExchangePacket
						                         {
						                         	ExchangeType = Enums.ExchangeType.MapARGB,
						                         	ClientId = player.ClientId,
						                         	Data1 = 0
						                         });
					}
				}
			}
			else
			{
				var weatherPacket = new Models.Packets.Location.WeatherPacket
				{
					Weather = Map.Weather
				};
				var showDark =
					(Map.Weather.WeatherType == Enums.WeatherType.Rain || Map.Weather.WeatherType == Enums.WeatherType.RainWind);
				
				foreach (var mapObject in Map.MapObjects.Values)
				{
					var player = mapObject as Models.Entities.Player;
					if (player != null && player.LoggedIn)
					{
						player.ClientSocket.Send(weatherPacket);
						
						if (showDark)
						{
							player.ClientSocket.Send(new Models.Packets.Client.DataExchangePacket
							                         {
							                         	ExchangeType = Enums.ExchangeType.MapARGB,
							                         	ClientId = player.ClientId,
							                         	Data1 = 5855577
							                         });
						}
						else
						{
							player.ClientSocket.Send(new Models.Packets.Client.DataExchangePacket
							                         {
							                         	ExchangeType = Enums.ExchangeType.MapARGB,
							                         	ClientId = player.ClientId,
							                         	Data1 = 0
							                         });
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Displays thunder for all players in the map.
		/// </summary>
		public void DisplayThunder()
		{
			foreach (var mapObject in Map.MapObjects.Values)
			{
				var player = mapObject as Models.Entities.Player;
				if (player != null && player.LoggedIn)
				{
					player.ClientSocket.Send(new Models.Packets.Misc.StringPacket
					                         {
					                         	String = "lounder1",
					                         	Action = Enums.StringAction.MapEffect,
					                         	PositionX = (ushort)(player.X - 20),
					                         	PositionY = player.Y
					                         });
				}
			}
		}
		
		/// <summary>
		/// Performs a drop within the map.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="mobId">The monster id.</param>
		/// <param name="player">The player that caused the drop.</param>
		public void Drop(ushort x, ushort y, int mobId, Models.Entities.Player player)
		{
			foreach (var drop in Map.Drops)
			{
				drop.Perform(Map, x, y, mobId, player);
			}
		}
		
		/// <summary>
		/// Creates a direction point.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="dir">The direction.</param>
		/// <returns>The coordinate.</returns>
		public static Coordinate CreateDirectionPoint(ushort x, ushort y, byte dir)
		{
			return new Coordinate((ushort)(x + Data.Constants.Movement.DeltaX[dir]),
			                      (ushort)(y + Data.Constants.Movement.DeltaY[dir]));
		}
	}
}
