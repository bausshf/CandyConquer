// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Enums
{
	/// <summary>
	/// Enumeration of map flags
	/// </summary>
	[Flags]
	public enum MapFlag : ulong
    {
		/// <summary>
		/// Normal
		/// </summary>
        Normal = 0,
        /// <summary>
        /// PkField
        /// </summary>
        PkField = 1 << 0,//0x1
        /// <summary>
        /// ChangeMapDisabled
        /// </summary>
        ChangeMapDisable = 1 << 1,//0x2
        /// <summary>
        /// RecordDisable
        /// </summary>
        RecordDisable = 1 << 2,//0x4
        /// <summary>
        /// PkDisable
        /// </summary>
        PkDisable = 1 << 3,//0x8
        /// <summary>
        /// BoothEnable
        /// </summary>
        BoothEnable = 1 << 4,//0x10
        /// <summary>
        /// TeamDisable
        /// </summary>
        TeamDisable = 1 << 5,//0x20
        /// <summary>
        /// TeleportDisable
        /// </summary>
        TeleportDisable = 1 << 6,
        /// <summary>
        /// GuildMap
        /// </summary>
        GuildMap = 1 << 7,
        /// <summary>
        /// PrisonMap
        /// </summary>
        PrisonMap = 1 << 8,
        /// <summary>
        /// WingDisable
        /// </summary>
        WingDisable = 1 << 9,
        /// <summary>
        /// Family
        /// </summary>
        Family = 1 << 10,
        /// <summary>
        /// MineField
        /// </summary>
        MineField = 1 << 11,
        /// <summary>
        /// PkGame
        /// </summary>
        PkGame = 1 << 12,
        /// <summary>
        /// NeverWound
        /// </summary>
        NeverWound = 1 << 13,
        /// <summary>
        /// DeadIsland
        /// </summary>
        DeadIsland = 1 << 14
    }
}
