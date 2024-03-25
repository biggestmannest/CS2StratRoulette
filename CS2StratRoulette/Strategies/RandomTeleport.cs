using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class RandomTeleport : Strategy
	{
		public override string Name =>
			"Where the fuck am I????????";

		public override string Description =>
			"Hope you like your new position :)";

		private static readonly Dictionary<string, Vector[]> Maps = new()
		{
			{ "de_mirage", RandomTPs.Mirage },
			{ "de_overpass", RandomTPs.Overpass },
			{ "de_nuke", RandomTPs.Nuke },
			{ "de_dust2", RandomTPs.Dust2 },
			{ "cs_italy", RandomTPs.Italy },
			{ "de_vertigo", RandomTPs.Vertigo },
		};

		private const string BuyAnywhereEnable = "mp_buy_anywhere 1";
		private const string BuyAnywhereDisable = "mp_buy_anywhere 0";

		private static readonly System.Random Random = new();

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

			foreach (var player in Utilities.GetPlayers())
			{
				if (!player.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				var angle = pawn.AbsRotation;
				Server.NextFrame(() =>
				{
					if (angle is not null)
					{
						pawn.Teleport(
							randomSpots[RandomTeleport.Random.Next(randomSpots.Length)],
							angle,
							VectorExtensions.Zero
						);
					}
				});
			}

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
