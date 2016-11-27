// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Items
{
	/// <summary>
	/// Controller for the composition packet's sub types.
	/// </summary>
	[ApiController()]
	public static class Composition
	{
		/// <summary>
		/// Normal composition.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		/// <returns>True if the packet was handled correct.</returns>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.CompositionPacket,
		         SubIdentity = (uint)Enums.CompositionAction.Normal)]
		public static bool HandleNormal(Models.Entities.Player player, Models.Packets.Items.CompositionPacket packet)
		{
			Models.Items.Item mainItem;
			Models.Items.Item minorItem;
			
			if (VerifyComposition(player, packet, out mainItem, out minorItem))
			{
				DoNormalComposition(player, minorItem, mainItem);
			}
			
			UpdateClient(player, mainItem);
			
			return true;
		}
		
		/// <summary>
		/// Handler for the vip sub type.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		/// <returns>True if the packet was handled normal.</returns>
		/// <remarks>The VIP validation is done in the composition packet controller and not here.</remarks>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.CompositionPacket,
		         SubIdentity = (uint)Enums.CompositionAction.VIP)]
		public static bool HandleVIP(Models.Entities.Player player, Models.Packets.Items.CompositionPacket packet)
		{
			return HandleNormal(player, packet);
		}
		
		/// <summary>
		/// Handler for the steed sub type.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		/// <returns>True if the packet was handled correct.</returns>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.CompositionPacket,
		         SubIdentity = (uint)Enums.CompositionAction.Steed)]
		public static bool HandleSteed(Models.Entities.Player player, Models.Packets.Items.CompositionPacket packet)
		{
			Models.Items.Item mainItem;
			Models.Items.Item minorItem;
			
			if (VerifyComposition(player, packet, out mainItem, out minorItem) &&
			    mainItem.IsSteed && minorItem.IsSteed)
			{
				player.AddActionLog("SteedComposition", mainItem.DbOwnerItem.Id);
				
				int color1 = (int)mainItem.DbOwnerItem.SocketRGB;
				int color2 = (int)minorItem.DbOwnerItem.SocketRGB;
				int B1 = color1 & 0xFF;
				int B2 = color2 & 0xFF;
				int G1 = (color1 >> 8) & 0xFF;
				int G2 = (color2 >> 8) & 0xFF;
				int R1 = (color1 >> 16) & 0xFF;
				int R2 = (color2 >> 16) & 0xFF;
				int newB = (int)Math.Floor(0.9 * B1) + (int)Math.Floor(0.1 * B2);
				int newG = (int)Math.Floor(0.9 * G1) + (int)Math.Floor(0.1 * G2);
				int newR = (int)Math.Floor(0.9 * R1) + (int)Math.Floor(0.1 * R2);
				uint newColor = (uint)(newB | (newG << 8) | (newR << 16));
				
				if (newColor == mainItem.DbOwnerItem.SocketRGB)
				{
					return true;
				}

				mainItem.DbOwnerItem.SocketRGB = newColor;
				
				DoNormalComposition(player, minorItem, mainItem);
			}
			
			UpdateClient(player, mainItem);
			
			return true;
		}
		
		/// <summary>
		/// Verifies that the composition can be done.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		/// <param name="mainItem">The main item.</param>
		/// <param name="minorItem">The minor item.</param>
		/// <returns>True if the composition can be done, false otherwise.</returns>
		private static bool VerifyComposition(Models.Entities.Player player, Models.Packets.Items.CompositionPacket packet,
		                                      out Models.Items.Item mainItem, out Models.Items.Item minorItem)
		{
			mainItem = null;
			minorItem = null;
			
			if (!player.Alive)
			{
				return false;
			}
			
			if (!player.Inventory.TryGetItem(packet.MainItemClientId, out mainItem))
			{
				player.SendSystemMessage("COMPOSITION_ITEM_NOT_FOUND");
				return false;
			}
			
			if (!player.Inventory.TryGetItem(packet.MinorItemClientId, out minorItem))
			{
				player.SendSystemMessage("COMPOSITION_ITEM_NOT_FOUND");
				return false;
			}
			
			if (mainItem.IsGarment || mainItem.IsArrow || mainItem.IsBottle || mainItem.IsMountArmor || mainItem.IsMisc)
			{
				player.SendSystemMessage("COMPOSITION_ITEM_INVALID");
				return false;
			}
			
			if (mainItem.DbOwnerItem.CurrentDura < mainItem.DbOwnerItem.MaxDura)
			{
				player.SendSystemMessage("COMPOSITION_ITEM_LOW_DURA");
				return false;
			}
			
			if (mainItem.DbOwnerItem.Plus >= Data.Constants.GameMode.MaxPlus)
			{
				player.SendSystemMessage("COMPOSITION_ITEM_MAX_PLUS");
				return false;
			}
			
			if (minorItem.DbOwnerItem.Plus == 0)
			{
				player.SendSystemMessage("COMPOSITION_MINOR_ITEM_NO_PLUS");
				return false;
			}
			
			if (minorItem.DbOwnerItem.Plus < mainItem.DbOwnerItem.Plus)
			{
				player.SendSystemMessage("COMPOSITION_MINOR_ITEM_LOW_PLUS");
				return false;
			}
			
			return true;
		}
		
		/// <summary>
		/// Does a normal composition by adding plus.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="minorItem">The minor item.</param>
		/// <param name="mainItem">The main item.</param>
		private static void DoNormalComposition(Models.Entities.Player player, Models.Items.Item minorItem, Models.Items.Item mainItem)
		{
			player.AddActionLog("Composition", mainItem.DbOwnerItem.Id);
			
			if (player.Inventory.Remove(minorItem.ClientId))
			{
				if (mainItem.DbOwnerItem.Composition > 0)
				{
					mainItem.DbOwnerItem.Plus++;
					mainItem.DbOwnerItem.Composition = 0;
				}
				else
				{
					mainItem.DbOwnerItem.Composition = 1;
				}
				
				mainItem.UpdateDatabase();
			}
		}
		
		/// <summary>
		/// Updates the client with the changes.
		/// </summary>
		/// <param name="player">The player composing.</param>
		/// <param name="mainItem">The main item for the composition.</param>
		/// <remarks>It's necessary to remove and add to support the old composition way.</remarks>
		private static void UpdateClient(Models.Entities.Player player, Models.Items.Item mainItem)
		{
			if (mainItem != null)
			{
				player.ClientSocket.Send(new Models.Packets.Items.ItemActionPacket
				                         {
				                         	Action = Enums.ItemAction.Remove,
				                         	ClientId = mainItem.ClientId
				                         });
				mainItem.UpdateClient(player, Enums.ItemUpdateAction.Add);
			}
		}
	}
}
