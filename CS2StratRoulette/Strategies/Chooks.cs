using CS2StratRoulette.Constants;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Chooks : Strategy
	{
		public override string Name =>
			"Chooks";

		public override string Description =>
			"*whatever sound chickens make*";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.PrecacheModel(Models.Chicken);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				pawn.SetModel(Models.Chicken);
			}

			return true;
		}
	}
}
