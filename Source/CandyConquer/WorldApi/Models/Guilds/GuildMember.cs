// Project by Bauss
using System;
using CandyConquer.Database.Models;
using CandyConquer.Drivers;

namespace CandyConquer.WorldApi.Models.Guilds
{
	/// <summary>
	/// Model for a guild member.
	/// </summary>
	public class GuildMember
	{
		/// <summary>
		/// Gets the database model for the member.
		/// </summary>
		public DbGuildRank DbGuildRank { get; private set; }
		
		/// <summary>
		/// Gets or sets the player associated with the member inforamtion.
		/// </summary>
		public Models.Entities.Player Player { get; set; }
		
		/// <summary>
		/// The guild rank.
		/// </summary>
		private Enums.GuildRank _rank;
		
		/// <summary>
		/// Gets or sets the rank.
		/// </summary>
		public Enums.GuildRank Rank
		{
			get { return _rank; }
			set
			{
				_rank = value;
				DbGuildRank.GuildRank = _rank.ToString();
			}
		}
		
		/// <summary>
		/// Gets a boolean indicating whether the member is online or not.
		/// </summary>
		public bool Online
		{
			get
			{
				return Player != null && Player.ClientSocket != null && !Player.ClientSocket.Disconnected && Player.LoggedIn;
			}
		}
		
		/// <summary>
		/// Gets the guild nobility rank.
		/// </summary>
		public Enums.GuildNobilityRank NobilityRank
		{
			get
			{
				Models.Nobility.NobilityDonation donation;
				if (!Collections.NobilityBoard.TryGetNobility(DbGuildRank.PlayerId, out donation))
				{
					return Enums.GuildNobilityRank.None;
				}
				
				var nobility = (int)donation.Rank;
				if (nobility == 0)
				{
					return Enums.GuildNobilityRank.None;
				}
				
				nobility *= 10;
				bool isFemale;
				if (Player != null)
				{
					isFemale = Player.IsFemale;
				}
				else
				{
					isFemale = Database.Dal.Players.GetPlayerById(DbGuildRank.PlayerId).Model >= 2001; // lazy ... (2001 || 2002) == female
				}
				
				nobility += isFemale ? 2 : 1;
				
				return (Enums.GuildNobilityRank)nobility;
			}
		}
		
		/// <summary>
		/// Creates a new guild member.
		/// </summary>
		/// <param name="dbGuildRank">The database model associated with the member.</param>
		public GuildMember(DbGuildRank dbGuildRank)
		{
			DbGuildRank = dbGuildRank;
			_rank = dbGuildRank.GuildRank.ToEnum<Enums.GuildRank>();
		}
	}
}
