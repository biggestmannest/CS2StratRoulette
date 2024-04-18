using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Timers;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Ryan : Strategy
	{
		private const float Interval = 6.5f;
		private const string Sounds = "sounds/sfx/ryan/ryan";

		public override string Name =>
			"Callouts";

		public override string Description =>
			"Better listen to the voices!";

		private readonly System.Random random = new();

		private Timer? timer;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			this.timer = new Timer(Ryan.Interval, this.OnInterval, TimerFlags.REPEAT);

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
			if (this.random.FiftyFifty())
			{
				return;
			}

			var randomNum = this.random.Next(1, 32).Str().PadLeft(2, '0');

			foreach (var controller in Utilities.GetPlayers())
			{
				if (controller.TryGetPlayerPawn(out var pawn) && pawn.IsAlive())
				{
					controller.ExecuteClientCommand($"play {Ryan.Sounds}_{randomNum}");
				}
			}
		}
	}
}
