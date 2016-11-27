// Project by Bauss
using System;

namespace CandyConquer.ApiServer
{
	/// <summary>
	/// A socket packet.
	/// </summary>
	public class SocketPacket : NetworkPacket
	{
		/// <summary>
		/// Creates a new socket packet.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name="offset">The offset.</param>
		internal SocketPacket(byte[] buffer, int offset = 0)
			: base(buffer, offset)
		{
			
		}
	}
}
