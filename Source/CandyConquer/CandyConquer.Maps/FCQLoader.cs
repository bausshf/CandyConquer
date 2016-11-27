// Project by Bauss
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace CandyConquer.Maps
{
	/// <summary>
	/// FCQMap loader.
	/// </summary>
	public static class FCQMapLoader
	{
		/// <summary>
		/// Loads and gets all maps based on a path.
		/// </summary>
		/// <param name="path">The path of the maps.</param>
		public static void GetMaps(string path, Action<FCQMap> action)
		{
			foreach (var dmap in Directory.GetFiles(path).AsParallel())//.Select(x => new FCQMap(x));
			{
				action.Invoke(new FCQMap(dmap));
			}
		}
	}
}
