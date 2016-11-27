// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Models.Spells
{
	/// <summary>
	/// Interface for a spell model.
	/// </summary>
	public interface ISpellData
	{
		/// <summary>
		/// Gets the id.
		/// </summary>
		ushort Id { get; }
		
		/// <summary>
		/// Gets the level.
		/// </summary>
		ushort Level { get; }
		
		/// <summary>
		/// Gets the experience.
		/// </summary>
		uint Experience { get; }
		
		/// <summary>
		/// Sends the model to the client.
		/// </summary>
		void SendToClient();
		
		/// <summary>
		/// Raises the level.
		/// </summary>
		/// <param name="experience">The experience.</param>
		void Raise(uint experience);
	}
}
