// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Data.Constants
{
	/// <summary>
	/// Packet type constants.
	/// </summary>
	public static class PacketTypes
	{
		public const ushort
			CharacterCreationPacket = 1001, // done
			MessagePacket = 1004, // done
			CharacterInitPacket = 1006, // done
			ItemInfoPacket = 1008, // done
			ItemActionPacket = 1009, // done (equip, unequip, use, buy, sell, repair, repair-all, improve, uplevel, bless)
			StringPacket = 1015, // done (Except for handling some sub types that may or may not be implemented)
			WeatherPacket = 1016, // done
			InteractionPacket = 1022, // done
			TeamActionPacket = 1023, // done
			SendProfPacket = 1025, // done
			TeamMemberPacket = 1026, // done
			GemSocketingPacket = 1027,
			DateTimeVigorPacket = 1033, // done
			CharacterStatsPacket = 1040, // done
			WorldAuthenticationPacket = 1052, // done
			TradePacket = 1056, // done
			GuildDonationPacket = 1058, // done
			GroundItemPacket = 1101, // done
			WarehousePacket = 1102, // done
			SendSpellPacket = 1103, // done
			UseSpellPacket = 1105, // done
			GuildAttributePacket = 1106, // done
			GuildPacket = 1107, // done
			ViewItemPacket = 1108, // done
			SobSpawnPacket = 1109,
			MapInfoPacket = 1110, // done
			NpcSpawnPacket = 2030, // done
			NpcRequestPacket = 2031, // done
			NpcResponsePacket = 2032, // done
			CompositionPacket = 2036, // done
			BroadcastPacket = 2050, // done
			NobilityPacket = 2064, // done
			GuildMemberListPacket = 2102, // done
			ArenaActionPacket = 2205, // done
			ArenaTopPlayersPacket = 2208, // done
			ArenaBattleInfoPacket = 2206, // done
			ArenaStatisticPacket = 2209, // done
			ArenaMatchPacket = 2210, // done
			ArenaWatchPacket = 2211, // done
			SubClassPacket = 2320,
			WalkPacket = 10005, // done
			DataExchangePacket = 10010, // done
			SpawnPacket = 10014; // done
	}
}
