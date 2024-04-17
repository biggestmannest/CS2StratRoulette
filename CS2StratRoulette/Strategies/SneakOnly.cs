using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class SneakOnly : Strategy
	{
		public override string Name =>
			"Sneak Only";

		public override string Description =>
			"Any footstep noises will kill you. Jumping/falling counts too.";

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

			if (@event.Userid.IsValid && @event.Step)
			{
				@event.Userid.CommitSuicide(false, true);
			}

			return HookResult.Continue;
		}
	}
}
