// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Guilds
{
	/// <summary>
	/// Controller for querying.
	/// </summary>
	[ApiController()]
	public static class Query
	{
		/// <summary>
		/// Handles the query syndicate attribute.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.GuildPacket,
		         SubIdentity = (uint)Enums.GuildAction.QuerySyndicateAttribute)]
		public static bool QueryAttribute(Models.Entities.Player player, Models.Packets.Guilds.GuildPacket packet)
		{
			if (player.Guild != null)
			{
				player.UpdateClientGuild();
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the query syndicate name.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.GuildPacket,
		         SubIdentity = (uint)Enums.GuildAction.QuerySyndicateName)]
		public static bool QueryName(Models.Entities.Player player, Models.Packets.Guilds.GuildPacket packet)
		{
			Models.Guilds.Guild guild;
			if (Collections.GuildCollection.TryGetGuild((int)packet.Data, out guild))
			{
				player.ClientSocket.Send(new Models.Packets.Misc.StringPacket
				                         {
				                         	Action = Enums.StringAction.Guild,
				                         	Data = packet.Data,
				                         	String = guild.StringInfo
				                         });
				return true;
			}
			
			return false;
		}
	}
}
