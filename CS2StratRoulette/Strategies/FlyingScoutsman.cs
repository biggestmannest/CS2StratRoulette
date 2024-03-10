using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CS2StratRoulette.Interfaces;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class FlyingScoutsman : Strategy, IStrategyPostStop
	{
		private const string EnableFs =
			"sv_cheats 1;sv_gravity 230;sv_airaccelerate 20000;sv_maxspeed 420;sv_friction 4;sv_cheats 0";

		private const string DisableFs =
			"sv_cheats 1;sv_gravity 800;sv_airaccelerate 12;sv_maxspeed 320;sv_friction 5.2;sv_cheats 0";

		private static readonly string EnableBuy =
			$"mp_buy_allow_guns {BuyAllow.All.Str()}";

		private static readonly string DisableBuy =
			$"mp_buy_allow_guns {BuyAllow.None.Str()}";

		public override string Name =>
			"Flying Scoutsman";

		public override string Description =>
			"Low gravity + Scouts";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(FlyingScoutsman.DisableBuy);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				pawn.KeepWeaponsByType(
					CSWeaponType.WEAPONTYPE_KNIFE,
					CSWeaponType.WEAPONTYPE_C4,
					CSWeaponType.WEAPONTYPE_EQUIPMENT
				);

				controller.GiveNamedItem(CsItem.SSG08);
			}

			Server.ExecuteCommand(FlyingScoutsman.EnableFs);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(FlyingScoutsman.EnableBuy);

			return true;
		}

		public void PostStop()
		{
			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				pawn.RemoveWeaponsByType(CSWeaponType.WEAPONTYPE_SNIPER_RIFLE);
			}

			Server.ExecuteCommand(FlyingScoutsman.DisableFs);
		}
	}
}
