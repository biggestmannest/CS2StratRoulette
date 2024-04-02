using System;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class GhostSlaps : Strategy
	{
		public override string Name =>
			"Ghost Slaps";

		public override string Description =>
			"You know the drill.";

		private readonly Random random = new();

		private Timer? timer;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			var randomNum = this.random.Next(10);

			this.timer = new Timer(5f, () =>
			{
				foreach (var players in Utilities.GetPlayers())
				{
					if (randomNum < 5)
					{
						this.DoSlap(players);
					}
				}
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

		private void DoSlap(CCSPlayerController controller)
		{
			if (!controller.TryGetPlayerPawn(out var pawn) || pawn.AbsRotation is null || pawn.AbsOrigin is null)
			{
				return;
			}

			var position = pawn.AbsOrigin;
			var angle = pawn.AbsRotation;

			var velocityX = this.random.Next(151, 401);
			var velocityY = this.random.Next(151, 401);
			var velocityZ = this.random.Next(401, 600);

			Vector finalVel = new(velocityX, velocityY, velocityZ);

			pawn.Teleport(
				position,
				angle,
				finalVel
			);
		}
	}
}
