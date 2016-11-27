// Project by Bauss
using System;
using CandyConquer.Database.Models;

namespace CandyConquer.WorldApi.Models.Nobility
{
	/// <summary>
	/// Model for nobility donations.
	/// </summary>
	public sealed class NobilityDonation : Controllers.Nobility.NobilityDonationController
	{
		/// <summary>
		/// The database model associated with the donation.
		/// </summary>
		private DbPlayerNobility _dbPlayerNobility;
		
		/// <summary>
		/// The player associated with the donation.
		/// </summary>
		private Models.Entities.Player _player;
		
		/// <summary>
		/// Gets or sets the player associated with the donation.
		/// </summary>
		public Models.Entities.Player Player
		{
			get { return _player; }
			set
			{
				_player = value;
				
				if (_player != null)
				{
					_dbPlayerNobility.PlayerName = _player.Name;
					_dbPlayerNobility.PlayerId = _player.DbPlayer.Id;
					
					if (_dbPlayerNobility.Id == 0)
					{
						_dbPlayerNobility.Create();
					}
					else
					{
						_dbPlayerNobility.Update();
					}
				}
			}
		}
		
		/// <summary>
		/// Gets the name of the player.
		/// </summary>
		public string Name
		{
			get { return Player == null ? _dbPlayerNobility.PlayerName : Player.Name; }
		}
		
		/// <summary>
		/// Gets the client id of the player.
		/// </summary>
		public uint ClientId
		{
			get { return Player != null ? Player.ClientId : (uint)Id; }
		}
		
		/// <summary>
		/// Gets the id of the donation.
		/// </summary>
		public int Id { get { return _dbPlayerNobility.Id; } }
		
		/// <summary>
		/// Gets the player id of the donation.
		/// </summary>
		public int PlayerId { get { return _dbPlayerNobility.PlayerId; } }
		
		/// <summary>
		/// Gets or sets the rank of the donation.
		/// </summary>
		public Enums.NobilityRank Rank { get; set; }
		
		/// <summary>
		/// Gets or sets the old rank of the donation.
		/// </summary>
		public Enums.NobilityRank OldRank { get; set; }
		
		/// <summary>
		/// Gets or sets the ranking of the donation.
		/// </summary>
		public int Ranking { get; set; }
		
		/// <summary>
		/// Gets or sets the donation.
		/// </summary>
		public long Donation
		{
			get { return _dbPlayerNobility.Donation; }
			set
			{
				_dbPlayerNobility.Donation = value;
				_dbPlayerNobility.Update();
			}
		}
		
		/// <summary>
		/// Creates a new nobility donation.
		/// </summary>
		/// <param name="dbPlayerNobility">The nobility donation.</param>
		public NobilityDonation(DbPlayerNobility dbPlayerNobility)
		{
			_dbPlayerNobility = dbPlayerNobility;
			_dbPlayerNobility.Server = Drivers.Settings.WorldSettings.Server;
			
			Nobility = this;
		}
		
		/// <summary>
		/// Gets a string equivalent to the donation.
		/// </summary>
		/// <returns>The string.</returns>
		public override string ToString()
		{
			return base.ToString();
		}
	}
}
