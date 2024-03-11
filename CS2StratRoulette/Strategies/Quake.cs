using CounterStrikeSharp.API;
using CS2StratRoulette.Extensions;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CS2StratRoulette.Constants;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Quake : Strategy
	{
		private const uint QuakeFov = 160u;
		private const uint DefaultFov = 90u;

		private const string Enable =
			"sv_cheats 1;sv_enablebunnyhopping 1;sv_maxvelocity 4000;sv_staminamax 0;sv_staminalandcost 0;sv_staminajumpcost 0;sv_accelerate_use_weapon_speed 0;sv_staminarecoveryrate 0;sv_autobunnyhopping 1;sv_airaccelerate 24;sv_cheats 0";

		private const string Disabled =
			"sv_cheats 1;sv_enablebunnyhopping 0;sv_maxvelocity 3500;sv_staminamax 80;sv_staminalandcost 0.05;sv_staminajumpcost 0.08;sv_accelerate_use_weapon_speed 1;sv_staminarecoveryrate 60;sv_autobunnyhopping 0;sv_airaccelerate 12;sv_cheats 0";

		public override string Name =>
			"Quake";

		public override string Description =>
			"It's Quake.";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Commands.BuyAllowNone);
			Server.ExecuteCommand(Commands.BuyAllowGrenadesDisable);
			Server.ExecuteCommand(Quake.Enable);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn) || controller.IsBot)
				{
					continue;
				}

				pawn.KeepWeaponsByType(CSWeaponType.WEAPONTYPE_KNIFE, CSWeaponType.WEAPONTYPE_C4);

				controller.GiveNamedItem(CsItem.SSG08);

				controller.DesiredFOV = Quake.QuakeFov;

				Utilities.SetStateChanged(controller, "CBasePlayerController", "m_iDesiredFOV");
			}

			plugin.RegisterEventHandler<EventPlayerShoot>(this.OnPlayerShoot, HookMode.Post);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Commands.BuyAllowAll);
			Server.ExecuteCommand(Commands.BuyAllowGrenadesEnable);
			Server.ExecuteCommand(Quake.Disabled);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn) || controller.IsBot)
				{
					continue;
				}

				pawn.KeepWeaponsByType(
					CSWeaponType.WEAPONTYPE_KNIFE,
					CSWeaponType.WEAPONTYPE_C4,
					CSWeaponType.WEAPONTYPE_EQUIPMENT
				);

				controller.DesiredFOV = Quake.DefaultFov;

				Utilities.SetStateChanged(controller, "CBasePlayerController", "m_iDesiredFOV");
			}

			return true;
		}

		private HookResult OnPlayerShoot(EventPlayerShoot @event, GameEventInfo _)
		{
			var controller = @event.Userid;

			if (!controller.TryGetPlayerPawn(out var pawn))
			{
				return HookResult.Continue;
			}

			Quake.ResetAmmo(pawn);

			return HookResult.Continue;
		}

		private static void ResetAmmo(CCSPlayerPawn pawn)
		{
			pawn.ForEachWeapon((weapon) =>
			{
				if (weapon.IsValid)
				{
					weapon.Clip1 = 2;
				}
			});
		}
	}
}
