using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Timers;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Schizophrenia : Strategy
	{
		public override string Name =>
			"Schizophrenia";

		public override string Description =>
			"??????????????????? (recommended to type \"snd_toolvolume 0.05\" in console)";

		private readonly Random random = new();

		private Timer? timer;

		private const float Interval = 5.0f;

		private const string EffectOne = "sounds/weapons/fx/nearmiss/bulletby_subsonic";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			foreach (var players in Utilities.GetPlayers())
			{
				players.ExecuteClientCommand("snd_toolvolume 0.3");
			}

			this.timer = new Timer(Schizophrenia.Interval, this.OnInterval, TimerFlags.REPEAT);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			this.timer?.Kill();
			return true;
		}

		private void OnInterval()
		{
			if (this.random.Next(2) == 0)
			{
				return;
			}

			var number = this.random.Next(1, 8);
			foreach (var player in Utilities.GetPlayers())
			{
				if (player.IsValid)
				{
					player.ExecuteClientCommand($"play {Schizophrenia.EffectOne}_0{number.Str()}");
				}
			}
		}
	}
}
