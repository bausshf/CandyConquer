// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.ApiServer;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Controllers.Packets.Entities
{
	/// <summary>
	/// Controller for npc packets.
	/// </summary>
	[ApiController()]
	public static class NpcPacketController
	{
		/// <summary>
		/// Handles the NpcRequestPacket packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = 2031)]
		public static bool HandleNpcRequest(Models.Entities.Player player, Models.Packets.Entities.NpcRequestPacket packet)
		{
			if (!player.Alive)
			{
				return true;
			}
			
			if (packet.Option == 255)
			{
				player.CurrentNpc = null;
				return true;
			}
			
			if (Collections.ShopCollection.ContainsShop(packet.NpcId))
			{
				player.AddActionLog("OpenShop", packet.NpcId);
				
				if (player.ContainsInScreen(packet.NpcId))
				{
					Helpers.Packets.Npc.Dialog.OpenShop(player, packet.NpcId);
				}
			}
			else
			{
				Models.Maps.IMapObject mapObject;
				if (player.GetFromScreen(packet.NpcId, out mapObject))
				{
					var npc = mapObject as Models.Entities.Npc;
					if (npc != null)
					{
						player.AddActionLog("NpcRequest", packet.NpcId + " " + packet.Option);
						
						player.CurrentNpc = npc;
						npc.Invoke(player, packet.Option);
					}
				}
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the NpcResponsePacket packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = 2032)]
		public static bool HandleNpcResponse(Models.Entities.Player player, Models.Packets.Entities.NpcResponsePacket packet)
		{
			if (!player.Alive)
			{
				return true;
			}
			
			if (player.CurrentNpc == null)
			{
				return true;
			}
			
			if (packet.Option == 255)
			{
				return true;
			}
			
			switch (packet.Action)
			{
				case Enums.NpcDialogAction.Popup:
				case Enums.NpcDialogAction.Answer:
					player.AddActionLog("NpcResponse", player.CurrentNpc.ClientId + " : " + packet.Option);
					player.NpcInputData = packet.InputData;
					player.CurrentNpc.Invoke(player, packet.Option);
					break;
			}
			return true;
		}
	}
}
