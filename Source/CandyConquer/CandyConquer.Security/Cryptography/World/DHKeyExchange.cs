// Project by Bauss
using System;
using System.IO;

namespace CandyConquer.Security.Cryptography.World
{
	/// <summary>
	/// DHKeyExchange wrapper.
	/// </summary>
	public class DHKeyExchange
	{
		/// <summary>
		/// The key exchange.
		/// </summary>
		private OpenSSL.DH _keyExchange;
		/// <summary>
		/// The server Iv.
		/// </summary>
		private byte[] _serverIv;
		/// <summary>
		/// The client Iv.
		/// </summary>
		private byte[] _clientIv;
		/// <summary>
		/// P
		/// </summary>
		private string P;
		/// <summary>
		/// G
		/// </summary>
		private string G;
		
		/// <summary>
		/// Gets the server Iv.
		/// </summary>
		public byte[] ServerIv { get { return _serverIv; } }
		
		/// <summary>
		/// Gets the client Iv.
		/// </summary>
		public byte[] ClientIv { get { return _clientIv; } }
		
		/// <summary>
		/// Creates a new DHKeyExchange.
		/// </summary>
		/// <param name="serverIv">The server Iv.</param>
		/// <param name="clientIv">The clientIv.</param>
		public DHKeyExchange(byte[] serverIv, byte[] clientIv)
		{
			_clientIv = clientIv != null ? clientIv : new byte[8];
			_serverIv = serverIv != null ? serverIv : new byte[8];
			
			P = "E7A69EBDF105F2A6BBDEAD7E798F76A209AD73FB466431E2E7352ED262F8C558F10BEFEA977DE9E21DCEE9B04D245F300ECCBBA03E72630556D011023F9E857F";
			G = "05";
			
			_keyExchange = new OpenSSL.DH(OpenSSL.BigNumber.FromHexString(P), OpenSSL.BigNumber.FromHexString(G));
			_keyExchange.GenerateKeys();
		}
		
		/// <summary>
		/// Computes the key.
		/// </summary>
		/// <param name="pubkey">The public key.</param>
		/// <returns>The computed key.</returns>
		public byte[] ComputeKey(OpenSSL.BigNumber pubkey)
		{
			return _keyExchange.ComputeKey(pubkey);
		}
		
		/// <summary>
		/// Creates the key packet from the server.
		/// </summary>
		/// <returns>The packet.</returns>
		public byte[] CreateServerKeyPacket()
		{
			return GeneratePacket(_serverIv, _clientIv, P, G, _keyExchange.PublicKey.ToHexString());
		}
		
		/// <summary>
		/// Generates the packet.
		/// </summary>
		/// <param name="ServerIV1">The serverIV1</param>
		/// <param name="ServerIV2">The serverIV2</param>
		/// <param name="P">The P.</param>
		/// <param name="G">The G.</param>
		/// <param name="ServerPublicKey">The server public key.</param>
		/// <returns></returns>
		private byte[] GeneratePacket(byte[] ServerIV1, byte[] ServerIV2, string P, string G, string ServerPublicKey)
		{
			int PAD_LEN = 11;
			int _junk_len = 12;
			string tqs = "TQServer";
			using (var ms = new MemoryStream())
			{
				byte[] pad = new byte[PAD_LEN];
				CryptographicRandom.NextBytes(pad);
				byte[] junk = new byte[_junk_len];
				CryptographicRandom.NextBytes(junk);
				int size = 47 + P.Length + G.Length + ServerPublicKey.Length + 12 + 8 + 8;
				using (var bw = new BinaryWriter(ms))
				{
					bw.Write(pad);
					bw.Write(size - PAD_LEN);
					bw.Write((UInt32)_junk_len);
					bw.Write(junk);
					bw.Write((UInt32)ServerIV2.Length);
					bw.Write(ServerIV2);
					bw.Write((UInt32)ServerIV1.Length);
					bw.Write(ServerIV1);
					bw.Write((UInt32)P.Length);
					foreach (var fP in P)
					{
						bw.BaseStream.WriteByte((byte)fP);
					}
					bw.Write((UInt32)G.ToCharArray().Length);
					foreach (var fG in G)
					{
						bw.BaseStream.WriteByte((byte)fG);
					}
					bw.Write((UInt32)ServerPublicKey.ToCharArray().Length);
					foreach (var SPK in ServerPublicKey)
					{
						bw.BaseStream.WriteByte((byte)SPK);
					}
					foreach (var tq in tqs)
					{
						bw.BaseStream.WriteByte((byte)tq);
					}
					byte[] Packet = new byte[ms.Length];
					Packet = ms.ToArray();
					return Packet;
				}
			}
		}
	}
}
