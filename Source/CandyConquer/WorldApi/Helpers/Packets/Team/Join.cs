// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Team
{
	/// <summary>
	/// Controller for join.
	/// </summary>
	[ApiController()]
	public static class Join
	{
		/// <summary>
		/// Handles the request join.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.TeamActionPacket,
		         SubIdentity = (uint)Enums.TeamAction.RequestJoin)]
		public static bool HandleRequestJoin(Models.Entities.Player player, Models.Packets.Team.TeamActionPacket packet)
		{
			if (player.Team != null)
			{
				return true;
			}
			
			Models.Maps.IMapObject mapObject;
			if (player.GetFromScreen(packet.ClientId, out mapObject))
			{
				var leader = mapObject as Models.Entities.Player;
				if (leader != null && leader.Team != null && leader.Team.IsLeader(leader))
				{
					leader.Team.PendingJoin = player.ClientId;
					packet.ClientId = player.ClientId;
					leader.ClientSocket.Send(packet);
				}
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the request invite.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.TeamActionPacket,
		         SubIdentity = (uint)Enums.TeamAction.RequestInvite)]
		public static bool HandleRequestInvite(Models.Entities.Player player, Models.Packets.Team.TeamActionPacket packet)
		{
			if (player.Team == null)
			{
				return true;
			}
			
			if (!player.Team.IsLeader(player))
			{
				return true;
			}
			
			Models.Maps.IMapObject mapObject;
			if (player.GetFromScreen(packet.ClientId, out mapObject))
			{
				var newMember = mapObject as Models.Entities.Player;
				if (newMember != null && newMember.Team == null)
				{
					player.Team.PendingInvite = newMember.ClientId;
					packet.ClientId = player.ClientId;
					newMember.ClientSocket.Send(packet);
				}
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the accept join.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.TeamActionPacket,
		         SubIdentity = (uint)Enums.TeamAction.AcceptJoin)]
		public static bool HandleAcceptJoin(Models.Entities.Player player, Models.Packets.Team.TeamActionPacket packet)
		{
			if (player.Team == null)
			{
				return true;
			}
			
			if (!player.Team.IsLeader(player))
			{
				return true;
			}
			
			Models.Maps.IMapObject mapObject;
			if (packet.ClientId == player.Team.PendingJoin &&
			    player.GetFromScreen(packet.ClientId, out mapObject))
			{
				var newMember = mapObject as Models.Entities.Player;
				if (newMember != null && newMember.Team == null)
				{
					if (player.Team.Add(newMember))
					{
						var newTeamMemberPacket = new Models.Packets.Team.TeamMemberPacket
						{
							Name = newMember.Name,
							ClientId = newMember.ClientId,
							Mesh = newMember.Mesh,
							MaxHP = newMember.MaxHP,
							HP = newMember.HP
						};
						
						foreach (var member in player.Team.GetMembers())
						{
							if (member.ClientId != newMember.ClientId)
							{
								newMember.ClientSocket.Send(new Models.Packets.Team.TeamMemberPacket
								                            {
								                            	Name = member.Name,
								                            	ClientId = member.ClientId,
								                            	Mesh = member.Mesh,
								                            	MaxHP = member.MaxHP,
								                            	HP = member.HP
								                            });
								member.ClientSocket.Send(newTeamMemberPacket);
							}
						}
						
						newMember.ClientSocket.Send(newTeamMemberPacket);
						
						packet.ClientId = player.ClientId;
						newMember.ClientSocket.Send(packet);
					}
				}
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the accept invite.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.TeamActionPacket,
		         SubIdentity = (uint)Enums.TeamAction.AcceptInvite)]
		public static bool HandleAcceptInvite(Models.Entities.Player player, Models.Packets.Team.TeamActionPacket packet)
		{
			if (player.Team != null)
			{
				return true;
			}
			
			Models.Maps.IMapObject mapObject;
			if (player.GetFromScreen(packet.ClientId, out mapObject))
			{
				var leader = mapObject as Models.Entities.Player;
				if (leader != null && leader.Team != null && leader.Team.IsLeader(leader) &&
				   player.ClientId == leader.Team.PendingInvite)
				{
					if (leader.Team.Add(player))
					{
						var newTeamMemberPacket = new Models.Packets.Team.TeamMemberPacket
						{
							Name = player.Name,
							ClientId = player.ClientId,
							Mesh = player.Mesh,
							MaxHP = player.MaxHP,
							HP = player.HP
						};
						
						foreach (var member in leader.Team.GetMembers())
						{
							if (member.ClientId != player.ClientId)
							{
								player.ClientSocket.Send(new Models.Packets.Team.TeamMemberPacket
								                            {
								                            	Name = member.Name,
								                            	ClientId = member.ClientId,
								                            	Mesh = member.Mesh,
								                            	MaxHP = member.MaxHP,
								                            	HP = member.HP
								                            });
								member.ClientSocket.Send(newTeamMemberPacket);
							}
						}
						
						player.ClientSocket.Send(newTeamMemberPacket);
						
						packet.ClientId = leader.ClientId;
						player.ClientSocket.Send(packet);
					}
				}
			}
			
			return true;
		}
	}
}