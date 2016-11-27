// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;
using CandyConquer.WorldApi.Models.Packets.Items;

namespace CandyConquer.WorldApi.Controllers.Packets.Items
{
	/// <summary>
	/// Controller for the item action packet.
	/// </summary>
	[ApiController()]
	public static class ItemActionPacketController
	{
		/// <summary>
		/// Retrieves the item action packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.ItemActionPacket, TypeReturner = true)]
		public static ItemActionPacket HandlePacket(Models.Entities.Player player, ItemActionPacket packet, out uint subPacketId)
		{
			subPacketId = (uint)packet.Action;
			return packet;
		}
	}
}
