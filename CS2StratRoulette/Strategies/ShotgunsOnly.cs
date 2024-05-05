using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Core;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class ShotgunsOnly : Strategy
	{
		private static readonly string Enable = $"mp_buy_allow_guns {BuyAllow.Shotguns.Str()}";

		public override string Name =>
			"Shotguns Only";

		public override string Description =>
			"You're only allowed to buy shotguns.";

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

			Server.ExecuteCommand(ShotgunsOnly.Enable);

			Player.ForEach((controller) =>
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					return;
				}

				Server.NextFrame(() => controller.EquipKnife());
				Server.NextFrame(() =>
				{
					pawn.KeepWeaponsByType(
						CSWeaponType.WEAPONTYPE_KNIFE,
						CSWeaponType.WEAPONTYPE_EQUIPMENT
					);
				});
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

			return true;
		}
	}
}
