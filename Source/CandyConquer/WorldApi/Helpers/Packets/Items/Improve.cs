// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Items
{
	/// <summary>
	/// Controller for the improve sub type.
	/// </summary>
	[ApiController()]
	public static class Improve
	{
		/// <summary>
		/// Handles the improve sub type.
		/// </summary>
		/// <param name="player">The player.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ItemActionPacket,
		         SubIdentity = (uint)Enums.ItemAction.Improve)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Items.ItemActionPacket packet)
		{
			if (!player.Alive)
			{
				return true;
			}
			
			Models.Items.Item upgradeItem;
			Models.Items.Item dragonBall;
			
			if (!player.Inventory.TryGetItem(packet.ClientId, out upgradeItem) ||
			    !player.Inventory.TryGetItem(packet.Data1, out dragonBall) ||
			    dragonBall.DbItem.Id != 1088000)
			{
				player.SendSystemMessage("IMPROVE_ITEM_NOT_FOUND_OR_NO_DRAGONBALLS");
				return true;
			}
			
			if (upgradeItem.DbOwnerItem.CurrentDura < upgradeItem.DbOwnerItem.MaxDura)
			{
				player.SendSystemMessage("IMPROVE_ITEM_LOW_DURA");
				return true;
			}
			
			if (upgradeItem.IsGarment || upgradeItem.IsArrow || upgradeItem.IsBottle || upgradeItem.IsMountArmor || upgradeItem.IsMisc)
			{
				player.SendSystemMessage("IMPROVE_ITEM_INVALID");
				return true;
			}
			
			if (upgradeItem.Quality == Enums.ItemQuality.Super)
			{
				player.SendSystemMessage("IMPROVE_ITEM_IS_SUPER");
				return true;
			}
			
			int extraChance = Math.Max(0, (80 - ((int)upgradeItem.Quality) * 10));
			int chance = Math.Min(90, Data.Constants.Chances.UpgradeQualityChance + extraChance);
			
			player.AddActionLog("Improve", upgradeItem.DbOwnerItem.Id);
			if (player.Inventory.Remove(packet.Data1) && Tools.CalculationTools.ChanceSuccess(chance))
			{
				upgradeItem.UpgradeQuality(player);
				upgradeItem.UpgradeSockets(player);
				
				player.SendSystemMessage("IMPROVE_SUCCESS");
			}
			else
			{
				player.SendSystemMessage("IMPROVE_FAIL");
			}
			
			return true;
		}
	}
}
