/// <summary>
/// The price of making a guild.
/// </summary>
private const int GuildCreationCost = 1000000;

/// <summary>
/// The minimum level required to create a guild.
/// </summary>
private const byte GuildCreationLevel = 90;

/// <summary>
/// GuildDirector
/// </summary>
[ScriptEntry(Entry = 1001)]
public static void GuildDirector(Player player, byte option)
{
	switch (option)
	{
		case 0:
			{
				Dialog.SendDialog(player, GuildDirectorMessage(player, "WELCOME_MSG"));
				if (player.Guild == null)
				{
					Dialog.SendOption(player, GuildDirectorMessage(player, "ABOUT_GUILDS"), 1);
					Dialog.SendOption(player, GuildDirectorMessage(player, "MANAGE_NEW_GUILD"), 2);
				}
				else if (player.GuildMember.Rank == GuildRank.GuildLeader || player.GuildMember.Rank == GuildRank.DeputyLeader)
				{
					Dialog.SendOption(player, GuildDirectorMessage(player, "MANAGE_GUILD"), 2);
				}
				Dialog.SendOption(player,  Message(player, "NEVERMIND"), 255);
				Dialog.Finish(player);
				break;
			}
			
			case 1: GuildDirector_AboutGuilds(player); break;
		case 2:
			{
				if (player.Guild != null)
				{
					if (player.GuildMember.Rank == GuildRank.GuildLeader)
					{
						GuildDirector_ManageGuild_Leader_Page1(player);
					}
					else if (player.GuildMember.Rank == GuildRank.DeputyLeader)
					{
						GuildDirector_ManageGuild_Deputy(player);
					}
				}
				else
				{
					GuildDirector_ManageNewGuild(player);
				}
				break;
			}
			case 30: GuildDirector_ManageGuild_Leader_Page2(player); break;
			
			case 3: GuildDirector_CreateGuild(player); break;
			case 4: GuildDirector_DisbanGuild(player); break;
			case 5: GuildDirector_Kick(player); break;
			case 6: GuildDirector_KickMember(player); break;
			case 7: GuildDirector_AddAllie(player); break;
			case 8: GuildDirector_AddAllie_Process(player); break;
			case 9: GuildDirector_AddEnemy(player); break;
			case 10: GuildDirector_AddEnemy_Process(player); break;
			case 11: GuildDirector_RemoveAllie(player); break;
			case 12: GuildDirector_RemoveAllie_Process(player); break;
			case 13: GuildDirector_RemoveEnemy(player); break;
			case 14: GuildDirector_RemoveEnemy_Process(player); break;
			case 16: GuildDirector_Deputy(player); break;
			case 17: GuildDirector_DeputyMember(player); break;
			case 18: GuildDirector_RemoveDeputy(player); break;
			case 19: GuildDirector_RemoveDeputyMember(player); break;
			
		case 15:
			{
				if (player.Guild != null && player.GuildMember.Rank == GuildRank.GuildLeader)
				{
					player.NpcInputData = string.Empty;
					player.Guild.AllianceId = 0;
				}
				
				break;
			}
	}
}

private static void GuildDirector_AboutGuilds(Player player)
{
	Dialog.SendDialog(player, GuildDirectorMessage(player, "ABOUT"));
	Dialog.SendOption(player,  Message(player, "THANKS"), 255);
	Dialog.Finish(player);
}

private static void GuildDirector_ManageNewGuild(Player player)
{
	if (player.Guild != null)
	{
		return;
	}
	
	Dialog.SendDialog(player, GuildDirectorMessage(player, "CREATE_GUILD_MSG"));
	Dialog.SendInput(player, 3);
	Dialog.SendOption(player,  Message(player, "NEVERMIND"), 255);
	Dialog.Finish(player);
}

private static void GuildDirector_CreateGuild(Player player)
{
	if (player.Guild != null)
	{
		return;
	}
	
	if (player.Level < GuildCreationLevel)
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "LEVEL_TOO_LOW"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	if (player.Money < GuildCreationCost)
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "MONEY_TOO_LOW"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	player.Money -= GuildCreationCost;
	
	if (GuildCollection.Create(player, player.NpcInputData))
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "SUCCESS"));
		Dialog.SendOption(player,  Message(player, "THANKS"), 255);
		Dialog.Finish(player);
	}
	else
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "FAIL"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
	}
}

private static void GuildDirector_ManageGuild_Deputy(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.DeputyLeader)
	{
		return;
	}
	
	Dialog.SendDialog(player, GuildDirectorMessage(player, "MANAGE_DEPUTY_LEADER_MSG"));
	Dialog.SendOption(player, GuildDirectorMessage(player, "KICK"), 5);
	Dialog.SendOption(player,  Message(player, "NOTHING"), 255);
	Dialog.Finish(player);
}

