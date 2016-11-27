[ScriptEntry(Entry = "CreateTournamentQueue")]
public static void CreateTournamentQueue()
{
	TournamentQueueThread.Add(new TeamDeathMatch());
}