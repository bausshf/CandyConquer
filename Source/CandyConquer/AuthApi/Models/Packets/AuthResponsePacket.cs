// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.AuthApi.Models.Packets
{
	/// <summary>
	/// The auth response packet.
	/// </summary>
	public class AuthResponsePacket : NetworkPacket
	{
		/// <summary>
		/// The client id.
		/// </summary>
		public uint ClientId { get; set; }
		/// <summary>
		/// The authentication status.
		/// </summary>
		public Enums.AuthenticationStatus Status { get; set; }
		/// <summary>
		/// The port.
		/// </summary>
		public int Port { get; set; }
		/// <summary>
		/// The hash.
		/// </summary>
		public uint Hash { get; set; }
		/// <summary>
		/// The IP address.
		/// </summary>
		public string IPAddress { get; set; }
		
		/// <summary>
		/// Creates a new auth response packet.
		/// </summary>
		public AuthResponsePacket()
			: base(52, 1055)
		{
			Status = Enums.AuthenticationStatus.FailedToLogin;
			IPAddress = string.Empty;
		}
		
		/// <summary>
		/// Resets the response.
		/// </summary>
		public void Reset()
		{
			ClientId = 0;
			Status = Enums.AuthenticationStatus.FailedToLogin;
			Port = 0;
			Hash = 0;
			IPAddress = string.Empty;
		}
		
		/// <summary>
		/// Implicit conversion from AuthRequestResponsePacket to byte[].
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>byte[]</returns>
		public static implicit operator byte[](AuthResponsePacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32(packet.ClientId);
			packet.WriteUInt32((uint)packet.Status);
			packet.WriteInt32(packet.Port);
			packet.WriteUInt32(packet.Hash);
			packet.WriteString(packet.IPAddress);
			
			return packet.Buffer;
		}
	}
}
