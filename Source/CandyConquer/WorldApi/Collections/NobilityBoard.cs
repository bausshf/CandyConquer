// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CandyConquer.WorldApi.Models.Nobility;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// Nobility board collection.
	/// </summary>
	public static class NobilityBoard
	{
		/// <summary>
		/// All nobility donations.
		/// </summary>
		private static ConcurrentDictionary<int, NobilityDonation> _donations;
		
		/// <summary>
		/// Static constructor for NobilityBoard.
		/// </summary>
		static NobilityBoard()
		{
			_donations = new ConcurrentDictionary<int, NobilityDonation>();
		}
		
		/// <summary>
		/// Loads the nobility board.
		/// </summary>
		public static void Load()
		{
			var nobilities = Database.Dal.Nobility.GetAll(Drivers.Settings.WorldSettings.Server);
			
			foreach (var dbNobility in nobilities)
			{
				_donations.TryAdd(dbNobility.PlayerId, new NobilityDonation(dbNobility));
			}
			
			GetTop50();
		}
		
		/// <summary>
		/// Attempts to get the nobility information for a player.
		/// </summary>
		/// <param name="playerId">The player.</param>
		/// <param name="donation">The donation.</param>
		/// <returns>True if the nobility information was retrieved.</returns>
		public static bool TryGetNobility(int playerId, out NobilityDonation donation)
		{
			return _donations.TryGetValue(playerId, out donation);
		}
		
		/// <summary>
		/// Attempts to add a new nobility donation.
		/// </summary>
		/// <param name="donation">The donation.</param>
		/// <returns>True if the nobility donation was added.</returns>
		public static bool TryAdd(NobilityDonation donation)
		{
			return _donations.TryAdd(donation.PlayerId, donation);
		}
		
		/// <summary>
		/// Gets the top 50 nobility donations.
		/// </summary>
		/// <returns>A read only collection of the top 50 donations.</returns>
		public static ReadOnlyCollection<NobilityDonation> GetTop50()
		{
			var sortedDonations = _donations.Values.OrderByDescending(donation => donation.Donation);
			var listDonations = new List<NobilityDonation>();
			
			int count = 0;
			foreach (var donation in sortedDonations)
			{
				donation.OldRank = donation.Rank;
				
				if (listDonations.Count < 50)
				{
					if (listDonations.Count < 3)
					{
						donation.Rank = Enums.NobilityRank.King;
					}
					else if (listDonations.Count < 15)
					{
						donation.Rank = Enums.NobilityRank.Prince;
					}
					else // (implicit) if (listDonations.Count < 50)
					{
						donation.Rank = Enums.NobilityRank.Duke;
					}
					
					listDonations.Add(donation);
				}
				else
				{
					if (donation.Donation >= 200000000)
					{
						donation.Rank = Enums.NobilityRank.Earl;
					}
					else if (donation.Donation >= 100000000)
					{
						donation.Rank = Enums.NobilityRank.Baron;
					}
					else if (donation.Donation >= 30000000)
					{
						donation.Rank = Enums.NobilityRank.Knight;
					}
				}
				
				donation.Ranking = count;
				count++;
				
				var player = donation.Player;
				
				if (player != null && donation.OldRank != donation.Rank)
				{
					if (donation.OldRank < donation.Rank)
					{
						string message = string.Empty;
						switch (donation.Rank)
						{
							case Enums.NobilityRank.King:
								{
									message = player.IsFemale ? "NEW_QUEEN" : "NEW_KING";
									break;
								}
								
							case Enums.NobilityRank.Prince:
								{
									message = player.IsFemale ? "NEW_PRINCESS" : "NEW_PRINCE";
									break;
								}
						}
						
						if (!string.IsNullOrWhiteSpace(message))
						{
							Collections.PlayerCollection.BroadcastFormattedMessage(message, player.Name);
						}
					}
					
					player.UpdateClientNobility();
				}
			}
			
			return listDonations.AsReadOnly();
		}
		
		/// <summary>
		/// Gets a paged collection of the nobility board's top 50.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="pageMax">The max page.</param>
		/// <returns>A read only collection of the paged nobility board entries.</returns>
		public static ReadOnlyCollection<NobilityDonation> GetPaged(int page, out int pageMax)
		{
			var listDonations = new List<NobilityDonation>();
			pageMax = 0;
			
			if (_donations.Count > 0)
			{
				page = Math.Max(0, page);
				page = Math.Min(4, page);
				
				pageMax = _donations.Count / 10;
				if (page <= pageMax)
				{
					var donations = GetTop50();
					
					if (donations.Count > 0)
					{
						var takeOff = (donations.Count - (page * 10));
						var fromPage = Math.Min(Math.Max(0, (donations.Count - takeOff)), (page * 10));
						var toPage = Math.Min((donations.Count - fromPage), 10);
						
						listDonations.AddRange(donations.Skip(fromPage).Take(toPage));
					}
				}
			}
			
			return listDonations.AsReadOnly();
		}
	}
}
