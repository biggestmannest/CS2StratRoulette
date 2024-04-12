using System;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Timers;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Ryan : Strategy
	{
		public override string Name =>
			"Callouts";

		public override string Description =>
			"Better listen to the voices!";

		public override StrategyFlags Flags { get; protected set; } = StrategyFlags.Hidden;

		private const string Sounds = "sounds/sfx/ryan/ryan";

		private readonly Random random = new();

		private const float Interval = 6.5f;

		private Timer? timer;

		public override bool Start(ref Base plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			this.timer = new Timer(Ryan.Interval, this.OnInterval, TimerFlags.REPEAT);

			return true;
		}

		public override bool Stop(ref Base plugin)
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

			var randomNum = this.random.Next(1, 32).Str().PadLeft(2, '0');
			foreach (var players in Utilities.GetPlayers())
			{
				if (players.IsValid)
				{
					players.ExecuteClientCommand($"play {Ryan.Sounds}_{randomNum}");
				}
			}
		}
	}
}
