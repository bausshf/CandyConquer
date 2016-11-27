// Project by Bauss
using System;
using System.Collections.Generic;
using System.Linq;
using Candy;
using CandyConquer.Database.Models;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for players.
	/// </summary>
	public static class Players
	{
		/// <summary>
		/// Gets a player.
		/// </summary>
		/// <param name="pars">The parameters.</param>
		/// <returns>The player if found, null otherwise.</returns>
		private static DbPlayer Get(Dictionary<string,object> pars)
		{
			return SqlDalHelper.Get<DbPlayer>(ConnectionStrings.World,
			                                   string.Format("SELECT * FROM Players WHERE {0}", Sql.GetSel(pars)),
			                                   pars);
		}
		
		/// <summary>
		/// Gets a player by their id.
		/// </summary>
		/// <param name="playerId">The player id.</param>
		/// <returns>The player.</returns>
		public static DbPlayer GetPlayerById(int playerId)
		{
			var pars = Sql.GetParsDict();
			pars.Add("Id", playerId);
			
			return Get(pars);
		}
		
		/// <summary>
		/// Gets a player by its associated account.
		/// </summary>
		/// <param name="account">The associated account.</param>
		/// <returns>The player if existing, otherwise null.</returns>
		public static DbPlayer GetPlayerByAccount(DbAccount account)
		{
			var pars = Sql.GetParsDict();
			pars.Add("AccountId", account.Id);
			pars.Add("Server", account.LastServer);
			
			return Get(pars);
		}
		
		/// <summary>
		/// Creates a new player.
		/// </summary>
		/// <param name="account">The account to associate with it.</param>
		/// <returns>The newly created player.</returns>
		public static Models.DbPlayer Create(Models.DbAccount account)
		{
			var player = new DbPlayer();
			player.AccountId = account.Id;
			player.Server = account.LastServer;
			player.New = true;
			
			player.Create();
			player._account = account;
			
			return player;
		}
		
		/// <summary>
		/// Creates a new player.
		/// </summary>
		/// <param name="player">The player to create the data from.</param>
		/// <param name="stats">The stats of the player.</param>
		/// <param name="hp">The hp of the player.</param>
		/// <param name="mp">The mp of the player.</param>
		public static Models.DbPlayer Create(Models.DbPlayer player, ushort[] stats, int hp, int mp)
		{
			player.Level = 1;
			player.Money = 100;
			player.CPs = 0;
			player.BoundCPs = 0;
			player.Avatar = Avatars.GetRandomAvatar(player.Model == 1003 || player.Model == 1004);
			player.MapId = 1002;
			player.X = 400;
			player.Y = 400;
			player.LastMapId = 1002;
			player.LastMapX = 400;
			player.LastMapY = 400;
			player.Strength = stats[0];
			player.Vitality = stats[1];
			player.Agility = stats[2];
			player.Spirit = stats[3];
			player.Hair = 410;
			player.Experience = 0;
			player.AttributePoints = 0;
			player.PKPoints = 0;
			player.Title = "None";
			player.Spouse = "None";
			player.HP = hp;
			player.MP = mp;
			player.Reborns = 0;
			player.New = false;
			
			player.CanWrite = true;
			player.Update();
			
			return player;
		}
		
		
		/// <summary>
		/// Gets a player by authentication.
		/// </summary>
		/// <param name="authKey">The auth key.</param>
		/// <param name="ip">The ip from where the authentication is done.</param>
		/// <returns>The player if existing, null otherwise.</returns>
		public static DbPlayer GetPlayerByAuthentication(string authKey, string server, string ip)
		{
			var pars = Sql.GetParsDict();
			pars.Add("Server", server);
			
			return SqlDalHelper.GetAll<DbPlayer>(ConnectionStrings.World,
			                                   string.Format("SELECT * FROM Players WHERE {0}", Sql.GetSel(pars)),
			                                   pars)
				.Where(p => {
				       	if (p == null || p.Account == null) return false;
				       	if (p.Account.LastIP != ip) return false;
				       	if (string.IsNullOrWhiteSpace(p.Account.LastAuthKey)) return false;
				       	var lastAuthKey = p.Account.LastAuthKey.Split(';');
				       	if (lastAuthKey.Length != 2) return false;
				       	if (lastAuthKey[0] != authKey) return false;
				       	if (DateTime.Now >= DateTime.FromBinary(long.Parse(lastAuthKey[1])).AddMinutes(10)) return false;
				       	return true;
				       })
				.FirstOrDefault();
		}
		
		/// <summary>
		/// Gets a player by name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="server">The server.</param>
		/// <returns>The player if existing, null otherwise.</returns>
		public static Models.DbPlayer GetPlayerByName(string name, string server)
		{
			var pars = Sql.GetParsDict();
			pars.Add("Server", server);
			pars.Add("Name", name);
			
			var player = Get(pars);
			if (player != null)
			{
				return player;
			}
			
			pars["Name"] = string.Concat(name, "[GM]");
			player = Get(pars);
			if (player != null)
			{
				return player;
			}
			
			pars["Name"] = string.Concat(name, "[PM]");
			player = Get(pars);
			return player;
		}
	}
}
