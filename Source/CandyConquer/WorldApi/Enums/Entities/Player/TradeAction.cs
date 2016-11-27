// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Enums
{
	/// <summary>
	/// Enumeration of trade actions.
	/// </summary>
	public enum TradeAction
	{
		/// <summary>
		/// Request
		/// </summary>
		Request = 1,
		/// <summary>
		/// Close
		/// </summary>
		Close = 2,
		/// <summary>
		/// ShowTable
		/// </summary>
		ShowTable = 3,
		/// <summary>
		/// HideTable
		/// </summary>
		HideTable = 5,
		/// <summary>
		/// AddItem
		/// </summary>
		AddItem = 6,
		/// <summary>
		/// SetMoney
		/// </summary>
		SetMoney = 7,
		/// <summary>
		/// ShowMoney
		/// </summary>
		ShowMoney = 8,
		/// <summary>
		/// Accept
		/// </summary>
		Accept = 10,
		/// <summary>
		/// RemoveItem
		/// </summary>
		RemoveItem = 11,
		/// <summary>
		/// ShowConquerPoints
		/// </summary>
		ShowConquerPoints = 12,
		/// <summary>
		/// SetConquerPoints
		/// </summary>
		SetConquerPoints = 13,
		/// <summary>
		/// TimeOut
		/// </summary>
		TimeOut = 17,
	}
}
