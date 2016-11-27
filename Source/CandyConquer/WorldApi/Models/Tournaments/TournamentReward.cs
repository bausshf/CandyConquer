// Project by Bauss
using System;
using System.Collections.Generic;

namespace CandyConquer.WorldApi.Models.Tournaments
{
	/// <summary>
	/// Model for a tournament reward.
	/// </summary>
	public sealed class TournamentReward
	{
		/// <summary>
		/// Gets or sets the money.
		/// </summary>
		public uint Money { get; set; }
		
		/// <summary>
		/// Gets or sets the cps.
		/// </summary>
		public uint CPs { get; set; }
		
		/// <summary>
		/// Gets or sets the bound cps.
		/// </summary>
		public uint BoundCPs { get; set; }
		
		/// <summary>
		/// Gets a list of items.
		/// </summary>
		public List<Models.Items.Item> Items { get; private set; }
		
		/// <summary>
		/// Creates a new tournament reward.
		/// </summary>
		public TournamentReward()
		{
			Items = new List<Models.Items.Item>();
		}
	}
}
