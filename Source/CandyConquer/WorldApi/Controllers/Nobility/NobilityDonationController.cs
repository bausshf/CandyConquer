// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Controllers.Nobility
{
	/// <summary>
	/// Controller for a nobility donation.
	/// </summary>
	public abstract class NobilityDonationController
	{
		/// <summary>
		/// Gets the nobility donation associated with the controller.
		/// </summary>
		public Models.Nobility.NobilityDonation Nobility { get; protected set; }
		
		/// <summary>
		/// Creates a new nobility donation controller.
		/// </summary>
		protected NobilityDonationController()
		{
			
		}
		
		/// <summary>
		/// Gets a string equivalent to the nobility donation.
		/// </summary>
		/// <returns>The string.</returns>
		public override string ToString()
		{
			return string.Format("{0} 0 0 {1} {2} {3} {4}", Nobility.Id, Nobility.Name, Nobility.Donation, (uint)Nobility.Rank, Nobility.Ranking);
		}
	}
}
