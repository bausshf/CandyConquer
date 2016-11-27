// Project by Bauss
using System;
using System.Linq;

namespace CandyConquer.WorldApi.Controllers.Threads
{
	/// <summary>
	/// The dynamic database thread.
	/// </summary>
	public static class DynamicDatabaseThreadController
	{
		/// <summary>
		/// Handles the dynamic database thread.
		/// </summary>
		public static void Handle()
		{
			#region Load npcs
			var dbNpcs = Database.Dal.Npcs.GetAll(Drivers.Settings.WorldSettings.Server);
			if (dbNpcs.Count != Collections.NPCCollection.Count || dbNpcs.Count(npc =>
			                 {
			                 	Models.Entities.Npc dummy;
			                 	return !Collections.NPCCollection.TryGetNpc(npc.NpcId, out dummy);
			                 }) > 0)
			{
				Collections.NPCCollection.Load();
			}
			#endregion
		}
	}
}
