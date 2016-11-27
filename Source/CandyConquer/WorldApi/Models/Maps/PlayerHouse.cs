// Project by Bauss
using System;
using CandyConquer.Database.Models;

namespace CandyConquer.WorldApi.Models.Maps
{
	/// <summary>
	/// Model for a player house.
	/// </summary>
	public sealed class PlayerHouse : Controllers.Maps.PlayerHouseController
	{
		/// <summary>
		/// The dynamic map associated with the house.
		/// </summary>
		private DynamicMap _map;
		
		/// <summary>
		/// The player.
		/// </summary>
		public Models.Entities.Player Player { get; private set; }
		
		/// <summary>
		/// The database model for the house.
		/// </summary>
		public DbPlayerHouse DbPlayerHouse { get; private set; }
		
		/// <summary>
		/// Gets the dynamic map id of the house.
		/// </summary>
		public int DynamicMapId { get; private set; }
		
		/// <summary>
		/// Creates a new player house.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="dbPlayerHouse">The database model associated with it.</param>
		public PlayerHouse(Models.Entities.Player player, DbPlayerHouse dbPlayerHouse)
			: base()
		{
			Player = player;
			DbPlayerHouse = dbPlayerHouse;
			
			_map = Collections.MapCollection.GetDynamicMap(dbPlayerHouse.IsBig ? 1099 : 1098);
			
			if (!_map.Show())
			{
				Player.ClientSocket.Disconnect(Drivers.Messages.Errors.FAILED_TO_LOAD_HOUSE);
			}
			
			DynamicMapId = _map.Id;
			PlayerHouse = this;
		}
	}
}