private static void GuildDirector_ManageGuild_Leader_Page1(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader)
	{
		return;
	}
	
	Dialog.SendDialog(player, GuildDirectorMessage(player, "MANAGE_GUILD_LEADER_MSG"));
	Dialog.SendOption(player, GuildDirectorMessage(player, "DISBAN"), 4);
	Dialog.SendOption(player, GuildDirectorMessage(player, "KICK"), 5);
	Dialog.SendOption(player, GuildDirectorMessage(player, "DEPUTY"), 16);
	Dialog.SendOption(player, GuildDirectorMessage(player, "REMOVE_DEPUTY"), 18);
	Dialog.SendOption(player,  Message(player, "NEXT"), 30);
	Dialog.SendOption(player,  Message(player, "NOTHING"), 255);
	Dialog.Finish(player);
}

private static void GuildDirector_ManageGuild_Leader_Page2(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader)
	{
		return;
	}
	
	Dialog.SendDialog(player, GuildDirectorMessage(player, "MANAGE_GUILD_LEADER_MSG"));
	if (player.Guild.AllieCount < 5) Dialog.SendOption(player, GuildDirectorMessage(player, "ADD_ALLIE"), 7);
	if (player.Guild.AllieCount > 0) Dialog.SendOption(player, GuildDirectorMessage(player, "REMOVE_ALLIE"), 11);
	if (player.Guild.EnemyCount < 5) Dialog.SendOption(player, GuildDirectorMessage(player, "ADD_ENEMY"), 9);
	if (player.Guild.EnemyCount > 0) Dialog.SendOption(player, GuildDirectorMessage(player, "REMOVE_ENEMY"), 13);
	Dialog.SendOption(player,  Message(player, "BACK"), 2);
	Dialog.SendOption(player,  Message(player, "NOTHING"), 255);
	Dialog.Finish(player);
}

private static void GuildDirector_DisbanGuild(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader)
	{
		return;
	}
	
	player.Guild.Disban();
	
	Dialog.SendDialog(player, GuildDirectorMessage(player, "DISBAN_MSG"));
	Dialog.SendOption(player,  Message(player, "THANK_YOU"), 255);
	Dialog.Finish(player);
}

private static void GuildDirector_Kick(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader && player.GuildMember.Rank != GuildRank.DeputyLeader)
	{
		return;
	}
	
	Dialog.SendDialog(player, GuildDirectorMessage(player, "KICK_MSG"));
	Dialog.SendInput(player, 6);
	Dialog.SendOption(player,  Message(player, "NEVERMIND"), 255);
	Dialog.Finish(player);
}

private static void GuildDirector_KickMember(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader && player.GuildMember.Rank != GuildRank.DeputyLeader)
	{
		return;
	}
	
	player.Guild.RemoveMember(player.NpcInputData, true);
}

private static void GuildDirector_AddAllie(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader)
	{
		return;
	}
	
	Guild guild;
	if (player.Guild.AllianceId > 0 && GuildCollection.TryGetGuild(player.Guild.AllianceId, out guild))
	{
		player.AllianceGuildName = guild.DbGuild.Name;
		
		Dialog.SendDialog(player, string.Format(GuildDirectorMessage(player, "PENDING_ALLIE_MSG"), guild.DbGuild.Name));
		Dialog.SendOption(player,  Message(player, "YES"), 8);
		Dialog.SendOption(player,  Message(player, "NO"), 15);
		Dialog.Finish(player);
		return;
	}
	else
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "ADD_ALLIE_MSG"));
		Dialog.SendInput(player, 8);
		Dialog.SendOption(player,  Message(player, "NEVERMIND"), 255);
		Dialog.Finish(player);
	}
}

private static void GuildDirector_AddAllie_Process(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader)
	{
		return;
	}
	
	var guild = GuildCollection.GetGuildByName(!string.IsNullOrWhiteSpace(player.AllianceGuildName) ? player.AllianceGuildName : player.NpcInputData);
	if (guild == null)
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "GUILD_NOT_FOUND"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	if (guild.Id == player.Guild.Id)
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "OWN_GUILD_ALLIE"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	if (player.Guild.IsAllie(guild))
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "ALREADY_ALLIE"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	if (player.Guild.IsEnemy(guild))
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "ALREADY_ENEMY"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	if (guild.AllianceId == 0)
	{
		guild.AllianceId = player.Guild.Id;
		player.Guild.AllianceId = guild.Id;
	}
	else if (guild.AllianceId == player.Guild.Id)
	{
		guild.AllianceId = 0;
		player.Guild.AllianceId = 0;
		player.AllianceGuildName = string.Empty;
		
		if (!player.Guild.AddAllie(guild))
		{
			Dialog.SendDialog(player, GuildDirectorMessage(player, "TOO_MANY_ALLIES"));
			Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
			Dialog.Finish(player);
		}
	}
}

