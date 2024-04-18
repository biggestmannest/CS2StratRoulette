using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class SneakOnly : Strategy
	{
		public override string Name =>
			"Sneak Only";

		public override string Description =>
			"Any footstep noises will kill you. Jumping/falling counts too.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			plugin.RegisterEventHandler<EventPlayerSound>(this.OnPlayerSound);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			plugin.DeregisterEventHandler<EventPlayerSound>(this.OnPlayerSound);

			return true;
		}

		private HookResult OnPlayerSound(EventPlayerSound @event, GameEventInfo _)
		{
			if (!this.Running)
			{
				return HookResult.Continue;
			}

			if (@event.Step &&
				@event.Userid is not null &&
				@event.Userid.IsValid)
			{
				@event.Userid.CommitSuicide(false, true);
			}

			return HookResult.Continue;
		}
	}
}
