// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Enums
{
	/// <summary>
	/// Enumeration of guild donation flags.
	/// </summary>
	[Flags()]
	public enum GuildDonationFlags : uint
	{
		/// <summary>
		/// None
		/// </summary>
		None = 0,
		/// <summary>
		/// Silver
		/// </summary>
        Silver = 1u << 0,
        /// <summary>
        /// CPs
        /// </summary>
        CP = 1u << 1,
        /// <summary>
        /// Guide
        /// </summary>
        Guide = 1u << 2,
        /// <summary>
        /// Pk
        /// </summary>
        Pk = 1u << 3,
        /// <summary>
        /// Arsenal
        /// </summary>
        Arsenal = 1u << 4,
        /// <summary>
        /// Unknown5
        /// </summary>
        Unknown5 = 1u << 5,
        /// <summary>
        /// Unknown6
        /// </summary>
        Unknown6 = 1u << 6,
        /// <summary>
        /// Unknown7
        /// </summary>
        Unknown7 = 1u << 7,
        /// <summary>
        /// Unknown8
        /// </summary>
        Unknown8 = 1u << 8,
        /// <summary>
        /// Unknown9
        /// </summary>
        Unknown9 = 1u << 9,
        /// <summary>
        /// Unknown10
        /// </summary>
        Unknown10 = 1u << 10,
        /// <summary>
        /// Unknown11
        /// </summary>
        Unknown11 = 1u << 11,
        /// <summary>
        /// Unknown 12
        /// </summary>
        Unknown12 = 1u << 12,
        /// <summary>
        /// Unknown 13
        /// </summary>
        Unknown13 = 1u << 13,
        /// <summary>
        /// Unknown 14
        /// </summary>
        Unknown14 = 1u << 14,
        /// <summary>
        /// Unknown 15
        /// </summary>
        Unknown15 = 1u << 15,
        /// <summary>
        /// Unknown 16
        /// </summary>
        Unknown16 = 1u << 16,
        /// <summary>
        /// Unknown 17
        /// </summary>
        Unknown17 = 1u << 17,
        /// <summary>
        /// Unknown 18
        /// </summary>
        Unknown18 = 1u << 18,
        /// <summary>
        /// Unknown 19
        /// </summary>
        Unknown19 = 1u << 19,
        /// <summary>
        /// Unknown 20
        /// </summary>
        Unknown20 = 1u << 20,
        /// <summary>
        /// Unknown 21
        /// </summary>
        Unknown21 = 1u << 21,
        /// <summary>
        /// Unknown 22
        /// </summary>
        Unknown22 = 1u << 22,
        /// <summary>
        /// Unknown 23
        /// </summary>
        Unknown23 = 1u << 23,
        /// <summary>
        /// Unknown 24
        /// </summary>
        Unknown24 = 1u << 24,
        /// <summary>
        /// Unknown 25
        /// </summary>
        Unknown25 = 1u << 25,
        /// <summary>
        /// Unknown 26
        /// </summary>
        Unknown26 = 1u << 26,
        /// <summary>
        /// Unknwon 27
        /// </summary>
        Unknown27 = 1u << 27,
        /// <summary>
        /// Unknown 28
        /// </summary>
        Unknown28 = 1u << 28,
        /// <summary>
        /// Unknown 29
        /// </summary>
        Unknown29 = 1u << 29,
        /// <summary>
        /// Unknown 30
        /// </summary>
        Unknown30 = 1u << 30,
        /// <summary>
        /// Unknown 31
        /// </summary>
        Unknown31 = 1u << 31,

        /// <summary>
        /// All donations.
        /// </summary>
        AllDonations = Silver | CP | Guide | Pk | Arsenal
	}
}
