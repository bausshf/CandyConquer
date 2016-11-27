/// <summary>
/// Team death match tournament implementation.
/// </summary>
public sealed class TeamDeathMatch : CandyConquer.WorldApi.Controllers.Battle.BattleController, ITournamentBase
{
	/// <summary>
	/// The id of the tournament. Do not touch this as it's used internally by the tournament system.
	/// </summary>
	public int Id { get; set; }
	
	/// <summary>
	/// The black team id.
	/// </summary>
	private const int Black = 1;
	
	/// <summary>
	/// The white team id.
	/// </summary>
	private const int White = 2;
	
	/// <summary>
	/// The blue team id.
	/// </summary>
	private const int Blue = 3;
	
	/// <summary>
	/// The red team id.
	/// </summary>
	private const int Red = 4;
	
	/// <summary>
	/// The black team.
	/// </summary>
	private TournamentTeam BlackTeam;
	
	/// <summary>
	/// The white team.
	/// </summary>
	private TournamentTeam WhiteTeam;
	
	/// <summary>
	/// The blue team.
	/// </summary>
	private TournamentTeam BlueTeam;
	
	/// <summary>
	/// The red team.
	/// </summary>
	private TournamentTeam RedTeam;
	
	/// <summary>
	/// The next team to add a player to.
	/// </summary>
	private int _nextTeam;
	
	/// <summary>
	/// Creates a new team death match tournament.
	/// </summary>
	public TeamDeathMatch()
		: base()
	{
		TournamentCollection.Add(this);
		
		BlackTeam = new TournamentTeam(this, Black, "Black", 224, 329);
		WhiteTeam = new TournamentTeam(this, White, "White", 181, 81);
		BlueTeam = new TournamentTeam(this, Blue, "Blue", 68, 190);
		RedTeam = new TournamentTeam(this, Red, "Red", 347, 222);
	}
	
	#region Info
	/// <summary>
	/// Gets the name of the tournament.
	/// </summary>
	public string Name { get { return "TeamDeathMatch"; } }
	
	/// <summary>
	/// Gets the short name of the tournament.
	/// </summary>
	public string ShortName { get { return "TDM"; } }
	
	/// <summary>
	/// Gets the description of team death match.
	/// </summary>
	public string Description { get { return "There are 4 teams (Black, white, blue and red.)\nThe 4 teams has to compete against each other.\nThe team with most kills win."; } }
	#endregion
	
	#region Requirements
	/// <summary>
	/// Gets the minimum level required to join the tournament.
	/// </summary>
	public byte RequiredLevel { get { return 1; } }
	
	/// <summary>
	/// Gets the required job to join the tournament.
	/// </summary>
	public Job RequiredJob { get { return Job.Unknown; } }
	
	/// <summary>
	/// Gets the required reborns to join the tournament.
	/// </summary>
	public byte RequiredReborns { get { return 0; } }
	
	/// <summary>
	/// Gets a boolean determining whether players need to be VIP to join the tournament or not.
	/// </summary>
	public bool RequiredToBeVIP { get { return false; } }
	
	/// <summary>
	/// Gets a boolean determining whether players need to be female to join the tournament or not.
	/// </summary>
	public bool RequiredToBeFemale { get { return false; } }
	
	/// <summary>
	/// Gets a boolean determining whether players need to be male to join the tournament or not.
	/// </summary>
	public bool RequiredToBeMale { get { return false; } }
	
	/// <summary>
	/// Gets a boolean determining whether players need to be in a guild to join the tournament.
	/// </summary>
	public bool RequiredGuild { get { return false; } }
	
	/// <summary>
	/// Gets a boolean determining whether players need to be a guild leader to join the tournament.
	/// </summary>
	public bool RequiredGuildLeader { get { return false; } }
	
	/// <summary>
	/// Gets a boolean determining whether players need to be a deputy leader to join the tournament.
	/// </summary>
	public bool RequiredDeputyLeader { get { return false; } }
	
	/// <summary>
	/// Gets the minimum required members a player must have in their team to join the tournament.
	/// </summary>
	public int MinimumTeamMembers { get { return 0; } }
	#endregion
	
