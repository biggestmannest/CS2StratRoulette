using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CS2StratRoulette.Interfaces;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class FlyingScoutsman : Strategy, IStrategyPostStop
	{
		private const string EnableFs =
			"sv_cheats 1;sv_gravity 230;sv_airaccelerate 20000;sv_maxspeed 420;sv_friction 4;sv_cheats 0";

		private const string DisableFs =
			"sv_cheats 1;sv_gravity 800;sv_airaccelerate 12;sv_maxspeed 320;sv_friction 5.2;sv_cheats 0";

		public override string Name =>
			"Flying Scoutsman";

		public override string Description =>
			"Low gravity + Scouts";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(ConsoleCommands.BuyAllowNone);

			Player.ForEach((controller) =>
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					return;
				}

				Server.NextFrame(() =>
				{
					pawn.KeepWeaponsByType(
						CSWeaponType.WEAPONTYPE_KNIFE,
						CSWeaponType.WEAPONTYPE_C4,
						CSWeaponType.WEAPONTYPE_EQUIPMENT
					);

					controller.GiveNamedItem(CsItem.SSG08);
				});

				Server.NextFrame(() => controller.EquipPrimary());
			});

			Server.ExecuteCommand(FlyingScoutsman.EnableFs);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(ConsoleCommands.BuyAllowAll);

			return true;
		}

		public void PostStop()
		{
			Player.ForEach((controller) =>
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					return;
				}

				Server.NextFrame(() =>
				{
					controller.EquipKnife();
					pawn.RemoveWeaponsByType(CSWeaponType.WEAPONTYPE_SNIPER_RIFLE);
				});
			});

			Server.ExecuteCommand(FlyingScoutsman.DisableFs);
		}
	}
}
