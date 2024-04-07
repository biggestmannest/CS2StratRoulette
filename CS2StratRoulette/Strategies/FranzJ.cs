using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Extensions;

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
		private Timer? timer2;

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
					System.Console.WriteLine("players are invalid!!!!!!!!!!!!!! franzj");
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

			this.timer = new Timer(5f, () =>
			{
				this.averageVelocityCts += this.AverageVelocity(cts);
				this.averageVelocityTs += this.AverageVelocity(ts);
				this.count++;
			}, TimerFlags.REPEAT);
			
			//SINCE TERMINATEROUND DOESNT WORK!!!!!!!!!!!!!!!!!!!!!!!
			this.timer2 = new Timer(20f, () =>
			{
				this.timer.Kill();
				var totalAverageCt = this.averageVelocityCts / this.count;
				var totalAverageT = this.averageVelocityTs / this.count;
				var message = "";
				
				if (totalAverageCt < totalAverageT)
				{
					message =
						$"CTs win. They had an average velocity of {totalAverageCt}, while Ts had an average velocity of {totalAverageT}.";
					foreach (var player in cts)
					{
						player.CommitSuicide(true, false);
					} 
				} else if (totalAverageT < totalAverageCt)
				{
					message =
						$"Ts win. They had an average velocity of {totalAverageT}, while CTs had an average velocity of {totalAverageCt}.";
					foreach (var player in ts)
					{
						player.CommitSuicide(true, false);
					}
				}
				Server.PrintToChatAll(message);
			});

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			this.timer2?.Kill();
			return true;
		}

		private float AverageVelocity(List<CCSPlayerController> players)
		{
			var totalVelocity = 0f;
			foreach (var player in players)
			{
				
				if (!player.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}
				System.Console.WriteLine($"X velocity: {pawn.Velocity.X}");
				System.Console.WriteLine($"Y velocity: {pawn.Velocity.Y}");
				System.Console.WriteLine($"Z velocity: {pawn.Velocity.Z}");
				totalVelocity += (pawn.Velocity.X + pawn.Velocity.Y + pawn.Velocity.Z) / 3f;
			}

			return totalVelocity;
		}
	}
}
