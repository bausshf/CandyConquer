// Project by Bauss
using System;
using System.Linq;

namespace CandyConquer.ApiServer
{
	/// <summary>
	/// Packet logger.
	/// </summary>
	public static class PacketLogger
	{
		/// <summary>
		/// List of packet ids to ignore.
		/// </summary>
		private static ushort[] ignoreList = new ushort[]
		{
			1100,
			1052
		};
		
		/// <summary>
		/// Logs a packet.
		/// </summary>
		/// <param name="packet">The packet to log.</param>
		/// <param name="logBody">If set to true then the packet dump is logged too.</param>
		/// <param name="logSub">If set to true then the packet sub type is logged too.</param>
		/// <param name="subObject">An object or value to display instead of the sub type numeric value.</param>
		public static void Log(NetworkPacket packet, bool logBody, bool logSub = false, object subObject = null)
		{
			if (ignoreList.Contains(packet.PacketType))
			{
				return;
			}
			
			if (logSub)
			{
				Console.WriteLine("[Packet: {0}:{1} Physical-Size: {2} Virtual-Size: {3}]",
				                  packet.PacketType,
				                  subObject == null ? packet.PacketSubType : subObject,
				                  packet.PhysicalSize, packet.VirtualSize);
			}
			else
			{
				Console.WriteLine("[Packet: {0} Physical-Size: {1} Virtual-Size: {2}]",
				                  packet.PacketType,
				                  packet.PhysicalSize, packet.VirtualSize);
			}
			
			if (logBody)
			{
				Console.WriteLine(packet.ToString());
			}
		}
	}
}
