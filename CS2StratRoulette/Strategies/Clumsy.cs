using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;

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

		private static readonly System.Random Random = new();

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
			foreach (var controller in Utilities.GetPlayers())
			{
				if (Clumsy.Random.FiftyFifty() || !controller.IsValid)
				{
					continue;
				}

				if (!controller.TryGetPlayerPawn(out var pawn) || pawn.WeaponServices is null)
				{
					continue;
				}

				if (!pawn.WeaponServices.ActiveWeapon.TryGetValue(out var weapon) || !weapon.TryGetData(out var data))
				{
					continue;
				}

				// @todo
				if (data.Slot != 3)
				{
					controller.DropActiveWeapon();
				}
			}
		}
	}
}
