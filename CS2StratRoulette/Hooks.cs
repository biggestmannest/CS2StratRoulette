using CS2StratRoulette.Helpers;
using CS2StratRoulette.Managers;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;

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
			if (!this.Active)
			{
				return HookResult.Continue;
			}

			var rules = Game.Rules();

			if (rules is not null && rules.WarmupPeriod)
			{
				return HookResult.Continue;
			}

			StrategyManager.PostStop();
			StrategyManager.Next();

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
			if (!this.Active)
			{
				return HookResult.Continue;
			}

			StrategyManager.Stop();

			return HookResult.Continue;
		}

		[GameEventHandler]
		public HookResult OnGameEnd(EventGameEnd _, GameEventInfo __)
		{
			StrategyManager.Kill();

			return HookResult.Continue;
		}

		[GameEventHandler]
		public HookResult OnMapTransition(EventMapTransition _, GameEventInfo __)
		{
			StrategyManager.Kill();

			return HookResult.Continue;
		}

		[GameEventHandler]
		public HookResult OnPlayerConnect(EventPlayerConnect @event, GameEventInfo _)
		{
			if (@event.Userid is not null)
			{
				Player.Replace(@event.Userid);
			}

			return HookResult.Continue;
		}

		[GameEventHandler]
		public HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo _)
		{
			if (@event.Userid is not null)
			{
				Player.Remove(@event.Userid);
			}

			return HookResult.Continue;
		}
	}
}
