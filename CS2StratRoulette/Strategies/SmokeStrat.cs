using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Constants;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class SmokeStrat : Strategy
	{
		public override string Name =>
			"Smoke Strat";

		public override string Description =>
			"was it a jump throw or a run throw?? or a normal throw... whats the lineup again???";

		private static readonly Dictionary<string, Vector[]> Maps = new(System.StringComparer.OrdinalIgnoreCase)
		{
			{ "de_dust2", SmokeSpots.Dust2 },
			{ "de_mirage", SmokeSpots.Mirage },
			{ "de_nuke", SmokeSpots.Nuke },
			{ "de_vertigo", SmokeSpots.Vertigo },
			{ "cs_italy", SmokeSpots.Italy }
		};
		
		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			var serverMap = Server.MapName;

			if (!SmokeStrat.Maps.TryGetValue(serverMap, out var spots))
			{
				return false;
			}

			Vector velocity = new(0f, 0f, 0f);
			QAngle angle = new(0f, 0f, 0f);
            
			foreach (var position in spots)
			{
				Signatures.CreateSmoke.Invoke(
					position.Handle,
					angle.Handle,
					velocity.Handle,
					velocity.Handle,
					IntPtr.Zero,
					45,
					2
				);
			}
			
			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			return true;
		}
	}
}
