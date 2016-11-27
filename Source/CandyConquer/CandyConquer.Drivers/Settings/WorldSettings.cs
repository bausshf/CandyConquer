// Project by Bauss
using System;

namespace CandyConquer.Drivers.Settings
{
	/// <summary>
	/// The world settings.
	/// </summary>
	public static class WorldSettings
	{
		/// <summary>
		/// Gets the server IP.
		/// </summary>
		public static string IPAddress { get; private set; }
		
		/// <summary>
		/// Gets the server port.
		/// </summary>
		public static int Port { get; private set; }
		
		/// <summary>
		/// Gets the cryptography key.
		/// </summary>
		public static byte[] CryptoKey { get; private set; }
		
		/// <summary>
		/// Gets the server name.
		/// </summary>
		public static string Server { get; private set; }
		
		/// <summary>
		/// Boolean determining whether the server allows fan or not.
		/// </summary>
		public static bool AllowFan { get; private set; }
		
		/// <summary>
		/// Boolean determining whether the server allows towers or not.
		/// </summary>
		public static bool AllowTower { get; private set; }
		
		/// <summary>
		/// Boolean determining whether the server allows steeds or not.
		/// </summary>
		public static bool AllowSteed { get; private set; }
		
		/// <summary>
		/// The max level of the server.
		/// </summary>
		public static byte MaxLevel { get; private set; }
		
		/// <summary>
		/// Gets the maximum amount of money.
		/// </summary>
		public static uint MaxMoney { get; private set; }
		
		/// <summary>
		/// Gets the maximum amount of attribute points allowed for a player.
		/// </summary>
		public static ushort MaxAttributePoints { get; private set; }
		
		/// <summary>
		/// Gets the maximum amount of pk points.
		/// </summary>
		public static short MaxPkPoints { get; private set; }
		
		/// <summary>
		/// Gets the maximum amount of reborns.
		/// </summary>
		public static byte MaxReborns { get; private set; }
		
		/// <summary>
		/// Gets the amount of stamina upon login and revival.
		/// </summary>
		public static byte StartStamina { get; private set; }
		
		/// <summary>
		/// Gets the player experience rate.
		/// </summary>
		public static uint PlayerExperienceRate { get; private set; }
		
		/// <summary>
		/// Gets the proficiency experience rate.
		/// </summary>
		public static uint ProficiencyExperienceRate { get; private set; }
		
		/// <summary>
		/// Gets the skill experience rate.
		/// </summary>
		public static uint SkillExperienceRate { get; private set; }
		
		/// <summary>
		/// Gets the spell experience rate.
		/// </summary>
		public static uint SpellExperienceRate { get; private set; }
		
		/// <summary>
		/// Gets the max item plus.
		/// </summary>
		public static byte MaxPlus { get; private set; }
		
		/// <summary>
		/// Gets the maximum nobility donation.
		/// </summary>
		public static long MaxNobilityDonation { get; private set; }
		
		/// <summary>
		/// Gets the chance of stamina to update when jumping.
		/// </summary>
		public static int StaminaOnJump { get; private set; }
		
		/// <summary>
		/// Gets the chance of getting first socket when upgrading.
		/// </summary>
		public static int FirstSocketChanceUpgrade { get; private set; }
		
		/// <summary>
		/// Gets the chance of getting second socket when upgrading.
		/// </summary>
		public static int SecondSocketChanceUpgrade { get; private set; }
		
		/// <summary>
		/// Gets the chance of getting a quality upgrade success.
		/// </summary>
		public static int UpgradeQualityChance { get; private set; }
		
		/// <summary>
		/// Gets the chance of getting a level upgrade success.
		/// </summary>
		public static int UpgradeLevelChance { get; private set; }
		
		/// <summary>
		/// Gets the time it takes for a dropped item to be removed. (Milliseconds)
		/// </summary>
		public static int DroppedItemRemovalTime { get; private set; }
		/// <summary>
		/// Gets the time it takes before a dropped item can be picked up by anoGets ther player. (Milliseconds)
		/// </summary>
		public static int DropTimeShare { get; private set; }
		/// <summary>
		/// Gets the time it takes for stamina to update when lying down. (Milliseconds)
		/// </summary>
		public static int StaminaLieTime { get; private set; }
		/// <summary>
		/// Gets the time a broadcast message lasts. (Milliseconds)
		/// </summary>
		public static int BroadcastWaitTime { get; private set; }
		/// <summary>
		/// Gets the time blue name lasts. (Milliseconds)
		/// </summary>
		public static int BlueNameTime { get; private set; }
		/// <summary>
		/// Gets the time it takes till a monster revives after dying. (Milliseconds)
		/// </summary>
		public static int MonsterReviveTime { get; private set; }
		/// <summary>
		/// Gets the time that has to be between each manual attack. (Milliseconds)
		/// </summary>
		public static int AttackInterval { get; private set; }
		/// <summary>
		/// Gets the time that a player is protected after reviving. (Seconds)
		/// </summary>
		public static int ReviveProtectionTime { get; private set; }
		/// <summary>
		/// Gets the time between each long skill.
		/// </summary>
		public static int LongSkillTime { get; private set; }
		/// <summary>
		/// Gets the time between each small long skill.
		/// </summary>
		public static int SmallLongSkillTime { get; private set; }
		/// <summary>
		/// Gets the time between each pk point removal.
		/// </summary>
		public static int PKPointsRemovalTime { get; private set; }
		/// <summary>
		/// Gets the time to wait between each tournament in the tournament queue. (Milliseconds)
		/// </summary>
		public static int TournamentQueueDelayTime { get; private set; }
		
