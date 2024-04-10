using CounterStrikeSharp.API.Modules.Utils;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Constants
{
	public static class GladiatorBounds
	{
		public static readonly GladiatorMapBounds Mirage = new(
			"de_mirage",
			new (Vector, QAngle)[]
			{
				// CT -> A
				(new(-1038f, -2588f, -153f), new(0f, 0f, 0f)),
				(new(-1038f, -2448f, -153f), new(0f, 0f, 0f)),
				(new(-1038f, -2308f, -153f), new(0f, 0f, 0f)),
				// CT -> Window
				(new(-1335f, -1050f, -165f), new(0f, 0f, 0f)),
				// CT -> Market
				(new(-1544f, -735f, -166f), new(0f, 90f, 0f)),
			},
			(new(-1760f, -1350f, -153f), new(-1850f, -1620f, -153f)),
			(new(-1615f, -2120f, -250f), new(-1605f, -1260f, -250f))
		);

		public static readonly GladiatorMapBounds Overpass = new(
			"de_overpass",
			new (Vector, QAngle)[]
			{
				// Connector door
				(new(-1680f, -889f, 100f), new(0f, 90f, 0f)),
				// Water -> T
				(new(-1175f, -895f, 5f), new(0f, -180f, 0f)),
				// B Short
				(new(-1067f, -520f, 100f), new(0f, -180f, 0f)),
				// For spectators
				(new(-1184f, -1155f, 149f), new(0f, -180f, 0f)),
				(new(-1184f, -1155f, 279f), new(0f, -180f, 0f)),
			},
			(new(-1260f, -1150f, 162f), new(-1540f, -1245f, 162f)),
			(new(-1420f, -520f, 20f), new(-1420f, -1020f, 20f))
		);

		public static readonly GladiatorMapBounds Nuke = new(
			"de_nuke",
			new (Vector, QAngle)[]
			{
				//Ramp -> Control Room
				(new(279f, 16f, -412), new(0f, 0f, 0f)),
				//Control room -> Trophy
				(new(224f, -264f, -409f), new(0f, 90f, 0f))
			},
			(new(-100f, -200f, -401f), new(0f, 100f, -401f)),
			(new(160f, -190f, -401f), new(160f, 110f, -401f))
		);

		public static readonly GladiatorMapBounds Dust2 = new(
			"de_dust2",
			new (Vector, QAngle)[]
			{
				//CT Spawn -> A
				(new(525f, 2302f, -112f), new(0f, -180f, 0f)),
				(new(525f, 2166f, -112f), new(0f, -180f, 0f)),
				//CT Spawn -> Mid
				(new(-233f, 2218f, -118f), new(0f, -180f, 0f)),
				(new(-233f, 2095f, -118f), new(0f, -180f, 0f))
			},
			(new(210f, 2620f, -103f), new(370f, 2726f, -103f)),
			(new(460f, 2160f, -114f), new(70f, 2160f, -111f))
		);

		public static readonly GladiatorMapBounds Italy = new(
			"cs_italy",
			new (Vector, QAngle)[]
			{
				//Market -> Right Alley
				(new(116f, -510f, -154f), new(0f, 0f, 0f)),
				//Market -> Long alley
				(new(892f, -30f, -157f), new(0f, -90f, 0f)),
				//Market -> Tunnel
				(new(237f, -44f, -157f), new(0f, -90f, 0f))
			},
			(new(930f, -560f, 96f), new(1050f, -430f, 96f)),
			(new(225f, -670f, -135f), new(760f, -670f, -144f))
		);

		public static readonly GladiatorMapBounds Vertigo = new(
			"de_vertigo",
			new (Vector, QAngle)[]
			{
				//Fence 1
				(new(-1026f, -1319f, 11138f), new(0f, 0f, 0f)),
				(new(-1026f, -1208f, 11138f), new(0f, 0f, 0f))
			},
			(new(-870f, -1150f, 11150f), new(-980f, -1290f, 11150f)),
			(new(-1440f, -1280f, 11150f), new(-1111f, -1280f, 11150f))
		);

		public static readonly GladiatorMapBounds Inferno = new(
			"de_inferno",
			new (Vector, QAngle)[]
			{
				(new(1057f, 1250f, 97f), new(0f, 180f, 0f)),
				(new(1057f, 1107f, 97f), new(0f, 180f, 0f)),
				(new(1057f, 967f, 97f), new(0f, 180f, 0f)),
			},
			(new(1090f, 900f, 108f), new(1136f, 1236f, 108f)),
			(new(930f, 1340f, 108f), new(930f, 910f, 108f))
		);

		public static readonly Dictionary<string, GladiatorMapBounds> Maps =
			new(System.StringComparer.OrdinalIgnoreCase)
			{
				{ GladiatorBounds.Mirage.Map, GladiatorBounds.Mirage },
				{ GladiatorBounds.Overpass.Map, GladiatorBounds.Overpass },
				{ GladiatorBounds.Nuke.Map, GladiatorBounds.Nuke },
				{ GladiatorBounds.Dust2.Map, GladiatorBounds.Dust2 },
				{ GladiatorBounds.Italy.Map, GladiatorBounds.Italy },
				{ GladiatorBounds.Vertigo.Map, GladiatorBounds.Vertigo },
				{ GladiatorBounds.Inferno.Map, GladiatorBounds.Inferno },
			};
	}

	[SuppressMessage("Design", "MA0048")]
	public struct GladiatorMapBounds
	{
		public readonly string Map;
		public readonly (Vector pos, QAngle angle)[] Fences;
		public readonly (Vector min, Vector max) Spectators;
		public readonly (Vector ct, Vector t) Gladiators;

		public GladiatorMapBounds(string map,
								  (Vector, QAngle)[] fences,
								  (Vector min, Vector max) spectators,
								  (Vector ct, Vector t) gladiators)
		{
			this.Map = map;
			this.Fences = fences;
			this.Spectators = spectators;
			this.Gladiators = gladiators;
		}
	}
}
