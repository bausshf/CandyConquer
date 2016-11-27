// Project by Bauss
using System;
using System.Linq;
using CandyConquer.WorldApi.Models.Packets.Login;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.ApiServer;
using CandyConquer.Security.Api;
using CandyConquer.Drivers;

namespace CandyConquer.WorldApi.Controllers.Packets.Login
{
	/// <summary>
	/// Controller for the world authentication packet.
	/// </summary>
	[ApiController()]
	public static class WorldAuthenticationController
	{
		/// <summary>
		/// Handles the world authentication packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Once, Identity = 1052)]
		public static bool HandlePacket(Models.Entities.Player player, WorldAuthenticationPacket packet)
		{
			player.ClientId = packet.ClientId;
			
			var dbPlayer = Database.Dal.Players.GetPlayerByAuthentication(player.ClientId.ToString("X"), Drivers.Settings.WorldSettings.Server, player.ClientSocket.IPAddress);
			if (dbPlayer == null)
			{
				player.ClientSocket.Disconnect(Drivers.Messages.Errors.INVALID_AUTHKEY_OR_PROXY);
				return false;
			}
			
			if (packet.Key != 2)
			{
				Database.Dal.Accounts.Ban(dbPlayer.Account, Drivers.Messages.AUTHENTICATION_BYPASS, Database.Models.DbAccount.BanRangeType.Perm);
				return false;
			}
			
			var alreadyLoggedIn = Collections.PlayerCollection.GetPlayerById(dbPlayer.Id).ToList();
			if (alreadyLoggedIn.Count > 0)
			{
				alreadyLoggedIn.Add(player);
				foreach (var loggedPlayer in alreadyLoggedIn)
				{
					loggedPlayer.ClientSocket.Disconnect(Drivers.Messages.Errors.MULTI_LOGIN);
				}
				return false;
			}
			
			player.ClientSocket.Send(new Models.Packets.Login.UnknownPacket_2079());
			player.ClientSocket.Send(new Models.Packets.Login.UnknownPacket_2078());
			player.DbPlayer = dbPlayer;
			player.DbPlayer.CanWrite = false;
			
			if (dbPlayer.New)
			{
				player.ClientSocket.Send(Controllers.Packets.Message.MessageController.CreateLogin("NEW_ROLE"));
			}
			else
			{
				player.Job = dbPlayer.Job.ToEnum<Enums.Job>();
				player.Title = dbPlayer.Title.ToEnum<Enums.PlayerTitle>();
				player.Permission = dbPlayer.Permission.ToEnum<Enums.PlayerPermission>();
				
				if (Collections.PlayerCollection.TryAdd(player))
				{
					player.UpdateStats();
					
					player.ClientSocket.Send(Controllers.Packets.Message.MessageController.CreateLogin("ANSWER_OK"));
					player.ClientSocket.Send(new Models.Packets.Login.CharacterInitPacket
					                         {
					                         	Player = player
					                         });
					player.ClientSocket.Send(new Models.Packets.Login.DateTimePacket());
				}
				else
				{
					player.ClientSocket.Disconnect(Drivers.Messages.Errors.FAILED_TO_ADD_PLAYER_TO_GLOBAL_COLLECTION);
				}
			}
			
			return true;
		}
	}
}
