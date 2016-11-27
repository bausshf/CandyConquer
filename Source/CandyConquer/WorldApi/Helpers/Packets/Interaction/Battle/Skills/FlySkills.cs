// Project by Bauss
using System;
using CandyConquer.WorldApi.Controllers.Entities;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction.Battle.Skills
{
	/// <summary>
	/// Handler for fly skills.
	/// </summary>
	public static class FlySkills
	{
		/// <summary>
		/// Handles the fly skills.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <returns>True if the skill was handled correctly.</returns>
		public static bool Handle(AttackableEntityController attacker)
		{
			var player = attacker as Models.Entities.Player;
			if (player == null)
			{
				return false;
			}
			
			var bow = player.Equipments.Get(Enums.ItemPosition.WeaponR);
			if (bow == null || !bow.IsBow)
			{
				return false;
			}
			
			player.AddStatusFlag(Enums.StatusFlag.Fly, 40000);
			return true;
		}
	}
}
