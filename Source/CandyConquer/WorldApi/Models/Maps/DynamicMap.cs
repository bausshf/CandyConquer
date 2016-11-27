// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Models.Maps
{
	/// <summary>
	/// A model for a dynamic map.
	/// </summary>
	public class DynamicMap : Controllers.Maps.DynamicMapController
	{
		/// <summary>
		/// Creates a new dynamic map.
		/// </summary>
		/// <param name="map">The associated map.</param>
		public DynamicMap(Map map)
			: base(map.DbMap)
		{
			_id = Drivers.Repositories.Safe.IdentityGenerator.GetDynamicMapId();
			
			DynamicMap = this;
		}
		
		/// <summary>
		/// The amount of time the dynamic map should be showable.
		/// </summary>
		public int ShowTime { get; set; }
		
		/// <summary>
		/// An action to trigger once the dynamic map hides.
		/// </summary>
		public Action OnHide { get; set; }
	}
}
