// Project by Bauss
using System;
using System.Linq;
using System.Collections.Concurrent;
using CandyConquer.Drivers.Repositories.Collections;
using CandyConquer.Database.Models;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A collection of portals.
	/// </summary>
	public static class PortalCollection
	{
		/// <summary>
		/// The list of portals.
		/// </summary>
		private static ConcurrentList<DbPortal> _portals;
		
		/// <summary>
		/// Static constructor for PortalCollection.
		/// </summary>
		static PortalCollection()
		{
			_portals = new ConcurrentList<DbPortal>();
		}
		
		/// <summary>
		/// Loads all portals.
		/// </summary>
		public static void Load()
		{
			_portals.TryAddRange(Database.Dal.Portals.GetAll());
		}
		
		/// <summary>
		/// Teleports a map object through a portal.
		/// </summary>
		/// <param name="mapObject">The map object.</param>
		/// <param name="startMapId">The start map id.</param>
		/// <param name="startX">The start x.</param>
		/// <param name="startY">The start y.</param>
		/// <returns>True if the map object was teleported.</returns>
		public static bool Teleport(Controllers.Maps.MapObjectController mapObject, int startMapId, ushort startX, ushort startY)
		{
			var endpoint = _portals
				.Where(portal => portal.StartMapId == startMapId &&
			                              portal.StartX >= (startX - 3) && portal.StartX <= (startX + 3)  &&
			                              portal.StartY >= (startY - 3) && portal.StartY <= (startY + 3))
				.FirstOrDefault();
			
			if (endpoint != null)
			{
				mapObject.Teleport(endpoint.EndMapId, endpoint.EndX, endpoint.EndY);
				return true;
			}
			
			return false;
		}
	}
}
