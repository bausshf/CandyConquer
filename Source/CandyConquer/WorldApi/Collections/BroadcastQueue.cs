// Project by Bauss
using System;
using System.Collections.Concurrent;
using CandyConquer.WorldApi.Models.Packets.Message;

namespace CandyConquer.WorldApi.Collections
{
	/// <summary>
	/// The broadcast queue collection.
	/// </summary>
	public static class BroadcastQueue
	{
		/// <summary>
		/// The queue of broadcasts.
		/// </summary>
		private static ConcurrentQueue<MessagePacket> _messageQueue;
		
		/// <summary>
		/// The last broadcast message.
		/// </summary>
		private static MessagePacket _lastMessage;
		
		/// <summary>
		/// Static constructor for BroadcastQueue.
		/// </summary>
		static BroadcastQueue()
		{
			_messageQueue = new ConcurrentQueue<MessagePacket>();
		}
		
		/// <summary>
		/// Gets the last broadcast message.
		/// </summary>
		public static MessagePacket LastMessage
		{
			get { return _lastMessage; }
		}
		
		/// <summary>
		/// Attempts to get a broadcast.
		/// </summary>
		/// <param name="message">The broadcast.</param>
		/// <returns>True if the broadcast was retrieved.</returns>
		public static bool TryGetBroadcast(out MessagePacket message)
		{
			MessagePacket broadcast;
			var success = _messageQueue.TryDequeue(out broadcast);
			
			if (success)
			{
				_lastMessage = broadcast;
				message = _lastMessage;
			}
			else
			{
				message = null;
			}
			
			return success;
		}
		
		/// <summary>
		/// Enqueues a broadcast to the queue.
		/// </summary>
		/// <param name="message">The broadcast message.</param>
		public static void Enqueue(MessagePacket message)
		{
			_messageQueue.Enqueue(message);
		}
	}
}
