// Project by Bauss
using System;
using CandyConquer.WorldApi.Controllers.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Skills
{
	/// <summary>
	/// Handler for target validation.
	/// </summary>
	public static class TargetValidation
	{
		/// <summary>
		/// Validates a target.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="target">The target.</param>
		/// <returns>True if the target is valid.</returns>
		public static bool Validate(AttackableEntityController attacker, AttackableEntityController target)
		{
			if (target == null)
			{
				return false;
			}
			
			if (!target.AttackableEntity.Alive)
			{
				return false;
			}
			
			var targetMonster = target as Models.Entities.Monster;
			if (targetMonster != null)
			{
				if (targetMonster.IsGuard)
				{
					return false;
				}
			}
			
			var targetPlayer = target as Models.Entities.Player;
			var attackerPlayer = attacker as Models.Entities.Player;
			
			if (targetPlayer != null)
			{
				if (!targetPlayer.LoggedIn)
				{
					return false;
				}
				
				if (attackerPlayer != null)
				{
					var pkStatus = attackerPlayer.ValidPkTarget(targetPlayer);
					if (pkStatus != 0)
					{
						return false;
					}
				}
				
				if (DateTime.UtcNow < targetPlayer.LoginProtectionEndTime)
				{
					return false;
				}
				
				if (DateTime.UtcNow < targetPlayer.ReviveProtectionEndTime)
				{
					return false;
				}
			}
			
			return true;
		}
	}
}
