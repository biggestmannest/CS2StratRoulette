using CS2StratRoulette.Constants;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Modules.Entities.Constants;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class TasersOnly : Strategy
	{
		private const string InfiniteTasersEnable = "mp_taser_recharge_time 0";

		private const string InfiniteTasersDisable = "mp_taser_recharge_time 30";

		private const string PartyModeEnable = "sv_party_mode true";

		private const string PartyModeDisable = "sv_party_mode false";

		public override string Name =>
			"TASER TASER TASER";

		public override string Description =>
			"You can only use tasers, with instant recharge time.";

		public override StrategyFlags Flags { get; protected set; } = StrategyFlags.Hidden;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Commands.BuyAllowNone);
			Server.ExecuteCommand(TasersOnly.InfiniteTasersEnable);
			Server.ExecuteCommand(TasersOnly.PartyModeEnable);

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

				controller.GiveNamedItem(CsItem.Taser);
				controller.EquipKnife();
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
			Server.ExecuteCommand(TasersOnly.InfiniteTasersDisable);
			Server.ExecuteCommand(TasersOnly.PartyModeDisable);

			return true;
		}
	}
}
