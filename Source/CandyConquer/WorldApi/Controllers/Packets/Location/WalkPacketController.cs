// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.ApiServer;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Controllers.Packets.Location
{
	/// <summary>
	/// The walk packet controller.
	/// </summary>
	[ApiController()]
	public static class WalkPacketController
	{
		/// <summary>
		/// Handles the walk packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = 10005)]
		public static bool HandlePacket(Models.Entities.Player player, Models.Packets.Location.WalkPacket packet)
		{
			if (packet.ClientId != player.ClientId)
			{
				Database.Dal.Accounts.Ban(
					player.DbPlayer.Account, Drivers.Messages.MOVE_OTHER_PLAYER,
					Database.Models.DbAccount.BanRangeType.Perm);
				player.ClientSocket.Disconnect(Drivers.Messages.MOVE_OTHER_PLAYER);
				return false;
			}
			player.Action = Enums.PlayerAction.None;
			player.AttackPacket = null;
			player.LastMovementMode = packet.Mode;
			
			int newX = 0, newY = 0;
			int newDir = 0;
			
			switch (packet.Mode)
			{
				case Enums.WalkMode.Walk:
				case Enums.WalkMode.Run:
					{
						newDir = (int)packet.Direction % 8;
						newX = player.X + Data.Constants.Movement.DeltaX[newDir];
						newY = player.Y + Data.Constants.Movement.DeltaY[newDir];
						
						if (DateTime.UtcNow < player.LastMovementTime.AddMilliseconds(100))
						{
							player.SpeedHackChecks++;
							if (player.SpeedHackChecks >= 3)
							{
								// speedhack ...
								if (player.SpeedHackChecks >= 10)
								{
									player.ClientSocket.Disconnect(Drivers.Messages.SPEEDHACK);
									return false;
								}
								
								player.Pullback();
								return true;
							}
						}
						else
						{
							player.SpeedHackChecks = 0;
						}
						break;
					}
					
				case Enums.WalkMode.Mount:
					{
						newDir = (int)packet.Direction % 24;
						newX = player.X + Data.Constants.Movement.DeltaMountX[newDir];
						newY = player.Y + Data.Constants.Movement.DeltaMountY[newDir];
						
						if (DateTime.UtcNow < player.LastMovementTime.AddMilliseconds(400))
						{
							player.SpeedHackChecks++;
							if (player.SpeedHackChecks >= 3)
							{
								// speedhack ...
								if (player.SpeedHackChecks >= 10)
								{
									player.ClientSocket.Disconnect(Drivers.Messages.SPEEDHACK);
									return false;
								}
								
								player.Pullback();
								return true;
							}
						}
						else
						{
							player.SpeedHackChecks = 0;
						}
						break;
					}
					
				default:
					return false;
			}
			
			if (player.Map.ValidCoord((ushort)newX, (ushort)newY))
			{
				player.LastMovementTime = DateTime.UtcNow;
				player.X = (ushort)newX;
				player.Y = (ushort)newY;
				
				player.ClientSocket.Send(packet);
				player.UpdateScreen(false, packet, Enums.UpdateScreenFlags.Idle);
				
				if (player.Battle != null)
				{
					if (!player.Battle.EnterArea(player))
					{
						player.Pullback();
					}
					else if (!player.Battle.LeaveArea(player))
					{
						player.Pullback();
					}
				}
				
				return true;
			}
			
			return false;
		}
	}
}
