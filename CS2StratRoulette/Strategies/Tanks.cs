using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Tanks : Strategy
	{
		private const string EnableHeavyAssaultSuite = "mp_weapons_allow_heavyassaultsuit 1";
		private const string DisableHeavyAssaultSuite = "mp_weapons_allow_heavyassaultsuit 0";

		public override string Name =>
			"Tanks";

		public override string Description =>
			"Every team gets a tank.";

		private readonly System.Random random = new();

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Tanks.EnableHeavyAssaultSuite);

			var cts = new List<CCSPlayerController>(10);
			var ts = new List<CCSPlayerController>(10);

			// ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
			foreach (var player in Utilities.GetPlayers())
			{
				if (!player.IsValid)
				{
					continue;
				}

				// ReSharper disable once ConvertIfStatementToSwitchStatement
				if (player.Team is CsTeam.CounterTerrorist)
				{
					cts.Add(player);
				}
				else if (player.Team is CsTeam.Terrorist)
				{
					ts.Add(player);
				}
			}

			if (cts.Count > 0)
			{
				Tanks.MakeTank(cts[this.random.Next(cts.Count)]);
			}

			// ReSharper disable once InvertIf
			if (ts.Count > 0)
			{
				Tanks.MakeTank(ts[this.random.Next(ts.Count)]);
			}

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Tanks.DisableHeavyAssaultSuite);

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				pawn.ArmorValue = 0;

				Utilities.SetStateChanged(controller, "CCSPlayerPawnBase", "m_ArmorValue");
			}

			return true;
		}

		private static void MakeTank(CCSPlayerController controller)
		{
			if (!controller.TryGetPlayerPawn(out var pawn))
			{
				return;
			}

			controller.GiveNamedItem(CsItem.AssaultSuit);

			pawn.ArmorValue = 999;

			Utilities.SetStateChanged(controller, "CCSPlayerPawnBase", "m_ArmorValue");
		}
	}
}
