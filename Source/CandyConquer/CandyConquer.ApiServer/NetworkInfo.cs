// Project by Bauss
using System;

namespace CandyConquer.ApiServer
{
	/// <summary>
	/// Network information.
	/// </summary>
	public static class NetworkInfo
	{
		/// <summary>
		/// The buffer head size for receiving. (Anything below 8 is not valid.)
		/// </summary>
		public const int BufferHeadSize = 8;
		
		/// <summary>
		/// The buffer size for key exchanging.
		/// </summary>
		public const int KeyBufferSize = 1024;
		
		/// <summary>
		/// The maximum virtual size of the packet body.
		/// </summary>
		public const int BufferBodyMaxSize = 2048;
	}
}
