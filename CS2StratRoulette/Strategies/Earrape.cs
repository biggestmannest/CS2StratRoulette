using CS2StratRoulette.Constants;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CS2StratRoulette.Helpers;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Earrape : Strategy
	{
		private static readonly string StartCommands =
			$"sv_cheats 1; sv_infinite_ammo 2; sv_cheats 0; {ConsoleCommands.BuyAllowNone}; mp_buy_allow_grenades 0";

		private static readonly string StopCommands =
			$"sv_cheats 1; sv_infinite_ammo 0; sv_cheats 0; {ConsoleCommands.BuyAllowAll}; mp_buy_allow_grenades 1";

		public override string Name =>
			"Earrape";

		public override string Description =>
			"Everyone gets a Negev and decoy grenades, successfully ruining the enemies ears.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Earrape.StartCommands);

			plugin.RegisterEventHandler<EventWeaponFire>(this.OnWeaponFire);

			Player.ForEach((controller) =>
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					return;
				}

				Server.NextFrame(() => controller.EquipKnife());
				Server.NextFrame(() =>
				{
					pawn.KeepWeaponsByType(
						CSWeaponType.WEAPONTYPE_KNIFE,
						CSWeaponType.WEAPONTYPE_C4,
						CSWeaponType.WEAPONTYPE_EQUIPMENT
					);

					controller.GiveNamedItem(CsItem.Negev);
					controller.GiveNamedItem(CsItem.Decoy);
				});
			});

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Earrape.StopCommands);

			plugin.DeregisterEventHandler<EventWeaponFire>(this.OnWeaponFire);

			Player.ForEach((controller) =>
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					return;
				}

				pawn.RemoveWeaponsByType(CSWeaponType.WEAPONTYPE_MACHINEGUN);
			});

			return true;
		}

		private HookResult OnWeaponFire(EventWeaponFire @event, GameEventInfo _)
		{
			var controller = @event.Userid;

			if (controller is null)
			{
				return HookResult.Continue;
			}

			var weapon = @event.Weapon;
			var isGrenade = Weapon.IsGrenade(weapon);

			if (!isGrenade)
			{
				return HookResult.Continue;
			}

			Server.NextFrame(() => { controller.GiveNamedItem(weapon); });

			return HookResult.Continue;
		}
	}
}
