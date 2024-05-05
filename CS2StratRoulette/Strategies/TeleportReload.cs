﻿using System.Collections.Frozen;
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
	public sealed class TeleportReload : Strategy
	{
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
			"Hold on, gotta reload";

		public override string Description =>
			"You teleport to a random place when you reload.";

		private Vector[]? positions;
		private uint index;

		public override bool CanRun()
		{
			return TeleportReload.Maps.ContainsKey(Server.MapName);
		}

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			var serverMap = Server.MapName;

			if (!TeleportReload.Maps.TryGetValue(serverMap, out this.positions))
			{
				return false;
			}

			TeleportReload.Random.Shuffle(this.positions);

			plugin.RegisterEventHandler<EventWeaponReload>(this.OnReload);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			plugin.DeregisterEventHandler<EventWeaponReload>(this.OnReload);

			return true;
		}

		private HookResult OnReload(EventWeaponReload @event, GameEventInfo _)
		{
			if (this.positions is null)
			{
				return HookResult.Continue;
			}

			var player = @event.Userid;

			if (player is null || !player.TryGetPlayerPawn(out var pawn))
			{
				return HookResult.Continue;
			}

			if (this.index >= this.positions.Length)
			{
				TeleportReload.Random.Shuffle(this.positions);
				this.index = 0;
			}

			var position = this.positions[this.index++];
			var angle = pawn.V_angle;

			Server.NextFrame(() =>
			{
				pawn.Teleport(
					position,
					angle,
					Vector.Zero
				);
			});

			return HookResult.Continue;
		}
	}
}
