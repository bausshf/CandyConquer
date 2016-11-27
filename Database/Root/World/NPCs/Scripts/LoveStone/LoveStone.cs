/// <summary>
/// LoveStone
/// </summary>
[ScriptEntry(Entry = 702)]
public static void LoveStone(Player player, byte option)
{
	switch (option)
	{
		case 0:
			{
				Dialog.SendDialog(player, LoveStoneMessage(player, "WELCOME_MSG"));
				if (player.Spouse == "None")
				{
					Dialog.SendOption(player,  LoveStoneMessage(player, "MARRY"), 1);
				}
				else
				{
					Dialog.SendOption(player,  LoveStoneMessage(player, "DIVORCE"), 1);
				}
				Dialog.SendOption(player,  Message(player, "NEVERMIND"), 255);
				Dialog.Finish(player);
				break;
			}
		case 1:
			{
				if (player.Spouse == "None")
				{
					LoveStone_Marriage(player);
				}
				else
				{
					LoveStone_Divorce(player);
				}
				
				break;
			}
	}
}

private static void LoveStone_Marriage(Player player)
{
	player.ClientSocket.Send(new CandyConquer.WorldApi.Models.Packets.Client.DataExchangePacket
	                         {
	                         	ClientId = player.ClientId,
	                         	Data1 = 1067,
	                         	ExchangeType = ExchangeType.PostCmd,
	                         	Data3Low = player.X,
	                         	Data3High = player.Y
	                         });
}

private static void LoveStone_Divorce(Player player)
{
	var spouse = PlayerCollection.GetPlayerByName(player.Spouse);
	
	if (spouse != null)
	{
		spouse.Spouse = "None";
		spouse.ClientSocket.Send(new CandyConquer.WorldApi.Models.Packets.Misc.StringPacket
		                         {
		                         	String = "None",
		                         	Data = spouse.ClientId,
		                         	Action = StringAction.Mate
		                         });
	}
	else
	{
		var dbPlayer = CandyConquer.Database.Dal.Players.GetPlayerByName(player.Spouse, CandyConquer.Drivers.Settings.WorldSettings.Server);
		if (dbPlayer != null)
		{
			dbPlayer.Spouse = "None";
			dbPlayer.Update();
		}
	}
	
	player.Spouse = "None";
	player.ClientSocket.Send(new CandyConquer.WorldApi.Models.Packets.Misc.StringPacket
	                         {
	                         	String = "None",
	                         	Data = player.ClientId,
	                         	Action = StringAction.Mate
	                         });
}