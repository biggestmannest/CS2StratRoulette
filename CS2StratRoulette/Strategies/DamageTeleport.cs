using System.Collections.Generic;
using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class DamageTeleport : Strategy
	{
		public override string Name =>
			"aghhhh!!! dont shoot!!!!!";

		public override string Description =>
			"You teleport to a random place when you are shot.";

		private static readonly Dictionary<string, Vector[]> Maps = new(System.StringComparer.OrdinalIgnoreCase)
		{
			{ "de_mirage", RandomTPs.Mirage },
			{ "de_overpass", RandomTPs.Overpass },
			{ "de_nuke", RandomTPs.Nuke },
			{ "de_dust2", RandomTPs.Dust2 },
			{ "cs_italy", RandomTPs.Italy },
			{ "de_vertigo", RandomTPs.Vertigo },
		};

		private static readonly System.Random Random = new();

		public override bool Start(ref Base plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			plugin.RegisterEventHandler<EventPlayerHurt>(this.OnHurt);

			return true;
		}

		public override bool Stop(ref Base plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			const string playerHurt = "player_hurt";

			plugin.DeregisterEventHandler(playerHurt, this.OnHurt, false);

			return true;
		}

		private HookResult OnHurt(EventPlayerHurt @event, GameEventInfo _)
		{
			var serverMap = Server.MapName;

			if (!DamageTeleport.Maps.TryGetValue(serverMap, out var spots))
			{
				return HookResult.Continue;
			}

			DamageTeleport.Random.Shuffle(spots);

			var player = @event.Userid;

			var n = spots.Length;

			if (!player.TryGetPlayerPawn(out var pawn))
			{
				return HookResult.Continue;
			}

			if (n <= 0)
			{
				n = spots.Length;
			}

			var position = spots[--n];
			var angle = pawn.V_angle;

			Server.NextFrame(() =>
			{
				pawn.Teleport(
					position,
					angle,
					VectorExtensions.Zero
				);
			});

			return HookResult.Continue;
		}
	}
}
