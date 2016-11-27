// Project by Bauss
using System;
using System.Threading.Tasks;

namespace CandyConquer.WorldApi.Controllers.Entities
{
	/// <summary>
	/// Controller for monsters.
	/// </summary>
	public class MonsterController : AttackableEntityController
	{
		/// <summary>
		/// Gets the monster associated with the controller.
		/// </summary>
		public Models.Entities.Monster Monster { get; protected set; }
		
		/// <summary>
		/// Constructor for the controller.
		/// </summary>
		public MonsterController()
			: base()
		{
		}
		
		/// <summary>
		/// Handler for when the monster has been killed.
		/// </summary>
		/// <param name="attacker">The attacker.</param>
		/// <param name="damage">The damage.</param>
		protected override void OnKill(AttackableEntityController attacker, uint damage)
		{
			var attackerPlayer = attacker as Models.Entities.Player;
			
			Monster.Map.Drop(Monster.X, Monster.Y, Monster.Id, attackerPlayer);
			if (attackerPlayer != null)
			{
				if (damage < 3)
				{
					damage = 3;
				}
				
				if (attackerPlayer.Level > Monster.Level)
				{
					damage = 3;
				}
				
				if (Monster.Level > (attackerPlayer.Level - 10))
				{
					attackerPlayer.AddExperience((ulong)(((damage * 2) / 3) + Monster.ExtraExperience));
				}
				
				if (attackerPlayer.Battle != null)
				{
					attackerPlayer.Battle.KillMonster(attackerPlayer, Monster);
				}
			}
			
			SetStatusFlag(Enums.StatusFlag.Ghost);
			Task.Run(async() => await ReviveAsync());
		}
		
		/// <summary>
		/// Revives the monster asynchronous with a delay.
		/// </summary>
		private async Task ReviveAsync()
		{
			await Task.Delay(Data.Constants.Time.MonsterReviveTime);
			
			try
			{
				Revive(false);
			}
			catch (Exception e)
			{
				Drivers.Global.RaiseException(e);
			}
		}
		
		/// <summary>
		/// Handler for when the monster revives.
		/// </summary>
		/// <param name="reviveHere">Unused for monsters.</param>
		protected override void OnRevive(bool reviveHere)
		{
			SetStatusFlag(Enums.StatusFlag.None);
			
			ushort x;
			ushort y;
			
			if (Monster.IsGuard)
			{
				x = Monster.OriginalX;
				y = Monster.OriginalY;
			}
			else
			{
				var location = Monster.Map.GetValidMonsterCoordinate(Monster.OriginalX, Monster.OriginalY, Monster.OriginalRangeSize);
				if (!location.Valid)
				{
					// No valid spots at the moment, wait for another revival ...
					Task.Run(async() => await ReviveAsync());
					return;
				}
				
				x = location.X;
				y = location.Y;
			}
			
			Monster.HP = Monster.MaxHP;
			Monster.MP = Monster.MaxMP;
			Monster.Alive = true;
			Monster.Direction = (Enums.Direction)Drivers.Repositories.Safe.Random.NextEnum(typeof(Enums.Direction));
			
			if (Monster.MapId != Monster.OriginalMapId)
			{
				Monster.Teleport(Monster.OriginalMapId, x, y);
			}
			else
			{
				Monster.X = x;
				Monster.Y = y;
				
				UpdateScreen(true);
			}
			
			Monster.Idle = !Monster.IsGuard && FindClosest<Models.Entities.Player>() == null;
		}
	}
}
