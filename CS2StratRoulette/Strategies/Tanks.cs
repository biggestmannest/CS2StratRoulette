using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Extensions;

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

			const uint max = (uint)CsTeam.CounterTerrorist + 1;
			var added = new bool[max];

			foreach (var player in Utilities.GetPlayers())
			{
				if (player.IsValid || player.Team is not (CsTeam.Terrorist or CsTeam.CounterTerrorist))
				{
					continue;
				}

				var team = (uint)player.Team;

				if (added[team])
				{
					continue;
				}

				var shouldAdd = this.random.Next(0, 5) == 3;

				if (!shouldAdd)
				{
					continue;
				}

				added[team] = true;

				player.GiveNamedItem(CsItem.AssaultSuit);
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
