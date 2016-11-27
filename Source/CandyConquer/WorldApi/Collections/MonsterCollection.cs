// Project by Bauss
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// The collection of monsters.
	/// </summary>
	public static class MonsterCollection
	{
		/// <summary>
		/// The monster collection.
		/// </summary>
		private static ConcurrentDictionary<int, Database.Models.DbMonster> _monsters;
		
		/// <summary>
		/// Static constructor for MonsterCollection.
		/// </summary>
		static MonsterCollection()
		{
			_monsters = new ConcurrentDictionary<int, Database.Models.DbMonster>();
		}
		
		/// <summary>
		/// Loads all the monsters.
		/// </summary>
		public static void Load()
		{
			var monsters = Database.Dal.Monsters.GetAll();
			foreach (var dbMonster in monsters)
			{
				_monsters.TryAdd(dbMonster.MobId, dbMonster);
			}
		}
		
		/// <summary>
		/// Creates a new list of monsters.
		/// </summary>
		/// <param name="mobId">The monster id.</param>
		/// <param name="amount">The amount of create.</param>
		/// <returns>The list of monsters.</returns>
		public static List<Models.Entities.Monster> Create(int mobId, int amount)
		{
			var monsters = new List<Models.Entities.Monster>();
			
			var monster = _monsters.Values
				.Where(m => m.MobId == mobId)
				.FirstOrDefault();
			
			if (monster != null)
			{
				for (int i = 0; i < amount; i++)
				{
					monsters.Add(new Models.Entities.Monster(monster));
				}
			}
			
			return monsters;
		}
		
		/// <summary>
		/// Loads all monster spawns.
		/// </summary>
		public static void LoadSpawns()
		{
			var spawns = Database.Dal.Monsters.GetAllSpawns(Drivers.Settings.WorldSettings.Server);
			
			foreach (var spawn in spawns)
			{
				var monsters = Create(spawn.MonsterId, spawn.Amount);
				var map = MapCollection.GetMap(spawn.MapId);
				
				foreach (var monster in monsters)
				{
					var location = map.GetValidMonsterCoordinate(spawn.X, spawn.Y, spawn.RangeSize);
					if (location.Valid)
					{
						monster.OriginalMapId = map.Id;
						monster.OriginalX = spawn.X;
						monster.OriginalY = spawn.Y;
						monster.OriginalRangeSize = spawn.RangeSize;
						
						monster.Teleport(map.Id, location.X, location.Y);
					}
				}
			}
		}
		
		/// <summary>
		/// Loads all guards.
		/// </summary>
		public static void LoadGuards()
		{
			var guards = Database.Dal.Monsters.GetAllGuards();
			
			foreach (var dbGuard in guards)
			{
				var guard = Create(dbGuard.GuardId, 1).FirstOrDefault();
				if (guard != null)
				{
					var map = MapCollection.GetMap(dbGuard.MapId);
					
					guard.OriginalMapId = map.Id;
					guard.OriginalX = dbGuard.X;
					guard.OriginalY = dbGuard.Y;
					guard.OriginalRangeSize = 0;
					guard.IsWalking = dbGuard.CanMove;
					
					guard.Teleport(map.Id, dbGuard.X, dbGuard.Y);
				}
			}
		}
		
		/// <summary>
		/// Creates a new monster spawn.
		/// </summary>
		/// <param name="mobId">The monster id.</param>
		/// <param name="amount">The amount to create.</param>
		/// <param name="mapId">The map id.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="rangeSize">The range size.</param>
		public static void CreateSpawn(int mobId, int amount, int mapId, ushort x, ushort y, ushort rangeSize)
		{
			var spawn = new Database.Models.DbMonsterSpawn
			{
				MapId = mapId,
				MonsterId = mobId,
				Amount = amount,
				RangeSize = rangeSize,
				X = x,
				Y = y,
				Server = Drivers.Settings.WorldSettings.Server
			};
			
			if (spawn.Create())
			{
				var monsters = Create(spawn.MonsterId, spawn.Amount);
				var map = MapCollection.GetMap(spawn.MapId);
				
				foreach (var monster in monsters)
				{
					var location = map.GetValidMonsterCoordinate(spawn.X, spawn.Y, spawn.RangeSize);
					if (location.Valid)
					{
						monster.OriginalMapId = map.Id;
						monster.OriginalX = spawn.X;
						monster.OriginalY = spawn.Y;
						monster.OriginalRangeSize = spawn.RangeSize;
						
						monster.Teleport(map.Id, location.X, location.Y);
					}
				}
			}
		}
	}
}
