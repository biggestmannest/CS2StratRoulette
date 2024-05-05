using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Constants;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class SmokeStrat : Strategy
	{
		private const float Interval = 20f;

		private static readonly FrozenDictionary<string, Vector[]> Maps =
			new Dictionary<string, Vector[]>(System.StringComparer.OrdinalIgnoreCase)
			{
				{ "de_dust2", SmokeSpots.Dust2 },
				{ "de_mirage", SmokeSpots.Mirage },
				{ "de_nuke", SmokeSpots.Nuke },
				{ "de_vertigo", SmokeSpots.Vertigo },
				{ "de_inferno", SmokeSpots.Inferno },
				{ "de_overpass", SmokeSpots.Overpass },
				{ "cs_italy", SmokeSpots.Italy },
			}.ToFrozenDictionary();

		public override string Name =>
			"Smoke Strat";

		public override string Description =>
			"was it a jump throw or a run throw?? or a normal throw... whats the lineup again???";

		private Timer? timer;

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			var serverMap = Server.MapName;

			if (!SmokeStrat.Maps.TryGetValue(serverMap, out var positions))
			{
				return false;
			}

			SmokeStrat.OnInterval(positions);

			this.timer = new Timer(SmokeStrat.Interval, () => { SmokeStrat.OnInterval(positions); }, TimerFlags.REPEAT);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			this.timer?.Kill();

			return true;
		}

		private static void OnInterval(Vector[] positions)
		{
			var velocity = Vector.Zero;
			var angle = QAngle.Zero;

			foreach (var position in positions)
			{
				Signatures.CreateSmoke.Invoke(
					position.Handle,
					angle.Handle,
					velocity.Handle,
					velocity.Handle,
					System.IntPtr.Zero,
					45,
					2
				);
			}
		}
	}
}
