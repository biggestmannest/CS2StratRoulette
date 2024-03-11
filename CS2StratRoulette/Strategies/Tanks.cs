using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Enums;

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

		public override StrategyFlags Flags { get; protected set; } = StrategyFlags.Hidden;

		private readonly System.Random random = new();

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.PrecacheModel(Models.JuggernautCt);
			Server.PrecacheModel(Models.JuggernautT);

			Server.ExecuteCommand(Tanks.EnableHeavyAssaultSuite);

			var cts = new List<CCSPlayerController>(10);
			var ts = new List<CCSPlayerController>(10);

			// ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.IsValid)
				{
					continue;
				}

				// ReSharper disable once ConvertIfStatementToSwitchStatement
				if (controller.Team is CsTeam.CounterTerrorist)
				{
					cts.Add(controller);
				}
				else if (controller.Team is CsTeam.Terrorist)
				{
					ts.Add(controller);
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

				Server.NextFrame(() =>
				{
					pawn.MaxHealth = 100;
					pawn.Health = 100;
					pawn.ArmorValue = 0;
				});

				Server.NextFrame(() =>
				{
					Utilities.SetStateChanged(controller, "CBaseEntity", "m_iMaxHealth");
					Utilities.SetStateChanged(controller, "CBaseEntity", "m_iHealth");
					Utilities.SetStateChanged(controller, "CCSPlayerPawnBase", "m_ArmorValue");
				});
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

			Server.NextFrame(() =>
			{
				pawn.MaxHealth = 999;
				pawn.Health = 999;
				pawn.ArmorValue = 999;
			});

			Server.NextFrame(() =>
			{
				Utilities.SetStateChanged(controller, "CBaseEntity", "m_iMaxHealth");
				Utilities.SetStateChanged(controller, "CBaseEntity", "m_iHealth");
				Utilities.SetStateChanged(controller, "CCSPlayerPawnBase", "m_ArmorValue");
			});

			Server.NextFrame(() =>
			{
				pawn.SetModel(controller.Team is CsTeam.CounterTerrorist
					? Models.JuggernautCt
					: Models.JuggernautT);
			});
		}
	}
}
