// Project by Bauss
using System;
using CandyConquer.Drivers.Repositories.Api.Attributes;
using CandyConquer.Security.Api;

namespace CandyConquer.WorldApi.Helpers.Packets.Interaction
{
	/// <summary>
	/// Controller for marriage.
	/// </summary>
	[ApiController()]
	public static class Marriage
	{
		/// <summary>
		/// Handles the court.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.InteractionPacket,
		         SubIdentity = (uint)Enums.InteractionAction.Court)]
		public static bool HandleCourt(Models.Entities.Player player, Models.Packets.Entities.InteractionPacket packet)
		{
			if (packet.TargetClientId == player.ClientId)
			{
				return false;
			}
			
			if (player.Spouse != "None")
			{
				return true;
			}
			
			Models.Maps.IMapObject obj;
			if (player.GetFromScreen(packet.TargetClientId, out obj))
			{
				var marriageTarget = obj as Models.Entities.Player;
				if (marriageTarget != null)
				{
					if (marriageTarget.Spouse != "None")
					{
						return true;
					}
					
					player.PendingSpouse = marriageTarget.ClientId;
					marriageTarget.ClientSocket.Send(packet);
				}
			}
			
			return true;
		}
		
		/// <summary>
		/// Handles the marriage.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="packet">The packet.</param>
		[ApiCall(CallSecurity = CallSecurity.Always,
		         Identity = Data.Constants.PacketTypes.InteractionPacket,
		         SubIdentity = (uint)Enums.InteractionAction.Marry)]
		public static bool HandleMarry(Models.Entities.Player player, Models.Packets.Entities.InteractionPacket packet)
		{
			if (packet.TargetClientId == player.ClientId)
			{
				return false;
			}
			
			if (player.Spouse != "None")
			{
				return true;
			}
			
			Models.Maps.IMapObject obj;
			if (player.GetFromScreen(packet.TargetClientId, out obj))
			{
				var marriageTarget = obj as Models.Entities.Player;
				if (marriageTarget != null)
				{
					if (marriageTarget.Spouse != "None")
					{
						return true;
					}
					
					if (marriageTarget.PendingSpouse != player.ClientId)
					{
						return true;
					}
					
					player.AddActionLog("Marriage", player.Name + " : " + marriageTarget.Name);
					
					marriageTarget.Spouse = player.Name;
					player.Spouse = marriageTarget.Name;
					
					player.ClientSocket.Send(new Models.Packets.Misc.StringPacket
					                         {
					                         	Action = Enums.StringAction.Mate,
					                         	Data = player.ClientId,
					                         	String = marriageTarget.Name
					                         });
					
					marriageTarget.ClientSocket.Send(new Models.Packets.Misc.StringPacket
					                         {
					                         	Action = Enums.StringAction.Mate,
					                         	Data = marriageTarget.ClientId,
					                         	String = player.Name
					                         });
					
					var fireworks = new Models.Packets.Misc.StringPacket
					{
						Action = Enums.StringAction.MapEffect,
						PositionX = player.X,
						PositionY = player.Y,
						String = "firework-2love"
					};
					
					player.ClientSocket.Send(fireworks);
					
					fireworks.PositionX = marriageTarget.X;
					fireworks.PositionY = marriageTarget.Y;
					marriageTarget.ClientSocket.Send(fireworks);
					
					Collections.PlayerCollection.BroadcastFormattedMessage("MARRIAGE_CONGRATZ", player.Name, marriageTarget.Name);
				}
			}
			
			return true;
		}
	}
}
