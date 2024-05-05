using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class ScratchGuns : Strategy
	{
		public override string Name =>
			"Don't scratch the goods!";

		public override string Description =>
			"You can only use your primary while at 100 health.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

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

			var controller = @event.Userid;

			if (controller is null || !controller.TryGetPlayerPawn(out var pawn))
			{
				return HookResult.Continue;
			}

			Server.NextFrame(controller.EquipKnife);
			Server.NextFrame(() =>
			{
				pawn.KeepWeaponsByType(
					CSWeaponType.WEAPONTYPE_PISTOL,
					CSWeaponType.WEAPONTYPE_KNIFE,
					CSWeaponType.WEAPONTYPE_C4
				);
			});

			return HookResult.Continue;
		}
	}
}
