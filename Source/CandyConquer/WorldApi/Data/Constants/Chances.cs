// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Data.Constants
{
	/// <summary>
	/// Chance constants.
	/// </summary>
	public static class Chances
	{
		/// <summary>
		/// Gets the chance of stamina to update when jumping.
		/// </summary>
		public static int StaminaOnJump
		{
			get { return Drivers.Settings.WorldSettings.StaminaOnJump; }
		}
		
		/// <summary>
		/// Gets the chance of getting first socket when upgrading.
		/// </summary>
		public static int FirstSocketChanceUpgrade
		{
			get { return Drivers.Settings.WorldSettings.FirstSocketChanceUpgrade; }
		}
		
		/// <summary>
		/// Gets the chance of getting second socket when upgrading.
		/// </summary>
		public static int SecondSocketChanceUpgrade
		{
			get { return Drivers.Settings.WorldSettings.SecondSocketChanceUpgrade; }
		}
		
		/// <summary>
		/// Gets the chance of getting a quality upgrade success.
		/// </summary>
		public static int UpgradeQualityChance
		{
			get { return Drivers.Settings.WorldSettings.UpgradeQualityChance; }
		}
		
		/// <summary>
		/// Gets the chance of getting a level upgrade success.
		/// </summary>
		public static int UpgradeLevelChance
		{
			get { return Drivers.Settings.WorldSettings.UpgradeLevelChance; }
		}
	}
}
