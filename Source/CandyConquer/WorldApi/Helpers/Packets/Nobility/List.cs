// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Nobility
{
	/// <summary>
	/// Controller for the list sub type.
	/// </summary>
	[ApiController()]
	public static class List
	{
		/// <summary>
		/// Handles the list sub type.
		/// </summary>
		/// <param name="player">The player.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.NobilityPacket,
		         SubIdentity = (uint)Enums.NobilityAction.List)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Nobility.NobilityPacket packet)
		{
			int pageMax;
			var pagedList = Collections.NobilityBoard.GetPaged((int)packet.Data1LowA, out pageMax);
			pageMax = pageMax > 0 ? (pageMax + 1) : 0;
			
			if (pagedList.Count > 0)
			{
				var nobilityPacket = new Models.Packets.Nobility.NobilityPacket(48 * pagedList.Count)
				{
					Action = Enums.NobilityAction.List,
					Data1LowLow = packet.Data1LowA,
					Data1LowHigh = (ushort)pageMax,
					Data1HighLow = (ushort)pagedList.Count
				};
				nobilityPacket.WriteData();
				
				nobilityPacket.Offset = 32;
				foreach (var donation in pagedList)
				{
					nobilityPacket.WriteDonation(donation);
				}
				
				player.ClientSocket.Send(nobilityPacket);
			}
			
			return true;
		}
	}
}
