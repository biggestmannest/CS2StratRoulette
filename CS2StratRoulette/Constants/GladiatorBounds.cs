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

		public static readonly Dictionary<string, GladiatorMapBounds> Maps =
			new(System.StringComparer.OrdinalIgnoreCase)
			{
				{ GladiatorBounds.Mirage.Map, GladiatorBounds.Mirage },
				{ GladiatorBounds.Overpass.Map, GladiatorBounds.Overpass },
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
