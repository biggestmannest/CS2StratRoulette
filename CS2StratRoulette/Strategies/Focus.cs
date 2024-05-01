using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class StratName : Strategy
	{
		public override string Name =>
			"Focus";

		public override string Description =>
			"FOCUS ON THE FAKING GAME!!!!!!!!!! (you cant inspect your weapon)";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			plugin.RegisterEventHandler<EventInspectWeapon>(this.OnInspect);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			plugin.DeregisterEventHandler<EventInspectWeapon>(this.OnInspect);

			return true;
		}

		private HookResult OnInspect(EventInspectWeapon @event, GameEventInfo _)
		{
			var controller = @event.Userid;

			if (!controller.IsValid)
			{
				return HookResult.Continue;
			}

			controller.CommitSuicide(false, true);

			return HookResult.Continue;
		}
	}
}
