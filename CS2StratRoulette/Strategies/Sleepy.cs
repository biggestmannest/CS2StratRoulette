using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Sleepy : Strategy
	{
		private const string Fade = "fadeout 5 0 0 0";

		private const string Reset = "fadein .25 0 0 0";

		public override string Name =>
			"Sleepy";

		public override string Description =>
			"Occasionally shoot to stay awake.";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			plugin.RegisterEventHandler<EventPlayerShoot>(this.OnPlayerShoot);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			plugin.DeregisterEventHandler("player_shoot", this.OnPlayerShoot, true);

			return true;
		}

		private HookResult OnPlayerShoot(EventPlayerShoot @event, GameEventInfo _)
		{
			if (!this.Running || !@event.Userid.IsValid || @event.Userid.IsBot || @event.Userid.PawnIsAlive)
			{
				return HookResult.Continue;
			}

			@event.Userid.ExecuteClientCommandFromServer(Sleepy.Reset);
			@event.Userid.ExecuteClientCommandFromServer(Sleepy.Fade);

			return HookResult.Continue;
		}
	}
}
