// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Controllers.Entities
{
	/// <summary>
	/// Controller for entities.
	/// </summary>
	public class EntityController : Controllers.Maps.MapObjectController
	{
		/// <summary>
		/// Gets the entity associated with the controller.
		/// </summary>
		public Models.Entities.IEntity Entity { get; protected set; }
		
		/// <summary>
		/// Constructor for the controller.
		/// </summary>
		public EntityController()
			: base()
		{
			
		}
		
		/// <summary>
		/// Creates the entity spawn packet based on the entity.
		/// </summary>
		/// <remarks>Only call this for players or mobs.</remarks>
		/// <returns>The buffer.</returns>
		protected byte[] CreateSpawnPacket()
		{
			return new Models.Packets.Location.EntitySpawnPacket
			{
				Entity = this.Entity
			};
		}
		
		/// <summary>
		/// Creates the remove entity spawn packet.
		/// Call this only for entities that are associated with this packet.
		/// </summary>
		/// <returns>The buffer.</returns>
		protected byte[] CreateRemoveSpawnPacket()
		{
			return new Models.Packets.Client.DataExchangePacket
			{
				ClientId = MapObject.ClientId,
				ExchangeType = Enums.ExchangeType.RemoveEntity
			};
		}
	}
}
