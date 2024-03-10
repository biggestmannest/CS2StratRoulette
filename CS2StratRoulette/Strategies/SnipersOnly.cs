using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Core;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public class SnipersOnly : Strategy
	{
		private static readonly string Enable = $"mp_buy_allow_guns {BuyAllow.Rifles.Str()}";
		private static readonly string Disable = $"mp_buy_allow_guns {BuyAllow.All.Str()}";

		public override string Name =>
			"Snipers Only";

		public override string Description =>
			"You're only allowed to buy snipers.";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(SnipersOnly.Enable);

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
			}

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(SnipersOnly.Disable);

			return true;
		}
	}
}
