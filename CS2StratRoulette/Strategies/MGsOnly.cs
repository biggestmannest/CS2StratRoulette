using CS2StratRoulette.Constants;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class MGsOnly : Strategy
	{
		private static readonly string Enable = $"mp_buy_allow_guns {BuyAllow.Heavy.Str()}";

		public override string Name =>
			"MGs Only";

		public override string Description =>
			"You're only allowed to buy machine guns.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		public override bool CanRun()
		{
			var rules = Game.Rules();

			if (rules is null)
			{
				return false;
			}

			return rules.TotalRoundsPlayed is not (0 or 12);
		}

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(MGsOnly.Enable);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				Server.NextFrame(() =>
				{
					controller.EquipKnife();

					pawn.KeepWeaponsByType(
						CSWeaponType.WEAPONTYPE_KNIFE,
						CSWeaponType.WEAPONTYPE_C4,
						CSWeaponType.WEAPONTYPE_EQUIPMENT
					);
				});
			}

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
	}
}
