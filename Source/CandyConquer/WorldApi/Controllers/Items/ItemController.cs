// Project by Bauss
using System;
using System.Threading.Tasks;

namespace CandyConquer.WorldApi.Controllers.Items
{
	/// <summary>
	/// Controller for items.
	/// </summary>
	public class ItemController : Controllers.Maps.MapObjectController
	{
		/// <summary>
		/// The item tied to the controller.
		/// </summary>
		public Models.Items.Item Item { get; protected set; }
		
		/// <summary>
		/// Creates a new item controller.
		/// </summary>
		public ItemController()
			: base()
		{
		}
		
		/// <summary>
		/// Updates a client with the item.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="action">The update action.</param>
		public void UpdateClient(Models.Entities.Player player, Enums.ItemUpdateAction action)
		{
			player.ClientSocket.Send(new Models.Packets.Items.ItemInfoPacket
			                         {
			                         	Item = this.Item,
			                         	Data = (ushort)action,
			                         	Position = this.Item.Position
			                         });
		}
		
		/// <summary>
		/// Updates the client as a view item.
		/// </summary>
		/// <param name="owner">The owner of the item.</param>
		/// <param name="viewer">The view of the item.</param>
		public void ViewItem(Models.Entities.Player owner, Models.Entities.Player viewer)
		{
			viewer.ClientSocket.Send(new Models.Packets.Items.ViewItemPacket
			                         {
			                         	OwnerClientId = owner.ClientId,
			                         	Item = this.Item
			                         });
		}
		
		/// <summary>
		/// Updates the database of the item.
		/// </summary>
		public void UpdateDatabase()
		{
			if (Item.Position == Enums.ItemPosition.Inventory)
			{
				Item.DbOwnerItem.Update(Database.Models.DbOwnerItem.Inventories);
			}
			else
			{
				Item.DbOwnerItem.Update(Database.Models.DbOwnerItem.Equipments);
			}
		}
		
		/// <summary>
		/// Drops the item.
		/// </summary>
		/// <param name="mapId">The map id.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="playerDrop">Boolean indicating whether the drop is by a player or not.</param>
		/// <param name="dropClientId">The client id of the player who dropped it.</param>
		public void Drop(int mapId, ushort x, ushort y, bool playerDrop, uint dropClientId)
		{
			Item.DropTime = DateTime.UtcNow;
			Item.PlayerDrop = true;
			Item.DropClientId = dropClientId;
			if (mapId >= 1000000)
			{
				Item.TeleportDynamic(mapId, x, y);
			}
			else
			{
				Item.Teleport(mapId, x, y);
			}
			
			Task.Run(async() => await Item.ResetLocationAsync());
		}
		
		/// <summary>
		/// Upgrades the sockets by upgrade chance.
		/// </summary>
		/// <param name="owner">The owner of the item.</param>
		public void UpgradeSockets(Models.Entities.Player owner)
		{
			if (Item.Gem1 == Enums.Gem.NoSocket &&
			    Tools.CalculationTools.ChanceSuccess(Data.Constants.Chances.FirstSocketChanceUpgrade))
			{
				Item.Gem1 = Enums.Gem.EmptySocket;
			}
			
			if (Item.Gem1 != Enums.Gem.NoSocket &&
			    Item.Gem2 == Enums.Gem.NoSocket &&
			    Tools.CalculationTools.ChanceSuccess(Data.Constants.Chances.SecondSocketChanceUpgrade))
			{
				Item.Gem2 = Enums.Gem.EmptySocket;
			}
			
			UpdateDatabase();
			UpdateClient(owner, Enums.ItemUpdateAction.Update);
		}
	}
}
