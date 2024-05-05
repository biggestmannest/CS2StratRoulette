using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class GhostSlaps : Strategy
	{
		private const float Interval = 2f;

		public override string Name =>
			"Ghost Slaps";

		public override string Description =>
			"You know the drill.";

		private static readonly System.Random Random = new();

		private Timer? timer;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			this.timer = new Timer(GhostSlaps.Interval, static () =>
			{
				Player.ForEach((controller) =>
				{
					if (GhostSlaps.Random.Next(10) < 4)
					{
						GhostSlaps.DoSlap(controller);
					}
				});
			}, TimerFlags.REPEAT);

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

		private static void DoSlap(CCSPlayerController controller)
		{
			if (!controller.TryGetPlayerPawn(out var pawn))
			{
				return;
			}

			var velocity = new Vector(GhostSlaps.Random.Next(150, 350),
									  GhostSlaps.Random.Next(150, 350),
									  GhostSlaps.Random.Next(200, 500));

			velocity.X = (GhostSlaps.Random.FiftyFifty() ? -velocity.X : velocity.X);
			velocity.Y = (GhostSlaps.Random.FiftyFifty() ? -velocity.Y : velocity.Y);

			pawn.Teleport(pawn.AbsOrigin, pawn.V_angle, pawn.AbsVelocity + velocity);
		}
	}
}
