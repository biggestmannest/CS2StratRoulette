using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public class Earrape : Strategy
	{
		private static readonly string StartCommands =
			$"sv_cheats 1; sv_infinite_ammo 2; sv_cheats 0; mp_buy_allow_guns {BuyAllow.None.Str()}; mp_buy_allow_grenades 0; mp_weapons_buy_allow_zeus 0";

		private static readonly string StopCommands =
			$"sv_cheats 1; sv_infinite_ammo 0; sv_cheats 0; mp_buy_allow_guns {BuyAllow.All.Str()}; mp_buy_allow_grenades 1; mp_weapons_buy_allow_zeus 1";

		private const float Interval = 5.0f;

		public override string Name =>
			"Earrape";

		public override string Description =>
			"Everyone gets a Negev and decoy grenades, successfully ruining the enemies ears.";

		private readonly System.Random random = new();
		private Timer? timer;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Earrape.StartCommands);

			var players = Utilities.GetPlayers();
			var ts = new List<CCSPlayerController>(10);

			foreach (var controller in players)
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

				controller.GiveNamedItem(CsItem.Negev);
				controller.GiveNamedItem(CsItem.Decoy);

				if (controller.Team is CsTeam.Terrorist)
				{
					ts.Add(controller);
				}
			}

			var giveC4 = ts[this.random.Next(ts.Count)];

			if (giveC4.IsValid)
			{
				giveC4.GiveNamedItem(CsItem.C4);
			}

			this.timer = new Timer(Earrape.Interval, this.OnInterval, TimerFlags.REPEAT);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			this.timer?.Kill();

			Server.ExecuteCommand(Earrape.StopCommands);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				pawn.RemoveWeaponsByType(
					false,
					CSWeaponType.WEAPONTYPE_MACHINEGUN
				);
			}

			return true;
		}

		private void OnInterval()
		{
			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				pawn.RemoveWeaponsByType(
					false,
					CSWeaponType.WEAPONTYPE_GRENADE
				);

				controller.GiveNamedItem(CsItem.Decoy);
			}
		}
	}
}
