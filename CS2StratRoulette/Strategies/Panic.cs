using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Panic : Strategy
	{
		public override string Name =>
			"Panic";

		public override string Description =>
			"Every time you are shot, you want to run away.";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			plugin.RegisterEventHandler<EventPlayerHurt>(this.OnPlayerHurt);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			plugin.DeregisterEventHandler<EventPlayerHurt>(this.OnPlayerHurt);

			return true;
		}

		private HookResult OnPlayerHurt(EventPlayerHurt @event, GameEventInfo _)
		{
			if (!this.Running)
			{
				return HookResult.Continue;
			}

			if (@event.Userid.TryGetPlayerController(out var controller))
			{
				controller.EquipKnife();
				controller.ExecuteClientCommandFromServer("say \"OUCH!!!!!!!!!!!!!!!!\"");
			}

			return HookResult.Continue;
		}
	}
}
