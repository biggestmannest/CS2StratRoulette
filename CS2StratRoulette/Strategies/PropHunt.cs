using CS2StratRoulette.Constants;
using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class PropHunt : Strategy
	{
		private const byte Tries = 4;
		private const byte DamageOnTry = (100 /*HP*/ / PropHunt.Tries);

		public override string Name =>
			"Prop Hunt";

		public override string Description =>
			"garry simon's modifications.";

		private readonly System.Random random = new();

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Commands.BuyAllowNone);
			Server.ExecuteCommand(Commands.BuyAllowGrenadesDisable);

			Server.ExecuteCommand(Commands.CheatsEnable);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				if (controller.Team is CsTeam.Terrorist)
				{
					pawn.KeepWeaponsByType(CSWeaponType.WEAPONTYPE_KNIFE);

					controller.GiveNamedItem(CsItem.HEGrenade);
					controller.GiveNamedItem(CsItem.Molotov);

					continue;
				}

				if (controller.Team is not CsTeam.CounterTerrorist)
				{
					continue;
				}

				controller.RemoveWeapons();
				pawn.Health = 1;

				controller.ExecuteClientCommandFromServer(Commands.ThirdPersonEnable);

				Server.NextFrame(() =>
				{
					pawn.SetModel(Models.Props[this.random.Next(Models.Props.Length)]);
					Utilities.SetStateChanged(controller, "CBaseEntity", "m_iHealth");
				});
			}

			Server.ExecuteCommand(Commands.CheatsDisable);

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

			Server.ExecuteCommand(Commands.CheatsEnable);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.IsValid)
				{
					continue;
				}

				controller.RemoveWeapons();

				controller.ExecuteClientCommandFromServer(Commands.ThirdPersonDisable);
			}

			Server.ExecuteCommand(Commands.CheatsDisable);

			return true;
		}
	}
}
