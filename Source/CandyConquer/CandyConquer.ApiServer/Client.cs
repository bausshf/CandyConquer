// Project by Bauss
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Collections.Generic;
using CandyConquer.Drivers;
using CandyConquer.Security.Cryptography;
using CandyConquer.Debugging;
using CandyConquer.Drivers.Repositories.Safe;
using CandyConquer.Security.Api;

namespace CandyConquer.ApiServer
{
	/// <summary>
	/// A client socket.
	/// </summary>
	public class Client<T>
	{
		/// <summary>
		/// Single calls.
		/// </summary>
		private HashSet<ushort> _singleCalls;
		/// <summary>
		/// Gets the associated socket.
		/// </summary>
		public Socket Socket { get; private set; }
		/// <summary>
		/// Gets a value indicating whether the client has been disconnected or not.
		/// This value is only an indication if the disconnection has been triggered internally.
		/// </summary>
		public bool Disconnected { get; private set; }
		/// <summary>
		/// Gets the reason for the disconnection.
		/// </summary>
		public string DisconnectReason { get; private set; }
		/// <summary>
		/// Gets the IP address of the client.
		/// </summary>
		public string IPAddress
		{
			get { return (Socket.RemoteEndPoint as IPEndPoint).Address.ToString(); }
		}
		/// <summary>
		/// Gets or sets the cryptography for the client.
		/// </summary>
		public ICryptography Cryptography { get; set; }
		/// <summary>
		/// The disconnection event.
		/// </summary>
		public Action<T> OnDisconnect { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether the disconnection should be handled or not.
		/// </summary>
		internal bool ShouldHandleDisconnect { get; set; }
		/// <summary>
		/// The compiled method for controller calls.
		/// </summary>
		internal MethodInfo ControllerCall { get; set; }
		/// <summary>
		/// Gets or sets the data associated with this client.
		/// </summary>
		public T Data { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether the packet type is excluded from the packet or not.
		/// </summary>
		public bool KeyExchange { get; set; }
		/// <summary>
		/// Gets or sets the suffix.
		/// </summary>
		public byte[] Suffix { get; set; }
		/// <summary>
		/// The idle state of the client.
		/// </summary>
		public CallSecurity IdleState { get; set; }
		/// <summary>
		/// Gets the packet trace for the client.
		/// </summary>
		public Drivers.Repositories.Collections.DiscardableList<SocketPacket> PacketTrace { get; private set; }
		/// <summary>
		/// Gets or sets the last exception.
		/// </summary>
		public Exception LastException { get; set; }
		
		#region Internal Buffers
		/// <summary>
		/// The header buffer.
		/// </summary>
		private byte[] _headBuffer;
		/// <summary>
		/// The expected amount of bytes to receive in the header.
		/// </summary>
		private int _headExpected;
		/// <summary>
		/// The received amount of bytes for the header.
		/// </summary>
		private int _headReceived;
		/// <summary>
		/// The packet identifier.
		/// </summary>
		private ushort _packetIdentifier;
		/// <summary>
		/// The body buffer.
		/// </summary>
		private byte[] _bodyBuffer;
		/// <summary>
		/// The expected amount of bytes to receive in the body.
		/// </summary>
		private int _bodyExpected;
		/// <summary>
		/// The received amount of bytes in the body.
		/// </summary>
		private int _bodyReceived;
		/// <summary>
		/// The final buffer (A combination of both the header and body.)
		/// </summary>
		private byte[] _finalBuffer;
		#endregion
		
		/// <summary>
		/// Creates a new api client.
		/// </summary>
		/// <param name="socket">The associated socket.</param>
		public Client(Socket socket)
		{
			Socket = socket;
			Disconnected = false;
			
			_headReceived = 0;
			KeyExchange = false;
			
			IdleState = CallSecurity.NonIdle;
			_singleCalls = new HashSet<ushort>();
			PacketTrace = new CandyConquer.Drivers.Repositories.Collections.DiscardableList<SocketPacket>(10);
		}
		
		/// <summary>
		/// Adds a single call if it hasn't been called.
		/// </summary>
		/// <param name="call">The call.</param>
		/// <returns>True if the call has been done once before, false otherwise.</returns>
		public bool AddOrHasSingleCall(ushort call)
		{
			if (!_singleCalls.Contains(call))
			{
				_singleCalls.Add(call);
				return false;
			}
			
			return true;
		}
		
		/// <summary>
		/// Begins to receive from the client.
		/// </summary>
		public void BeginReceive()
		{
			_finalBuffer = new byte[KeyExchange ? NetworkInfo.KeyBufferSize : NetworkInfo.BufferHeadSize];
			
			if (KeyExchange)
			{
				ReceiveBody(NetworkInfo.KeyBufferSize);
			}
			else
			{
				ReceiveHead();
			}
		}
		
		/// <summary>
		/// Receives the head.
		/// </summary>
		/// <param name="size">The size to receive.</param>
		/// <param name="reset">A value indicating if the receive should be reset.</param>
		internal void ReceiveHead(int size = NetworkInfo.BufferHeadSize, bool reset = true)
		{
			if (Socket.Connected)
			{
				_headBuffer = new byte[size];
				_headReceived = reset ? 0 : _headReceived;
				_headExpected = size;
				_packetIdentifier = 0;
				
				Socket.BeginReceive(_headBuffer, 0, size, SocketFlags.None, Handle_ReceiveHead, null);
			}
		}
		
		/// <summary>
		/// Asynchronous handler for the head receive.
		/// </summary>
		/// <param name="asyncResult">The asynchronous result.</param>
		private void Handle_ReceiveHead(IAsyncResult asyncResult)
		{
			try
			{
				SocketError socketError;
				int received = Socket.EndReceive(asyncResult, out socketError);
				
				if (!Socket.Connected)
				{
					Disconnect(Messages.Errors.CLIENT_DISCONNECTED);
					return;
				}
				
				if (socketError != SocketError.Success)
				{
					Disconnect(Messages.Errors.SOCKET_ERROR_RECEIVE_HEAD);
					return;
				}
				
				if (received == 0)
				{
					Disconnect(Messages.Errors.SOCKET_DISCONNECTED);
					return;
				}
				
				lock (Drivers.Locks.NetworkLock)
				{
					if (Cryptography != null)
					{
						_headBuffer = Cryptography.Decrypt(_headBuffer);
					}
				}
				
				System.Buffer.BlockCopy(_headBuffer, 0, _finalBuffer, _headReceived, received);
				_headReceived += received;
				
				if (received < _headExpected)
				{
					ReceiveHead(_headExpected - received, false);
				}
				else
				{
					//int packetSize = (int)new NetworkPacket(_headBuffer).ReadUInt16();
					int packetSize;
					unsafe
					{
						fixed (byte* ptr = _headBuffer)
						{
							packetSize = (int)(*(ushort*)(ptr + 0));
							_packetIdentifier = (*(ushort*)(ptr + 2));
						}
					}
					
					if (packetSize > NetworkInfo.BufferBodyMaxSize)
					{
						Disconnect(Messages.Errors.SOCKET_RECEIVE_TOO_BIG);
						return;
					}
					
					if (Suffix != null)
					{
						packetSize += Suffix.Length;
					}
					
					Array.Resize(ref _finalBuffer, packetSize);
					ReceiveBody(packetSize - _headReceived);
				}
			}
			catch (Exception e)
			{
				LastException = e;
				
				#if DEBUG
				#if TRACE
				ErrorLogger.Log(StackTracing.GetCurrentMethod().Name, e);
				#else
				ErrorLogger.Log(Messages.Errors.FATAL_NETWORK_ERROR_TITLE, e);
				#endif
				#else
				Global.Message(e.ToString());
				#endif
				
				Disconnect(Messages.Errors.FATAL_NETWORK_ERROR_TITLE);
			}
		}
		
		/// <summary>
		/// Receives the body.
		/// </summary>
		/// <param name="size">The size to receive.</param>
		/// <param name="reset">A value indicating whether the receive should be reset.</param>
		private void ReceiveBody(int size, bool reset = true)
		{
			if (Socket.Connected)
			{
				_bodyBuffer = new byte[size];
				_bodyReceived = reset ? 0 : _bodyReceived;
				_bodyExpected = size;
				
				Socket.BeginReceive(_bodyBuffer, 0, size, SocketFlags.None, Handle_ReceiveBody, null);
			}
		}
		
		/// <summary>
		/// Asynchronous handler for the body receive.
		/// </summary>
		/// <param name="asyncResult">The asynchronous result.</param>
		private void Handle_ReceiveBody(IAsyncResult asyncResult)
		{
			try
			{
				SocketError socketError;
				int received = Socket.EndReceive(asyncResult, out socketError);
				
				if (!Socket.Connected)
				{
					Disconnect(Messages.Errors.CLIENT_DISCONNECTED);
					return;
				}
				
				if (socketError != SocketError.Success)
				{
					Disconnect(Messages.Errors.SOCKET_ERROR_RECEIVE_BODY);
					return;
				}
				
				if (received == 0)
				{
					Disconnect(Messages.Errors.SOCKET_DISCONNECTED);
					return;
				}
				
				lock (Locks.NetworkLock)
				{
					if (Cryptography != null)
					{
						_bodyBuffer = Cryptography.Decrypt(_bodyBuffer);
					}
				}
				
				System.Buffer.BlockCopy(_bodyBuffer, 0, _finalBuffer, _headReceived + _bodyReceived, received);
				_bodyReceived += received;

				if (!KeyExchange && received < _bodyExpected)
				{
					ReceiveBody(_bodyExpected - received, false);
				}
				else
				{
					int copyLen = _finalBuffer.Length;
					if (Suffix != null)
					{
						copyLen -= Suffix.Length;
					}
					byte[] buffer = new Byte[copyLen];
					System.Buffer.BlockCopy(_finalBuffer, 0, buffer, 0, buffer.Length);
					
					bool result = false;
					
					if (KeyExchange)
					{
						result = (bool)ControllerCall.Invoke(null,
						                                     new object[]
						                                     {
						                                     	this,
						                                     	IdleState,
						                                     	(ushort)0,
						                                     	new SocketPacket(buffer, 0)
						                                     });
					}
					else
					{
						var socketPacket = new SocketPacket(buffer, 4);
						PacketTrace.Add(socketPacket);
						
						result = (bool)ControllerCall.Invoke(null,
						                                     new object[]
						                                     {
						                                     	this,
						                                     	IdleState,
						                                     	_packetIdentifier,
						                                     	socketPacket
						                                     });
					}
					
					if (result)
					{
						BeginReceive();
					}
					else
					{
						Disconnect("Failed to handle packet. Look packet trace.");
					}
				}
			}
			catch (Exception e)
			{
				LastException = e;
				
				#if DEBUG
				#if TRACE
				ErrorLogger.Log(StackTracing.GetCurrentMethod().Name, e);
				#else
				ErrorLogger.Log(Messages.Errors.FATAL_NETWORK_ERROR_TITLE, e);
				#endif
				#else
				Global.Message(e.ToString());
				#endif
				
				Disconnect(Messages.Errors.FATAL_NETWORK_ERROR_TITLE);
			}
		}
		
		/// <summary>
		/// Disconnects the client.
		/// </summary>
		/// <param name="reason">The reason for the disconnection.</param>
		public void Disconnect(string reason)
		{
			lock (Drivers.Locks.NetworkLock)
			{
				if (Disconnected)
				{
					return;
				}
				
				Disconnected = true;
				DisconnectReason = reason;
				
				ThrowSafe.Execute(() => Socket.Shutdown(SocketShutdown.Both));
				ThrowSafe.Execute(() => Socket.Disconnect(false));
				
				if (ShouldHandleDisconnect && OnDisconnect != null && Data != null)
				{
					OnDisconnect.Invoke(Data);
				}
			}
		}
		
		/// <summary>
		/// Sends a packet to the client.
		/// </summary>
		/// <param name="packet">The packet to send.</param>
		public void Send(byte[] packet)
		{
			if (Socket == null || packet == null)
			{
				return;
			}
			
			try
			{
				lock (Locks.NetworkLock)
				{
					byte[] pbuffer = packet;
					byte[] buffer = packet;
					
					if (Suffix != null)
					{
						Array.Resize(ref buffer, buffer.Length + Suffix.Length);
						System.Buffer.BlockCopy(Suffix, 0, buffer, buffer.Length - Suffix.Length, Suffix.Length);
						pbuffer = buffer;
					}
					
					if (Cryptography != null)
					{
						buffer = new byte[pbuffer.Length];
						System.Buffer.BlockCopy(pbuffer, 0, buffer, 0, buffer.Length);
						buffer = Cryptography.Encrypt(buffer);
					}
					
					if (Socket.Connected)
					{
						Socket.Send(buffer);
					}
				}
			}
			catch (Exception e)
			{
				#if DEBUG
				#if TRACE
				ErrorLogger.Log(StackTracing.GetCurrentMethod().Name, e);
				#else
				ErrorLogger.Log(Messages.Errors.FATAL_NETWORK_ERROR_TITLE, e);
				#endif
				#else
				Global.Message(e.ToString());
				#endif
				
				Disconnect(Messages.Errors.FATAL_NETWORK_ERROR_TITLE);
			}
		}
		
		/// <summary>
		/// Logs the packet.
		/// </summary>
		/// <param name="packet">The packet to log.</param>
		/// <param name="logBody">If set to true the body of the packet is logged too.</param>
		/// <param name="logSub">If set to true the sub type of the packet is logged too.</param>
		/// <param name="subObject">The object associated with the sub type.</param>
		public void LogPacket(NetworkPacket packet, bool logBody, bool logSub = false, object subObject = null)
		{
			#if LOCAL
			PacketLogger.Log(packet, logBody, logSub, subObject);
			#endif
		}
		
		/// <summary>
		/// Implicit conversion from Socket to Client.
		/// </summary>
		/// <param name="socket">The socket.</param>
		/// <returns>A client socket inheriting the socket.</returns>
		public static implicit operator Client<T>(Socket socket)
		{
			return new Client<T>(socket);
		}
	}
}
