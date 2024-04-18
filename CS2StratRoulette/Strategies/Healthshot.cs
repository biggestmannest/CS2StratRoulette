using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Healthshot : Strategy
	{
		public override string Name =>
			"Morphine!!!!!!!!!!!!!!!!!";

		public override string Description =>
			"Every player gets a healthshot.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			foreach (var players in Utilities.GetPlayers())
			{
				players.GiveNamedItem(CsItem.Healthshot);
			}

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			return true;
		}
	}
}
