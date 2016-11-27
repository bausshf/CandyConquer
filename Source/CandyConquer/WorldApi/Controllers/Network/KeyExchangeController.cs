// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.ApiServer;
using CandyConquer.Security.Api;
using CandyConquer.Security.Cryptography.World;

namespace CandyConquer.WorldApi.Controllers.Network
{
	/// <summary>
	/// Key exchange controller.
	/// </summary>
	[ApiController()]
	public static class KeyExchangeController
	{
		/// <summary>
		/// Handles the dh key exchange.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="socketPacket">The packet for the key exchange.</param>
		/// <returns></returns>
		[ApiCall(CallSecurity = CallSecurity.Once)]
		public static bool HandleKeyExchange(Models.Entities.Player player, SocketPacket socketPacket)
		{
			byte[] packet = socketPacket;
			
			ushort position = 11;
			int JunkLen = BitConverter.ToInt32(packet, position);
			position += 4;
			position += (ushort)JunkLen;
			int Len = BitConverter.ToInt32(packet, position);
			position += 4;
			byte[] pubKey = new byte[Len];
			for (int x = 0; x < Len; x++)
				pubKey[x] = packet[x + position];
			position += (ushort)Len;
			string PubKey = System.Text.ASCIIEncoding.ASCII.GetString(pubKey);
			
			byte[] key = player.KeyExchange.ComputeKey(OpenSSL.BigNumber.FromHexString(PubKey));
			var crypto = (BlowfishCryptography)player.ClientSocket.Cryptography;
			crypto.SetKey(key);
			crypto.EncryptIV = player.KeyExchange.ClientIv;
			crypto.DecryptIV = player.KeyExchange.ServerIv;
			
			player.Exchanged = true;
			player.ClientSocket.KeyExchange = false;
			player.ClientSocket.Suffix = System.Text.Encoding.ASCII.GetBytes("TQServer");
			return true;
		}
	}
}
