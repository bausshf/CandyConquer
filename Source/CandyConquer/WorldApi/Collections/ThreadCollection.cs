// Project by Bauss
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// A thread collection.
	/// </summary>
	public class ThreadCollection
	{
		/// <summary>
		/// A thread handler.
		/// </summary>
		private sealed class ThreadHandler
		{
			/// <summary>
			/// The bag of actions tied to the thread.
			/// </summary>
			public ConcurrentBag<Action> Actions { get; private set; }
			/// <summary>
			/// The thread.
			/// </summary>
			public Thread Thread { get; private set; }
			/// <summary>
			/// The interval of the thread.
			/// </summary>
			public int Interval { get; private set; }
			
			/// <summary>
			/// Creates a new thread handler.
			/// </summary>
			/// <param name="interval">The interval.</param>
			public ThreadHandler(int interval)
			{
				Actions = new ConcurrentBag<Action>();
				Thread = new Thread(Handle);
				Interval = interval;
				
				Thread.Start();
			}
			
			/// <summary>
			/// Handles the actions of the thread.
			/// </summary>
			private void Handle()
			{
				while (true)
				{
					foreach (var action in Actions)
					{
						action();
					}
					
					Thread.Sleep(Interval);
				}
			}
		}
		
		/// <summary>
		/// A collection of thread handlers.
		/// </summary>
		private static ConcurrentDictionary<int, ThreadHandler> _threadCollection;
		
		/// <summary>
		/// Static constructor for ThreadCollection.
		/// </summary>
		static ThreadCollection()
		{
			_threadCollection = new ConcurrentDictionary<int, ThreadHandler>();
		}
		
		/// <summary>
		/// Runs a specific action on the best fitting thread.
		/// If no threads are fitting the specifications, then a new thread is created.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="interval">The interval.</param>
		public static void Run(Action action, int interval)
		{
			ThreadHandler thread;
			if (!_threadCollection.TryGetValue(interval, out thread))
			{
				thread = new ThreadHandler(interval);
				_threadCollection.TryAdd(interval, thread);
			}
			
			thread.Actions.Add(action);
		}
	}
}
