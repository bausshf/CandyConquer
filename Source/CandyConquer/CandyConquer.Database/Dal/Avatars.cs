// Project by Bauss
using System;
using System.Collections.Generic;

namespace CandyConquer.Database.Dal
{
	/// <summary>
	/// Dal for avatars.
	/// </summary>
	public static class Avatars
	{
		/// <summary>
		/// The male avatars.
		/// </summary>
		private static ushort[] _maleAvatars;
		
		/// <summary>
		/// The female avatars.
		/// </summary>
		private static ushort[] _femaleAvatars;
		
		/// <summary>
		/// Static constructor for Avatars.
		/// </summary>
		static Avatars()
		{
			#region Male Avatars
			var avatars = new List<ushort>();
			
			for (ushort i = 1; i <= 107; i++)
				avatars.Add(i);
			for (ushort i = 109; i <= 113; i++)
				avatars.Add(i);
			for (ushort i = 129; i <= 153; i++)
				avatars.Add(i);
			
			_maleAvatars = avatars.ToArray();
			avatars.Clear();
			#endregion
			
			#region Female Avatars
			for (ushort i = 201; i <= 295; i++)
				avatars.Add(i);
			for (ushort i = 300; i <= 304; i++)
				avatars.Add(i);
			for (ushort i = 320; i <= 344; i++)
				avatars.Add(i);
			avatars.Add(2511);
			
			_femaleAvatars = avatars.ToArray();
			#endregion
		}
		
		/// <summary>
		/// Gets a random avatar.
		/// </summary>
		/// <param name="male">A boolean indicating whether the avatar should be a male or female.</param>
		/// <returns>The random selected avatar.</returns>
		public static ushort GetRandomAvatar(bool male)
		{
			lock (Drivers.Locks.GlobalLock)
			{
				if (male)
				{
					return _maleAvatars[Drivers.Repositories.Safe.Random.Next(_maleAvatars.Length)];
				}
				else
				{
					return _femaleAvatars[Drivers.Repositories.Safe.Random.Next(_femaleAvatars.Length)];
				}
			}
		}
	}
}
