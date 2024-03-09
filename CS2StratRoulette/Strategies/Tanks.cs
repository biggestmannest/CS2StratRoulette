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

			foreach (var player in Utilities.GetPlayers())
			{
				if (player.IsValid)
				{
					continue;
				}

				if (player.Team is CsTeam.Terrorist)
				{
					ts.Add(player);
				}
				else if (player.Team is CsTeam.CounterTerrorist)
				{
					cts.Add(player);
				}
			}

			var ct = cts[this.random.Next(0, cts.Count)];
			var t = ts[this.random.Next(0, cts.Count)];

			if (ct.IsValid)
			{
				ct.GiveNamedItem(CsItem.AssaultSuit);
			}

			if (t.IsValid)
			{
				t.GiveNamedItem(CsItem.AssaultSuit);
			}

			return true;
		}

		// TODO: remove assault suit from players or just kill them if we can't remove it
		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Tanks.DisableHeavyAssaultSuite);

			foreach (var player in Utilities.GetPlayers())
			{
				if (player.IsValid || !player.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				if (pawn.ItemServices is null)
				{
					continue;
				}

				var itemServices = new CCSPlayer_ItemServices(pawn.ItemServices.Handle);

				System.Console.WriteLine($"[Tanks]: {player.Globalname}: {itemServices.HasHeavyArmor}");
			}

			return true;
		}
	}
}
