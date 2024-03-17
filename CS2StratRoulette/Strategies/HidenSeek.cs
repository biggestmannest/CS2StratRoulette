using System.Collections.Generic;
using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class HidenSeek : Strategy
	{
		public override string Name =>
			"Hide & Seek";

		public override string Description =>
			"Ts must kill the CTs with their knives. CTs only have 1 person with a knife.";

		private readonly System.Random random = new();

		private CCSPlayerController? ctKnife;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			var cts = new List<CCSPlayerController>(10);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn) || controller.IsBot)
				{
					continue;
				}

				if (controller.Team is CsTeam.CounterTerrorist)
				{
					cts.Add(controller);
					controller.RemoveWeapons();
				}
				else if (controller.Team is CsTeam.Terrorist)
				{
					pawn.KeepWeaponsByType(CSWeaponType.WEAPONTYPE_KNIFE);
				}
			}

			if (cts.Count > 0)
			{
				this.ctKnife = HidenSeek.GiveKnife(cts[this.random.Next(cts.Count)]);
			}

			{
				return true;
			}
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			return true;
		}

		private static CCSPlayerController? GiveKnife(CCSPlayerController controller)
		{
			if (!controller.TryGetPlayerPawn(out var pawn))
			{
				return null;
			}

			Server.NextFrame(() =>
			{
				pawn.KeepWeaponsByType(
					CSWeaponType.WEAPONTYPE_KNIFE
				);
			});

			return controller;
		}
	}
}
