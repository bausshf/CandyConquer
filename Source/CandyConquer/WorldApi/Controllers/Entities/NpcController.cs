// Project by Bauss
using System;
using System.Linq;

namespace CandyConquer.WorldApi.Controllers.Entities
{
	/// <summary>
	/// Controller for npcs.
	/// </summary>
	public class NpcController : EntityController
	{
		/// <summary>
		/// Gets the npc associated with the controller.
		/// </summary>
		public Models.Entities.Npc Npc { get; protected set; }
		
		/// <summary>
		/// Constructor for the controller.
		/// </summary>
		public NpcController()
			: base()
		{
			
		}
		
		/// <summary>
		/// Gets the npc spawn packet.
		/// </summary>
		/// <returns>The npc spawn packet.</returns>
		protected byte[] GetNpcSpawnPacket()
		{
			return new Models.Packets.Location.NpcSpawnPacket()
			{
				Name = Npc.Name,
				ClientId = Npc.ClientId,
				NpcId = Npc.ClientId,
				X = Npc.X,
				Y = Npc.Y,
				Mesh = Npc.Mesh,
				Flag = Npc.Flag
			};
		}
		
		/// <summary>
		/// Invokes the dialog tied with the npc.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="option">The option.</param>
		public void Invoke(Models.Entities.Player player, byte option)
		{
			var invokeId = Npc.ClientId;
			if (player.Map.IsDynamic)
			{
				if (player.Houses.Count > 0 && invokeId != 200000)
				{
					var houseId = player.Houses.GetAll()
						.Where(house => house.DynamicMapId == player.MapId)
						.Select(house => house.DbPlayerHouse.MapId).FirstOrDefault() + 100000;
					
					if (houseId == Npc.ClientId)
					{
						invokeId = 100000;
					}
				}
			}
			
			if (!Collections.NPCScriptCollection.Invoke(player, invokeId, option))
			{
				player.SendFormattedSystemMessage("NPC_NOT_FOUND", true, invokeId);
			}
		}
	}
}
