// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;
using CandyConquer.Database.Dal;
using CandyConquer.Database.Models;
using CandyConquer.Debugging;
using CandyConquer.ApiServer;

namespace CandyConquer.AuthApi.Controllers.Packets
{
	/// <summary>
	/// Controller for the auth request packet.
	/// </summary>
	[ApiController()]
	public static class AuthRequestController
	{
		/// <summary>
		/// The handler for the auth request packet. (Type 1060)
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Once, Identity = 1060)]
		public static bool HandlePacket_1060(Models.Client.AuthClient client, SocketPacket packet)
		{
			return Handle(client,
			       new Models.Packets.AuthRequestPacketInit
			       {
			       	PasswordSeed = client.PasswordSeed,
			       	Packet = packet
			       });
		}
		
		/// <summary>
		/// The handler for the auth request packet. (Type 1086)
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Once, Identity = 1086)]
		public static bool HandlePacket_1086(Models.Client.AuthClient client, SocketPacket packet)
		{
			return Handle(client,
			       new Models.Packets.AuthRequestPacketInit
			       {
			       	PasswordSeed = client.PasswordSeed,
			       	Packet = packet
			       });
		}
		
		/// <summary>
		/// The handler for the auth request packet.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="packet">The packet.</param>
		private static bool Handle(Models.Client.AuthClient client, Models.Packets.AuthRequestPacket packet)
		{
			client.Account = packet.Account;
			client.Password = packet.Password;
			var server = Repositories.Server.ServerList.GetServerInfo(packet.Server);
			var canLogin = server != null;
			var response = new Models.Packets.AuthResponsePacket();
			
			if (canLogin)
			{
				try
				{
					var dbAccount = Accounts.GetAccountByUserNameAndPassword(client.Account, client.Password);
					if (dbAccount == null)
					{
						response.Status = Enums.AuthenticationStatus.InvalidAccountIDOrPassword;
					}
					else
					{
						client.Authenticated = true;
						if (dbAccount.Banned && dbAccount.BanDate.HasValue)
						{
							bool shouldUnban = false;
							switch (dbAccount.BanRange)
							{
								case DbAccount.BanRangeType.OneDay:
									{
										shouldUnban = DateTime.Now >= dbAccount.BanDate.Value.AddDays(1);
										break;
									}
								case DbAccount.BanRangeType.ThreeDays:
									{
										shouldUnban = DateTime.Now >= dbAccount.BanDate.Value.AddDays(3);
										break;
									}
								case DbAccount.BanRangeType.OneWeek:
									{
										shouldUnban = DateTime.Now >= dbAccount.BanDate.Value.AddDays(7);
										break;
									}
								case DbAccount.BanRangeType.OneMonth:
									{
										shouldUnban = DateTime.Now >= dbAccount.BanDate.Value.AddMonths(1);
										break;
									}
								case DbAccount.BanRangeType.ThreeMonths:
									{
										shouldUnban = DateTime.Now >= dbAccount.BanDate.Value.AddMonths(3);
										break;
									}
								case DbAccount.BanRangeType.SixMonths:
									{
										shouldUnban = DateTime.Now >= dbAccount.BanDate.Value.AddMonths(6);
										break;
									}
								case DbAccount.BanRangeType.OneYear:
									{
										shouldUnban = DateTime.Now >= dbAccount.BanDate.Value.AddYears(1);
										break;
									}
							}
							
							canLogin = shouldUnban;
							if (shouldUnban)
							{
								dbAccount.Banned = false;
								dbAccount.Update();
							}
							else
							{
								response.Status = Enums.AuthenticationStatus.AccountBanned;
							}
						}
						
						if (canLogin)
						{
							client.ClientId = Drivers.Repositories.Safe.IdentityGenerator.GetClientId();
							
							response.ClientId = client.ClientId;
							response.IPAddress = server.IPAddress;
							response.Port = server.Port;
							response.Status = Enums.AuthenticationStatus.Ready;
							
							if (string.IsNullOrWhiteSpace(dbAccount.FirstLoginIP))
							{
								dbAccount.FirstLoginIP = client.ClientSocket.IPAddress;
								dbAccount.FirstLoginDate = DateTime.Now;
							}
							
							dbAccount.LastServer = server.Name;
							dbAccount.LastIP = client.ClientSocket.IPAddress;
							dbAccount.LastAuthKey = client.ClientId.ToString("X") + ";" + DateTime.Now.ToBinary();
							dbAccount.LastLoginDate = DateTime.Now;
							dbAccount.Update();
							
							var dbPlayer = Players.GetPlayerByAccount(dbAccount);
							if (dbPlayer == null)
							{
								dbPlayer = Players.Create(dbAccount);
							}
							dbPlayer.AuthKey = dbAccount.LastAuthKey;
							dbPlayer.Update();
						}
					}
				}
				catch (Exception e)
				{
					response.Reset();
					response.Status = Enums.AuthenticationStatus.DatebaseError;
					
					#if DEBUG
					#if TRACE
					ErrorLogger.Log(StackTracing.GetCurrentMethod().Name, e);
					#else
					ErrorLogger.Log(Drivers.Messages.Errors.DB_ERROR, e);
					#endif
					#else
					Global.Message(e.ToString());
					#endif
				}
			}
			
			client.ClientSocket.Send(response);
			return false;
		}
	}
}
