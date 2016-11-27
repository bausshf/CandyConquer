// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Models.Nobility
{
	/// <summary>
	/// Model for a nobility info string.
	/// </summary>
	public sealed class NobilityInfoString
	{
		/// <summary>
		/// Gets or sets the client id.
		/// </summary>
		public uint ClientId { get; set; }
		
		/// <summary>
		/// Gets or sets the donation.
		/// </summary>
		public long Donation { get; set; }
		
		/// <summary>
		/// Gets or sets the rank.
		/// </summary>
		public Enums.NobilityRank Rank { get; set; }
		
		/// <summary>
		/// Gets or sets the ranking.
		/// </summary>
		public int Ranking { get; set; }
		
		/// <summary>
		/// Creates a new nobility info string.
		/// </summary>
		public NobilityInfoString()
		{
		}
		
		/// <summary>
		/// Gets a string equivalent to the nobility info.
		/// </summary>
		/// <returns>The string.</returns>
		public override string ToString()
		{
			return string.Format("{0} {1} {2} {3}", ClientId, Donation, (uint)Rank, Ranking);
		}
	}
}
