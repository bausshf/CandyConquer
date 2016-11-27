// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Items
{
	/// <summary>
	/// Helper class for the item action packet's subtype for unequip.
	/// </summary>
	[ApiController()]
	public static class Unequip
	{
		/// <summary>
		/// Handles the unequip subtype of the item action packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		/// <returns>True if the packet was handled correctly.</returns>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.ItemActionPacket,
		         SubIdentity = (uint)Enums.ItemAction.Unequip)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Items.ItemActionPacket packet)
		{
			if (!player.Alive)
			{
				return true;
			}
			
			if (player.Battle != null)
			{
				return true;
			}
			
			player.Equipments.Unequip((Enums.ItemPosition)packet.Data1);
			
			return true;
		}
	}
}
