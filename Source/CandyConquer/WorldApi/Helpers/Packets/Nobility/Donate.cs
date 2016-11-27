// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Nobility
{
	/// <summary>
	/// Controller for the donate sub type.
	/// </summary>
	[ApiController()]
	public static class Donate
	{
		/// <summary>
		/// Handles the donate sub type.
		/// </summary>
		/// <param name="player">The player.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.NobilityPacket,
		         SubIdentity = (uint)Enums.NobilityAction.Donate)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Nobility.NobilityPacket packet)
		{
			if (player.Level < 70)
			{
				return true;
			}
			
			if (packet.Data3 == 0)
			{
				return Handle(player, packet.Data1HighA, true);
			}
			else
			{
				return Handle(player, packet.Data1HighA, false);
			}
		}
		
		/// <summary>
		/// Handles the donation-
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="amount">The amount.</param>
		/// <param name="isMoney">Boolean determining if it's a silver donation or not. If set to false it will be a cps donation.</param>
		/// <returns>True if the packet was handled correctly.</returns>
		private static bool Handle(Models.Entities.Player player, long amount, bool isMoney)
		{
			uint requiredAmount = (uint)(isMoney ? amount : (amount / 50000));
			if (isMoney && player.Money < requiredAmount || !isMoney && player.CPs < requiredAmount ||
			    amount < 3000000 || amount > 999999999)
			{
				return true;
			}
			
			player.AddActionLog("NobilityDonate" + (isMoney ? "Silver" : "CPs"), requiredAmount);
			
			var donation = player.Nobility;
			if (donation == null)
			{
				donation = new Models.Nobility.NobilityDonation(new Database.Models.DbPlayerNobility());
				donation.Player = player;
				player.Nobility = donation;
				
				Collections.NobilityBoard.TryAdd(donation);
			}
			
			if (isMoney)
			{
				player.Money -= requiredAmount;
			}
			else
			{
				player.CPs -= requiredAmount;
			}
			
			amount = (uint)Math.Min(Data.Constants.GameMode.MaxNobilityDonation, (donation.Donation + amount));
			donation.Donation = amount;
			
			Collections.NobilityBoard.GetTop50();
			player.UpdateClientNobility();
			
			return true;
		}
	}
}
