using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Helpers;
using CS2StratRoulette.Interfaces;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CS2StratRoulette.Managers;

namespace CS2StratRoulette
{
	[SuppressMessage("Design", "MA0048")]
	// ReSharper disable once InconsistentNaming
	public sealed partial class CS2StratRoulettePlugin
	{
		[GameEventHandler]
		public static HookResult OnRoundStart(EventRoundStart _, GameEventInfo _2)
		{
			var rules = Game.Rules();

			if (rules is not null && rules.WarmupPeriod)
			{
				return HookResult.Continue;
			}

			if (StrategyManager.ActiveStrategy is IStrategyPostStop strategy)
			{
				strategy.PostStop();
			}

			StrategyManager.CycleStrategy();

			if (StrategyManager.StartActiveStrategy() && StrategyManager.ActiveStrategy is not null)
			{
				StrategyManager.AnnounceStrategy(StrategyManager.ActiveStrategy);
			}

			return HookResult.Continue;
		}

		[GameEventHandler]
		public static HookResult OnRoundEnd(EventRoundEnd _, GameEventInfo __)
		{
			StrategyManager.StopActiveStrategy();

			return HookResult.Continue;
		}

		[GameEventHandler(HookMode.Pre)]
		public static HookResult OnRoundEndPre(EventRoundEnd _, GameEventInfo __)
		{
			if (StrategyManager.ActiveStrategy is IStrategyPreStop strategy)
			{
				strategy.PreStop();
			}

			return HookResult.Continue;
		}
	}
}
