// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.AuthApi.Models.Packets
{
	/// <summary>
	/// Internal auth request packet initialization.
	/// </summary>
	public class AuthRequestPacketInit
	{
		/// <summary>
		/// The password seed.
		/// </summary>
		public uint PasswordSeed { get; set; }
		/// <summary>
		/// The packet.
		/// </summary>
		public NetworkPacket Packet { get; set; }
		
		/// <summary>
		/// Ctor.
		/// </summary>
		internal AuthRequestPacketInit() { }
	}
	
	/// <summary>
	/// The auth request packet.
	/// </summary>
	public class AuthRequestPacket : NetworkPacket
	{
		/// <summary>
		/// Gets the account.
		/// </summary>
		public string Account { get; private set; }
		/// <summary>
		/// Gets the password.
		/// </summary>
		public string Password { get; private set; }
		/// <summary>
		/// Gets the server.
		/// </summary>
		public string Server { get; private set; }
		
		/// <summary>
		/// Creates a new auth request packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <param name="passwordSeed">The password seed.</param>
		private AuthRequestPacket(NetworkPacket packet, uint passwordSeed)
			: base(packet, 4)
		{
			Account = ReadString(16);
			lock (Drivers.Locks.GlobalLock)
			{
				Native.Msvcrt.srand((int)passwordSeed);
				Offset = 132;
				byte[] passBytes = ReadBytes(16);
				byte[] rc5Key = new byte[16];
				for (int i = 0; i < rc5Key.Length; i++)
				{
					rc5Key[i] = (byte)Native.Msvcrt.rand();
				}
				var password = System.Text.Encoding.ASCII.GetString(
					new Security.Cryptography.Auth.PasswordCryptography(Account)
					.Decrypt(
						new Security.Cryptography.Auth.RC5(rc5Key).Decrypt(passBytes),
						16
					)
				).Replace("\0", "");
				password = password.Replace("-", "0");
				password = password.Replace("#", "1");
				password = password.Replace("(", "2");
				password = password.Replace("\"", "3");
				password = password.Replace("%", "4");
				password = password.Replace("\f", "5");
				password = password.Replace("'", "6");
				password = password.Replace("$", "7");
				password = password.Replace("&", "8");
				password = password.Replace("!", "9");
				Password = password;
			}
			Offset = packet.PhysicalSize - 16;
			Server = ReadString(16);
		}
		
		/// <summary>
		/// Implicit conversion from AuthRequestPacketInit.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>AuthRequestPacket.</returns>
		public static implicit operator AuthRequestPacket(AuthRequestPacketInit packet)
		{
			return new AuthRequestPacket(packet.Packet, packet.PasswordSeed);
		}
	}
}
