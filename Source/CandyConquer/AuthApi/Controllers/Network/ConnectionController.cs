// Project by Bauss
using System;
using CandyConquer.ApiServer;
using CandyConquer.AuthApi.Models.Client;

namespace CandyConquer.AuthApi.Controllers.Network
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
		public static void HandleNewConnection(Client<AuthClient> client)
		{
			Console.WriteLine(Drivers.Messages.SOCKET_CONNECT, client.IPAddress);
			
			client.Cryptography = new Security.Cryptography.Auth.AuthCrypto();
			client.Data = new AuthClient(client);
			client.BeginReceive();
			
			client.Data.PasswordSeed = (uint)Drivers.Repositories.Safe.Random.Next(1000000, 9999999);
			var passwordSeedPacket = new Models.Packets.PasswordSeedPacket();
			passwordSeedPacket.Seed = client.Data.PasswordSeed;
			client.Send(passwordSeedPacket);
		}
		
		/// <summary>
		/// Handler for disconnections.
		/// </summary>
		/// <param name="client">The disconnected client.</param>
		public static void HandleDisconnection(AuthClient client)
		{
			string identity = client.Authenticated ? client.Account : client.ClientSocket.IPAddress;
			
			Console.WriteLine(Drivers.Messages.SOCKET_DISCONNECT_MESSAGE, identity, client.ClientSocket.DisconnectReason);
		}
	}
}
