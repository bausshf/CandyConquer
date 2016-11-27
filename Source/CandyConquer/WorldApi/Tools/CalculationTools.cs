// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Tools
{
	/// <summary>
	/// Tools for calculation.
	/// </summary>
	public static class CalculationTools
	{
		/// <summary>
		/// Gets the success result of specific chance. (Out of 100)
		/// </summary>
		/// <param name="chance">The chance for success.</param>
		/// <returns>True if the chance was a success.</returns>
		public static bool ChanceSuccess(int chance)
		{
			if (chance >= 100)
			{
				return true;
			}
			
			return Drivers.Repositories.Safe.Random.Next(100) <= chance;
		}
		
		/// <summary>
		/// Gets the success result of specific chance. (out of 1000)
		/// </summary>
		/// <param name="chance">The chance for success.</param>
		/// <returns>True if the chance was a success.</returns>
		public static bool ChanceSuccessBig(int chance)
		{
			if (chance >= 1000)
			{
				return true;
			}
			
			return Drivers.Repositories.Safe.Random.Next(1000) <= chance;
		}
		
		/// <summary>
		/// Gets the ghost transformation model.
		/// </summary>
		/// <param name="model">The original model.</param>
		/// <returns>The ghost transformation model.</returns>
		public static ushort GetGhostTransform(ushort model)
		{
			ushort transform = 98;
			if (model % 10 < 3)
				transform = 99;
			return transform;
		}
	}
}
