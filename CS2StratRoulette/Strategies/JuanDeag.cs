using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class JuanDeag : Strategy
	{
		public override string Name =>
			"Juan Deag";

		public override string Description =>
			"Everyone gets a deagle, and can only hit headshots.";

		private const string Enable = "mp_damage_headshot_only 1";
		private const string Disable = "mp_damage_headshot_only 0";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Commands.BuyAllowNone);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn) || controller.IsBot)
				{
					continue;
				}

				pawn.KeepWeaponsByType(CSWeaponType.WEAPONTYPE_KNIFE);
				controller.GiveNamedItem("weapon_deagle");
			}

			Server.ExecuteCommand(JuanDeag.Enable);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Commands.BuyAllowAll);

			Server.ExecuteCommand(JuanDeag.Disable);

			return true;
		}
	}
}
