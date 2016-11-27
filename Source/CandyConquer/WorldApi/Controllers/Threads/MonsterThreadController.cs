// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace CandyConquer.WorldApi.Controllers.Threads
{
	/// <summary>
	/// The monster thread controller.
	/// </summary>
	public static class MonsterThreadController
	{
		/// <summary>
		/// The collection of monsters to handle.
		/// </summary>
		private static ConcurrentDictionary<uint, Models.Entities.Monster> _monsters;
		
		/// <summary>
		/// Static constructor for MonsterThreadController.
		/// </summary>
		static MonsterThreadController()
		{
			_monsters = new ConcurrentDictionary<uint, CandyConquer.WorldApi.Models.Entities.Monster>();
		}
		
		/// <summary>
		/// Handles the monster thread controller.
		/// </summary>
		public static void Handle()
		{
			foreach (var monster in _monsters.Values)
			{
				if (!monster.Idle)
				{
					if (monster.Map != null)
					{
						if (monster.IsGuard)
						{
							HandleGuard(monster);
						}
						else
						{
							HandleAttack(monster);
						}
						
						if (monster.IsWalking)
						{
							HandleMovement(monster);
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Handles a monster by the thread.
		/// </summary>
		/// <param name="monster">The monster to handle.</param>
		public static void HandleMonster(Models.Entities.Monster monster)
		{
			_monsters.TryAdd(monster.ClientId, monster);
		}
		
		/// <summary>
		/// Handles movement by a monster.
		/// </summary>
		/// <param name="monster">The monster to handle.</param>
		private static void HandleMovement(Models.Entities.Monster monster)
		{
			if (!monster.Alive)
			{
				return;
			}
			
			var target = monster.Target as Models.Maps.IMapObject;
			
			if (monster.IsGuard || monster.Behaviour == Enums.MonsterBehaviour.Peaceful || target != null)
			{
				if (DateTime.UtcNow >= monster.NextMoveTime)
				{
					byte dir = 0;
					int nextMoveSpeed = 0;
					
					if (monster.IsGuard || monster.Behaviour == Enums.MonsterBehaviour.Peaceful && target == null || Tools.RangeTools.GetDistance(monster.X, monster.Y, target.X, target.Y) >= monster.ViewRange)
					{
						dir = (byte)Drivers.Repositories.Safe.Random.Next(8);
						nextMoveSpeed = Drivers.Repositories.Safe.Random.Next(monster.MoveSpeed * 4, monster.MoveSpeed * 6);
					}
					else
					{
						dir = (byte)Tools.RangeTools.GetFacing(
							Tools.RangeTools.GetAngle(monster.X, monster.Y, target.X, target.Y));
						nextMoveSpeed = Drivers.Repositories.Safe.Random.Next(monster.MoveSpeed, monster.MoveSpeed * 2);
					}
					
					monster.NextMoveTime = DateTime.UtcNow.AddMilliseconds(nextMoveSpeed);
					var location = Controllers.Maps.MapController.CreateDirectionPoint(monster.X, monster.Y, dir);
					
					if (monster.ValidMoveCoord<Models.Entities.Monster>(location.X, location.Y))
					{
						monster.Direction = (Enums.Direction)dir;
						monster.X = location.X;
						monster.Y = location.Y;
						
						monster.UpdateScreen(false, new Models.Packets.Location.WalkPacket
						                     {
						                     	Direction = monster.Direction,
						                     	ClientId = monster.ClientId,
						                     	Mode = Enums.WalkMode.Run,
						                     	Timestamp = Drivers.Time.GetSystemTime()
						                     });
					}
				}
			}
		}
		
		/// <summary>
		/// Handles attack for a monster.
		/// </summary>
		/// <param name="monster">The monster to handle attack for.</param>
		private static void HandleAttack(Models.Entities.Monster monster)
		{
			if (!monster.Alive)
			{
				return;
			}
			
			#region Poison
			if (monster.ContainsStatusFlag(Enums.StatusFlag.Poisoned))
			{
				if (DateTime.UtcNow >= monster.NextPoison)
				{
					monster.NextPoison = DateTime.UtcNow.AddMilliseconds(3000);
					
					if (monster.PoisonEffect > 0)
					{
						uint damage = (uint)Math.Max(1, (monster.HP / 100) * monster.PoisonEffect);
						
						if (monster.HP > damage)
						{
							Helpers.Packets.Interaction.Battle.Damage.Hit(null, monster, damage);
							
							var poisonPacket = new Models.Packets.Entities.InteractionPacket
							{
								Action = Enums.InteractionAction.Attack,
								ClientId = monster.ClientId,
								TargetClientId = monster.ClientId,
								X = monster.X,
								Y = monster.Y,
								Data = damage
							};
							
							monster.UpdateScreen(false, poisonPacket);
						}
						else
						{
							monster.PoisonEffect = 0;
							monster.RemoveStatusFlag(Enums.StatusFlag.Poisoned);
						}
					}
					else
					{
						monster.RemoveStatusFlag(Enums.StatusFlag.Poisoned);
					}
				}
			}
			#endregion
			
			var target = monster.Target as Models.Maps.IMapObject;
			
			if (target != null && target.Alive)
			{
				if (Tools.RangeTools.GetDistance(monster.X, monster.Y, target.X, target.Y) <= monster.AttackRange &&
				    monster.ContainsInScreen(target.ClientId) && DateTime.UtcNow >= monster.NextAttackTime)
				{
					monster.NextAttackTime = DateTime.UtcNow.AddMilliseconds(
						Drivers.Repositories.Safe.Random.Next(monster.AttackSpeed, monster.AttackSpeed * 3));
					
					Helpers.Packets.Interaction.Battle.Physical.Handle(monster, new Models.Packets.Entities.InteractionPacket
					                                                   {
					                                                   	Action = Enums.InteractionAction.Attack,
					                                                   	ClientId = monster.ClientId,
					                                                   	TargetClientId = target.ClientId,
					                                                   	X = target.X,
					                                                   	Y = target.Y
					                                                   });
					return;
				}
			}
			
			var possibleTarget = monster.FindClosest<Models.Entities.Player>();
			if (possibleTarget != null &&
			    possibleTarget.Alive &&
			    !possibleTarget.ContainsStatusFlag(Enums.StatusFlag.PartiallyInvisible))
			{
				if (monster.Behaviour == Enums.MonsterBehaviour.Peaceful &&
				    (possibleTarget.Target == null || possibleTarget.Target.ClientId != monster.ClientId))
				{
					monster.Target = null;
				}
				else
				{
					monster.Target = possibleTarget;
				}
			}
			else
			{
				monster.Target = null;
			}
			
			monster.Idle = possibleTarget == null;
		}
		
		/// <summary>
		/// Handles guards.
		/// </summary>
		/// <param name="guard">The guard to handle.</param>
		private static void HandleGuard(Models.Entities.Monster guard)
		{
			switch (guard.Behaviour)
			{
					#region Magic Guard
				case Enums.MonsterBehaviour.MagicGuard:
					{
						var target = guard.FindInScreen<Controllers.Entities.AttackableEntityController>(
							entity =>
							{
								if (entity != null)
								{
									var player = entity as Models.Entities.Player;
									if (player != null)
									{
										return player.ContainsStatusFlag(Enums.StatusFlag.BlueName) &&
											player.Alive &&
											Tools.RangeTools.GetDistance(player.X, player.Y, guard.X, guard.Y) <= guard.AttackRange;
									}
									
									var monster = entity as Models.Entities.Monster;
									if (monster != null)
									{
										return monster.Alive &&
											!monster.IsGuard &&
											Tools.RangeTools.GetDistance(monster.X, monster.Y, guard.X, guard.Y) <= guard.AttackRange;
									}
								}
								
								return false;
							}
						);
						
						if (target != null)
						{
							Helpers.Packets.Interaction.Battle.Magic
								.Handle(guard,
								        new Models.Packets.Entities.InteractionPacket
								        {
								        	Action = Enums.InteractionAction.MagicAttack,
								        	MagicType = 1002,
								        	ClientId = guard.ClientId,
								        	TargetClientId = target.AttackableEntity.ClientId,
								        	X = target.MapObject.X,
								        	Y = target.MapObject.Y
								        });
						}
						break;
					}
					#endregion
					
					#region Physical Guard
				case Enums.MonsterBehaviour.PhysicalGuard:
					{
						var target = guard.FindInScreen<Models.Entities.Player>(
							player =>
							{
								if (player != null)
								{
									return (player.ContainsStatusFlag(Enums.StatusFlag.BlueName) ||
									        player.PKPoints >= 100) &&
										player.Alive &&
										Tools.RangeTools.GetDistance(player.X, player.Y, guard.X, guard.Y) <= guard.ViewRange;
								}
								
								return false;
							}
						);
						
						if (Tools.RangeTools.GetDistanceU(guard.X, guard.Y, guard.OriginalX, guard.OriginalY) >= (ulong)(guard.ViewRange * 2))
						{
							if (target != null)
							{
								target.ClientSocket.Send(guard.GetRemoveSpawnPacket());
							}
							
							guard.Teleport(guard.MapId, guard.OriginalX, guard.OriginalY);
							
							target = null;
						}
						
						if (target != null)
						{
							if (Tools.RangeTools.GetDistanceU(guard.X, guard.Y, target.X, target.Y) > 1)
							{
								var nearest = target.NearBy();
								guard.X = nearest.X;
								guard.Y = nearest.Y;
								
								guard.UpdateScreen(false, new Models.Packets.Client.DataExchangePacket
								                   {
								                   	ExchangeType = Enums.ExchangeType.Jump,
								                   	Data1Low = nearest.X,
								                   	Data1High = nearest.Y,
								                   	ClientId = guard.ClientId,
								                   	Timestamp = Drivers.Time.GetSystemTime()
								                   });
							}
							
							if (Tools.CalculationTools.ChanceSuccess(33))
							{
								Helpers.Packets.Interaction.Battle.Physical
									.Handle(guard,
									        new Models.Packets.Entities.InteractionPacket
									        {
									        	Action = Enums.InteractionAction.Attack,
									        	ClientId = guard.ClientId,
									        	TargetClientId = target.ClientId,
									        	X = target.X,
									        	Y = target.Y
									        });
							}
						}
						break;
					}
					#endregion
					
					#region Death Guard
				case Enums.MonsterBehaviour.DeathGuard:
					{
						var target = guard.FindInScreen<Controllers.Entities.AttackableEntityController>(
							entity =>
							{
								if (entity != null)
								{
									var player = entity as Models.Entities.Player;
									if (player != null)
									{
										return (player.ContainsStatusFlag(Enums.StatusFlag.BlueName) ||
										        player.PKPoints >= 100) &&
											player.Alive &&
											Tools.RangeTools.GetDistance(player.X, player.Y, guard.X, guard.Y) <= guard.ViewRange;
									}
									
									var monster = entity as Models.Entities.Monster;
									if (monster != null)
									{
										return monster.Alive &&
											!monster.IsGuard &&
											Tools.RangeTools.GetDistance(monster.X, monster.Y, guard.X, guard.Y) <= guard.AttackRange;
									}
								}
								
								return false;
							}
						);
						
						if (target != null)
						{
							Helpers.Packets.Interaction.Battle.Magic
								.Handle(guard,
								        new Models.Packets.Entities.InteractionPacket
								        {
								        	Action = Enums.InteractionAction.MagicAttack,
								        	MagicType = 1002,
								        	ClientId = guard.ClientId,
								        	TargetClientId = target.AttackableEntity.ClientId,
								        	X = target.MapObject.X,
								        	Y = target.MapObject.Y
								        });
						}
						break;
					}
					#endregion
					
					#region Revive Guard
				case Enums.MonsterBehaviour.ReviverGuard1:
				case Enums.MonsterBehaviour.ReviverGuard2:
					{
						if (guard.HP < guard.MaxHP)
						{
							Helpers.Packets.Interaction.Battle.Magic
								.Handle(guard,
								        new Models.Packets.Entities.InteractionPacket
								        {
								        	Action = Enums.InteractionAction.MagicAttack,
								        	MagicType = 1190,
								        	ClientId = guard.ClientId,
								        	TargetClientId = guard.ClientId,
								        	X = guard.X,
								        	Y = guard.Y
								        });
						}
						
						var target = guard.FindInScreen<Models.Entities.Player>(
							player =>
							{
								if (player != null)
								{
									return !player.Alive &&
										Tools.RangeTools.GetDistance(player.X, player.Y, guard.X, guard.Y) <= guard.ViewRange;
								}
								
								return false;
							}
						);
						
						if (target != null)
						{
							Helpers.Packets.Interaction.Battle.Magic
								.Handle(guard,
								        new Models.Packets.Entities.InteractionPacket
								        {
								        	Action = Enums.InteractionAction.MagicAttack,
								        	MagicType = 1100,
								        	ClientId = guard.ClientId,
								        	TargetClientId = target.ClientId,
								        	X = target.X,
								        	Y = target.Y
								        });
							
							if (guard.Behaviour == Enums.MonsterBehaviour.ReviverGuard1)
							{
								target.Teleport(target.MapId);
							}
						}
						break;
					}
					#endregion
			}
		}
	}
}
