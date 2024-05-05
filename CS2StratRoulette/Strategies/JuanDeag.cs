using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class JuanDeag : Strategy
	{
		private const string Enable = "mp_damage_headshot_only 1";
		private const string Disable = "mp_damage_headshot_only 0";

		public override string Name =>
			"Juan Deag";

		public override string Description =>
			"Everyone gets a deagle, and can only hit headshots.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(ConsoleCommands.BuyAllowNone);
			Server.ExecuteCommand(JuanDeag.Enable);

			Player.ForEach((controller) =>
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					return;
				}

				Server.NextFrame(() => controller.EquipKnife());
				Server.NextFrame(() =>
				{
					pawn.KeepWeaponsByType(CSWeaponType.WEAPONTYPE_KNIFE);

					controller.GiveNamedItem(CsItem.Deagle);
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
			Server.ExecuteCommand(JuanDeag.Disable);

			return true;
		}
	}
}
