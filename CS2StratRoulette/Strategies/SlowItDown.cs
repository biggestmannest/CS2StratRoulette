using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class SlowItDown : Strategy
	{
		private const string HostTimescale = "host_timescale";

		public override string Name =>
			"SlowItDown";

		public override string Description =>
			"Time slows with each kill.";

		private float speed = 1.0f;
		private float step = 0.1f;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			this.step = (1f / Utilities.GetPlayers().Count);

			plugin.RegisterEventHandler<EventPlayerDeath>(this.OnPlayerDeath);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			const string playerDeath = "player_death";

			plugin.DeregisterEventHandler(playerDeath, this.OnPlayerDeath, true);

			ConVar.Find(SlowItDown.HostTimescale)?.SetValue(1f);

			return true;
		}

		private HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo _)
		{
			if (!this.Running || !@event.Userid.IsValid)
			{
				return HookResult.Continue;
			}

			this.speed = float.Max(this.speed - this.step, 0.1f);

			ConVar.Find(SlowItDown.HostTimescale)?.SetValue(this.step);

			return HookResult.Continue;
		}
	}
}
