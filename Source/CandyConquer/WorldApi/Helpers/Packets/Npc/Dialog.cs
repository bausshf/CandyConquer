// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Helpers.Packets.Npc
{
	/// <summary>
	/// Dialog helper for npc dialogs.
	/// </summary>
	public static class Dialog
	{
		/// <summary>
		/// Sends a dialog text.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="text">The text.</param>
		public static void SendDialog(Models.Entities.Player player, string text)
		{
			player.ClientSocket.Send(new Models.Packets.Entities.NpcResponsePacket
			                         {
			                         	Text = text,
			                         	Action = Enums.NpcDialogAction.Text,
			                         	Option = 255
			                         });
		}
		
		/// <summary>
		/// Sends a dialog option.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="text">The text.</param>
		/// <param name="option">The option.</param>
		public static void SendOption(Models.Entities.Player player, string text, byte option)
		{
			player.ClientSocket.Send(new Models.Packets.Entities.NpcResponsePacket
			                         {
			                         	Text = text,
			                         	Action = Enums.NpcDialogAction.Link,
			                         	Option = option
			                         });
		}
		
		/// <summary>
		/// Sends a dialog input.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="option">The option.</param>
		public static void SendInput(Models.Entities.Player player, byte option)
		{
			player.ClientSocket.Send(new Models.Packets.Entities.NpcResponsePacket
			                         {
			                         	Data = 16,
			                         	Action = Enums.NpcDialogAction.Edit,
			                         	Option = option
			                         });
		}
		
		/// <summary>
		/// Finishes the dialog.
		/// </summary>
		/// <param name="player">The player.</param>
		public static void Finish(Models.Entities.Player player)
		{
			player.ClientSocket.Send(new Models.Packets.Entities.NpcResponsePacket
			                         {
			                         	Data = player.CurrentNpc.Avatar,
			                         	Action = Enums.NpcDialogAction.Pic
			                         });
			player.ClientSocket.Send(new Models.Packets.Entities.NpcResponsePacket
			                         {
			                         	Option = 255,
			                         	Action = Enums.NpcDialogAction.Create
			                         });
		}
		
		/// <summary>
		/// Opens a window.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="subtype">The window subtype.</param>
		public static void OpenWindow(Models.Entities.Player player, uint subtype)
		{
			player.AddActionLog("OpenWindow", subtype);
			
			player.ClientSocket.Send(new Models.Packets.Client.DataExchangePacket
			                         {
			                         	ExchangeType = Enums.ExchangeType.OpenDialog,
			                         	ClientId = player.ClientId,
			                         	Data1 = subtype,
			                         	Data2Low = player.X,
			                         	Data2High = player.Y
			                         });
		}
		
		/// <summary>
		/// Opens an upgrade.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="subtype">The upgrade subtype.</param>
		/// <param name="timestamp">The timestamp.</param>
		public static void OpenUpgrade(Models.Entities.Player player, uint subtype, uint timestamp = 0)
		{
			player.AddActionLog("OpenUpgrade", subtype);
			
			player.ClientSocket.Send(new Models.Packets.Client.DataExchangePacket
			                         {
			                         	ExchangeType = Enums.ExchangeType.OpenUpgrade,
			                         	ClientId = player.ClientId,
			                         	Data1 = subtype,
			                         	Data2Low = player.X,
			                         	Data2High = player.Y,
			                         	Timestamp = timestamp
			                         });
		}
		
		public static void OpenShop(Models.Entities.Player player, uint shopId)
		{
			player.AddActionLog("OpenShop", shopId);
			OpenUpgrade(player, 32, shopId);
		}
	}
}
