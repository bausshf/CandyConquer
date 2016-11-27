// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Client.DataExchange
{
	/// <summary>
	/// Helper for the jump sub type of the data exchange packet.
	/// </summary>
	[ApiController()]
	public static class Jump
	{
		/// <summary>
		/// Handles the jump.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.DataExchangePacket,
		         SubIdentity = (uint)Enums.ExchangeType.Jump)]
		public static bool Handle(Models.Entities.Player player, Models.Packets.Client.DataExchangePacket packet)
		{
			if (!player.Alive && DateTime.UtcNow > player.DeadTime)
			{
				player.ClientSocket.Disconnect(Drivers.Messages.Errors.JUMP_WHILE_DEAD);
				return false;
			}
			
			if (packet.ClientId != player.ClientId)
			{
				Database.Dal.Accounts.Ban(
					player.DbPlayer.Account, Drivers.Messages.JUMP_OTHER_PLAYER,
					Database.Models.DbAccount.BanRangeType.Perm);
				player.ClientSocket.Disconnect(Drivers.Messages.JUMP_OTHER_PLAYER);
				return false;
			}
			
			player.Action = Enums.PlayerAction.None;
			player.AttackPacket = null;
			
			ushort newX = packet.Data1Low;
			ushort newY = packet.Data1High;
			
			if (Tools.RangeTools.GetDistanceU(newX, newY, player.X, player.Y) > 28)
			{
				player.Pullback();
				return true;
			}
			
			if (!player.Map.ValidCoord(newX, newY))
			{
				player.Pullback();
				return false;
			}
			
			if (player.LastMovementMode == Enums.WalkMode.Jump && DateTime.UtcNow <= player.LastMovementTime.AddMilliseconds(400))
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
			
			if (player.Battle != null)
			{
				if (!player.Battle.EnterArea(player))
				{
					player.Pullback();
					return true;
				}
				else if (!player.Battle.LeaveArea(player))
				{
					player.Pullback();
					return true;
				}
			}
			
			if (Tools.CalculationTools.ChanceSuccess(Data.Constants.Chances.StaminaOnJump))
			{
				player.Stamina = (byte)Math.Min(100, player.Stamina + 7);
			}
			
			player.X = newX;
			player.Y = newY;
			player.LastMovementMode = Enums.WalkMode.Jump;
			player.LastMovementTime = DateTime.UtcNow;
			player.ClientSocket.Send(packet);
			player.UpdateScreen(false, packet, Enums.UpdateScreenFlags.Idle);
			
			return true;
		}
	}
}
