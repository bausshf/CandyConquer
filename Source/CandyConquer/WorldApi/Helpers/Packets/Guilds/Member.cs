// Project by Bauss
using System;
using System.Linq;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Guilds
{
	/// <summary>
	/// Controller for member.
	/// </summary>
	[ApiController()]
	public static class Member
	{
		/// <summary>
		/// Handles the invite join.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.GuildPacket,
		         SubIdentity = (uint)Enums.GuildAction.InviteJoin)]
		public static bool HandleInviteJoin(Models.Entities.Player player, Models.Packets.Guilds.GuildPacket packet)
		{
			if (player.Battle != null)
			{
				return true;
			}
			
			if (packet.Data == player.ClientId)
			{
				return false;
			}
			
			if (player.Guild == null)
			{
				return false;
			}
			
			if (player.GuildMember.Rank != Enums.GuildRank.GuildLeader &&
			    player.GuildMember.Rank != Enums.GuildRank.DeputyLeader)
			{
				return true;
			}
			
			Models.Maps.IMapObject mapObject = null;
				
			if (player.ApplyGuildMemberClientId == 0)
			{
				if (player.GetFromScreen(packet.Data, out mapObject))
				{
					var invitePlayer = mapObject as Models.Entities.Player;
					if (invitePlayer != null)
					{
						if (invitePlayer.Guild == null)
						{
							invitePlayer.ApplyGuildMemberClientId = player.ClientId;
							
							invitePlayer.ClientSocket.Send(new Models.Packets.Guilds.GuildPacket
							                               {
							                               	Action = Enums.GuildAction.InviteJoin,
							                               	Data = player.ClientId
							                               });
						}
					}
				}
				return true;
			}
			
			if (player.ApplyGuildMemberClientId != packet.Data)
			{
				return true;
			}
			
			player.ApplyGuildMemberClientId = 0;
			
			if (player.GetFromScreen(packet.Data, out mapObject))
			{
				var newPlayer = mapObject as Models.Entities.Player;
				if (newPlayer != null)
				{
					if (newPlayer.Guild == null)
					{
						player.Guild.AddMember(newPlayer, Enums.GuildRank.Member);
					}
				}
			}

			return true;
		}
		
		/// <summary>
		/// Handles the apply join.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.GuildPacket,
		         SubIdentity = (uint)Enums.GuildAction.ApplyJoin)]
		public static bool HandleApplyJoin(Models.Entities.Player player, Models.Packets.Guilds.GuildPacket packet)
		{
			if (player.Battle != null)
			{
				return true;
			}
			
			if (packet.Data == player.ClientId)
			{
				return false;
			}
			
			if (player.Guild != null)
			{
				return false;
			}
			
			Models.Maps.IMapObject mapObject = null;
			
			if (player.ApplyGuildMemberClientId == 0)
			{
				if (player.GetFromScreen(packet.Data, out mapObject))
				{
					var requestPlayer = mapObject as Models.Entities.Player;
					if (requestPlayer != null)
					{
						if (requestPlayer.Guild != null && (
							requestPlayer.GuildMember.Rank == Enums.GuildRank.GuildLeader ||
							requestPlayer.GuildMember.Rank == Enums.GuildRank.DeputyLeader
						))
						{
							requestPlayer.ApplyGuildMemberClientId = player.ClientId;
							
							requestPlayer.ClientSocket.Send(new Models.Packets.Guilds.GuildPacket
							                                {
							                                	Action = Enums.GuildAction.ApplyJoin,
							                                	Data = player.ClientId
							                                });
						}
					}
				}
				
				return true;
			}
			
			if (player.ApplyGuildMemberClientId != packet.Data)
			{
				return true;
			}
			
			player.ApplyGuildMemberClientId = 0;
			
			if (player.GetFromScreen(packet.Data, out mapObject))
			{
				var invitePlayer = mapObject as Models.Entities.Player;
				if (invitePlayer != null)
				{
					if (invitePlayer.Guild != null && (
						invitePlayer.GuildMember.Rank == Enums.GuildRank.GuildLeader ||
						invitePlayer.GuildMember.Rank == Enums.GuildRank.DeputyLeader
					))
					{
						invitePlayer.Guild.AddMember(player, Enums.GuildRank.Member);
					}
				}
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the leave syndicate.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.GuildPacket,
		         SubIdentity = (uint)Enums.GuildAction.LeaveSyndicate)]
		public static bool HandleLeaveSyndicate(Models.Entities.Player player, Models.Packets.Guilds.GuildPacket packet)
		{
			if (player.Battle != null)
			{
				return true;
			}
			
			if (player.Guild != null && player.GuildMember.Rank != Enums.GuildRank.GuildLeader)
			{
				player.Guild.RemoveMember(player.Name, false);
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the kickout member.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.GuildPacket,
		         SubIdentity = (uint)Enums.GuildAction.KickoutMember)]
		public static bool HandleKick(Models.Entities.Player player, Models.Packets.Guilds.GuildPacket packet)
		{
			return HandleDischarge(player, packet);
		}
		
		/// <summary>
		/// Handles the discharge member.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.GuildPacket,
		         SubIdentity = (uint)Enums.GuildAction.DischargeMember)]
		public static bool HandleDischarge(Models.Entities.Player player, Models.Packets.Guilds.GuildPacket packet)
		{
			if (player.Guild != null && player.GuildMember.Rank == Enums.GuildRank.GuildLeader)
			{
				player.Guild.RemoveMember(packet.Strings.FirstOrDefault(), true);
			}
			
			return true;
		}
	}
}
