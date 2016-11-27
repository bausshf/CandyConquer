// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Models.Arena
{
	/// <summary>
	/// Model for arena information.
	/// </summary>
	public sealed class ArenaInfo : Controllers.Arena.ArenaInfoController
	{
		/// <summary>
		/// Gets the database model associated with the arena info.
		/// </summary>
		public Database.Models.DbPlayerArenaQualifier DbPlayerArenaQualifier { get; private set; }
		
		/// <summary>
		/// The player associated with the arena info.
		/// </summary>
		private Models.Entities.Player _player;
		
		/// <summary>
		/// Gets or sets the player associated with the arena info.
		/// </summary>
		public Models.Entities.Player Player
		{
			get { return _player; }
			set
			{
				_player = value;
				
				if (_player != null)
				{
					if (DbPlayerArenaQualifier == null)
					{
						DbPlayerArenaQualifier = new Database.Models.DbPlayerArenaQualifier
						{
							PlayerId = _player.DbPlayer.Id,
							Name = _player.Name,
							Mesh = _player.Mesh,
							Level = _player.Level,
							Job = _player.Job.ToString(),
							Server = Drivers.Settings.WorldSettings.Server,
							
							HonorPoints = 100,
							ArenaPoints = 120
						};
						
						DbPlayerArenaQualifier.Create();
					}
					else
					{
						DbPlayerArenaQualifier.PlayerId = _player.DbPlayer.Id;
						DbPlayerArenaQualifier.Name = _player.Name;
						DbPlayerArenaQualifier.Mesh = _player.Mesh;
						DbPlayerArenaQualifier.Level = _player.Level;
						DbPlayerArenaQualifier.Job = _player.Job.ToString();
						
						DbPlayerArenaQualifier.Update();
					}
				}
			}
		}
		
		/// <summary>
		/// Gets the ratio.
		/// </summary>
		public uint Ratio
		{
			get
			{
				return (uint)Math.Max(0, (int)(DbPlayerArenaQualifier.TotalWins - DbPlayerArenaQualifier.TotalLoss));
			}
		}
		
		/// <summary>
		/// Gets the ratio for today.
		/// </summary>
		public uint RatioToday
		{
			get
			{
				return (uint)Math.Max(0, (int)(DbPlayerArenaQualifier.TotalWinsToday - DbPlayerArenaQualifier.TotalLossToday));
			}
		}
		
		/// <summary>
		/// Gets or sets the honor points.
		/// </summary>
		public int HonorPoints
		{
			get { return (int)DbPlayerArenaQualifier.HonorPoints; }
			set
			{
				DbPlayerArenaQualifier.HonorPoints = (uint)Math.Min(100000, Math.Max(0, value));
				
				DbPlayerArenaQualifier.Update();
			}
		}
		
		/// <summary>
		/// Gets or sets the ranking.
		/// </summary>
		public uint Ranking { get; set; }
		
		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		public Enums.ArenaStatus Status { get; set; }
		
		/// <summary>
		/// Creates a new arena info.
		/// </summary>
		/// <param name="dbPlayerArenaQualifier">The database model associated with the arena info.</param>
		public ArenaInfo(Database.Models.DbPlayerArenaQualifier dbPlayerArenaQualifier)
		{
			DbPlayerArenaQualifier = dbPlayerArenaQualifier;
			
			ArenaInfo = this;
		}
	}
}
