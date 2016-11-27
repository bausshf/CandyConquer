// Project by Bauss
using System;
using System.Text;
using System.Linq;
using CandyConquer.WorldApi.Models.Packets.Login;
using CandyConquer.WorldApi.Controllers.Packets.Message;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.ApiServer;
using CandyConquer.Security.Api;
using CandyConquer.Drivers;
using CandyConquer.Database.Dal;
using CandyConquer.Database.Models;

namespace CandyConquer.WorldApi.Controllers.Packets.Login
{
	/// <summary>
	/// The character creation packet controller.
	/// </summary>
	[ApiController()]
	public static class CharacterCreationController
	{
		/// <summary>
		/// Handles the character creation packet.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always, Identity = 1001)]
		public static bool HandlePacket(Models.Entities.Player player, CharacterCreationPacket packet)
		{
			switch (packet.PacketSubType)
			{
				case 0:
					return HandleCreation(player, packet);
				case 1:
					return HandleBackButton(player, packet);
					
				default:
					#if LOCAL
					PacketLogger.Log(packet, true, true);
					#else
					PacketLogger.Log(packet, false, true);
					#endif
					break;
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the creation of the character.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		private static bool HandleCreation(Models.Entities.Player player, CharacterCreationPacket packet)
		{
			if (player.ClientId != packet.ClientId)
			{
				Accounts.Ban(player.DbPlayer.Account, Drivers.Messages.INVALID_CLIENT_ID, DbAccount.BanRangeType.Perm);
				return false;
			}
			
			if (!packet.Job.ToString().StartsWith("Intern"))
			{
				Accounts.Ban(player.DbPlayer.Account, Drivers.Messages.INVALID_CLIENT_ID, DbAccount.BanRangeType.Perm);
				return false;
			}
			
			if (packet.Model != 1003 && packet.Model != 1004 &&
			    packet.Model != 2001 && packet.Model != 2002)
			{
				Accounts.Ban(player.DbPlayer.Account, Drivers.Messages.INVALID_MODEL, DbAccount.BanRangeType.Perm);
				return false;
			}
			
			if (!ValidateCharacters(packet.Name))
			{
				player.ClientSocket.Send(MessageController.CreateNewPlayer(Drivers.Messages.Errors.INVALID_CHARS));
				return true;
			}
			
			if (IsBannedName(packet.Name))
			{
				player.ClientSocket.Send(MessageController.CreateNewPlayer(Drivers.Messages.Errors.NAME_BANNED));
				return true;
			}
			
			if (Players.GetPlayerByName(packet.Name, Drivers.Settings.WorldSettings.Server) != null)
			{
				player.ClientSocket.Send(MessageController.CreateNewPlayer(Drivers.Messages.Errors.NAME_TAKEN));
				return true;
			}
			
			// stats fallback ...
			player.DbPlayer.Strength = 0;
			player.DbPlayer.Agility = 0;
			player.DbPlayer.Vitality = 0;
			player.DbPlayer.Spirit = 0;
			
			player.DbPlayer.Name = packet.Name;
			player.Job = packet.Job;
			player.Level = 1;
			player.DbPlayer.Model = packet.Model;
			
			Players.Create(player.DbPlayer,
			                        new ushort[] { player.Strength, player.Vitality, player.Agility, player.Spirit },
			                        player.MaxHP, player.MaxMP);
			player.ClientSocket.Disconnect(Drivers.Messages.SUCCESS_CREATE);
			
			return true;
		}
		
		/// <summary>
		/// Validates characters in a name.
		/// </summary>
		/// <param name="name">The name to validate.</param>
		/// <returns>True if the name is valid, false otherwise.</returns>
		private static bool ValidateCharacters(string name)
		{
			int count = 0;
			foreach (var c in name)
			{
				var ascii = (byte)c;
				if (ascii >= 48 && ascii <= 57 ||
				    ascii >= 65 && ascii <= 90 ||
				    ascii >= 97 && ascii <= 122)
				{
					count++;
				}
			}
			
			return count == name.Length;
		}
		
		/// <summary>
		/// Checks whether a name is banned or not.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>True if the name is banned, false otherwise.</returns>
		private static bool IsBannedName(string name)
		{
			return Collections.BannedNames.Contains(name);
		}
		
		/// <summary>
		/// Handles the back button press.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		private static bool HandleBackButton(Models.Entities.Player player, CharacterCreationPacket packet)
		{
			return false;
		}
	}
}
