using CS2StratRoulette.Constants;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class RandomWeapon : Strategy
	{
		public override string Name =>
			"Random weapon";

		public override string Description =>
			"I hope you like your new weapon :)";

		private readonly System.Random random = new();

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
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				pawn.KeepWeaponsByType(
					CSWeaponType.WEAPONTYPE_KNIFE,
					CSWeaponType.WEAPONTYPE_C4,
					CSWeaponType.WEAPONTYPE_EQUIPMENT
				);

				var item = Items.Weapons[this.random.Next(Items.Weapons.Length)];

				controller.GiveNamedItem(item);
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
