/// <summary>
/// TwinCityGate
/// </summary>
[ScriptEntry(Entry = 1060020)]
public static void TwinCityGate(Player player)
{
	player.Teleport(1002);
	player.Inventory.RemoveById(1060020);
}

/// <summary>
/// PhoenixCastleGate
/// </summary>
[ScriptEntry(Entry = 1060023)]
public static void PhoenixCastleGate(Player player)
{
	player.Teleport(1011);
	player.Inventory.RemoveById(1060023);
}

/// <summary>
/// ApeCityGate
/// </summary>
[ScriptEntry(Entry = 1060022)]
public static void ApeCityGate(Player player)
{
	player.Teleport(1020);
	player.Inventory.RemoveById(1060022);
}

/// <summary>
/// DesertCityGate
/// </summary>
[ScriptEntry(Entry = 1060021)]
public static void DesertCityGate(Player player)
{
	player.Teleport(1000);
	player.Inventory.RemoveById(1060021);
}

/// <summary>
/// BirdIslandGate
/// </summary>
[ScriptEntry(Entry = 1060024)]
public static void BirdIslandGate(Player player)
{
	player.Teleport(1015);
	player.Inventory.RemoveById(1060024);
}