	#region Config
	/// <summary>
	/// Gets the type of the tournament.
	/// </summary>
	public TournamentType TournamentType { get { return TournamentType.Queue; } }
	
	/// <summary>
	/// Gets the timeout period in milliseconds.
	/// </summary>
	public int Timeout { get { return 60000 * 5; } }
	
	/// <summary>
	/// Gets or sets a boolean determining whether players can sign up for this tournament currently.
	/// </summary>
	public bool CanSignUp { get; set; }
	
	/// <summary>
	/// Gets or sets a boolean determining whether the tournament has ended or not.
	/// </summary>
	public bool Ended { get; set; }
	
	/// <summary>
	/// Gets the map id of the tournament.
	/// </summary>
	public int MapId { get { return 10001; } }
	
	/// <summary>
	/// Gets the reward of the tournament.
	/// </summary>
	public TournamentReward Reward
	{
		get
		{
			return new TournamentReward
			{
				CPs = (uint)CandyConquer.Drivers.Repositories.Safe.Random.Next(10, 100)
			};
		}
	}
	
	/// <summary>
	/// Gets a boolean determining whether the tournament is weekly or not.
	/// </summary>
	public bool IsWeekly { get { return false; } }
	
	/// <summary>
	/// If the tournament is weekly then, gets the day of the week the tournament runs.
	/// </summary>
	public DayOfWeek DayOfWeek { get { return (DayOfWeek)0; } }
	
	/// <summary>
	/// Gets the hour the tournament should run in.
	/// </summary>
	public int Hour { get { return 00; } }
	
	/// <summary>
	/// Gets the minute of the hour the tournament should run in.
	/// </summary>
	public int Minute { get { return 00; } }
	
	/// <summary>
	/// Gets or sets a boolean whether the tournament is currently running. Do not touch this, it's used internally by the tournament system.
	/// </summary>
	public bool Running { get; set; }
	#endregion
	
	#region Functions
	/// <summary>
	/// Handling player sign up.
	/// </summary>
	/// <param name="player">The player.</param>
	/// <returns>The sign up response.</returns>
	public TournamentSignUpResponse SignUp(Player player)
	{
		var response = TournamentSignUpResponse.SignUp(this, player);
		
		if (response.Success)
		{
			switch (_nextTeam)
			{
				case Black:
					BlackTeam.Add(player);
					player.Equipments.Mask(ItemPosition.Garment, 181525);
					break;
					
				case White:
					WhiteTeam.Add(player);
					player.Equipments.Mask(ItemPosition.Garment, 181325);
					break;
					
				case Blue:
					BlueTeam.Add(player);
					player.Equipments.Mask(ItemPosition.Garment, 181825);
					break;
					
				case Red:
					RedTeam.Add(player);
					player.Equipments.Mask(ItemPosition.Garment, 181625);
					break;
			}
			
			_nextTeam = Math.Max(1, Math.Min(4, _nextTeam + 1));
			
			player.Battle = this;
			
			player.Equipments.Mask(ItemPosition.WeaponL, 420229);
			player.Equipments.Mask(ItemPosition.WeaponR, 410039);
			
			player.MaskedSkills.Clear();
			player.MaskedSkills.TryAdd(1045);
			player.MaskedSkills.TryAdd(1046);
			
			foreach (var skillId in player.MaskedSkills.GetHashes())
			{
				player.ClientSocket.Send(new CandyConquer.WorldApi.Models.Packets.Spells.SkillPacket
			                               {
			                               	Experience = 0,
			                               	Id = skillId,
			                               	Level = 4
			                               });
			}
		}
		
		return response;
	}
	
	/// <summary>
	/// Handler for when the tournament has to open sign up.
	/// </summary>
	public void OnSignUp()
	{
		CanSignUp = true;
		Ended = false;
		_nextTeam = 1;
		
		BlackTeam.Clear();
		WhiteTeam.Clear();
		BlueTeam.Clear();
		RedTeam.Clear();
	}
	
