using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class InfiniteGrenades : Strategy
	{
		public override string Name =>
			"Infinite Grenades";

		public override string Description =>
			"Use all of your nades lineups.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			plugin.RegisterEventHandler<EventWeaponFire>(this.OnWeaponFire);

			Server.ExecuteCommand("mp_weapons_allow_typecount 0");
			Server.ExecuteCommand("ammo_grenade_limit_total 10");
			Server.ExecuteCommand("ammo_grenade_limit_default 2");

			Player.ForEach((controller) =>
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					return;
				}

				Server.NextFrame(controller.EquipKnife);
				Server.NextFrame(() =>
				{
					pawn.KeepWeaponsByType(
						CSWeaponType.WEAPONTYPE_KNIFE,
						CSWeaponType.WEAPONTYPE_C4,
						CSWeaponType.WEAPONTYPE_EQUIPMENT,
						CSWeaponType.WEAPONTYPE_GRENADE
					);
				});

				controller.GiveNamedItem(CsItem.HEGrenade);
				controller.GiveNamedItem(CsItem.IncendiaryGrenade);
				controller.GiveNamedItem(CsItem.SmokeGrenade);
				controller.GiveNamedItem(CsItem.Flashbang);
			});

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			plugin.DeregisterEventHandler<EventWeaponFire>(this.OnWeaponFire);

			Server.ExecuteCommand("mp_weapons_allow_typecount 5");
			Server.ExecuteCommand("ammo_grenade_limit_default 1");
			Server.ExecuteCommand("ammo_grenade_limit_total 4");

			return true;
		}

		private HookResult OnWeaponFire(EventWeaponFire @event, GameEventInfo _)
		{
			if (!this.Running)
			{
				return HookResult.Continue;
			}

			var controller = @event.Userid;

			if (controller is null)
			{
				return HookResult.Continue;
			}

			var weapon = @event.Weapon.Substring(7 /*="weapon_".Length*/);

			var isGrenade = weapon is "hegrenade" or
									  "flashbang" or
									  "smokegrenade" or
									  "molotov" or
									  "incgrenade";

			if (!isGrenade)
			{
				return HookResult.Continue;
			}

			Server.NextFrame(() => { controller.GiveNamedItem(weapon); });

			return HookResult.Continue;
		}
	}
}
