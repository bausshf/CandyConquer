// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Packets.Entities;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Controllers.Packets.Interaction
{
	/// <summary>
	/// The Interaction packet controller.
	/// </summary>
	[ApiController()]
	public static class InteractionPacketController
	{
		/// <summary>
		/// Retrieves the Interaction packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.InteractionPacket, TypeReturner = true)]
		public static InteractionPacket HandlePacket(Models.Entities.Player player, InteractionPacket packet, out uint subPacketId)
		{
			packet.Packetstamp = Drivers.Time.GetSystemTime();
			
			if (packet.Action == Enums.InteractionAction.MagicAttack ||
			    packet.Action == Enums.InteractionAction.Attack ||
			    packet.Action == Enums.InteractionAction.Shoot)
			{
				packet.Decrypt(player);
				
				Helpers.Packets.Interaction.Battle.Combat.Handle(player, packet);
				subPacketId = Security.Api.SubCallState.DontHandle;
			}
			else
			{
				subPacketId = (uint)packet.Action;
			}
			
			return packet;
		}
	}
}
