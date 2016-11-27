// Project by Bauss
using System;

namespace CandyConquer.WorldApi.Controllers.Arena
{
	/// <summary>
	/// Controller for arena info.
	/// </summary>
	public class ArenaInfoController
	{
		/// <summary>
		/// Gets the arena info associated with the controller.
		/// </summary>
		public Models.Arena.ArenaInfo ArenaInfo { get; protected set; }
		
		/// <summary>
		/// Creates a new arena info controller.
		/// </summary>
		public ArenaInfoController()
		{
			
		}
		
		/// <summary>
		/// Handles winning.
		/// </summary>
		public void Win()
		{
			var time = DateTime.UtcNow;
			
			if (time.Day != ArenaInfo.DbPlayerArenaQualifier.Timestamp.Day)
			{
				ArenaInfo.DbPlayerArenaQualifier.TotalWinsToday = 0;
			}
			
			if (time.Month != ArenaInfo.DbPlayerArenaQualifier.Timestamp.Month)
			{
				ArenaInfo.DbPlayerArenaQualifier.TotalWinsSeason = 0;
			}
			
			ArenaInfo.DbPlayerArenaQualifier.TotalWinsToday++;
			ArenaInfo.DbPlayerArenaQualifier.TotalWins++;
			
			ArenaInfo.DbPlayerArenaQualifier.Update();
			
			var player = ArenaInfo.Player;
			if (player != null)
			{
				player.MapChangeEvents.Enqueue((p) =>
				                               {
				                               	p.ClientSocket.Send(new Models.Packets.Arena.ArenaActionPacket
				                               	                    {
				                               	                    	Dialog = Enums.ArenaDialog.EndMatchDialog,
				                               	                    	Option = Enums.ArenaOption.Win
				                               	                    });
				                               });
			}
		}
		
		/// <summary>
		/// Handles losing.
		/// </summary>
		public void Lose()
		{
			var time = DateTime.UtcNow;
			
			if (time.Day != ArenaInfo.DbPlayerArenaQualifier.Timestamp.Day)
			{
				ArenaInfo.DbPlayerArenaQualifier.TotalLossToday = 0;
			}
			
			if (time.Month != ArenaInfo.DbPlayerArenaQualifier.Timestamp.Month)
			{
				ArenaInfo.DbPlayerArenaQualifier.TotalLossSeason = 0;
			}
			
			ArenaInfo.DbPlayerArenaQualifier.TotalLossToday++;
			ArenaInfo.DbPlayerArenaQualifier.TotalLoss++;
			
			ArenaInfo.DbPlayerArenaQualifier.Update();
			
			var player = ArenaInfo.Player;
			if (player != null)
			{
				player.MapChangeEvents.Enqueue((p) =>
				                               {
				                               	p.ClientSocket.Send(new Models.Packets.Arena.ArenaActionPacket
				                               	                    {
				                               	                    	Dialog = Enums.ArenaDialog.EndMatchDialog,
				                               	                    	Option = Enums.ArenaOption.Lose
				                               	                    });
				                               });
			}
		}
	}
}
