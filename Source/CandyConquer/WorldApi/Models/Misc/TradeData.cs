// Project by Bauss
using System;
using System.Collections.Concurrent;

namespace CandyConquer.WorldApi.Models.Misc
{
	/// <summary>
	/// Trade data.
	/// </summary>
	public sealed class TradeData
	{
		/// <summary>
		/// Gets or sets a boolean determining whether the trade has been accepted.
		/// </summary>
		public bool Accepted { get; set; }
		
		/// <summary>
		/// Gets or sets a boolean determining whether the trade has been confirmed.
		/// </summary>
		public bool Confirmed { get; set; }
		
		/// <summary>
		/// Gets or sets the cps of the trade.
		/// </summary>
		public uint CPs { get; set; }
		
		/// <summary>
		/// Gets or sets the money of the trade.
		/// </summary>
		public uint Money { get; set; }
		
		/// <summary>
		/// Gets or sets a boolean determining whether the trade window is open or not.
		/// </summary>
		public bool WindowOpen { get; set; }
		
		/// <summary>
		/// Gets or sets a boolean determining whether the player is trading.
		/// </summary>
		public bool Trading { get; set; }
		
		/// <summary>
		/// Gets or sets a booleaning determining whether trade is requesting.
		/// </summary>
		public bool Requesting { get; set; }
		
		/// <summary>
		/// Gets the trade partner.
		/// </summary>
		public Models.Entities.Player Partner { get; private set; }
		
		/// <summary>
		/// Gets the items.
		/// </summary>
		public ConcurrentBag<Models.Items.Item> Items { get; private set; }
		
		/// <summary>
		/// Gets a boolean determining whether the partner has accepted the trade.
		/// </summary>
		public bool PartnerAccepted
		{
			get { return Partner.Trade.Accepted; }
		}
		
		/// <summary>
		/// Gets a boolean determining whether the partner has confirmed the trade.
		/// </summary>
		public bool PartnerConfirmed
		{
			get { return Partner.Trade.Confirmed; }
		}
		
		/// <summary>
		/// Gets the partners cps.
		/// </summary>
		public uint PartnerCPs
		{
			get { return Partner.Trade.CPs; }
		}
		
		/// <summary>
		/// Gets the partners money.
		/// </summary>
		public uint PartnerMoney
		{
			get { return Partner.Trade.Money; }
		}
		
		/// <summary>
		/// Gets a boolean determining whether the partners trade window is open.
		/// </summary>
		public bool PartnerWindowOpen
		{
			get { return Partner.Trade.WindowOpen; }
		}
		
		/// <summary>
		/// Gets a boolean determining whether the partner is trading.
		/// </summary>
		public bool PartnerTrading
		{
			get { return Partner.Trade.Trading; }
		}
		
		/// <summary>
		/// Gets a boolean determining wehther the partner is requesting.
		/// </summary>
		public bool PartnerRequesting
		{
			get { return Partner.Trade.Requesting; }
		}
		
		/// <summary>
		/// Gets the partners items.
		/// </summary>
		public ConcurrentBag<Models.Items.Item> PartnerItems
		{
			get { return Partner.Trade.Items; }
		}
		
		/// <summary>
		/// Creates a new trade data.
		/// </summary>
		public TradeData()
		{
			Items = new ConcurrentBag<Models.Items.Item>();
		}
		
		/// <summary>
		/// Begins the trade.
		/// </summary>
		/// <param name="partner">The partner.</param>
		public void Begin(Models.Entities.Player partner)
		{
			Reset();
			
			Trading = true;
			Partner = partner;
		}
		
		/// <summary>
		/// Resets the trade.
		/// </summary>
		public void Reset()
		{
			Items = new ConcurrentBag<CandyConquer.WorldApi.Models.Items.Item>();
			
			Accepted = false;
			Confirmed = false;
			CPs = 0;
			Money = 0;
			WindowOpen = false;
			Trading = false;
			Partner = null;
		}
	}
}
