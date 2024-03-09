using CS2StratRoulette.Extensions;
using CS2StratRoulette.Interfaces;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class FlyingScoutsman : Strategy, IStrategyPostStop
	{
		private const string EnableFS =
			"sv_cheats 1;sv_gravity 230;sv_airaccelerate 20000; sv_maxspeed 420; sv_friction 4; sv_cheats 0";

		private const string DisabledFS =
			"sv_cheats1;sv_gravity 800;sv_airaccelerate 12;sv_maxspeed 320;sv_friction 5.2;sv_cheats 0";

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

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				pawn.RemoveWeaponsByType(
					true,
					CSWeaponType.WEAPONTYPE_C4,
					CSWeaponType.WEAPONTYPE_KNIFE,
					CSWeaponType.WEAPONTYPE_MELEE,
					CSWeaponType.WEAPONTYPE_EQUIPMENT
				);

				controller.GiveNamedItem("weapon_ssg08");
			}

			Server.ExecuteCommand(FlyingScoutsman.EnableFS);

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

				pawn.RemoveWeaponsByType(false, CSWeaponType.WEAPONTYPE_SNIPER_RIFLE);
			}

			Server.ExecuteCommand(FlyingScoutsman.DisabledFS);
		}
	}
}