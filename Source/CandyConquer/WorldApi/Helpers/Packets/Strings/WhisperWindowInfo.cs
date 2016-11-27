// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Strings
{
	/// <summary>
	/// Controller for whisper window info.
	/// </summary>
	[ApiController()]
	public static class WhisperWindowInfo
	{
		/// <summary>
		/// Handles whisper window info.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.StringPacket,
		         SubIdentity = (uint)Enums.StringAction.WhisperWindowInfo)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Misc.StringPacket packet)
		{
			var whisperPlayer = Collections.PlayerCollection.GetPlayerByName(packet.Strings[0]);
			if (whisperPlayer != null)
			{
				var guildName = whisperPlayer.Guild != null ? whisperPlayer.Guild.DbGuild.Name : string.Empty;
				
				string infoString = whisperPlayer.ClientId + " ";
				infoString += whisperPlayer.Level + " ";
				infoString += whisperPlayer.Level + " ";//battle power
				infoString += "~" + guildName + " ";
				infoString += "# ";//unknown, family ??
				infoString += whisperPlayer.Spouse + " ";
				infoString += 0 + " ";//unknown
				if (whisperPlayer.Mesh % 10 < 3)
					infoString += "1 ";
				else
					infoString += "0 ";
				
				var stringPacket = new Models.Packets.Misc.StringPacket
				{
					Action = Enums.StringAction.WhisperWindowInfo
				};
				stringPacket.Strings.Add(whisperPlayer.Name);
				stringPacket.Strings.Add(infoString);
				player.ClientSocket.Send(stringPacket);
			}
			
			return true;
		}
	}
}
