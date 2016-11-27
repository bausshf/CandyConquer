// Project by Bauss
using System;
using System.Collections.Concurrent;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A npc collection.
	/// </summary>
	public static class NPCCollection
	{
		/// <summary>
		/// The collection of npcs.
		/// </summary>
		private static ConcurrentDictionary<uint, Models.Entities.Npc> _npcs;
		
		/// <summary>
		/// Gets the amount of npcs.
		/// </summary>
		public static int Count
		{
			get { return _npcs.Count; }
		}
		
		/// <summary>
		/// Static constructor for NPCCollection.
		/// </summary>
		static NPCCollection()
		{
			_npcs = new ConcurrentDictionary<uint, CandyConquer.WorldApi.Models.Entities.Npc>();
		}
		
		/// <summary>
		/// Loads the npc collection.
		/// </summary>
		public static void Load()
		{
			if (_npcs.Count > 0)
			{
				foreach (var npc in _npcs)
				{
					npc.Value.Despawn();
				}
			}
			
			_npcs.Clear();
			
			var dbNpcs = Database.Dal.Npcs.GetAll(Drivers.Settings.GlobalSettings.ServerName);
			
			foreach (var dbNpc in dbNpcs)
			{
				var npc = new Models.Entities.Npc(dbNpc);
				
				if (_npcs.TryAdd(npc.ClientId, npc))
				{
					npc.Teleport(dbNpc.MapId, npc.X, npc.Y);
				}
			}
		}
		
		/// <summary>
		/// Attempts to get a npc.
		/// </summary>
		/// <param name="npcId">The npc id.</param>
		/// <param name="npc">The npc.</param>
		/// <returns>True if the npc was retrieved, false otherwise.</returns>
		public static bool TryGetNpc(uint npcId, out Models.Entities.Npc npc)
		{
			return _npcs.TryGetValue(npcId, out npc);
		}
	}
}
