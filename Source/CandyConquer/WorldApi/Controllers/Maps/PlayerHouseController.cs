// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Controllers.Maps
{
	/// <summary>
	/// Controller for player houses.
	/// </summary>
	public class PlayerHouseController
	{
		/// <summary>
		/// Gets or sets the player house.
		/// </summary>
		public Models.Maps.PlayerHouse PlayerHouse { get; protected set; }
		
		/// <summary>
		/// Creates a new player house controller.
		/// </summary>
		protected PlayerHouseController()
		{
		}
		
		/// <summary>
		/// Let's a player enter the house.
		/// </summary>
		/// <param name="player">The player that should enter.</param>
		public void EnterHouse(Models.Entities.Player player)
		{
			if (player.ClientId != PlayerHouse.Player.ClientId)
			{
				return;
			}
			
			player.AddActionLog("EnterHouse", player.MapId);
			
			if (PlayerHouse.DbPlayerHouse.IsBig)
			{
				player.TeleportDynamic(PlayerHouse.DynamicMapId, 53, 83);
			}
			else
			{
				player.TeleportDynamic(PlayerHouse.DynamicMapId, 32, 40);
			}
			
			if (player.GetAllInScreen().Count < 10)
			{
				if (player.Team != null)
				{
					foreach (var member in player.Team.GetMembers())
					{
						if (PlayerHouse.DbPlayerHouse.IsBig)
						{
							member.TeleportDynamic(PlayerHouse.DynamicMapId, 53, 83);
						}
						else
						{
							member.TeleportDynamic(PlayerHouse.DynamicMapId, 32, 40);
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Pays the rent for the house.
		/// </summary>
		/// <returns>The status of the payment.</returns>
		public Enums.RentStatus PayRent()
		{
			uint rentAmount = (uint)(PlayerHouse.DbPlayerHouse.IsBig ? 10000000 : 1000000);
			
			if (PlayerHouse.Player.Money < rentAmount)
			{
				return Enums.RentStatus.NotEnoughMoney;
			}
			
			if (DateTime.UtcNow < PlayerHouse.DbPlayerHouse.NextRentDate)
			{
				return Enums.RentStatus.RentIsNotDue;
			}
			
			PlayerHouse.Player.AddActionLog("PayRent", DateTime.UtcNow);
			
			PlayerHouse.Player.Money -= rentAmount;
			PlayerHouse.DbPlayerHouse.InvestedMoney += (rentAmount / 1000000);
			var date = DateTime.UtcNow;
			int monthsToAdd = 1;
			if (date.Day > 25)
			{
				monthsToAdd = 2;
			}
			PlayerHouse.DbPlayerHouse.NextRentDate = new Nullable<DateTime>(new DateTime(date.Year, date.Month + monthsToAdd, 1));
			PlayerHouse.DbPlayerHouse.Update();
			
			return Enums.RentStatus.Success;
		}
		
		/// <summary>
		/// Creates a warehouse.
		/// </summary>
		public void CreateWarehouse()
		{
			if (PlayerHouse.DbPlayerHouse.Warehouse)
			{
				var dbNpc = new Database.Models.DbNpc
				{
					Id = 100000 + PlayerHouse.DbPlayerHouse.MapId,
					NpcId = (uint)(100000 + PlayerHouse.DbPlayerHouse.MapId),
					Type = "Normal",
					Name = "Warehouse",
					MapId = PlayerHouse.DynamicMapId,
					X = (ushort)(PlayerHouse.DbPlayerHouse.IsBig ? 44 : 32),
					Y = (ushort)(PlayerHouse.DbPlayerHouse.IsBig ? 23 : 25),
					Flag = 2,
					Mesh = 5280,
					Avatar = 0,
					Server = Drivers.Settings.WorldSettings.Server
				};
				var npc = new Models.Entities.Npc(dbNpc);
				npc.TeleportDynamic(dbNpc.MapId, npc.X, npc.Y);
			}
		}
	}
}
