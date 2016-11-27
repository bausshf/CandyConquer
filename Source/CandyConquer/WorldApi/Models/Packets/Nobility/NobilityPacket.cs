// Project by Bauss
using System;
using CandyConquer.ApiServer;

namespace CandyConquer.WorldApi.Models.Packets.Nobility
{
	/// <summary>
	/// Model for the nobility packet.
	/// </summary>
	public sealed class NobilityPacket : NetworkPacket
	{
		/// <summary>
		/// Gets or sets the action.
		/// </summary>
		public Enums.NobilityAction Action
		{
			get
			{
				Offset = 4;
				return (Enums.NobilityAction)ReadUInt32();
			}
			set
			{
				Offset = 4;
				WriteUInt32((uint)value);
			}
		}
		
		/// <summary>
		/// Gets or sets the data 1 low a.
		/// </summary>
		public ushort Data1LowA
		{
			get
			{
				Offset = 8;
				return ReadUInt16();
			}
			set
			{
				Offset = 8;
				WriteUInt16(value);
			}
		}
		
		/// <summary>
		/// Gets or sets the data 1 high a.
		/// </summary>
		public long Data1HighA
		{
			get
			{
				Offset = 8;
				return ReadInt64();
			}
			set
			{
				Offset = 8;
				WriteInt64(value);
			}
		}

		/// <summary>
		/// Gets or sets the data 2.
		/// </summary>
		public ushort Data2
		{
			get
			{
				Offset = 10;
				return ReadUInt16();
			}
			set
			{
				Offset = 10;
				WriteUInt16(value);
			}
		}
		
		/// <summary>
		/// Gets or sets the data 3.
		/// </summary>
		public uint Data3
		{
			get
			{
				Offset = 16;
				return ReadUInt32();
			}
			set
			{
				Offset = 16;
				WriteUInt32(value);
			}
		}
		
		/// <summary>
		/// Gets or sets the data 4.
		/// </summary>
		public uint Data4
		{
			get
			{
				Offset = 20;
				return ReadUInt32();
			}
			set
			{
				Offset = 20;
				WriteUInt32(value);
			}
		}
		
		/// <summary>
		/// Gets or sets the data 5.
		/// </summary>
		public uint Data5
		{
			get
			{
				Offset = 24;
				return ReadUInt32();
			}
			set
			{
				Offset = 24;
				WriteUInt32(value);
			}
		}
		
		#region Data1
		/// <summary>
		/// The data.
		/// </summary>
		private long _data;
		
		/// <summary>
		/// Gets or sets the data 1.
		/// </summary>
		public long Data1
		{
			get { return _data; }
			set { _data = value; }
		}

		/// <summary>
		/// Gets or sets the data 1 low.
		/// </summary>
		public uint Data1Low
		{
			get { return (uint)_data; }
			set { _data = (Data1High << 32) | value; }
		}

		/// <summary>
		/// Gets or sets the data 1 high.
		/// </summary>
		public uint Data1High
		{
			get { return (uint)(_data >> 32); }
			set { _data = (((long)value << 32) | Data1Low); }
		}

		/// <summary>
		/// Gets or sets the data1 low low.
		/// </summary>
		public ushort Data1LowLow
		{
			get { return (ushort)Data1Low; }
			set { Data1Low = (uint)((Data1LowHigh << 16) | value); }
		}

		/// <summary>
		/// Gets or sets the data 1 low high.
		/// </summary>
		public ushort Data1LowHigh
		{
			get { return (ushort)(Data1Low >> 16); }
			set { Data1Low = (uint)((value << 16) | Data1LowLow); }
		}

		/// <summary>
		/// Gets or sets the data 1 high low.
		/// </summary>
		public ushort Data1HighLow
		{
			get { return (ushort)Data1High; }
			set { Data1High = (uint)((Data1HighHigh << 16) | value); }
		}

		/// <summary>
		/// Gets or sets the data 1 high high.
		/// </summary>
		public ushort Data1HighHigh
		{
			get { return (ushort)(Data1High >> 16); }
			set { Data1High = (uint)((value << 16) | Data1HighLow); }
		}
		#endregion
		
		/// <summary>
		/// Creates a new nobiliy packet.
		/// </summary>
		/// <param name="size">The additional size of the packet.</param>
		public NobilityPacket(int size)
			: base(32 + size, Data.Constants.PacketTypes.NobilityPacket)
		{
		}
		
		/// <summary>
		/// Creates a new nobility packet (size 80)
		/// </summary>
		public NobilityPacket()
			: base(80, Data.Constants.PacketTypes.NobilityPacket)
		{
			
		}
		
		/// <summary>
		/// Creates a new nobility packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		private NobilityPacket(NetworkPacket packet)
			: base(packet, 4)
		{
		}
		
		/// <summary>
		/// Writes the data.
		/// </summary>
		public void WriteData()
		{
			Offset = 8;
			WriteInt64(_data);
		}
		
		/// <summary>
		/// Writes a donation to the packet.
		/// </summary>
		/// <param name="donation">The donation.</param>
		public void WriteDonation(Models.Nobility.NobilityDonation donation)
		{
			WriteUInt32(donation.ClientId);
			WriteUInt32(0); // unknown
			WriteUInt32(donation.Player != null ? donation.Player.Mesh : (uint)0);
			WriteStringWithReminder(donation.Name, 16);
			WriteUInt32(0); // unknown ... padding ??
			WriteInt64(donation.Donation);
			WriteUInt32((uint)donation.Rank);
			WriteInt32(donation.Ranking);
		}
		
		/// <summary>
		/// Implicit conversion from socket packet to NobilityPacket packet.
		/// </summary>
		/// <param name="packet">The socket packet.</param>
		public static implicit operator NobilityPacket(SocketPacket packet)
		{
			return new NobilityPacket(packet);
		}
		
		/// <summary>
		/// Implicit conversion from the NobilityPacket to byte array.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>The buffer.</returns>
		public static implicit operator byte[](NobilityPacket packet)
		{
			return packet.Buffer;
		}
	}
}
