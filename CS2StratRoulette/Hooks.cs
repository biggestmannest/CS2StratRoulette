using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Helpers;
using CS2StratRoulette.Interfaces;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CS2StratRoulette.Managers;

namespace CS2StratRoulette
{
	[SuppressMessage("Design", "MA0048")]
	[SuppressMessage("Design", "CA1822")]
	// ReSharper disable once InconsistentNaming
	public sealed partial class CS2StratRoulettePlugin
	{
		[GameEventHandler]
		public HookResult OnRoundStart(EventRoundStart _, GameEventInfo _2)
		{
			var rules = Game.Rules();

			if (rules is not null && rules.WarmupPeriod)
			{
				return HookResult.Continue;
			}

			StrategyManager.PostStop();
			StrategyManager.SetRandomStrategy();

			var started = StrategyManager.Start();

			if (started)
			{
				StrategyManager.Announce();
			}

			return HookResult.Continue;
		}

		[GameEventHandler]
		public HookResult OnRoundEnd(EventRoundEnd _, GameEventInfo __)
		{
			StrategyManager.Stop();

			return HookResult.Continue;
		}
	}
}
