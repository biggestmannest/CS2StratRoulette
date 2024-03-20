using System;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Timers;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Clumsy : Strategy
	{
		public override string Name =>
			"Clumsy";

		public override string Description =>
			"First day on the job";

		public override StrategyFlags Flags { get; protected set; } = StrategyFlags.Hidden;

		private static Random random = new();

		private Timer? timer;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			this.timer = new Timer(6.0f, Clumsy.OnInterval, TimerFlags.REPEAT);

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

		private static void OnInterval()
		{
			if ((Clumsy.random.Next() & 1) == 0)
			{
				foreach (var controller in Utilities.GetPlayers())
				{
					if (!controller.IsValid)
					{
						continue;
					}

					controller.DropActiveWeapon();
				}
			}
		}
	}
}
