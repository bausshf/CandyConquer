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
	public static class CompositionPacketController
	{
		/// <summary>
		/// Retrieves the item action packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.CompositionPacket, TypeReturner = true)]
		public static CompositionPacket HandlePacket(Models.Entities.Player player, CompositionPacket packet, out uint subPacketId)
		{
			// TODO: make a check for steed, then you have to match steed npc location + no remote.
			if (player.Battle != null || player.VIPLevel < 3 && (packet.Action == Enums.CompositionAction.VIP || player.MapId != 1036 || !Tools.RangeTools.ValidDistance(320, 224, player.X, player.Y)))
			{
				subPacketId = SubCallState.DontHandle;
			}
			else
			{
				subPacketId = (uint)packet.Action;
			}
			
			return packet;
		}
	}
}
