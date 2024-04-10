using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
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

			const string playerHurt = "player_hurt";

			plugin.DeregisterEventHandler(playerHurt, this.OnPlayerHurt, true);

			return true;
		}

		private HookResult OnPlayerHurt(EventPlayerHurt @event, GameEventInfo _)
		{
			if (!this.Running)
			{
				return HookResult.Continue;
			}

			var controller = @event.Userid;

			if (!controller.TryGetPlayerPawn(out var pawn))
			{
				return HookResult.Continue;
			}

			controller.EquipKnife();

			pawn.KeepWeaponsByType(
				CSWeaponType.WEAPONTYPE_PISTOL,
				CSWeaponType.WEAPONTYPE_KNIFE,
				CSWeaponType.WEAPONTYPE_C4
			);

			return HookResult.Continue;
		}
	}
}
