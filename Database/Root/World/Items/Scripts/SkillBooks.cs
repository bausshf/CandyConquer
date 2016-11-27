/// <summary>
/// Thunder
/// </summary>
[ScriptEntry(Entry = 725000)]
public static void ThunderSkillBook(Player player)
{
	if (player.Spirit < 20)
	{
		return;
	}
	
	var skill = player.Spells.GetOrCreateSkill(1000);
	if (skill != null)
	{
		player.Inventory.RemoveById(725000);
		skill.SendToClient();
	}
}