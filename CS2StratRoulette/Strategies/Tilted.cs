using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Modules.Utils;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Tilted : Strategy
	{
		public override string Name =>
			"Tilted";

		public override string Description =>
			"glhf";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		private readonly System.Random random = new();

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Player.ForEach((controller) =>
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					return;
				}

				if (controller.Team is not (CsTeam.Terrorist or CsTeam.CounterTerrorist))
				{
					return;
				}

				if (!pawn.IsAlive())
				{
					return;
				}

				var rotation = this.random.Next(35, 51);

				if ((rotation & 1) != 0)
				{
					rotation = -rotation;
				}

				var angle = pawn.V_angle;

				angle.Z = float.Abs(angle.Z - rotation);

				pawn.Teleport(pawn.AbsOrigin, angle, Vector.Zero);
			});

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Player.ForEach((controller) =>
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					return;
				}

				if (pawn.AbsRotation is null || !pawn.IsAlive())
				{
					return;
				}

				var angle = pawn.V_angle;

				angle.Z = 0f;

				pawn.Teleport(pawn.AbsOrigin, angle, Vector.Zero);
			});

			return true;
		}
	}
}
