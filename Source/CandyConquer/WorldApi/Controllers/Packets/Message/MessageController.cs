// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Packets.Message;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Controllers.Packets.Message
{
	/// <summary>
	/// Controller for the message packet.
	/// </summary>
	[ApiController()]
	public static class MessageController
	{
		/// <summary>
		/// Creates a message packet.
		/// </summary>
		/// <param name="messageType">The type.</param>
		/// <param name="sender">The sender.</param>
		/// <param name="to">The receiver.</param>
		/// <param name="message">The message.</param>
		/// <param name="senderMesh">The senders mesh.</param>
		/// <param name="toMesh">The receivers mesh.</param>
		/// <param name="color">The color.</param>
		/// <param name="timestamp">The timestamp.</param>
		/// <returns>The message packet.</returns>
		public static MessagePacket Create(Enums.MessageType messageType, string sender, string to, string message,
		                                   uint senderMesh = 0, uint toMesh = 0,
		                                   uint color = 0xffffffff, uint timestamp = 0)
		{
			return new MessagePacket
			{
				Color = color,
				MessageType = messageType,
				Timestamp = timestamp,
				FromMesh = senderMesh,
				ToMesh = toMesh,
				From = sender,
				To = to,
				Message = message
			};
		}
		
		/// <summary>
		/// Creates a system message packet.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="to">The receiver.</param>
		/// <returns>The message packet.</returns>
		public static MessagePacket CreateSystem(string message, string to = "ALLUSERS")
		{
			return Create(Enums.MessageType.System, "SYSTEM", to, message, color: 0xff000000);
		}
		
		/// <summary>
		/// Creates a system message packet.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="to">The receiver.</param>
		/// <returns>The message packet.</returns>
		public static MessagePacket CreateSystemTopLeft(string message, string to = "ALLUSERS")
		{
			return Create(Enums.MessageType.TopLeft, "SYSTEM", to, message, color: 0xff000000);
		}
		
		/// <summary>
		/// Creates a system message shown at bottom-left.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="to">The receiver.</param>
		/// <returns>The message packet.</returns>
		public static MessagePacket CreateSystemTalk(string message, string to = "ALLUSERS")
		{
			return Create(Enums.MessageType.Talk, "SYSTEM", to, message, color: 0xff000000);
		}
		
		/// <summary>
		/// Creates a login message packet.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns>The message packet.</returns>
		public static MessagePacket CreateLogin(string message)
		{
			return Create(Enums.MessageType.Login, "SYSTEM", "ALLUSERS", message);
		}
		
		/// <summary>
		/// Creates a new player message packet.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns>The message packet.</returns>
		public static MessagePacket CreateNewPlayer(string message)
		{
			return Create(Enums.MessageType.CharacterCreation, "SYSTEM", "ALLUSERS", message);
		}
		
		/// <summary>
		/// Creates a whisper message packet.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="sender">The sender.</param>
		/// <param name="to">The receiver.</param>
		/// <param name="senderMesh">The sender mesh.</param>
		/// <param name="toMesh">The receiver mesh.</param>
		/// <returns>The message packet.</returns>
		public static MessagePacket CreateWhisper(string message, string sender, string to, uint senderMesh, uint toMesh)
		{
			return Create(Enums.MessageType.Whisper, sender, to, message, senderMesh, toMesh, 0xffffffff);
		}
		
		/// <summary>
		/// Creates a center message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="sender">The sender.</param>
		/// <param name="to">The receiver.</param>
		/// <param name="senderMesh">The sender mesh.</param>
		/// <param name="toMesh">The receiver mesh.</param>
		/// <returns>The message packet.</returns>
		public static MessagePacket CreateCenter(string message, string sender = null, uint color = 0xffffffff)
		{
			message = !string.IsNullOrWhiteSpace(sender) ? string.Format("{0}: {1}", sender, message) : message;
			
			return Create(Enums.MessageType.Center, "SYSTEM", "ALLUSERS", message, color: 0xffffffff);
		}
		
		/// <summary>
		/// Creates a broadcast message packet.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="message">The message.</param>
		/// <returns>The message packet.</returns>
		public static MessagePacket CreateBroadcast(string sender, string message)
		{
			return Create(Enums.MessageType.Broadcast, sender, "ALL", message, color: 0xffffffff);
		}
		
		/// <summary>
		/// Handles the message packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = Data.Constants.PacketTypes.MessagePacket, TypeReturner = true)]
		public static MessagePacket HandlePacket(Models.Entities.Player player, MessagePacket packet, out uint subPacketId)
		{
			if (packet.From != player.Name)
			{
				CandyConquer.Database.Dal.Accounts.Ban(
					player.DbPlayer.Account, Drivers.Messages.SEND_MESSAGE_FROM_SOMEONE_ELSE,
					CandyConquer.Database.Models.DbAccount.BanRangeType.Perm);
				player.ClientSocket.Disconnect(Drivers.Messages.SEND_MESSAGE_FROM_SOMEONE_ELSE);
				subPacketId = SubCallState.Invalid;
			}
			else if (packet.Message.StartsWith("@") ||
			         packet.Message.StartsWith("/") ||
			         packet.Message.StartsWith("#") ||
			         packet.Message.StartsWith("."))
			{
				if (player.Battle != null)
				{
					subPacketId = SubCallState.DontHandle;
					return packet;
				}
				
				player.AddActionLog("Command", packet.Message);
				Collections.CommandScriptCollection.Invoke(player, packet.Message, packet.Message.Substring(1), packet.Message[0]);
				subPacketId = SubCallState.DontHandle;
			}
			else
			{
				player.AddActionLog("ChatMessage-" + packet.MessageType, string.Format("{0} -> {1} : {2}", packet.From, packet.To, packet.Message));
				subPacketId = (uint)packet.MessageType;
			}
			
			return packet;
		}
	}
}
