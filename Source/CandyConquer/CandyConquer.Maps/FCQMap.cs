// Project by Bauss
using System;
using System.IO;
using System.Collections.Generic;

namespace CandyConquer.Maps
{
	/// <summary>
	/// A FCQ Map (A stripped / compressed dmap.)
	/// Only contains information about walkable.
	/// </summary>
	public sealed class FCQMap
	{
		/// <summary>
		/// The tiles associated with the map.
		/// </summary>
		private bool[] _tiles;
		/// <summary>
		/// The width of the map.
		/// </summary>
		private ushort _width;
		
		/// <summary>
		/// Gets the id of the map.
		/// </summary>
		public ushort Id { get; private set; }
		
		/// <summary>
		/// Gets the specific status of a bit.
		/// </summary>
		/// <param name="b">The bit.</param>
		/// <param name="index">The index of the bit.</param>
		/// <returns>The status as a boolean. 1 = true, 0 = false.</returns>
		private static bool GetBit(byte b, int index)
		{
			return new System.Collections.BitArray(new byte[] { b }).Get(index);
		}
		
		/// <summary>
		/// Decompresses a buffer.
		/// </summary>
		/// <param name="buffer">The buffer to decompress.</param>
		/// <returns>The decompressed buffer.</returns>
		private static byte[] Decompress(byte[] buffer)
		{
			byte[] nBuffer = new byte[buffer.Length * 8];
			
			int counter = 0;
			for (int i = 0; i < buffer.Length; i++)
			{
				for (byte j = 0; j < 8; j++)
				{
					nBuffer[counter] = (byte)(GetBit(buffer[i], j) ? 1 : 0);
					counter++;
				}
			}
			
			byte[] cBuffer = new byte[counter];
			System.Buffer.BlockCopy(nBuffer, 0, cBuffer, 0, cBuffer.Length);
			return cBuffer;
		}
		
		/// <summary>
		/// Creates a new FCQMap.
		/// </summary>
		/// <param name="file">The file associated with the map.</param>
		internal FCQMap(string file)
		{
			string[] fsplit = file.Split('\\');
			Id = ushort.Parse(fsplit[fsplit.Length - 1].Split('.')[0]);
			
			using (var fs = new FileStream(file, FileMode.Open))
			{
				using (var br = new BinaryReader(fs))
				{
					_width = br.ReadUInt16();
					int count = br.ReadInt32();
					byte[] buffer = br.ReadBytes(count);
					buffer = Decompress(buffer);
					_tiles = new bool[buffer.Length];
					for (int i = 0; i < _tiles.Length; i++)
					{
						_tiles[i] = (buffer[i] == 1 ? true : false);
					}
				}
			}
		}
		
		/// <summary>
		/// Checks whether a specific coord is walkable.
		/// </summary>
		/// <param name="x">The x coord.</param>
		/// <param name="y">The y coord.</param>
		/// <returns>True if the coord is walkable.</returns>
		public bool Check(ushort x, ushort y)
		{
			var index = x * (int)_width + y;
			if (index > _tiles.Length - 1)
			{
				return false;
			}
			
			return _tiles[x * (int)_width + y];
		}
	}
}