private static void Heal(Player player, int amount, uint itemId)
{
	if (!player.Alive)
	{
		return;
	}
	
	if (player.ContainsStatusFlag(StatusFlag.NoPotion))
	{
		return;
	}
	
	if (player.HP >= player.MaxHP)
	{
		return;
	}
	
	if (player.Inventory.RemoveById(itemId))
	{
		player.HP += amount;
	}
}

/// <summary>
/// Stancher
/// </summary>
[ScriptEntry(Entry = 1000000)]
public static void Stancher(Player player)
{
	Heal(player, 70, 1000000);
}

/// <summary>
/// Resolutive
/// </summary>
[ScriptEntry(Entry = 1000010)]
public static void Resolutive(Player player)
{
	Heal(player, 100, 1000010);
}

/// <summary>
/// Painkiller
/// </summary>
[ScriptEntry(Entry = 1000020)]
public static void Painkiller(Player player)
{
	Heal(player, 250, 1000020);
}

/// <summary>
/// Amrita
/// </summary>
[ScriptEntry(Entry = 1000030)]
public static void Amrita(Player player)
{
	Heal(player, 500, 1000030);
}

/// <summary>
/// Ginseng
/// </summary>
[ScriptEntry(Entry = 1002010)]
public static void Ginseng(Player player)
{
	Heal(player, 1200, 1002010);
}

/// <summary>
/// Vanilla
/// </summary>
[ScriptEntry(Entry = 1002020)]
public static void Vanilla(Player player)
{
	Heal(player, 2000, 1002020);
}

/// <summary>
/// Mil.Ginseng
/// </summary>
[ScriptEntry(Entry = 1002050)]
public static void MilGinseng(Player player)
{
	Heal(player, 3000, 1002050);
}
