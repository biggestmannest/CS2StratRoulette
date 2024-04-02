using CS2StratRoulette.Constants;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

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

			Server.ExecuteCommand(Commands.BuyAllowNone);
			Server.ExecuteCommand(Commands.BuyAllowGrenadesDisable);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (controller.IsHLTV || !controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				if (controller.Team is CsTeam.CounterTerrorist)
				{
					controller.RemoveWeapons();
				}
				else if (controller.Team is CsTeam.Terrorist)
				{
					controller.EquipKnife();
					pawn.KeepWeaponsByType(CSWeaponType.WEAPONTYPE_KNIFE);
				}
			}

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

			return true;
		}
	}
}
