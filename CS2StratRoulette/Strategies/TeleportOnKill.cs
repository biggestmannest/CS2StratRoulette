using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class TeleportOnKill : Strategy
	{
		public override string Name =>
			"Turbo Peek";

		public override string Description =>
			"You teleport to the player you killed.";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			plugin.RegisterEventHandler<EventPlayerDeath>(this.OnDeath);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			plugin.DeregisterEventHandler<EventPlayerDeath>(this.OnDeath);

			return true;
		}

		private HookResult OnDeath(EventPlayerDeath @event, GameEventInfo _)
		{
			var attacker = @event.Attacker;

			var playerKilled = @event.Userid;

			if (!attacker.TryGetPlayerPawn(out var attackerPawn))
			{
				return HookResult.Continue;
			}

			if (!playerKilled.TryGetPlayerPawn(out var victimPawn))
			{
				return HookResult.Continue;
			}

			var position = victimPawn.AbsOrigin;
			var angle = attackerPawn.V_angle;

			attackerPawn.Teleport(position, angle, Vector.Zero);

			return HookResult.Continue;
		}
	}
}
