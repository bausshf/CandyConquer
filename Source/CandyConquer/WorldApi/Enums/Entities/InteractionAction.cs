// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Enums
{
	/// <summary>
	/// Enumeration of interaction actions.
	/// </summary>
	public enum InteractionAction
	{
		/// <summary>
		/// None.
		/// </summary>
		None = 0,
		
		/// <summary>
		/// Steal.
		/// </summary>
		Steal,
		
		/// <summary>
		/// Attack.
		/// </summary>
		Attack,
		
		/// <summary>
		/// Heal.
		/// </summary>
		Heal,
		
		/// <summary>
		/// Poison.
		/// </summary>
		Poison,
		
		/// <summary>
		/// Assassinate.
		/// </summary>
		Assassinate = 5,
		
		/// <summary>
		/// Freeze.
		/// </summary>
		Freeze,
		
		/// <summary>
		/// Unfreeze.
		/// </summary>
		Unfreeze,
		
		/// <summary>
		/// Court.
		/// </summary>
		Court,
		
		/// <summary>
		/// Marry.
		/// </summary>
		Marry,
		
		/// <summary>
		/// Divorce.
		/// </summary>
		Divorce = 10,
		
		/// <summary>
		/// PresentMoney.
		/// </summary>
		PresentMoney,
		
		/// <summary>
		/// PresentItem.
		/// </summary>
		PresentItem,
		
		/// <summary>
		/// SendFlowers.
		/// </summary>
		SendFlowers,
		
		/// <summary>
		/// Kill.
		/// </summary>
		Kill = 14,
		
		/// <summary>
		/// JoinGuild.
		/// </summary>
		JoinGuild = 15,
		
		/// <summary>
		/// AcceptGuildMember.
		/// </summary>
		AcceptGuildMember,
		
		/// <summary>
		/// KickoutGuildMember.
		/// </summary>
		KickoutGuildMember,
		
		/// <summary>
		/// PresentPower.
		/// </summary>
		PresentPower,
		
		/// <summary>
		/// QueryInfo.
		/// </summary>
		QueryInfo,
		
		/// <summary>
		/// RushAttack.
		/// </summary>
		RushAttack = 20,
		
		/// <summary>
		/// Unknown 21.
		/// </summary>
		Unknown21,
		
		/// <summary>
		/// AbortMagic.
		/// </summary>
		AbortMagic,
		
		/// <summary>
		/// ReflectWeapon.
		/// </summary>
		ReflectWeapon,
		
		/// <summary>
		/// MagicAttack.
		/// </summary>
		MagicAttack,
		
		/// <summary>
		/// Unknown 25.
		/// </summary>
		Unknown25 = 25,
		
		/// <summary>
		/// ReflectMagic.
		/// </summary>
		ReflectMagic,
		
		/// <summary>
		/// Dash.
		/// </summary>
		Dash,
		
		/// <summary>
		/// Shoot.
		/// </summary>
		Shoot,
		
		/// <summary>
		/// Quarry.
		/// </summary>
		Quarry,
		
		/// <summary>
		/// Chop.
		/// </summary>
		Chop = 30,
		
		/// <summary>
		/// Hustle.
		/// </summary>
		Hustle,
		
		/// <summary>
		/// Soul.
		/// </summary>
		Soul,
		
		/// <summary>
		/// AcceptMerchant.
		/// </summary>
		AcceptMerchant,
		
		/// <summary>
		/// CounterKill.
		/// </summary>
		CounterKill = 43,
		
		/// <summary>
		/// CounterKillSwitch.
		/// </summary>
		CounterKillSwitch = 44,
		
		/// <summary>
		/// FatalStrike.
		/// </summary>
		FatalStrike =45,
		
		/// <summary>
		/// InteractRequest.
		/// </summary>
		InteractRequest = 46,

		/// <summary>
		/// InteractConfirm.
		/// </summary>
		InteractConfirm,
		
		/// <summary>
		/// Interact.
		/// </summary>
		Interact,
		
		/// <summary>
		/// InteractUnknown.
		/// </summary>
		InteractUnknown,
		
		/// <summary>
		/// InteractStop.
		/// </summary>
		InteractStop
	}
}
