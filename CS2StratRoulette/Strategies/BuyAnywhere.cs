using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class BuyAnywhere : Strategy
	{
		private const string BuyAnywhereEnable = "mp_buy_anywhere 1";
		private const string BuyAnywhereDisable = "mp_buy_anywhere 0";

		public override string Name =>
			"Global Shop";

		public override string Description =>
			"You can buy anywhere on the map.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(BuyAnywhere.BuyAnywhereEnable);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(BuyAnywhere.BuyAnywhereDisable);

			return true;
		}
	}
}
