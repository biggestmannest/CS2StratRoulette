using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Timers;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Schizophrenia : Strategy
	{
		private const float Interval = 5.0f;

		private const string EffectOne = "sounds/sfx/nearmiss/bulletby_subsonic";

		public override string Name =>
			"Schizophrenia";

		public override string Description =>
			"???????????????????";

		private readonly System.Random random = new();

		private Timer? timer;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
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
			foreach (var controller in Utilities.GetPlayers())
			{
				if (this.random.FiftyFifty())
				{
					continue;
				}

				if (controller.TryGetPlayerPawn(out var pawn) && pawn.IsAlive())
				{
					controller.ExecuteClientCommand($"play {Schizophrenia.EffectOne}_0{this.random.Next(1, 8).Str()}");
				}
			}
		}
	}
}
