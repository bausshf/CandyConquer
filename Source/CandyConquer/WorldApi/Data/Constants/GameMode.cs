// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Data.Constants
{
	/// <summary>
	/// Different game modes.
	/// </summary>
	public static class GameMode
	{
		/// <summary>
		/// Gets a boolean determining whether the server allows fans.
		/// </summary>
		public static bool AllowFan
		{
			get { return Drivers.Settings.WorldSettings.AllowFan; }
		}
		
		/// <summary>
		/// Gets a boolean determining whether the server allows towers.
		/// </summary>
		public static bool AllowTower
		{
			get { return Drivers.Settings.WorldSettings.AllowTower; }
		}
		
		/// <summary>
		/// Gets a boolean determining whether the server allows steeds.
		/// </summary>
		public static bool AllowSteed
		{
			get { return Drivers.Settings.WorldSettings.AllowSteed; }
		}
		
		/// <summary>
		/// Gets the max level of the server.
		/// </summary>
		public static byte MaxLevel
		{
			get { return Drivers.Settings.WorldSettings.MaxLevel; }
		}
		
		/// <summary>
		/// The maximum amount of money.
		/// </summary>
		public static uint MaxMoney
		{
			get { return Drivers.Settings.WorldSettings.MaxMoney; }
		}
		
		/// <summary>
		/// The maximum amount of attribute points allowed for a player.
		/// </summary>
		public static ushort MaxAttributePoints
		{
			get { return Drivers.Settings.WorldSettings.MaxAttributePoints; }
		}
		
		/// <summary>
		/// The maximum amount of pk points.
		/// </summary>
		public static short MaxPkPoints
		{
			get { return Drivers.Settings.WorldSettings.MaxPkPoints; }
		}
		
		/// <summary>
		/// The maximum amount of reborns.
		/// </summary>
		public static byte MaxReborns
		{
			get { return Drivers.Settings.WorldSettings.MaxReborns; }
		}
		
		/// <summary>
		/// The amount of stamina upon login and revival.
		/// </summary>
		public static byte StartStamina
		{
			get { return Drivers.Settings.WorldSettings.StartStamina; }
		}
		
		/// <summary>
		/// The player experience rate.
		/// </summary>
		public static uint PlayerExperienceRate
		{
			get { return Drivers.Settings.WorldSettings.PlayerExperienceRate; }
		}
		
		/// <summary>
		/// The proficiency experience rate.
		/// </summary>
		public static uint ProficiencyExperienceRate
		{
			get { return Drivers.Settings.WorldSettings.ProficiencyExperienceRate; }
		}
		
		/// <summary>
		/// The skill experience rate.
		/// </summary>
		public static uint SkillExperienceRate
		{
			get { return Drivers.Settings.WorldSettings.SkillExperienceRate; }
		}
		
		/// <summary>
		/// The spell experience rate.
		/// </summary>
		public static uint SpellExperienceRate
		{
			get { return Drivers.Settings.WorldSettings.SpellExperienceRate; }
		}
		
		/// <summary>
		/// The max item plus.
		/// </summary>
		public static byte MaxPlus
		{
			get { return Drivers.Settings.WorldSettings.MaxPlus; }
		}
		
		/// <summary>
		/// The maximum nobility donation.
		/// </summary>
		public static long MaxNobilityDonation
		{
			get { return Drivers.Settings.WorldSettings.MaxNobilityDonation; }
		}
	}
}
