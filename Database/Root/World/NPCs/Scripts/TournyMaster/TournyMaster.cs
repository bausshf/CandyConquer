[ScriptEntry(Entry = 128008)]
public static void TournyMaster(Player player, byte option)
{
	if (player.Battle != null)
	{
		Dialog.SendDialog(player, TournyMasterMessage(player, "ALREADY_BATTLE"));
		Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
		Dialog.Finish(player);
		return;
	}
	
	switch (option)
	{
		case 0:
			{
				Dialog.SendDialog(player, TournyMasterMessage(player, "WELCOME_MSG"));
				Dialog.SendOption(player,  TournyMasterMessage(player, "SIGN_UP"), 1);
				Dialog.SendOption(player,  TournyMasterMessage(player, "GUIDE"), 2);
				Dialog.SendOption(player,  Message(player, "NEVERMIND"), 255);
				Dialog.Finish(player);
				break;
			}
			
		case 1:
			{
				var tournaments = TournamentCollection.GetAllTournamentsForSignUp().ToList();
				
				if (tournaments.Count == 0)
				{
					Dialog.SendDialog(player, TournyMasterMessage(player, "NO_TOURNAMENTS_SIGN_UP"));
					Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
					Dialog.Finish(player);
				}
				else
				{
					Dialog.SendDialog(player, TournyMasterMessage(player, "SIGN_UP_MSG"));
					
					foreach (var tournament in tournaments)
					{
						Dialog.SendOption(player, tournament.Name, (byte)(tournament.Id + 100));
					}
					
					Dialog.SendOption(player,  Message(player, "NONE"), 255);
					Dialog.Finish(player);
				}
				break;
			}
			
		default:
			{
				if (option >= 200)
				{
					var guideTournament = TournamentCollection.GetTournamentById(option - 200);
					
					if (guideTournament != null)
					{
						Dialog.SendDialog(player, guideTournament.Description);
						Dialog.SendOption(player,  Message(player, "THANKS"), 255);
						Dialog.Finish(player);
					}
				}
				else if (option >= 100)
				{
					var signUpTournament = TournamentCollection.GetTournamentById(option - 100);
					
					if (signUpTournament != null)
					{
						var response = signUpTournament.SignUp(player);
						
						if (response.Success)
						{
							Dialog.SendDialog(player, string.Format(
								Language.GetMessage(player.Language, response.Message),
								signUpTournament.Name
							));
							Dialog.SendOption(player,  Message(player, "THANKS"), 255);
						}
						else
						{
							Dialog.SendDialog(player, Language.GetMessage(player.Language, response.Message));
							Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
						}
						
						Dialog.Finish(player);
					}
				}
				else // (implicit) if (option < 100)
				{
					var tournaments = TournamentCollection.GetAllTournaments().ToList();
					
					if (tournaments.Count > 0)
					{
						option -= 2;
						
						var takeOff = (tournaments.Count - (option * 4));
						Console.WriteLine(takeOff);
						var fromPage = Math.Min(Math.Max(0, (tournaments.Count - takeOff)), (option * 4));
						var toPage = Math.Min((tournaments.Count - fromPage), 4);
						
						tournaments = tournaments.Skip(fromPage).Take(toPage).ToList();
						
						Dialog.SendDialog(player, TournyMasterMessage(player, "GUIDE_MSG"));
						
						foreach (var tournament in tournaments)
						{
							Dialog.SendOption(player, tournament.Name, (byte)(tournament.Id + 200));
						}
						
						option += 2;
						
						if (option > 2)
						{
							Dialog.SendOption(player,  Message(player, "BACK"), (byte)(option - 1));
						}
						if (takeOff > 4)
						{
							Dialog.SendOption(player,  Message(player, "NEXT"), (byte)(option + 1));
						}
						Dialog.SendOption(player,  Message(player, "NEVERMIND"), 255);
						Dialog.Finish(player);
					}
					else
					{
						Dialog.SendDialog(player, TournyMasterMessage(player, "NO_TOURNAMENTS"));
						Dialog.SendOption(player,  Message(player, "I_SEE"), 255);
						Dialog.Finish(player);
					}
				}
				break;
			}
	}
}