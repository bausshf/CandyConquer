// Project by Bauss
using System;
using System.Collections.Generic;
using Candy;
using CandyConquer.Database.Models;
using CandyConquer.Maps;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for maps.
	/// </summary>
	public static class Maps
	{
		/// <summary>
		/// Gets all maps by a server.
		/// </summary>
		/// <param name="server">The server</param>
		/// <returns>All maps associated with the server.</returns>
		public static List<DbMap> GetAllByServer(string server)
		{
			var pars = Sql.GetParsDict();
			pars.Add("Server", server);
			
			return SqlDalHelper.GetAll<DbMap>(ConnectionStrings.World,
			                                   string.Format("SELECT * FROM Maps WHERE {0} OR Server = 'All'", Sql.GetSel(pars)),
			                                   pars);
		}
		
		
		/// <summary>
		/// Loads all dmaps.
		/// </summary>
		public static void LoadDMaps()
		{
			DMaps.Load();
		}
		
		/// <summary>
		/// Gets a specific dmap based on a map id.
		/// </summary>
		/// <param name="mapId">The map id.</param>
		/// <returns>The dmap if existing, null otherwise.</returns>
		public static FCQMap Get(ushort mapId)
		{
			return DMaps.Get(mapId);
		}
		
		/// <summary>
		/// Sets an event for when the dmaps has been loaded.
		/// </summary>
		/// <param name="loadedEvent"></param>
		public static void SetLoadedEvent(Action loadedEvent)
		{
			DMaps.SetLoadedEvent(loadedEvent);
		}
		
		/// <summary>
		/// Gets all default coordinates.
		/// </summary>
		/// <returns></returns>
		public static List<Models.DbDefaultCoordinate> GetDefaultCoordinates()
		{
			return DefaultCoordinates.GetAll();
		}
	}
}