	/// <summary>
	/// Handler for when players signed up for the tournament has to be send to the tournament map.
	/// </summary>
	public void OnSend()
	{
		CanSignUp = false;
		
		BlackTeam.Enter();
		WhiteTeam.Enter();
		BlueTeam.Enter();
		RedTeam.Enter();
		
		SendScore();
	}
	
	/// <summary>
	/// Handler for when the tournament times out and has to end.
	/// </summary>
	public void OnEndTimeOut()
	{
		Ended = true;
		var winnerTeam = GetWinner();
		if (winnerTeam != null)
		{
			winnerTeam.Reward(Reward);
			
			PlayerCollection.BroadcastFormattedMessage("TOURNAMENT_END_WINNER_TEAM", Name, winnerTeam.Name);
		}
		else
		{
			PlayerCollection.BroadcastFormattedMessage("TOURNAMENT_END", Name);
		}
		
		BlackTeam.Leave(true);
		WhiteTeam.Leave(true);
		BlueTeam.Leave(true);
		RedTeam.Leave(true);
		
		ClearScore();
	}
	
	/// <summary>
	/// The next time the score should be updated.
	/// </summary>
	private DateTime _nextScoreUpdate = DateTime.UtcNow;
	
	/// <summary>
	/// Handler for a timed event. (Executed every 100 MS)
	/// </summary>
	public void OnTime()
	{
		if (DateTime.UtcNow >= _nextScoreUpdate)
		{
			_nextScoreUpdate = DateTime.UtcNow.AddMilliseconds(10000);
			
			SendScore();
		}
	}
	#endregion
	
	#region Handlers
	/// <summary>
	/// Handles begin attack.
	/// </summary>
	/// <param name="attacker">The attacker.</param>
	/// <returns>True if the attack can begin.</returns>
	public override bool HandleBeginAttack(Player attacker)
	{
		return true;
	}
	
	/// <summary>
	/// Handles the attack.
	/// </summary>
	/// <param name="attacker">The attacker.</param>
	/// <param name="attacked">The attacked.</param>
	/// <param name="damage">The damage.</param>
	/// <returns>True if the attack can be handled.</returns>
	public override bool HandleAttack(Player attacker, Player attacked, ref uint damage)
	{
		if (attacker.TournamentTeam != null && attacked.TournamentTeam != null)
		{
			if (attacker.TournamentTeam.Id != attacked.TournamentTeam.Id)
			{
				attacker.TournamentTeam.Kills++;
				attacked.Kill(attacker, 1);
				
				SendScore();
			}
		}
		
		return false;
	}
	/// <summary>
	/// Handles the begin of a physical hit.
	/// </summary>
	/// <param name="attacker">The attacker.</param>
	/// <returns>True if the physical attack can begin.</returns>
	public override bool HandleBeginHit_Physical(Player attacker)
	{
		return false;
	}
	/// <summary>
	/// Handles the begin of a ranged attack.
	/// </summary>
	/// <param name="attacker">The attacker.</param>
	/// <returns>True if the ranged attack can begin.</returns>
	public override bool HandleBeginHit_Ranged(Player attacker)
	{
		return false;
	}
	/// <summary>
	/// Handles the begin of a magic attack.
	/// </summary>
	/// <param name="attacker">The attacker.</param>
	/// <param name="packet">The spell packet.</param>
	/// <returns>True if the magic attack can begin.</returns>
	public override bool HandleBeginHit_Magic(Player attacker, CandyConquer.WorldApi.Models.Packets.Spells.SpellPacket packet)
	{
		return attacker.MaskedSkills.Contains(packet.SpellId);
	}
	/// <summary>
	/// Handles death.
	/// </summary>
	/// <param name="attacker">The attacker.</param>
	/// <param name="attacked">The attacked.</param>
	/// <returns>True if the death can be handled.</returns>
	public override bool HandleDeath(Player attacker, Player attacked)
	{
		attacked.RemoveStatusFlag(StatusFlag.IceBlock);
		
		return true;
	}
	/// <summary>
	/// Handles revive.
	/// </summary>
	/// <param name="killed">The killed player.</param>
	/// <returns>True if the revive can be handled.</returns>
	public override bool HandleRevive(Player killed)
	{
		killed.TournamentTeam.Respawn(killed);
		
		return false;
	}
	/// <summary>
	/// Handles the enter of an area.
	/// </summary>
	/// <param name="player">The player.</param>
	/// <returns>True if the player enters.</returns>
	public override bool EnterArea(Player player)
	{
		if (player.TournamentTeam != null)
		{
			var canMove = true;
			switch (player.TournamentTeam.Id)
			{
				case Black:
					canMove = !WhiteTeam.InArea(player) &&
						!BlueTeam.InArea(player) &&
						!RedTeam.InArea(player);
					break;
				case White:
					canMove = !BlackTeam.InArea(player) &&
						!BlueTeam.InArea(player) &&
						!RedTeam.InArea(player);
					break;
				case Blue:
					canMove = !WhiteTeam.InArea(player) &&
						!BlackTeam.InArea(player) &&
						!RedTeam.InArea(player);
					break;
				case Red:
					canMove = !BlackTeam.InArea(player) &&
						!WhiteTeam.InArea(player) &&
						!BlueTeam.InArea(player);
					break;
			}
			
			if (!canMove)
			{
				player.AddStatusFlag(StatusFlag.IceBlock);
			}
		}
		
		return true;
	}
	
