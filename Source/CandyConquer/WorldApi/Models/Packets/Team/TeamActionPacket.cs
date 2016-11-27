// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Team
{
	/// <summary>
	/// Model for the team action packet.
	/// </summary>
	public sealed class TeamActionPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the team action.
		/// </summary>
		public Enums.TeamAction Action { get; set; }
		
		/// <summary>
		/// Gets or sets the client id.
		/// </summary>
		public uint ClientId { get; set; }
		
		/// <summary>
		/// Creates a new team action packet.
		/// </summary>
		public TeamActionPacket()
			: base(20, Data.Constants.PacketTypes.TeamActionPacket)
		{
		}
		
		/// <summary>
		/// Creates a new team action packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private TeamActionPacket(NetworkPacket packet)
			: base(packet, 4)
		{
			Action = (Enums.TeamAction)ReadUInt32();
			ClientId = ReadUInt32();
			
			SubTypeObject = Action;
		}
		
		/// <summary>
		/// Implicit conversion from the TeamActionPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](TeamActionPacket packet)
		{
			packet.Offset = 4;
			
			packet.WriteUInt32((uint)packet.Action);
			packet.WriteUInt32(packet.ClientId);
			
			return packet.Buffer;
		}
		
		/// <summary>
		/// Implicit conversion from socketp acket to the TeamActionPacket packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator TeamActionPacket(SocketPacket packet)
		{
			return new TeamActionPacket(packet);
		}
	}
}
