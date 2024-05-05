using System.Collections.Frozen;
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
		private static readonly System.Random Random = new();

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

		public override string Name =>
			"aghhhh!!! dont shoot!!!!!";

		public override string Description =>
			"You teleport to a random place when you are shot.";

		private Vector[]? positions;
		private uint index;

		public override bool CanRun()
		{
			return DamageTeleport.Maps.ContainsKey(Server.MapName);
		}

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			var serverMap = Server.MapName;

			if (!DamageTeleport.Maps.TryGetValue(serverMap, out this.positions))
			{
				return false;
			}

			DamageTeleport.Random.Shuffle(this.positions);

			plugin.RegisterEventHandler<EventPlayerHurt>(this.OnHurt);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			plugin.DeregisterEventHandler<EventPlayerHurt>(this.OnHurt);

			return true;
		}

		private HookResult OnHurt(EventPlayerHurt @event, GameEventInfo _)
		{
			var controller = @event.Userid;

			if (controller is null || !controller.TryGetPlayerPawn(out var pawn) || this.positions is null)
			{
				return HookResult.Continue;
			}

			if (this.index >= this.positions.Length)
			{
				this.index = 0;
			}

			var position = this.positions[this.index++];

			Server.NextFrame(() =>
			{
				pawn.Teleport(
					position,
					pawn.V_angle,
					Vector.Zero
				);
			});

			return HookResult.Continue;
		}
	}
}
