// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Models.Maps
{
	/// <summary>
	/// Model for a map object.
	/// </summary>
	public interface IMapObject
	{
		/// <summary>
		/// Gets or sets the client id.
		/// This is a unique id known by the client.
		/// </summary>
		uint ClientId { get; set; }
		/// <summary>
		/// Gets or sets a boolean whether the map object is alive or not.
		/// </summary>
		bool Alive { get; set; }
		/// <summary>
		/// Gets or sets the map.
		/// </summary>
		Map Map { get; set; }
		/// <summary>
		/// Gets the map id.
		/// </summary>
		int MapId { get; }
		/// <summary>
		/// Gets or sets th x coordinate.
		/// </summary>
		ushort X { get; set; }
		/// <summary>
		/// Gets or sets th y coordinate.
		/// </summary>
		ushort Y { get; set; }
		/// <summary>
		/// Gets or sets th last map.
		/// </summary>
		Map LastMap { get; set; }
		/// <summary>
		/// Gets the last map id.
		/// </summary>
		int LastMapId { get; }
		/// <summary>
		/// Gets or sets the last map x coordinate.
		/// </summary>
		ushort LastMapX { get; set; }
		/// <summary>
		/// Gets or sets the last map y coordinate.
		/// </summary>
		ushort LastMapY { get; set; }
		/// <summary>
		/// Gets the last x coordinate.
		/// </summary>
		ushort LastX { get; }
		/// <summary>
		/// Gets the last y coordinate.
		/// </summary>
		ushort LastY { get; }
		/// <summary>
		/// Gets or sets the direction of the object.
		/// </summary>
		Enums.Direction Direction { get; set; }
		/// <summary>
		/// Gets a value indicating whether the screen of the map object can be updated or not.
		/// </summary>
		bool CanUpdateScreen { get; }
		/// <summary>
		/// Gets or sets the status flag.
		/// </summary>
		ulong StatusFlag { get; set; }
		/// <summary>
		/// Gets or sets a flag whether the map object should be idle or not.
		/// This is used by the UpdateScreenFlags.Monster
		/// </summary>
		bool Idle { get; set; }
		
		/// <summary>
		/// Method that must be implemented by any map objects that spawns to the client.
		/// </summary>
		/// <returns>A buffer corresponding to the spawn packet for the object.</returns>
		byte[] GetSpawnPacket();
		/// <summary>
		/// Method that must be implemented by any map objects that can be removed from the client.
		/// </summary>
		/// <returns>A buffer corresponding to the remove spawn packet for the object.</returns>
		byte[] GetRemoveSpawnPacket();
	}
}
