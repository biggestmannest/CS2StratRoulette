using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Modules.Utils;

namespace CS2StratRoulette.Constants
{
	public static class RetakeSpots
	{
		private static readonly AllRetakeSpots Mirage = new(
			"de_mirage",
			new Vector[] //Bomb A
			{
				new(-254f, -2134f, -175f),
				new(-555f, -2104f, -175f),
				new(-603f, -1984f, -175f),
				new(-323f, -2037f, -175f),
			},
			new Vector[] //Bomb B
			{
				new(-1978f, 434f, -159f),
				new(-2198f, 89f, -159f),
				new(-2001f, 159f, -159f),
				new(-1919f, 252f, -159f),
			},
			new Vector[] //CTsA
			{
				//Ramp
				new(804f, -1081f, -248f),
				//Palace
				new(1206f, -1425f, -154f),
				//Underpass
				new(-1012f, -83f, -354f),
				//Top Mid
				new(150f, -641f, -156f),
				//Market
				new(-1586f, -639f, -154),
			},
			new Vector[] //CTsB
			{
				//Short
				new(-298f, -436f, -153f),
				//Back Alley -> Aps
				new(-450f, 566f, -66f),
				//Underpass
				new(-976f, 417f, -354f),
				//CT
				new(-1193f, -2305f, -176f),
				//Jungle
				new(-1163f, -1367f, -153f),
			},
			new Vector[] //TsA
			{
				new(-399f, -2121f, -166f),
				new(-766f, -1962f, -166f),
				new(108f, -1941f, -154f),
				new(-177f, -2341f, -154f),
				new(-3443f, -1521f, -154f),
			},
			new Vector[] //TsB
			{
				new(-2146f, 786f, -114),
				new(-2512f, 269f, -154f),
				new(-1633f, 612f, -154f),
				new(-2196f, 511f, -154f),
				new(-1623f, -189f, -153f),
			}
		);

		public static readonly Vector[] OverpassA =
		{
			new(-2519f, 658f, 485f),
			new(-2121f, 410f, 485f),
			new(-2026f, 574f, 485f),
			new(-2487f, 788f, 584f),
		};

		public static readonly Vector[] OverpassB =
		{
			new(1167f, -85f, 105f),
			new(-1252f, 5f, 105f),
			new(-973f, -29f, 105f),
			new(-987f, 151f, 105f),
		};

		public static readonly Vector[] VertigoA =
		{
			new(-347f, 683f, 11772f),
			new(-1144f, -540f, 11772f),
			new(-318f, -613f, 11772f),
			new(-432f, -587f, 11772f),
		};

		public static readonly Vector[] VertigoB =
		{
			new(-2216f, 869f, 11742f),
			new(-2146f, 653f, 11742f),
			new(-2348f, 739f, 11742f),
			new(-2327f, 903f, 11742f),
		};

		public static readonly Vector[] AncientA =
		{
			new(-1280f, 794f, 55f),
			new(-1242f, 908f, 55f),
			new(1453f, 862f, 55f),
			new(-1241f, 730f, 55f),
		};

		public static readonly Vector[] AncientB =
		{
			new(756f, 18f, 137f),
			new(820f, 152f, 137f),
			new(869f, 83f, 137f),
			new(690f, -67f, 137f),
		};

		public static readonly Vector[] InfernoA =
		{
			new(2103f, 181f, 162f),
			new(1819f, 490f, 164f),
			new(1986f, 360f, 162f),
			new(2040f, 640f, 164f),
		};

		public static readonly Vector[] InfernoB =
		{
			new(463f, 2684f, 164f),
			new(268f, 2880f, 164f),
			new(544f, 2912f, 164f),
			new(148f, 2631f, 164f),
		};

		public static readonly Vector[] NukeA =
		{
			new(662f, -609f, -412f),
			new(755f, -770f, -391f),
			new(646f, -781f, -412f),
			new(560f, -698f, -391f),
		};

		public static readonly Vector[] NukeB =
		{
			new(633f, -1256f, -766f),
			new(652f, -1050f, -766f),
			new(419f, -805f, -766f),
			new(838f, -860f, -766f)
		};

		public static readonly Vector[] AnubisA =
		{
			new(1156f, 1817f, 187f),
			new(1512f, 2039f, 187f),
			new(964f, 1740f, 187f),
			new(1312f, 2150f, 187f),
		};

		public static readonly Vector[] AnubisB =
		{
			new(-1136f, 700f, -1f),
			new(-960f, 646f, 1f),
			new(-906f, 560f, 1f),
			new(-1173f, 566f, 1f),
		};

		private static readonly AllRetakeSpots Dust2 = new(
			"de_dust2",
			new Vector[]
			{
				new(1236f, 2461f, 98f),
				new(1235f, 2561f, 98f),
				new(987f, 2444f, 98f),
				new(1125f, 2509f, 98f),
			},
			new Vector[]
			{
				new(-1430f, 2676f, 17f),
				new(-1684f, 2549f, 8f),
				new(-1584f, 2753f, 65f),
				new(-1366f, 2566f, 8f),
			},
			new Vector[]
			{
				//Short
				new(-137f, 1407f, 1f),
				//Long		
				new(697f, 894f, 3f),
				//B Window
				new(-1166f, 2652f, 86f),
				//Lower Tunnel
				new(-695f, 1414f, -99f),
				//Suicide -> Mid
				new(-395f, 1016, -50f),
			},
			new Vector[]
			{
				//CT Spawn
				new(471f, 2485f, -105f),
				//Lower Tunnel
				new(-695f, 1414f, -99f),
				//Upper Tunnel
				new(-1595f, 1143f, 46f),
				//Mid
				new(-388f, 1548f, -112f),
				//Outside Tunnel
				new(246f, 2417f, -107f),
			},
			new Vector[]
			{
				new(1064f, 3050f, 134f),
				new(945f, 2667f, 98f),
				new(1470f, 2994f, 124f),
				new(1210f, 2899f, 134f),
				new(814f, 2446f, 98f),
			},
			new Vector[]
			{
				new(-2079f, 2964f, 36f),
				new(-1805f, 2546f, 35f),
				new(-2175f, 2276f, 16f),
				new(-1672f, 1732f, 3f),
				new(-1366f, 2410f, 5f),
			}
		);

		public static readonly Dictionary<string, AllRetakeSpots> Maps =
			new(System.StringComparer.OrdinalIgnoreCase)
			{
				{ RetakeSpots.Dust2.Map, RetakeSpots.Dust2 }
			};

		[SuppressMessage("Design", "MA0048")]
		public struct AllRetakeSpots
		{
			public readonly string Map;
			public readonly Vector[] BombA;
			public readonly Vector[] BombB;
			public readonly Vector[] CtsA;
			public readonly Vector[] CtsB;
			public readonly Vector[] TsA;
			public readonly Vector[] TsB;

			// ReSharper disable once ConvertToPrimaryConstructor
			public AllRetakeSpots(
				string map,
				Vector[] bombA,
				Vector[] bombB,
				Vector[] ctsA,
				Vector[] ctsB,
				Vector[] tsA,
				Vector[] tsB
			)
			{
				this.Map = map;
				this.BombA = bombA;
				this.BombB = bombB;
				this.CtsA = ctsA;
				this.CtsB = ctsB;
				this.TsA = tsA;
				this.TsB = tsB;
			}
		}
	}
}
