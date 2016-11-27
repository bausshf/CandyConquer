// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Enums
{
	/// <summary>
	/// Enumeration of status flags.
	/// </summary>
	[Flags]
    public enum StatusFlag : ulong
    {
    	/// <summary>
    	/// None
    	/// </summary>
        None = 0UL,
        /// <summary>
        /// BlueName
        /// </summary>
        BlueName = 1UL << 0,
        /// <summary>
        /// Poisoned.
        /// </summary>
        Poisoned = 1UL << 1,
        /// <summary>
        /// Full invisibility.
        /// </summary>
        FullInvis = 1UL << 2,//(Full invisibility)
        /// <summary>
        /// Fade
        /// </summary>
        Fade  = 1UL << 3,
        /// <summary>
        /// StartXp
        /// </summary>
        StartXp = 1UL << 4,
        /// <summary>
        /// Ghost
        /// </summary>
        Ghost = 1UL << 5,
        /// <summary>
        /// TeamLeader
        /// </summary>
        TeamLeader = 1UL << 6,
        /// <summary>
        /// StarOfAccuracy
        /// </summary>
        StarOfAccuracy = 1UL << 7,
        /// <summary>
        /// Shield
        /// </summary>
        Shield = 1UL << 8,
        /// <summary>
        /// Stigma
        /// </summary>
        Stigma = 1UL << 9,
        /// <summary>
        /// Dead
        /// </summary>
        Dead = 1UL << 10,
        /// <summary>
        /// PermanentInvisible
        /// </summary>
        PermanentInvisible = 1UL << 11,
        /// <summary>
        /// Unknown12
        /// </summary>
        Unknown12 = 1UL << 12,
        /// <summary>
        /// Unknown13
        /// </summary>
        Unknown13 = 1UL << 13,
        /// <summary>
        /// RedName
        /// </summary>
        RedName = 1UL << 14,
        /// <summary>
        /// BlackName
        /// </summary>
        BlackName = 1UL << 15,
        /// <summary>
        /// Unknown16
        /// </summary>
        Unknown16 = 1UL << 16,
        /// <summary>
        /// Unknown17
        /// </summary>
        Unknown17 = 1UL << 17,
        /// <summary>
        /// Superman
        /// </summary>
        Superman = 1UL << 18,
        /// <summary>
        /// ReflectThing
        /// </summary>
        ReflectThing = 1UL << 19,
        /// <summary>
        /// AltReflectThing
        /// </summary>
        AltReflectThing = 1UL << 20,
        /// <summary>
        /// Unknown21
        /// </summary>
        Unknown21 = 1UL << 21,
        /// <summary>
        /// PartiallyInvisible
        /// </summary>
        PartiallyInvisible = 1UL << 22,
        /// <summary>
        /// Cyclone
        /// </summary>
        Cyclone = 1UL << 23,
        /// <summary>
        /// Unknown24
        /// </summary>
        Unknown24 = 1UL << 24,
        /// <summary>
        /// Unknown25
        /// </summary>
        Unknown25 = 1UL << 25,
        /// <summary>
        /// Unknown26
        /// </summary>
        Unknown26 = 1UL << 26,
        /// <summary>
        /// Fly
        /// </summary>
        Fly = 1UL << 27,
        /// <summary>
        /// Unknown 28
        /// </summary>
        Unknown28 = 1UL << 28,
        /// <summary>
        /// Unknown29
        /// </summary>
        Unknown29 = 1UL << 29,
        /// <summary>
        /// LuckyTime
        /// </summary>
        LuckyTime = 1UL << 30,
        /// <summary>
        /// Pray
        /// </summary>
        Pray = 1UL << 31,
        /// <summary>
        /// Cursed
        /// </summary>
        Cursed = 1UL << 32,
        /// <summary>
        /// HeavenBless
        /// </summary>
        HeavenBless = 1UL << 33,
        /// <summary>
        /// TopGuild
        /// </summary>
        TopGuild = 1UL << 34,
        /// <summary>
        /// TopDeputy
        /// </summary>
        TopDeputy = 1UL << 35,
        /// <summary>
        /// MonthlyPk
        /// </summary>
        MonthlyPk = 1UL << 36,
        /// <summary>
        /// WeeklyPk
        /// </summary>
        WeeklyPk = 1UL << 37,
        /// <summary>
        /// TopWarrior
        /// </summary>
        TopWarrior = 1UL << 38,
        /// <summary>
        /// TopTrojan
        /// </summary>
        TopTrojan = 1UL << 39,
        /// <summary>
        /// TopArcher
        /// </summary>
        TopArcher = 1UL << 40,
        /// <summary>
        /// TopWaterTaoist
        /// </summary>
        TopWaterTaoist = 1UL << 41,
        /// <summary>
        /// TopFireTaoist
        /// </summary>
        TopFireTaoist = 1UL << 42,
        /// <summary>
        /// TopNinja
        /// </summary>
        TopNinja = 1UL << 43,
        /// <summary>
        /// Unknown44
        /// </summary>
        Unknown44 = 1UL << 44,
        /// <summary>
        /// Unknown45
        /// </summary>
        Unknown45 = 1UL << 45,
        /// <summary>
        /// Vortex
        /// </summary>
        Vortex = 1UL << 46,
        /// <summary>
        /// FatalStrike
        /// </summary>
        FatalStrike = 1UL << 47,
        /// <summary>
        /// OrangeHaloGlow
        /// </summary>
        OrangeHaloGlow = 1UL << 48,
        /// <summary>
        /// Unknown49
        /// </summary>
        Unknown49 = 1UL << 49,
        /// <summary>
        /// LowVigorUnableToJump
        /// </summary>
        LowVigorUnableToJump = 1UL << 50,
        /// <summary>
        /// Riding
        /// </summary>
        Riding = 1UL << 50,
        /// <summary>
        /// TopSpouse
        /// </summary>
        TopSpouse = 1UL << 51,
        /// <summary>
        /// SparkleHalo
        /// </summary>
        SparkleHalo = 1UL << 52,
        /// <summary>
        /// NoPotion
        /// </summary>
        NoPotion = 1UL << 53,
        /// <summary>
        /// Dazed
        /// </summary>
        Dazed = 1UL << 54,
        /// <summary>
        /// BlueRestoreAura
        /// </summary>
        BlueRestoreAura = 1UL << 55,
        /// <summary>
        /// MoveSpeedRecovered
        /// </summary>
        MoveSpeedRecovered = 1UL << 56,
        /// <summary>
        /// SuperShieldHalow
        /// </summary>
        SuperShieldHalo = 1UL << 57,
        /// <summary>
        /// HUDEDazed
        /// </summary>
        HUGEDazed = 1UL << 58,
        /// <summary>
        /// IceBlock
        /// </summary>
        IceBlock = 1UL << 59,
        /// <summary>
        /// Confused
        /// </summary>
        Confused = 1UL << 60,
        /// <summary>
        /// Unknown61
        /// </summary>
        Unknown61 = 1UL << 61,
        /// <summary>
        /// Unknown62
        /// </summary>
        Unknown62 = 1UL << 62,
        /// <summary>
        /// Unknown63
        /// </summary>
        Unknown63 = 1UL << 63
    }
}
