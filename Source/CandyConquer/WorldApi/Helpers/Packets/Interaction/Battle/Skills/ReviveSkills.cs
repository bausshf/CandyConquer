// Project by Bauss
using System;
using CandyConquer.WorldApi.Controllers.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Skills
{
	/// <summary>
	/// Handler for revive skills.
	/// </summary>
	public static class ReviveSkills
	{
		/// <summary>
		/// Handling the revive skills.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="target">The target.</param>
		/// <param name="packet">The packet.</param>
		/// <param name="spellPacket">The spell packet.</param>
		/// <returns>True if the skill was handled correctly.</returns>
		public static bool Handle(AttackableEntityController attacker, AttackableEntityController target,
		                          Models.Packets.Entities.InteractionPacket packet,
		                          Models.Packets.Spells.SpellPacket spellPacket)
		{
			if (target == null)
			{
				return false;
			}
			
			var targetPlayer = target as Models.Entities.Player;
			
			if (targetPlayer == null)
			{
				return false;
			}
			
			if (targetPlayer.Alive)
			{
				return false;
			}
			
			if (targetPlayer.ClientId == attacker.AttackableEntity.ClientId)
			{
				return false;
			}
			
			targetPlayer.Revive(true);
			
			TargetFinalization.SkillFinalize(attacker, target, spellPacket, 0);
			
			return true;
		}
	}
}
