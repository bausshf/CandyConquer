// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.AuthApi.Models.Client
{
	/// <summary>
	/// An authentication client.
	/// </summary>
	public class AuthClient
	{
		/// <summary>
		/// Gets the associated client socket.
		/// </summary>
		public Client<AuthClient> ClientSocket { get; private set; }
		/// <summary>
		/// Gets or sets a value indicating whether the client has been authenticated or not.
		/// </summary>
		public bool Authenticated { get; set; }
		/// <summary>
		/// Gets or sets the account id.
		/// </summary>
		public uint AccountId { get; set; }
		/// <summary>
		/// Gets or sets the client id.
		/// </summary>
		public uint ClientId { get; set; }
		/// <summary>
		/// Gets or sets the account name.
		/// </summary>
		public string Account { get; set; }
		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		public string Password { get; set; }
		/// <summary>
		/// Gets or sets the password seed for the client.
		/// </summary>
		public uint PasswordSeed { get; set; }
		
		/// <summary>
		/// Creates a new authentication client.
		/// </summary>
		/// <param name="clientSocket">The associated client socket.</param>
		public AuthClient(Client<AuthClient> clientSocket)
		{
			ClientSocket = clientSocket;
		}
	}
}
