using CS2StratRoulette.Constants;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class HidenSeek : Strategy
	{
		public override string Name =>
			"Hide & Seek";

		public override string Description =>
			"Ts must kill the CTs with their knives. CTs must survive.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(ConsoleCommands.BuyAllowNone);
			Server.ExecuteCommand(ConsoleCommands.BuyAllowGrenadesDisable);

			Player.ForEach((controller) =>
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					return;
				}

				if (controller.Team is CsTeam.CounterTerrorist)
				{
					controller.RemoveWeapons();
				}
				else if (controller.Team is CsTeam.Terrorist)
				{
					Server.NextFrame(() => controller.EquipKnife());
					Server.NextFrame(() =>
					{
						pawn.KeepWeaponsByType(CSWeaponType.WEAPONTYPE_KNIFE);
						pawn.RemoveC4();
					});
				}
			});

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(ConsoleCommands.BuyAllowAll);
			Server.ExecuteCommand(ConsoleCommands.BuyAllowGrenadesEnable);

			return true;
		}
	}
}
