using CS2StratRoulette.Constants;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Quake : Strategy
	{
		private const uint QuakeFov = 140u;
		private const uint DefaultFov = 90u;

		private const string WeaponAccuracyNoSpread = "weapon_accuracy_nospread";

		private const string Enable =
			"sv_cheats 1;sv_enablebunnyhopping 1;sv_maxvelocity 4000;sv_staminamax 0;sv_staminalandcost 0;sv_staminajumpcost 0;sv_accelerate_use_weapon_speed 0;sv_staminarecoveryrate 0;sv_autobunnyhopping 1;sv_airaccelerate 24;sv_cheats 0";

		private const string Disabled =
			"sv_cheats 1;sv_enablebunnyhopping 0;sv_maxvelocity 3500;sv_staminamax 80;sv_staminalandcost 0.05;sv_staminajumpcost 0.08;sv_accelerate_use_weapon_speed 1;sv_staminarecoveryrate 60;sv_autobunnyhopping 0;sv_airaccelerate 12;sv_cheats 0";

		public override string Name =>
			"Quake";

		public override string Description =>
			"It's Quake.";

		public override StrategyFlags Flags { get; protected set; } = StrategyFlags.Hidden;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			var noSpread = ConVar.Find(Quake.WeaponAccuracyNoSpread);

			noSpread?.SetValue(true);

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

				if (controller.Team is CsTeam.Terrorist)
				{
					pawn.RemoveC4();
				}

				controller.GiveNamedItem(CsItem.PPBizon);
				controller.EquipPrimary();

				controller.DesiredFOV = Quake.QuakeFov;

				Utilities.SetStateChanged(controller, "CBasePlayerController", "m_iDesiredFOV");
			}

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			var noSpread = ConVar.Find(Quake.WeaponAccuracyNoSpread);

			noSpread?.SetValue(false);

			Server.ExecuteCommand(Commands.BuyAllowAll);
			Server.ExecuteCommand(Commands.BuyAllowGrenadesEnable);
			Server.ExecuteCommand(Quake.Disabled);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn) || controller.IsBot)
				{
					continue;
				}

				controller.EquipKnife();

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
	}
}
