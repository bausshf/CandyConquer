private static void HealMagic(Player player, int amount, uint itemId)
{
	if (!player.Alive)
	{
		return;
	}
	
	if (player.ContainsStatusFlag(StatusFlag.NoPotion))
	{
		return;
	}
	
	if (player.MP >= player.MaxMP)
	{
		return;
	}
	
	if (player.Inventory.RemoveById(itemId))
	{
		player.MP += amount;
	}
}

/// <summary>
/// Agrypnotic
/// </summary>
[ScriptEntry(Entry = 1001000)]
public static void Agrypnotic(Player player)
{
	HealMagic(player, 70, 1001000);
}

/// <summary>
/// Tonic
/// </summary>
[ScriptEntry(Entry = 1001010)]
public static void Tonic(Player player)
{
	HealMagic(player, 200, 1001010);
}

/// <summary>
/// PearlOintment
/// </summary>
[ScriptEntry(Entry = 1001020)]
public static void PearlOintment(Player player)
{
	HealMagic(player, 450, 1001020);
}

/// <summary>
/// RecoveryPill
/// </summary>
[ScriptEntry(Entry = 1001030)]
public static void RecoveryPill(Player player)
{
	HealMagic(player, 1000, 1001030);
}

/// <summary>
/// SoulPill
/// </summary>
[ScriptEntry(Entry = 1001040)]
public static void SoulPill(Player player)
{
	HealMagic(player, 2000, 1001040);
}

/// <summary>
/// RefreshingPill
/// </summary>
[ScriptEntry(Entry = 1002030)]
public static void RefreshingPill(Player player)
{
	HealMagic(player, 3000, 1002030);
}

/// <summary>
/// ChantPill
/// </summary>
[ScriptEntry(Entry = 1002040)]
public static void ChantPill(Player player)
{
	HealMagic(player, 4500, 1002040);
}
