// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Items
{
	/// <summary>
	/// Controller for the bless sub type.
	/// </summary>
	[ApiController()]
	public static class Bless
	{
		/// <summary>
		/// Handles the bless sub type.
		/// </summary>
		/// <param name="player">The player.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ItemActionPacket,
		         SubIdentity = (uint)Enums.ItemAction.Bless)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Items.ItemActionPacket packet)
		{
			if (!player.Alive)
			{
				return true;
			}
			
			if (player.MapId != 1036 || !Tools.RangeTools.ValidDistance(320, 229, player.X, player.Y))
			{
				return true;
			}
			
			Models.Items.Item blessItem;
			if (!player.Inventory.TryGetItem(packet.ClientId, out blessItem))
			{
				player.SendSystemMessage("BLESS_ITEM_NOT_FOUND");
				return true;
			}
			
			if (blessItem.DbOwnerItem.CurrentDura < blessItem.DbOwnerItem.MaxDura)
			{
				player.SendSystemMessage("BLESS_ITEM_LOW_DURA");
				return true;
			}
			
			if (blessItem.IsGarment || blessItem.IsArrow || blessItem.IsBottle || blessItem.IsMountArmor || blessItem.IsMisc ||
			    blessItem.IsFan || blessItem.IsTower)
			{
				player.SendSystemMessage("BLESS_ITEM_INVALID");
				return true;
			}
			
			int requiredTortoiseGems = 5;
			byte setBless = 1;
			
			switch (blessItem.DbOwnerItem.Bless)
			{
				case 0:
					requiredTortoiseGems  = 5;
					break;
					
				default:
					requiredTortoiseGems = blessItem.DbOwnerItem.Bless;
					setBless = (byte)(requiredTortoiseGems + 2);
					break;
			}
			
			if (setBless > 7)
			{
				player.SendSystemMessage("BLESS_ITEM_MAX_BLESS");
				return true;
			}
			
			int tortoiseAmount;
			if (!player.Inventory.ContainsById(700073, out tortoiseAmount) || tortoiseAmount < requiredTortoiseGems)
			{
				player.SendSystemMessage("BLESS_NOT_ENOUGH_TORTOISE_GEMS");
				return true;
			}
			
			player.AddActionLog("BlessItem", blessItem.DbOwnerItem.Id);
			if (player.Inventory.RemoveByCount(700073, requiredTortoiseGems))
			{
				blessItem.DbOwnerItem.Bless = setBless;
				blessItem.DbOwnerItem.Update();
				blessItem.UpdateClient(player, Enums.ItemUpdateAction.Update);
				player.ClientSocket.Send(packet);
			}
			
			return true;
		}
	}
}
