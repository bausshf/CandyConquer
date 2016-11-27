// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Enums
{
	/// <summary>
	/// Enumeration of string actions.
	/// </summary>
	public enum StringAction
	{
		/// <summary>
		/// None
		/// </summary>
		None = 0,
		/// <summary>
		/// Fireworks
		/// </summary>
        Fireworks,
        /// <summary>
        /// CreateGuild
        /// </summary>
        CreateGuild,
        /// <summary>
        /// Guild
        /// </summary>
        Guild,
        /// <summary>
        /// ChangeTitle
        /// </summary>
        ChangeTitle,
        /// <summary>
        /// DeleteRole
        /// </summary>
        DeleteRole = 5,
        /// <summary>
        /// Mate
        /// </summary>
        Mate,
        /// <summary>
        /// QueryNpc
        /// </summary>
        QueryNpc,
        /// <summary>
        /// Wanted
        /// </summary>
        Wanted,
        /// <summary>
        /// MapEffect
        /// </summary>
        MapEffect,
        /// <summary>
        /// RoleEffect
        /// </summary>
        RoleEffect = 10,
 		/// <summary>
 		/// MemberList
 		/// </summary>
        MemberList,
        /// <summary>
        /// KickoutGuildMember
        /// </summary>
        KickoutGuildMember,
        /// <summary>
        /// QueryWanted
        /// </summary>
        QueryWanted,
        /// <summary>
        /// QueryPoliceWanted
        /// </summary>
        QueryPoliceWanted,
        /// <summary>
        /// PoliceWanted
        /// </summary>
        PoliceWanted = 15,
        /// <summary>
        /// QueryMate
        /// </summary>
        QueryMate,
        /// <summary>
        /// AddDicePlayer
        /// </summary>
        AddDicePlayer,
        /// <summary>
        /// DeleteDicePlayer
        /// </summary>
        DeleteDicePlayer,
        /// <summary>
        /// DiceBonus
        /// </summary>
        DiceBonus,
        /// <summary>
        /// PlayerWave
        /// </summary>
        PlayerWave = 20,
        /// <summary>
        /// SetAlly
        /// </summary>
        SetAlly,
        /// <summary>
        /// SetEnemy
        /// </summary>
        SetEnemy,
        /// <summary>
        /// WhisperWindowInfo
        /// </summary>
        WhisperWindowInfo = 26
	}
}
