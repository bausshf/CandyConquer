// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Tools
{
	/// <summary>
	/// Tools for job management.
	/// </summary>
	public static class JobTools
	{
		/// <summary>
		/// Gets the base job based on a job.
		/// </summary>
		/// <param name="job">The job.</param>
		/// <returns>The base job associated with it.</returns>
		public static Enums.Job GetBaseJob(Enums.Job job)
		{
			if (((int)job) >= 100)
			{
				return Enums.Job.InternTaoist;
			}
			
			var baseJob = ((int)job / 10) * 10;
			
			return (Enums.Job)baseJob;
		}
	}
}
