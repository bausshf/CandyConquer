// Project by Bauss
using System;
using CandyConquer.WorldApi.Models.Entities;

namespace CandyConquer.WorldApi.Controllers.Entities
{
	/// <summary>
	/// Controller for attackable entities.
	/// </summary>
	public abstract class AttackableEntityController : EntityController
	{
		/// <summary>
		/// Gets the attackable entity associated with the controller.
		/// </summary>
		public Models.Entities.IAttackableEntity AttackableEntity { get; protected set; }
		
		/// <summary>
		/// Constructor for the controller.
		/// </summary>
		public AttackableEntityController()
			: base()
		{
			
		}
		
		/// <summary>
		/// Validates an attack.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <param name="target">The target.</param>
		/// <returns>The status of the validation. (0 = success)</returns>
		public int ValidateAttack(Models.Packets.Entities.InteractionPacket packet, out AttackableEntityController target)
		{
			target = null;
			
			if (packet == null)
			{
				return 1;
			}
			
			Models.Maps.IMapObject targetMapObject;
			if (!GetFromScreen(packet.TargetClientId, out targetMapObject))
			{
				return 2;
			}
			
			target = targetMapObject as AttackableEntityController;
			
			if (target == null)
			{
				return 3;
			}
			
			if (AttackableEntity.ClientId != target.AttackableEntity.ClientId)
			{
				var player = target as Player;
				if (player != null)
				{
					if (!player.LoggedIn)
					{
						return 4;
					}
					
					if (DateTime.UtcNow < player.LoginProtectionEndTime)
					{
						return 5;
					}
					
					if (DateTime.UtcNow < player.ReviveProtectionEndTime)
					{
						return 6;
					}
					
					// Can't attack flying players if not ranged
					if (packet.Action != Enums.InteractionAction.Shoot &&
					    target.ContainsStatusFlag(Enums.StatusFlag.Fly))
					{
						return 7;
					}
					
					var attackerPlayer = AttackableEntity as Player;
					if (attackerPlayer != null)
					{
						var pkStatus = attackerPlayer.ValidPkTarget(player);
						if (pkStatus != 0)
						{
							return 8;
						}
					}
				}
			}
			
			if (!Tools.RangeTools.ValidDistance(MapObject.X, MapObject.Y, target.MapObject.X, target.MapObject.Y))
			{
				return 9;
			}
			
			if (!AttackableEntity.Alive)
			{
				return 10;
			}
			
			if (!target.AttackableEntity.Alive)
			{
				return 11;
			}
			
			return 0;
		}
		
		/// <summary>
		/// Kills the entity.
		/// </summary>
		/// <param name="attacker">The attacker (null if self or nobody.)</param>
		/// <param name="damage">The damage.</param>
		public void Kill(AttackableEntityController attacker, uint damage)
		{
			AttackableEntity.Alive = false;
			AttackableEntity.HP = 0;
			
			OnKill(attacker, damage);
			
			UpdateScreen(false, new Models.Packets.Entities.InteractionPacket
			             {
			             	Action = Enums.InteractionAction.Kill,
			             	TargetClientId = AttackableEntity.ClientId,
			             	X = MapObject.X,
			             	Y = MapObject.Y,
			             	Data = 1,
			             	ClientId = attacker != null ?
			             		attacker.AttackableEntity.ClientId : AttackableEntity.ClientId
			             });
		}
		
		/// <summary>
		/// Handler for when the entity has been killed.
		/// </summary>
		/// <param name="attacker">The attacker if any (null for self or none)</param>
		/// <param name="damage">The damage.</param>
		protected abstract void OnKill(AttackableEntityController attacker, uint damage);
		
		/// <summary>
		/// Revives the entity.
		/// </summary>
		/// <param name="reviveHere">Boolean determining whether the revive is on the entity's spot or not.</param>
		public void Revive(bool reviveHere)
		{
			OnRevive(reviveHere);
		}
		
		/// <summary>
		/// Handler for when the entity revives.
		/// </summary>
		/// <param name="reviveHere">Boolean determining whether the revive is on the entity's spot or not.</param>
		protected abstract void OnRevive(bool reviveHere);
	}
}
