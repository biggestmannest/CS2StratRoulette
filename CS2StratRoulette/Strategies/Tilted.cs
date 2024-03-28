using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CS2StratRoulette.Helpers;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Tilted : Strategy
	{
		private const string Activate = "mp_global_damage_per_second 1";
		private const string Deactivate = "mp_global_damage_per_second 0";

		public override string Name =>
			"Tilted";

		public override string Description =>
			"glhf";

		public override StrategyFlags Flags => StrategyFlags.Hidden;

		private readonly System.Random random = new();

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				if (controller.Team is not (CsTeam.Terrorist or CsTeam.CounterTerrorist))
				{
					continue;
				}

				if (pawn.AbsOrigin is null || pawn.AbsRotation is null)
				{
					continue;
				}

				var rotation = this.random.Next(20, 51);

				if ((rotation & 1) == 0)
				{
					rotation = -rotation;
				}

				pawn.AbsRotation.Z = float.Abs(pawn.AbsRotation.Z - rotation);

				pawn.Teleport(pawn.AbsOrigin, pawn.AbsRotation, pawn.AbsVelocity);
			}

			var a = Game.RulesProxy();

			return true;
		}
	}
}
