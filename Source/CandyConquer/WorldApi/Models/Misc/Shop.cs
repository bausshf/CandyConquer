// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Collections;

namespace CandyConquer.WorldApi.Models.Misc
{
	/// <summary>
	/// Model for a shop.
	/// </summary>
	public sealed class Shop
	{
		/// <summary>
		/// The npc id.
		/// </summary>
		private uint _npcId;
		
		/// <summary>
		/// Gets the npc associated with the shop.
		/// </summary>
		public Models.Entities.Npc Npc
		{
			get
			{
				Models.Entities.Npc npc;
				Collections.NPCCollection.TryGetNpc(_npcId, out npc);
				return npc;
			}
		}
		
		/// <summary>
		/// Gets the items associated with the shop.
		/// </summary>
		public ConcurrentHashSet<uint> Items { get; private set; }
		
		/// <summary>
		/// Gets the type of the shop.
		/// </summary>
		public Enums.ShopType ShopType { get; private set; }
		
		/// <summary>
		/// Creates a new shop.
		/// </summary>
		/// <param name="npcId">The npc id.</param>
		/// <param name="shopType">The shop type.</param>
		public Shop(uint npcId, Enums.ShopType shopType)
		{
			_npcId = npcId;
			ShopType = shopType;
			
			Items = new ConcurrentHashSet<uint>();
		}
	}
}
