using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class FranzJ : Strategy
	{
		public override string Name =>
			"FranzJ";

		public override string Description =>
			"Get the highest velocity possible, the team with the least average velocity loses.";

		private float averageVelocityTs;
		private float averageVelocityCts;

		private int count;

		private Timer? timer;
		
		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			var cts = new List<CCSPlayerController>(Server.MaxPlayers / 2);
			var ts = new List<CCSPlayerController>(Server.MaxPlayers / 2);

			foreach (var players in Utilities.GetPlayers())
			{
				if (!players.IsValid)
				{
					continue;
				}

				if (players.Team is CsTeam.CounterTerrorist)
				{
					cts.Add(players);
				}
				else if (players.Team is CsTeam.Terrorist)
				{
					ts.Add(players);
				}
			}
			
			plugin.RegisterEventHandler<EventRoundEnd>(this.onRoundEnd);

			this.timer = new Timer(5f, () =>
			{
				this.count += 1;
				this.averageVelocityCts += this.AverageVelocity(cts);
				this.averageVelocityTs += this.AverageVelocity(ts);
			});

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}
			
			var message = "";

			if (this.averageVelocityTs < this.averageVelocityCts)
			{
				message = "cts have won";
			}
			else
			{
				message = "ts have won";
			}

			Server.PrintToChatAll(message);

			return true;
		}

		private float AverageVelocity(List<CCSPlayerController> players)
		{
			var totalVelocity = 0f;
			foreach (var player in players)
			{
				totalVelocity += (player.Velocity.X + player.Velocity.Y + player.Velocity.Z) / 3;
			}

			return players.Count > 0 ? totalVelocity / players.Count : 0;
		}

		private HookResult onRoundEnd(EventRoundEnd @event, GameEventInfo _)
		{
			@event.Winner = (int)CsTeam.CounterTerrorist;

			return HookResult.Continue;
		}
	}
}
