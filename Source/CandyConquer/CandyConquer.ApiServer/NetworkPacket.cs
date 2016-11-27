// Project by Bauss
using System;
using System.Linq;

namespace CandyConquer.ApiServer
{
	/// <summary>
	/// A network packet based on a 4 byte header.
	/// </summary>
	public unsafe class NetworkPacket
	{
		/// <summary>
		/// The packet buffer.
		/// </summary>
		private byte[] _buffer;
		/// <summary>
		/// Gets the packet buffer.
		/// </summary>
		protected byte[] Buffer { get { return _buffer; } }
		/// <summary>
		/// The packet offset.
		/// </summary>
		private int _offset;
		/// <summary>
		/// The sub type offset.
		/// </summary>
		private int _subTypeOffset = 4;
		
		/// <summary>
		/// Gets or sets the current offset of the buffer.
		/// </summary>
		public int Offset
		{
			get { return _offset; }
			set
			{
				if (value > _buffer.Length)
				{
					throw new IndexOutOfRangeException("Offset is out of range.");
				}
				_offset = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the packetstamp.
		/// </summary>
		public uint Packetstamp { get; set; }
		
		/// <summary>
		/// Gets or sets the subtype offset of the buffer.
		/// </summary>
		public int SubTypeOffset
		{
			get { return _subTypeOffset; }
			set
			{
				if (value > _buffer.Length)
				{
					throw new IndexOutOfRangeException("Offset is out of range.");
				}
				_subTypeOffset = value;
			}
		}
		
		/// <summary>
		/// Gets or sets an object that holds subtype information.
		/// </summary>
		public object SubTypeObject { get; set; }
		
		/// <summary>
		/// Creates a new network packet.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name="offset">The offset.</param>
		public NetworkPacket(byte[] buffer, int offset = 0)
		{
			_buffer = buffer;
			_offset = offset;
		}
		
		/// <summary>
		/// Creates a new network packet.
		/// </summary>
		/// <param name="size">A 16 bit integer for the size of the packet.</param>
		/// <param name="type">The packet type.</param>
		public NetworkPacket(int size, ushort type)
		{
			_buffer = new byte[size];
			_offset = 0;
			WriteUInt16((ushort)size);
			WriteUInt16(type);
		}
		
		/// <summary>
		/// Creates a new network packet, inheriting data from an existing one.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <param name="offset">The offset.</param>
		public NetworkPacket(NetworkPacket packet, int offset = 0)
			: this(packet._buffer, offset)
		{
			
		}
		
		/// <summary>
		/// Creates a new empty network packet.
		/// </summary>
		public NetworkPacket()
		{
			_buffer = new byte[0];
			_offset = 0;
		}
		
		/// <summary>
		/// Gets the physical size of the packet.
		/// </summary>
		public int PhysicalSize
		{
			get { return _buffer.Length; }
		}
		
		/// <summary>
		/// Gets the virtual size of the packet.
		/// Usually it should match the physical size, unless suffixed then physical size - 8.
		/// </summary>
		public ushort VirtualSize
		{
			get
			{
				fixed (byte* ptr = _buffer)
				{
					return (*(ushort*)(ptr + 0));
				}
			}
			private set
			{
				fixed (byte* ptr = _buffer)
				{
					(*(ushort*)(ptr + 0)) = value;
				}
			}
		}
		
		/// <summary>
		/// Gets the packet type.
		/// </summary>
		public ushort PacketType
		{
			get
			{
				fixed (byte* ptr = _buffer)
				{
					return (*(ushort*)(ptr + 2));
				}
			}
		}
		
		/// <summary>
		/// Gets the packet sub type.
		/// </summary>
		public ushort PacketSubType
		{
			get
			{
				fixed (byte* ptr = _buffer)
				{
					return (*(ushort*)(ptr + _subTypeOffset));
				}
			}
		}
		
		/// <summary>
		/// Writes a signed byte.
		/// </summary>
		/// <param name="value"></param>
		public void WriteSByte(sbyte value)
		{
			fixed (byte* ptr = _buffer)
			{
				(*(sbyte*)(ptr + _offset)) = value;
			}
			
			_offset++;
		}
		
		/// <summary>
		/// Writes a signed 16 bit integer.
		/// </summary>
		/// <param name="value">The value.</param>
		public void WriteInt16(short value)
		{
			fixed (byte* ptr = _buffer)
			{
				(*(short*)(ptr + _offset)) = value;
			}
			
			_offset += 2;
		}
		
		/// <summary>
		/// Writes a signed 32 bit integer.
		/// </summary>
		/// <param name="value">The value.</param>
		public void WriteInt32(int value)
		{
			fixed (byte* ptr = _buffer)
			{
				(*(int*)(ptr + _offset)) = value;
			}
			
			_offset += 4;
		}
		
		/// <summary>
		/// Writes a signed 64 bit integer.
		/// </summary>
		/// <param name="value">The value.</param>
		public void WriteInt64(long value)
		{
			fixed (byte* ptr = _buffer)
			{
				(*(long*)(ptr + _offset)) = value;
			}
			
			_offset += 8;
		}
		
		/// <summary>
		/// Writes an unsigned byte.
		/// </summary>
		/// <param name="value">The value.</param>
		public void WriteByte(byte value)
		{
			fixed (byte* ptr = _buffer)
			{
				(*(byte*)(ptr + _offset)) = value;
			}
			
			_offset++;
		}
		
		/// <summary>
		/// Writes a 16 bit unsigned integer.
		/// </summary>
		/// <param name="value">The value.</param>
		public void WriteUInt16(ushort value)
		{
			fixed (byte* ptr = _buffer)
			{
				(*(ushort*)(ptr + _offset)) = value;
			}
			
			_offset += 2;
		}
		
		/// <summary>
		/// Writes a 32 bit unsigned integer.
		/// </summary>
		/// <param name="value">The value.</param>
		public void WriteUInt32(uint value)
		{
			fixed (byte* ptr = _buffer)
			{
				(*(uint*)(ptr + _offset)) = value;
			}
			
			_offset += 4;
		}
		
		/// <summary>
		/// Writes a 64 bit unsigned integer.
		/// </summary>
		/// <param name="value">The value.</param>
		public void WriteUInt64(ulong value)
		{
			fixed (byte* ptr = _buffer)
			{
				(*(ulong*)(ptr + _offset)) = value;
			}
			
			_offset += 8;
		}
		
		/// <summary>
		/// Writes a boolean.
		/// </summary>
		/// <param name="value">The value.</param>
		public void WriteBool(bool value)
		{
			WriteByte((byte)(value ? 1 : 0));
		}
		
		/// <summary>
		/// Writes a string.
		/// </summary>
		/// <param name="value">The value.</param>
		public void WriteString(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			
			System.Buffer.BlockCopy(System.Text.Encoding.ASCII.GetBytes(value), 0, _buffer, _offset, value.Length);
			_offset += value.Length;
		}
		
		/// <summary>
		/// Writes a string with reminder bytes.
		/// </summary>
		/// <param name="value">The string.</param>
		/// <param name="forcedSize">The size of the string.</param>
		public void WriteStringWithReminder(string value, int forcedSize)
		{
			int reminder = forcedSize;
			if (!string.IsNullOrWhiteSpace(value))
			{
				WriteString(value);
				reminder -= value.Length;
			}
			
			for (int i = 0; i < reminder; i++)
			{
				WriteByte(0);
			}
		}
		
		/// <summary>
		/// Writes a string along with its length.
		/// </summary>
		/// <param name="value">The string.</param>
		public void WriteStringWithLength(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				WriteByte(0);
			}
			else
			{
				WriteByte((byte)value.Length);
				WriteString(value);
			}
		}
		
		/// <summary>
		/// Writes strings to the packet.
		/// </summary>
		/// <param name="strings">Atached strings.</param>
		public void WriteStrings(params string[] strings)
		{
			if (strings == null || strings.Length == 0)
			{
				return;
			}
			
			int size = strings.Length + 1;
			foreach (var s in strings)
			{
				if (!string.IsNullOrWhiteSpace(s))
				{
					size += s.Length;
				}
			}
			
			if (_offset >= _buffer.Length && size == 1)
			{
				Expand(1);
				return;
			}
			
			if (_offset + size >= _buffer.Length)
			{
				Expand(size);
			}
			
			WriteByte((byte)strings.Length);
			foreach (var s in strings)
			{
				WriteStringWithLength(s);
			}
		}
		
		/// <summary>
		/// Reads a signed byte.
		/// </summary>
		/// <returns>The value.</returns>
		public sbyte ReadSByte()
		{
			fixed (byte* ptr = _buffer)
			{
				var value = (*(sbyte*)(ptr + _offset));
				_offset++;
				return value;
			}
		}
		
		/// <summary>
		/// Reads a signed 16 bit integer.
		/// </summary>
		/// <returns>The value.</returns>
		public short ReadInt16()
		{
			fixed (byte* ptr = _buffer)
			{
				var value = (*(short*)(ptr + _offset));
				_offset += 2;
				return value;
			}
		}
		
		/// <summary>
		/// Reads a signed 32 bit integer.
		/// </summary>
		/// <returns>The value.</returns>
		public int ReadInt32()
		{
			fixed (byte* ptr = _buffer)
			{
				var value = (*(int*)(ptr + _offset));
				_offset += 4;
				return value;
			}
		}
		
		/// <summary>
		/// Reads a 64 bit signed integer.
		/// </summary>
		/// <returns>The value.</returns>
		public long ReadInt64()
		{
			fixed (byte* ptr = _buffer)
			{
				var value = (*(long*)(ptr + _offset));
				_offset += 8;
				return value;
			}
		}
		
		/// <summary>
		/// Reads an unsigned byte.
		/// </summary>
		/// <returns>The value.</returns>
		public byte ReadByte()
		{
			fixed (byte* ptr = _buffer)
			{
				var value = (*(byte*)(ptr + _offset));
				_offset++;
				return value;
			}
		}
		
		/// <summary>
		/// Reads an unsigned 16 bit integer.
		/// </summary>
		/// <returns>The value.</returns>
		public ushort ReadUInt16()
		{
			fixed (byte* ptr = _buffer)
			{
				var value = (*(ushort*)(ptr + _offset));
				_offset += 2;
				return value;
			}
		}
		
		/// <summary>
		/// Reads an unsigned 32 bit integer.
		/// </summary>
		/// <returns>The value.</returns>
		public uint ReadUInt32()
		{
			fixed (byte* ptr = _buffer)
			{
				var value = (*(uint*)(ptr + _offset));
				_offset += 4;
				return value;
			}
		}
		
		/// <summary>
		/// Reads an unsigned 64 bit integer.
		/// </summary>
		/// <returns>The value.</returns>
		public ulong ReadUInt64()
		{
			fixed (byte* ptr = _buffer)
			{
				var value = (*(ulong*)(ptr + _offset));
				_offset += 8;
				return value;
			}
		}
		
		/// <summary>
		/// Reads a boolean.
		/// </summary>
		/// <returns>The value.</returns>
		public bool ReadBool()
		{
			return ReadByte() > 0;
		}
		
		/// <summary>
		/// Reads a string by a specific size.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns>The string.</returns>
		public string ReadString(int size)
		{
			if (size == 0)
			{
				return string.Empty;
			}
			
			return System.Text.Encoding.ASCII.GetString(ReadBytes(size))
				.Replace("\0", "");
		}
		
		/// <summary>
		/// Reads a string based on a dynamic length.
		/// </summary>
		/// <returns>The string.</returns>
		public string ReadString()
		{
			return ReadString((int)ReadByte());
		}
		
		/// <summary>
		/// Reads a sequence of bytes.
		/// </summary>
		/// <param name="amount">The amount of bytes to read.</param>
		/// <returns>The sequence of bytes</returns>
		public byte[] ReadBytes(int amount)
		{
			var buffer = new byte[amount];
			System.Buffer.BlockCopy(_buffer, _offset, buffer, 0, amount);
			_offset += amount;
			return buffer;
		}
		
		/// <summary>
		/// Reads attached strings.
		/// </summary>
		/// <returns>Attached strings.</returns>
		public string[] ReadStrings()
		{
			var strings = new string[(int)ReadByte()];
			for (int i = 0; i < strings.Length; i++)
			{
				strings[i] = ReadString();
			}
			return strings;
		}
		
		/// <summary>
		/// Expands the packet buffer.
		/// </summary>
		/// <param name="amount"></param>
		public void Expand(int amount)
		{
			Array.Resize(ref _buffer, _buffer.Length + amount);
			VirtualSize = (ushort)_buffer.Length;
		}
		
		/// <summary>
		/// Converts the packet to a readable string.
		/// </summary>
		/// <returns>The readable string.</returns>
		public override string ToString()
		{
			var dump = "{ " + BitConverter.ToString(_buffer).Replace("-", " ") + " }";
			var builder = new System.Text.StringBuilder();
			builder.AppendLine(dump).Append("{ ")
				.Append(_buffer.Select(b =>
				                       {
				                       	if (b == 32 || b >= 48 && b <= 57 || b >= 65 && b <= 90 ||
				                       	    b >= 97 && b <= 122)
				                       	{
				                       		return (char)b;
				                       	}
				                       	else
				                       	{
				                       		return '.';
				                       	}
				                       }).ToArray());
			builder.Append(" }");
			return builder.ToString();
		}

		
		/// <summary>
		/// Implicit conversion from network packet to byte array.
		/// </summary>
		/// <param name="packet">The network packet.</param>
		/// <returns>The byte array.</returns>
		public static implicit operator byte[](NetworkPacket packet)
		{
			return packet._buffer;
		}
		
		/// <summary>
		/// Implicit conversion from byte array to network packet.
		/// </summary>
		/// <param name="buffer">The byte array.</param>
		/// <returns>Returns a new network packet based on the byte array.</returns>
		public static implicit operator NetworkPacket(byte[] buffer)
		{
			return new NetworkPacket(buffer);
		}
	}
}
