using System.Collections.Generic;
using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class HidenSeek : Strategy
	{
		public override string Name =>
			"Hide & Seek";

		public override string Description =>
			"Ts must kill the CTs with their knives. CTs must survive.";

		public override StrategyFlags Flags { get; protected set; } = StrategyFlags.Hidden;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn) || controller.IsBot)
				{
					continue;
				}

				if (controller.Team is CsTeam.CounterTerrorist)
				{
					controller.RemoveWeapons();
				}
				else if (controller.Team is CsTeam.Terrorist)
				{
					pawn.KeepWeaponsByType(CSWeaponType.WEAPONTYPE_KNIFE);
				}
			}

			{
				return true;
			}
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			return true;
		}
	}
}
