// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Data.Constants
{
	/// <summary>
	/// Time constants.
	/// </summary>
	public static class Time
	{
		/// <summary>
		/// Gets the time it takes for a dropped item to be removed. (Milliseconds)
		/// </summary>
		public static int DroppedItemRemovalTime
		{
			get { return Drivers.Settings.WorldSettings.DroppedItemRemovalTime; }
		}
		/// <summary>
		/// Gets the time it takes before a dropped item can be picked up by anoGets ther player. (Milliseconds)
		/// </summary>
		public static int DropTimeShare
		{
			get { return Drivers.Settings.WorldSettings.DropTimeShare; }
		}
		/// <summary>
		/// Gets the time it takes for stamina to update when lying down. (Milliseconds)
		/// </summary>
		public static int StaminaLieTime
		{
			get { return Drivers.Settings.WorldSettings.StaminaLieTime; }
		}
		/// <summary>
		/// Gets the time a broadcast message lasts. (Milliseconds)
		/// </summary>
		public static int BroadcastWaitTime
		{
			get { return Drivers.Settings.WorldSettings.BroadcastWaitTime; }
		}
		/// <summary>
		/// Gets the time blue name lasts. (Milliseconds)
		/// </summary>
		public static int BlueNameTime
		{
			get { return Drivers.Settings.WorldSettings.BlueNameTime; }
		}
		/// <summary>
		/// Gets the time it takes till a monster revives after dying. (Milliseconds)
		/// </summary>
		public static int MonsterReviveTime
		{
			get { return Drivers.Settings.WorldSettings.MonsterReviveTime; }
		}
		/// <summary>
		/// Gets the time that has to be between each manual attack. (Milliseconds)
		/// </summary>
		public static int AttackInterval
		{
			get { return Drivers.Settings.WorldSettings.AttackInterval; }
		}
		/// <summary>
		/// Gets the time that a player is protected after reviving. (Seconds)
		/// </summary>
		public static int ReviveProtectionTime
		{
			get { return Drivers.Settings.WorldSettings.ReviveProtectionTime; }
		}
		/// <summary>
		/// Gets the time between each long skill.
		/// </summary>
		public static int LongSkillTime
		{
			get { return Drivers.Settings.WorldSettings.LongSkillTime; }
		}
		/// <summary>
		/// Gets the time between each small long skill.
		/// </summary>
		public static int SmallLongSkillTime
		{
			get { return Drivers.Settings.WorldSettings.SmallLongSkillTime; }
		}
		/// <summary>
		/// Gets the time between each pk point removal.
		/// </summary>
		public static int PKPointsRemovalTime
		{
			get { return Drivers.Settings.WorldSettings.PKPointsRemovalTime; }
		}
		/// <summary>
		/// Gets the time to wait between each tournament in the tournament queue.
		/// </summary>
		public static int TournamentQueueDelayTime
		{
			get { return Drivers.Settings.WorldSettings.TournamentQueueDelayTime; }
		}
	}
}
