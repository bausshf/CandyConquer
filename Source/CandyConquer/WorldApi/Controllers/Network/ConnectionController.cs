// Project by Bauss
using System;
using System.Linq;
using Candy;
using CandyConquer.ApiServer;
using CandyConquer.Security.Cryptography.World;

namespace CandyConquer.WorldApi.Controllers.Network
{
	/// <summary>
	/// Connection controller.
	/// </summary>
	public static class ConnectionController
	{
		/// <summary>
		/// Handler for new connections.
		/// </summary>
		/// <param name="client">The connected client.</param>
		public static void HandleNewConnection(Client<Models.Entities.Player> client)
		{
			Console.WriteLine(Drivers.Messages.SOCKET_CONNECT, client.IPAddress);
			
			client.Data = new Models.Entities.Player(client);
			
			var blowfish = new BlowfishCryptography(BlowfishCryptography.BlowfishAlgorithm.CFB64);
			blowfish.SetKey(Drivers.Settings.WorldSettings.CryptoKey);
			client.Data.ClientSocket.Cryptography = blowfish;

			client.Data.KeyExchange = new DHKeyExchange(null, null);
			client.Send(client.Data.KeyExchange.CreateServerKeyPacket());
			
			client.KeyExchange = true;
			client.BeginReceive();
		}
		
		/// <summary>
		/// Handler for disconnections.
		/// </summary>
		/// <param name="client">The disconnected client.</param>
		public static void HandleDisconnection(Models.Entities.Player player)
		{
			Collections.PlayerCollection.Remove(player);
			
			if (player.Map != null)
			{
				if (player.Map.IsDynamic)
				{
					player.TeleportToLastMap();
				}
				
				// Save coordinates and possible other unsaved changes ...
				player.DbPlayer.Update();
				
				player.Map.RemoveFromMap(player);
				player.ClearScreen();
				
				player.Map = null;
			}
			else if (player.DbPlayer != null)
			{
				// Save unsaved changes ...
				player.DbPlayer.Update();
			}
			
			if (player.Team != null)
			{
				if (player.Team.IsLeader(player))
				{
					var members = player.Team.Delete();
					
					foreach (var member in members)
					{
						member.Team = null;
						
						if (member.ClientId != player.ClientId)
						{
							member.ClientSocket.Send(new Models.Packets.Team.TeamActionPacket
							                         {
							                         	Action = Enums.TeamAction.Dismiss,
							                         	ClientId = player.ClientId
							                         });
						}
					}
				}
				
				player.Team = null;
			}
			
			if (player.Guild != null)
			{
				player.GuildMember.Player = null;
				player.GuildMember = null;
				player.Guild = null;
			}
			
			if (player.Nobility != null)
			{
				player.Nobility.Player = null;
				player.Nobility = null;
			}
			
			if (player.Battle != null)
			{
				player.Battle.HandleDisconnect(player);
			}
			
			if (player.ArenaInfo != null)
			{
				player.ArenaInfo.Player = null;
				player.ArenaInfo = null;
			}
			
			if (player.LoggedIn)
			{
				var traceId = player.ClientId.ToString("X2") + ";" + DateTime.UtcNow.ToBinary().ToString("X2");
				player.ClientSocket.PacketTrace.Clone()
					.Select(packet =>
					        {
					        	return new Database.Models.DbPacketTrace
					        	{
					        		PacketId = (int)packet.PacketType,
					        		PacketSubObject = packet.SubTypeObject != null ? packet.SubTypeObject.ToString() : string.Empty,
					        		OwnerId = player.DbPlayer.Id,
					        		TraceId = traceId,
					        		Size = packet.PhysicalSize,
					        		VirtualSize = (int)packet.VirtualSize,
					        		Buffer = (byte[])packet
					        	};
					        }).Create();
				(new Database.Models.DbDisconnectTrace
				{
					TraceId = traceId,
					OwnerId = player.DbPlayer.Id,
					DisconnectReason = player.ClientSocket.LastException != null ?
						player.ClientSocket.LastException.ToString() :
						player.ClientSocket.DisconnectReason
				 }).Create();
			}
			
			Console.WriteLine(Drivers.Messages.SOCKET_DISCONNECT_MESSAGE, player.ClientSocket.IPAddress, player.ClientSocket.DisconnectReason);
			player.AddActionLog("Disconnect");
			
			player.Free();
		}
	}
}
