// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Message
{
	/// <summary>
	/// Controller helper for Whisper.
	/// </summary>
	[ApiController()]
	public static class Whisper
	{
		/// <summary>
		/// Handles Whisper
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.MessagePacket,
		         SubIdentity = (uint)Enums.MessageType.Whisper)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Message.MessagePacket packet)
		{
			if (packet.To == player.Name)
			{
				return true;
			}
			
			var whisperPlayer = Collections.PlayerCollection.GetPlayerByName(packet.To);
			if (whisperPlayer != null)
			{
				packet.FromMesh = player.Mesh;
				packet.ToMesh = whisperPlayer.Mesh;
				
				whisperPlayer.ClientSocket.Send(packet);
			}
			else
			{
				player.SendFormattedSystemMessage("PLAYER_NOT_FOUND_WHISPER", false, packet.To);
				
				if (Database.Dal.Players.GetPlayerByName(packet.To, Drivers.Settings.WorldSettings.Server) != null)
				{
					var dbWhisper = (new Database.Models.DbWhisper
					                 {
					                 	From = player.Name,
					                 	To = packet.To,
					                 	Message = packet.Message,
					                 	Mesh = player.Mesh,
					                 	Server = Drivers.Settings.WorldSettings.Server
					                 })
						.Create();
				}
			}
			
			return true;
		}
	}
}
