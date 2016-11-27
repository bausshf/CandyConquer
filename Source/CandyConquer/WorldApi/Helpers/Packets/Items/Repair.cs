// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Items
{
	/// <summary>
	/// Controller for the repair sub types.
	/// </summary>
	[ApiController()]
	public static class Repair
	{
		/// <summary>
		/// Handles the repair sub type.
		/// </summary>
		/// <param name="player">The player.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ItemActionPacket,
		         SubIdentity = (uint)Enums.ItemAction.Repair)]
		public static bool HandleRepair(Models.Entities.Player player, Models.Packets.Items.ItemActionPacket packet)
		{
			Models.Items.Item repairItem;
			if (player.Inventory.TryGetItem(packet.ClientId, out repairItem))
			{
				if (repairItem.DbOwnerItem.CurrentDura <= 0)
				{
					int meteorAmount;
					if (!player.Inventory.ContainsById(1088001, out meteorAmount) || meteorAmount < 5)
					{
						player.SendSystemMessage("REPAIR_NOT_ENOUGH_METEORS");
						return true;
					}
					
					if (!player.Inventory.RemoveByCount(1088001, 5))
					{
						player.SendSystemMessage("REPAIR_NOT_ENOUGH_METEORS");
						return true;
					}
				}
				else
				{
					uint repairPrice = repairItem.RepairPrice;
					
					if (player.Money < repairPrice)
					{
						player.SendSystemMessage("REPAIR_NOT_ENOUGH_MONEY");
						return true;
					}
					
					player.Money -= repairPrice;
				}
				
				player.AddActionLog("Repair", repairItem.DbOwnerItem.Id);
				repairItem.DbOwnerItem.CurrentDura = repairItem.DbOwnerItem.MaxDura;
				repairItem.UpdateDatabase();
				repairItem.UpdateClient(player, Enums.ItemUpdateAction.Update);
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the repair sub type.
		/// </summary>
		/// <param name="player">The player.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ItemActionPacket,
		         SubIdentity = (uint)Enums.ItemAction.RepairAll)]
		public static bool HandleRepairAll(Models.Entities.Player player, Models.Packets.Items.ItemActionPacket packet)
		{
			var repairItems = player.Equipments.GetAll();
			uint repairPrice = 0;
			
			// Gets the price for the repair ...
			foreach (var item in repairItems)
			{
				if (item.DbOwnerItem.CurrentDura > 0)
				{
					repairPrice += item.RepairPrice;
				}
			}
			
			if (player.Money < repairPrice)
			{
				// Necessary, because the client automatically sets the durability to max durability without server-side validation.
				foreach (var item in repairItems)
				{
					item.UpdateClient(player, Enums.ItemUpdateAction.Update);
				}
				
				player.SendSystemMessage("REPAIR_ALL_NOT_ENOUGH_MONEY");
				return true;
			}
			
			player.AddActionLog("RepairAll");
			player.Money -= repairPrice;
			
			foreach (var repairItem in repairItems)
			{
				if (repairItem.DbOwnerItem.CurrentDura >= 0)
				{
					repairItem.DbOwnerItem.CurrentDura = repairItem.DbOwnerItem.MaxDura;
					repairItem.UpdateDatabase();
				}
				
				repairItem.UpdateClient(player, Enums.ItemUpdateAction.Update);
			}
			
			return true;
		}
	}
}
