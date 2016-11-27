// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Models.Entities
{
	/// <summary>
	/// Model for an entity.
	/// </summary>
	public interface IEntity
	{
		/// <summary>
		/// Gets or sets the client id.
		/// The client id is the unique id for an entity known by the client-side.
		/// </summary>
		uint ClientId { get; set; }
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		string Name { get; set; }
		/// <summary>
		/// Gets or sets the level of an entity.
		/// For non-attackable entities, the level indicates the requirement level.
		/// </summary>
		byte Level { get; set; }
	}
}