		/// <summary>
		/// Loads the world settings.
		/// </summary>
		public static void Load()
		{
			Console.WriteLine(Messages.LOADING_WORLD_SETTINGS);
			var settings = new Repositories.IO.IniFile(DatabaseSettings.WorldFlatDatabase + "\\Config.ini");
			if (settings.Exists())
			{
				settings.Open();
				var section = settings.GlobalSection;
				
				IPAddress = section.GetValue("IPAddress");
				Port = Convert.ToInt32(section.GetValue("Port"));
				CryptoKey = System.Text.Encoding.ASCII.GetBytes(section.GetValue("CryptoKey"));
				Server = section.GetValue("Server");
				AllowFan = section.GetValue("AllowFan") == "True";
				AllowTower = section.GetValue("AllowTower") == "True";
				AllowSteed = section.GetValue("AllowSteed") == "True";
				MaxLevel = Convert.ToByte(section.GetValue("MaxLevel"));
				MaxMoney = Convert.ToUInt32(section.GetValue("MaxMoney"));
				MaxAttributePoints = Convert.ToUInt16(section.GetValue("MaxAttributePoints"));
				MaxPkPoints = Convert.ToInt16(section.GetValue("MaxPkPoints"));
				MaxReborns = Convert.ToByte(section.GetValue("MaxReborns"));
				StartStamina = Convert.ToByte(section.GetValue("StartStamina"));
				PlayerExperienceRate = Convert.ToUInt32(section.GetValue("PlayerExperienceRate"));
				ProficiencyExperienceRate = Convert.ToUInt32(section.GetValue("ProficiencyExperienceRate"));
				SkillExperienceRate = Convert.ToUInt32(section.GetValue("SkillExperienceRate"));
				SpellExperienceRate = Convert.ToUInt32(section.GetValue("SpellExperienceRate"));
				MaxPlus = Convert.ToByte(section.GetValue("MaxPlus"));
				MaxNobilityDonation = Convert.ToInt64(section.GetValue("MaxNobilityDonation"));
				StaminaOnJump = Convert.ToInt32(section.GetValue("StaminaOnJump"));
				FirstSocketChanceUpgrade = Convert.ToInt32(section.GetValue("FirstSocketChanceUpgrade"));
				SecondSocketChanceUpgrade = Convert.ToInt32(section.GetValue("SecondSocketChanceUpgrade"));
				UpgradeQualityChance = Convert.ToInt32(section.GetValue("UpgradeQualityChance"));
				UpgradeLevelChance = Convert.ToInt32(section.GetValue("UpgradeLevelChance"));
				DroppedItemRemovalTime = Convert.ToInt32(section.GetValue("DroppedItemRemovalTime"));
				DropTimeShare = Convert.ToInt32(section.GetValue("DropTimeShare"));
				StaminaLieTime = Convert.ToInt32(section.GetValue("StaminaLieTime"));
				BroadcastWaitTime = Convert.ToInt32(section.GetValue("BroadcastWaitTime"));
				BlueNameTime = Convert.ToInt32(section.GetValue("BlueNameTime"));
				MonsterReviveTime = Convert.ToInt32(section.GetValue("MonsterReviveTime"));
				AttackInterval = Convert.ToInt32(section.GetValue("AttackInterval"));
				ReviveProtectionTime = Convert.ToInt32(section.GetValue("ReviveProtectionTime"));
				LongSkillTime = Convert.ToInt32(section.GetValue("LongSkillTime"));
				SmallLongSkillTime = Convert.ToInt32(section.GetValue("SmallLongSkillTime"));
				PKPointsRemovalTime = Convert.ToInt32(section.GetValue("PKPointsRemovalTime"));
				TournamentQueueDelayTime = Convert.ToInt32(section.GetValue("TournamentQueueDelayTime"));
			}
		}
	}
}