	/// <summary>
	/// Handles the leave of an area.
	/// </summary>
	/// <param name="player">The player.</param>
	/// <returns>True if the player leaves.</returns>
	public override bool LeaveArea(Player player)
	{
		return true;
	}
	
	/// <summary>
	/// Handles the kill of a monster.
	/// </summary>
	/// <param name="player">The player.</param>
	/// <param name="monster">The monster.</param>
	public override void KillMonster(Player player, Monster monster)
	{
		// ...
	}
	
	/// <summary>
	/// Handles the disconnect of a player.
	/// </summary>
	/// <param name="player">The player.</param>
	public override void HandleDisconnect(Player player)
	{
		if (player.TournamentTeam != null)
		{
			player.TournamentTeam.Remove(player);
			player.TournamentTeam = null;
		}
	}
	
	/// <summary>
	/// Gets the teams of the tournament sorted by score (kills)
	/// </summary>
	/// <returns>The list of teams sorted by score (kills)</returns>
	private List<TournamentTeam> GetTopTeams()
	{
		return (
			new List<TournamentTeam>
			{
				BlackTeam,
				WhiteTeam,
				BlueTeam,
				RedTeam
			}
		).OrderByDescending(team => team.Kills).ToList();
	}
	
	/// <summary>
	/// Gets the winner of the tournament.
	/// </summary>
	/// <returns>The winner of the tournament, null if no winners.</returns>
	private TournamentTeam GetWinner()
	{
		var teams = GetTopTeams();
		var possibleWinner = teams.FirstOrDefault();
		
		return possibleWinner.Kills > teams[1].Kills &&
			possibleWinner.Kills > teams[2].Kills &&
			possibleWinner.Kills > teams[3].Kills ?
			possibleWinner : null;
	}
	
	/// <summary>
	/// Sends the scores to all players in the tournament.
	/// </summary>
	private void SendScore()
	{
		ClearScore();
		
		var teams = GetTopTeams();
		var scores = new string[teams.Count];
		for (int i = 0; i < teams.Count; i++)
		{
			var team = teams[i];
			
			scores[i] = string.Format("{0}. {1}: {2}", i + 1, team.Name, team.Kills);
		}
		
		BlackTeam.BroadcastScore(scores);
		WhiteTeam.BroadcastScore(scores);
		BlueTeam.BroadcastScore(scores);
		RedTeam.BroadcastScore(scores);
	}
	
	/// <summary>
	/// Clears the scores for all players in the tournament.
	/// </summary>
	private void ClearScore()
	{
		BlackTeam.ClearScore();
		WhiteTeam.ClearScore();
		BlueTeam.ClearScore();
		RedTeam.ClearScore();
	}
	#endregion
}
