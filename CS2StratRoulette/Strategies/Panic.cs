using CS2StratRoulette.Enums;
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

		public override StrategyFlags Flags { get; protected set; } = StrategyFlags.Hidden;

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

			const string playerSound = "player_hurt";

			plugin.DeregisterEventHandler(playerSound, this.OnPlayerHurt, true);

			return true;
		}

		private HookResult OnPlayerHurt(EventPlayerHurt @event, GameEventInfo _)
		{
			if (!this.Running)
			{
				return HookResult.Continue;
			}

			if (@event.Userid.IsValid)
			{
				@event.Userid.EquipKnife();
				@event.Userid.ExecuteClientCommandFromServer("say \"OUCH!!!!!!!!!!!!!!!!\"");
			}

			return HookResult.Continue;
		}
	}
}
