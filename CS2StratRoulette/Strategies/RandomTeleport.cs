using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class RandomTeleport : Strategy
	{
		private const string BuyAnywhereEnable = "mp_buy_anywhere 1";
		private const string BuyAnywhereDisable = "mp_buy_anywhere 0";

		private static readonly FrozenDictionary<string, Vector[]> Maps =
			new Dictionary<string, Vector[]>(System.StringComparer.OrdinalIgnoreCase)
			{
				{ "de_mirage", RandomTPs.Mirage },
				{ "de_overpass", RandomTPs.Overpass },
				{ "de_nuke", RandomTPs.Nuke },
				{ "de_dust2", RandomTPs.Dust2 },
				{ "de_vertigo", RandomTPs.Vertigo },
				{ "de_inferno", RandomTPs.Inferno },
				{ "cs_italy", RandomTPs.Italy },
			}.ToFrozenDictionary();

		private static readonly System.Random Random = new();

		public override string Name =>
			"Where the fuck am I????????";

		public override string Description =>
			"Hope you like your new position :)";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		public override bool CanRun()
		{
			return RandomTeleport.Maps.ContainsKey(Server.MapName);
		}

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(RandomTeleport.BuyAnywhereEnable);

			var serverMap = Server.MapName;

			if (!RandomTeleport.Maps.TryGetValue(serverMap, out var randomSpots))
			{
				return false;
			}

			RandomTeleport.Random.Shuffle(randomSpots);

			var n = randomSpots.Length;

			Player.ForEach((controller) =>
			{
				if (!controller.TryGetPlayerPawn(out var pawn) || pawn.AbsRotation is null)
				{
					return;
				}

				if (n <= 0)
				{
					n = randomSpots.Length;
				}

				var position = randomSpots[--n];

				Server.NextFrame(() =>
				{
					pawn.Teleport(
						position,
						pawn.V_angle,
						Vector.Zero
					);
				});
			});

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(RandomTeleport.BuyAnywhereDisable);

			return true;
		}
	}
}
