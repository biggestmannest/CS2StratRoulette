using CS2StratRoulette.Enums;
using CS2StratRoulette.Helpers;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using System.Diagnostics.CodeAnalysis;

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

			Player.ForEach((controller) => { controller.GiveNamedItem(CsItem.Healthshot); });

			return true;
		}
	}
}
