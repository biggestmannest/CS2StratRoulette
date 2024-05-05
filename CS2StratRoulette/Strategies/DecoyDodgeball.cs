using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Timers;
using CS2StratRoulette.Extensions;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class DecoyDodgeball : Strategy
	{
		public override string Name =>
			"Decoy Dodgeball";

		public override string Description =>
			"1 HP + Infinite Decoys";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Player.ForEach((controller) =>
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					return;
				}

				Server.NextFrame(() => controller.EquipKnife());
				Server.NextFrame(() => controller.RemoveWeapons());

				controller.GiveNamedItem(CsItem.Decoy);

				Server.NextFrame(() =>
				{
					pawn.Health = 1;
					pawn.MaxHealth = 1;
					pawn.ArmorValue = 0;
				});

				Server.NextFrame(() =>
				{
					Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iMaxHealth");
					Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iMaxHealth");
					Utilities.SetStateChanged(pawn, "CCSPlayerPawn", "m_ArmorValue");
				});
			});

			plugin.RegisterEventHandler<EventWeaponFire>(this.OnWeaponFire);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			plugin.DeregisterEventHandler<EventWeaponFire>(this.OnWeaponFire);

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
