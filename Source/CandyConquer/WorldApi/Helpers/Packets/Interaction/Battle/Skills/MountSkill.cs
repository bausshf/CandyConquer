// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Skills
{
	/// <summary>
	/// Handler for the mount skill.
	/// </summary>
	public static class MountSkill
	{
		/// <summary>
		/// Handles the mount skill.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="spellPacket">The spell packet.</param>
		/// <returns>True if the skill was handled correctly.</returns>
		public static bool Handle(Models.Entities.Player player, Models.Packets.Spells.SpellPacket spellPacket)
		{
			if (player.Equipments.Get(Enums.ItemPosition.Steed, false) == null)
			{
				return false;
			}
			
			if (player.ContainsStatusFlag(Enums.StatusFlag.Riding))
			{
				player.RemoveStatusFlag(Enums.StatusFlag.Riding);
			}
			else if (player.Stamina < 100)
			{
				return false;
			}
			else
			{
				player.AddStatusFlag(Enums.StatusFlag.Riding);
			}
			
			player.ClientSocket.Send(new Models.Packets.Entities.SteedVigorPacket
			                         {
			                         	Type = 2,
			                         	Amount = 9001
			                         });
			
			TargetFinalization.SkillFinalize(player, null, spellPacket, 0);
			
			return true;
		}
	}
}
