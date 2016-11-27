// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// Collection of stats.
	/// </summary>
	public static class StatCollection
	{
		/// <summary>
		/// The stats collection.
		/// </summary>
		private static ConcurrentDictionary<byte, ushort[][]> _stats;
		
		/// <summary>
		/// Static constructor for StatsSettings.
		/// </summary>
		static StatCollection()
		{
			_stats = new ConcurrentDictionary<byte, ushort[][]>();
		}
		
		/// <summary>
		/// Loads all stats.
		/// </summary>
		public static void Load()
		{	
			var jobs = new System.Collections.Generic.Dictionary<byte,string>()
			{ 
				{ 10, "Trojan" },
				{ 20, "Warrior" },
				{ 40, "Archer" },
				{ 50, "Ninja" },
				{ 60, "Monk" },
				{ 100, "Taoist" }
			};
			foreach (var job in jobs)
			{
				var settings = new Drivers.Repositories.IO.IniFile(Drivers.Settings.DatabaseSettings.WorldFlatDatabase + string.Format("\\Stats\\{0}Stats.ini", job.Value));
				if (settings.Exists())
				{
					settings.Open();
					
					var stats = new ushort[121][];
					
					for (int i = 1; i <= 120; i++)
					{
						stats[i] = settings.GlobalSection.GetValue(i.ToString()).Split(',')
							.Select(x => ushort.Parse(x)).ToArray();
					}
					
					_stats.TryAdd(job.Key, stats);
				}
			}
		}
		
		/// <summary>
		/// Gets stats based on a job and level.
		/// </summary>
		/// <param name="job">The job.</param>
		/// <param name="level">The level.</param>
		/// <returns>The stats associated with the job and level.</returns>
		public static ushort[] Get(Enums.Job job, byte level)
		{
			var baseJob = Tools.JobTools.GetBaseJob(job);

			return Get((byte)baseJob, level);
		}
		
		/// <summary>
		/// Gets stats based on a job and level.
		/// </summary>
		/// <param name="job">The job.</param>
		/// <param name="level">The level.</param>
		/// <returns>An array of stats.</returns>
		private static ushort[] Get(byte job, byte level)
		{
			ushort[][] stats;
			if (_stats.TryGetValue(job, out stats))
			{
				ushort[] data = new ushort[4];
				System.Buffer.BlockCopy(stats[level], 0, data, 0, sizeof(ushort) * 4);
				return data;
			}
			return null;
		}
	}
}
