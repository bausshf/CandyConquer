// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Items
{
	/// <summary>
	/// Controller for the up level sub type.
	/// </summary>
	[ApiController()]
	public static class UpLevel
	{
		/// <summary>
		/// Handles the up level sub type.
		/// </summary>
		/// <param name="player">The player.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ItemActionPacket,
		         SubIdentity = (uint)Enums.ItemAction.UpLevel)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Items.ItemActionPacket packet)
		{
			if (!player.Alive)
			{
				return true;
			}
			
			Models.Items.Item upgradeItem;
			Models.Items.Item meteor;
			
			if (!player.Inventory.TryGetItem(packet.ClientId, out upgradeItem) ||
			    !player.Inventory.TryGetItem(packet.Data1, out meteor) ||
			    (meteor.DbItem.Id != 1088001 && meteor.DbItem.Id != 1088002))
			{
				player.SendSystemMessage("UPLEVEL_ITEM_NOT_FOUND_OR_NO_METEORS");
				return true;
			}
			
			if (upgradeItem.DbOwnerItem.CurrentDura < upgradeItem.DbOwnerItem.MaxDura)
			{
				player.SendSystemMessage("UPLEVEL_ITEM_LOW_DURA");
				return true;
			}
			
			if (upgradeItem.IsGarment || upgradeItem.IsArrow || upgradeItem.IsBottle || upgradeItem.IsMountArmor || upgradeItem.IsMisc)
			{
				player.SendSystemMessage("UPLEVEL_ITEM_INVALID");
				return true;
			}
			
			if (upgradeItem.DbItem.Level >= 120)
			{
				player.SendSystemMessage("UPLEVEL_ITEM_MAX_LEVEL");
				return true;
			}
			
			if (player.Inventory.Remove(packet.Data1))
			{
				int extraChance = Math.Max(0, (80 - upgradeItem.DbItem.Level));
				int chance = Math.Min(90, Data.Constants.Chances.UpgradeLevelChance + extraChance);
				
				player.AddActionLog("ItemUpLevel", upgradeItem.DbOwnerItem.Id);
				if (upgradeItem.UpgradeLevel(player) && Tools.CalculationTools.ChanceSuccess(chance))
				{
					upgradeItem.UpgradeSockets(player);
					
					player.SendSystemMessage("UPLEVEL_SUCCESS");
				}
				else
				{
					player.SendSystemMessage("UPLEVEL_FAIL");
				}
			}
			
			return true;
		}
	}
}
