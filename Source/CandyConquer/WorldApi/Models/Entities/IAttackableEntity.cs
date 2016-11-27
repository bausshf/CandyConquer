// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Models.Entities
{
	/// <summary>
	/// Model for an attackable entity.
	/// </summary>
	public interface IAttackableEntity
	{
		/// <summary>
		/// Gets or sets the client id.
		/// This is the unique id known by the client.
		/// </summary>
		uint ClientId { get; set; }
		/// <summary>
		/// Gets or sets the level.
		/// </summary>
		byte Level { get; set; }
		/// <summary>
		/// Gets or sets the strength.
		/// </summary>
		ushort Strength { get; set; }
		/// <summary>
		/// Gets or sets the vitality.
		/// </summary>
		ushort Vitality { get; set; }
		/// <summary>
		/// Gets or sets the agility.
		/// </summary>
		ushort Agility { get; set; }
		/// <summary>
		/// Gets or sets the spirit.
		/// </summary>
		ushort Spirit { get; set; }
		/// <summary>
		/// Gets or sets the maximum hp.
		/// </summary>
		int MaxHP { get; set; }
		/// <summary>
		/// Gets or sets the hp.
		/// </summary>
		int HP { get; set; }
		/// <summary>
		/// Gets or sets the max mp.
		/// </summary>
		int MaxMP { get; set; }
		/// <summary>
		/// Gets or sets the mp.
		/// </summary>
		int MP { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether the entity is alive or not.
		/// </summary>
		bool Alive { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether the entity can attack or not.
		/// </summary>
		bool CanAttack { get; set; }
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		IAttackableEntity Target { get; set; }
		/// <summary>
		/// Gets or sets the reborns of the entity.
		/// </summary>
		byte Reborns { get; set; }
		/// <summary>
		/// Gets or sets the poison effect of the entity.
		/// </summary>
		int PoisonEffect { get; set; }
		/// <summary>
		/// The next poison damage time.
		/// </summary>
		DateTime NextPoison { get; set; }
	}
}
