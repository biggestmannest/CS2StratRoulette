using CS2StratRoulette.Extensions;
using CS2StratRoulette.Helpers;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class FranzJ : Strategy
	{
		public override string Name =>
			"FranzJ";

		public override string Description =>
			"Get the highest velocity possible, the team with the least average velocity loses.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		private readonly List<CCSPlayerController> ts = new(Server.MaxPlayers / 2);
		private readonly List<CCSPlayerController> cts = new(Server.MaxPlayers / 2);

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

			var rules = Game.Rules();

			if (rules is null)
			{
				return false;
			}

			foreach (var players in Utilities.GetPlayers())
			{
				if (!players.IsValid)
				{
					System.Console.WriteLine("players are invalid!!!!!!!!!!!!!! franzj");
					continue;
				}

				// ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
				switch (players.Team)
				{
					case CsTeam.CounterTerrorist:
						this.cts.Add(players);
						continue;
					case CsTeam.Terrorist:
						this.ts.Add(players);
						continue;
				}
			}

			this.timer = new Timer(5f, this.RecordVelocities, TimerFlags.REPEAT);

			System.Console.WriteLine($"[FranzJ::Start] {rules.RoundTime.Str()}");

			//SINCE TERMINATEROUND DOESNT WORK!!!!!!!!!!!!!!!!!!!!!!!
			this.timer2 = new Timer(rules.RoundTime - 10f, this.Outcome, 0);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			this.timer?.Kill();
			this.timer2?.Kill();

			return true;
		}

		private void RecordVelocities()
		{
			if (!this.Running)
			{
				return;
			}

			this.averageVelocityCts += FranzJ.AverageVelocity(this.cts);
			this.averageVelocityTs += FranzJ.AverageVelocity(this.ts);

			this.count++;
		}

		private void Outcome()
		{
			if (!this.Running)
			{
				return;
			}

			this.timer?.Kill();

			var totalAverageCt = this.averageVelocityCts / this.count;
			var totalAverageT = this.averageVelocityTs / this.count;

			var winner = (totalAverageCt > totalAverageT) ? CsTeam.CounterTerrorist : CsTeam.Terrorist;
			var team = (winner is CsTeam.CounterTerrorist) ? this.ts : this.cts;
			var message = (winner is CsTeam.CounterTerrorist)
							  ? $"CTs win. They had an average velocity of {totalAverageCt.Str()}, while Ts had an average velocity of {totalAverageT.Str()}."
							  : $"Ts win. They had an average velocity of {totalAverageT.Str()}, while CTs had an average velocity of {totalAverageCt.Str()}.";

			// ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
			foreach (var controller in team)
			{
				if (controller.IsValid && !controller.IsHLTV)
				{
					controller.CommitSuicide(false, true);
				}
			}

			Server.PrintToChatAll(message);
		}

		private static float AverageVelocity(List<CCSPlayerController> players)
		{
			var totalVelocity = 0f;

			foreach (var controller in players)
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				var velocity = pawn.AbsVelocity;

				totalVelocity += (velocity.X + velocity.Y + velocity.Z) / 3f;
			}

			return totalVelocity;
		}
	}
}
