// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Guilds
{
	/// <summary>
	/// Controller for donations.
	/// </summary>
	[ApiController()]
	public static class Donation
	{
		/// <summary>
		/// Handles the donate money.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.GuildPacket,
		         SubIdentity = (uint)Enums.GuildAction.DonateMoney)]
		public static bool HandleSilvers(Models.Entities.Player player, Models.Packets.Guilds.GuildPacket packet)
		{
			if (player.Guild != null)
			{
				if (!player.Guild.DonateSilvers(player, packet.Data))
				{
					player.SendSystemMessage("LOW_MONEY_DONATE_GUILD");
				}
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the donate e-money.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.GuildPacket,
		         SubIdentity = (uint)Enums.GuildAction.DonateEMoney)]
		public static bool HandleCPs(Models.Entities.Player player, Models.Packets.Guilds.GuildPacket packet)
		{
			if (player.Guild != null)
			{
				if (!player.Guild.DonateCPs(player, packet.Data))
				{
					player.SendSystemMessage("LOW_CPS_DONATE_GUILD");
				}
			}
			
			return true;
		}
	}
}
