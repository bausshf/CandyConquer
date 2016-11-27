// Project by Bauss
using System;
using System.Threading.Tasks;
using CandyConquer.Database.Models;

namespace CandyConquer.WorldApi.Controllers.Maps
{
	/// <summary>
	/// Controller for dynamic maps.
	/// </summary>
	public class DynamicMapController : Models.Maps.Map
	{
		/// <summary>
		/// The dynamic map tied to the controller.
		/// </summary>
		public Models.Maps.DynamicMap DynamicMap { get; protected set; }
		
		/// <summary>
		/// Creates a new dynamic map controller.
		/// </summary>
		/// <param name="map">The database map tied to it.</param>
		public DynamicMapController(DbMap map)
			: base(map)
		{
		}
		
		/// <summary>
		/// Shows the dynamic map and adds it to the dynamic map collection.
		/// </summary>
		/// <returns>True if the map was added, false otherwise.</returns>
		public bool Show()
		{
			if (Collections.MapCollection.TryAddDynamicMap(DynamicMap))
			{
				if (DynamicMap.ShowTime > 0)
				{
					Task.Run(async () => await HideAsync());
				}
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>
		/// Hides the dynamic map asynchronous.
		/// </summary>
		public async Task HideAsync()
		{
			await Task.Delay(DynamicMap.ShowTime);
			
			try
			{
				Hide();
			}
			catch (Exception e)
			{
				Drivers.Global.RaiseException(e);
			}
		}
		
		/// <summary>
		/// Hides the dynamic map.
		/// </summary>
		public void Hide()
		{
			TeleportToLastMap();
			if (DynamicMap.OnHide != null)
			{
				DynamicMap.OnHide();
			}
		}
	}
}