private static void GuildDirector_AddEnemy(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader)
	{
		return;
	}
	
	Dialog.SendDialog(player, GuildDirectorMessage(player, "ADD_ENEMY_MSG"));
	Dialog.SendInput(player, 10);
	Dialog.SendOption(player,  Message(player, "NEVERMIND"), 255);
	Dialog.Finish(player);
}

private static void GuildDirector_AddEnemy_Process(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader)
	{
		return;
	}
	
	var guild = GuildCollection.GetGuildByName(player.NpcInputData);
	if (guild == null)
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "GUILD_NOT_FOUND"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	if (guild.Id == player.Guild.Id)
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "OWN_GUILD_ENEMY"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	if (player.Guild.IsAllie(guild))
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "ALREADY_ALLIE"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	if (player.Guild.IsEnemy(guild))
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "ALREADY_ENEMY"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	if (!player.Guild.AddEnemy(guild))
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "TOO_MANY_ENEMIES"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
	}
}

private static void GuildDirector_RemoveAllie(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader)
	{
		return;
	}
	
	Dialog.SendDialog(player, GuildDirectorMessage(player, "REMOVE_ALLIE_MSG"));
	Dialog.SendInput(player, 12);
	Dialog.SendOption(player,  Message(player, "NEVERMIND"), 255);
	Dialog.Finish(player);
}

private static void GuildDirector_RemoveAllie_Process(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader)
	{
		return;
	}
	
	var guild = GuildCollection.GetGuildByName(player.NpcInputData);
	if (guild == null)
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "GUILD_NOT_FOUND"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	if (!player.Guild.IsAllie(guild))
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "NOT_ALLIE"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	player.Guild.RemoveAllie(guild);
}

private static void GuildDirector_RemoveEnemy(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader)
	{
		return;
	}
	
	Dialog.SendDialog(player, GuildDirectorMessage(player, "REMOVE_ENEMY_MSG"));
	Dialog.SendInput(player, 14);
	Dialog.SendOption(player,  Message(player, "NEVERMIND"), 255);
	Dialog.Finish(player);
}

private static void GuildDirector_RemoveEnemy_Process(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader)
	{
		return;
	}
	
	var guild = GuildCollection.GetGuildByName(player.NpcInputData);
	if (guild == null)
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "GUILD_NOT_FOUND"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	if (!player.Guild.IsEnemy(guild))
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "NOT_ENEMY"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	player.Guild.RemoveEnemy(guild);
}

private static void GuildDirector_Deputy(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader)
	{
		return;
	}
	
	Dialog.SendDialog(player, GuildDirectorMessage(player, "DEPUTY_MSG"));
	Dialog.SendInput(player, 17);
	Dialog.SendOption(player,  Message(player, "NEVERMIND"), 255);
	Dialog.Finish(player);
}

private static void GuildDirector_DeputyMember(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader)
	{
		return;
	}
	
	var member = player.Guild.GetMemberByName(player.NpcInputData);
	
	if (member == null)
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "MEMBER_NOT_FOUND"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	if (member.DbGuildRank.PlayerId == player.DbPlayer.Id)
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "PROMOTE_SELF"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	player.Guild.Promote(member, GuildRank.DeputyLeader);
}

private static void GuildDirector_RemoveDeputy(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader)
	{
		return;
	}
	
	Dialog.SendDialog(player, GuildDirectorMessage(player, "REMOVE_DEPUTY_MSG"));
	Dialog.SendInput(player, 19);
	Dialog.SendOption(player,  Message(player, "NEVERMIND"), 255);
	Dialog.Finish(player);
}

private static void GuildDirector_RemoveDeputyMember(Player player)
{
	if (player.Guild == null || player.GuildMember.Rank != GuildRank.GuildLeader)
	{
		return;
	}
	
	var member = player.Guild.GetMemberByName(player.NpcInputData);
	
	if (member == null)
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "MEMBER_NOT_FOUND"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	if (member.Rank != GuildRank.DeputyLeader)
	{
		Dialog.SendDialog(player, GuildDirectorMessage(player, "NOT_DEPUTY"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	player.Guild.Promote(member, GuildRank.Member);
}
