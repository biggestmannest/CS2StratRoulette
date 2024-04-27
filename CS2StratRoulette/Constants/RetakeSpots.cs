using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Modules.Utils;

namespace CS2StratRoulette.Constants
{
	public static class RetakeSpots
	{
		private static readonly AllRetakeSpots Mirage = new(
			"de_mirage",
			new Vector[]
			{
				new(-254f, -2134f, -175f),
				new(-555f, -2104f, -175f),
				new(-603f, -1984f, -175f),
				new(-323f, -2037f, -175f),
			},
			new Vector[]
			{
				new(-1978f, 434f, -159f),
				new(-2198f, 89f, -159f),
				new(-2001f, 159f, -159f),
				new(-1919f, 252f, -159f),
			},
			new Vector[]
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
			new Vector[]
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
			new Vector[]
			{
				new(-399f, -2121f, -166f),
				new(-766f, -1962f, -166f),
				new(108f, -1941f, -154f),
				new(-177f, -2341f, -154f),
				new(-3443f, -1521f, -154f),
			},
			new Vector[]
			{
				new(-2146f, 786f, -114),
				new(-2512f, 269f, -154f),
				new(-1633f, 612f, -154f),
				new(-2196f, 511f, -154f),
				new(-1623f, -189f, -153f),
			}
		);

		private static readonly AllRetakeSpots Overpass = new(
			"de_overpass",
			new Vector[]
			{
				new(-2519f, 658f, 485f),
				new(-2121f, 410f, 485f),
				new(-2026f, 574f, 485f),
				new(-2487f, 788f, 490f),
			},
			new Vector[]
			{
				new(1167f, -85f, 105f),
				new(-1252f, 5f, 105f),
				new(-973f, -29f, 105f),
				new(-987f, 151f, 105f),
			},
			new Vector[]
			{
				//Walkway -> Under A
				new(-1848f, 8f, 146f),
				//Heaven
				new(-1685f, 500f, 274f),
				//Connector
				new(-1899f, -1259f, 258f),
				//Fountain
				new(-2335f, -2135f, 490f),
				//Park
				new(-3671f, -1514f, 509f)
			},
			new Vector[]
			{
				//T Spawn -> Alley
				new(-734f, -1567f, 163f),
				//Tunnels -> Connector
				new(-1802f, -1648f, 114f),
				//Bank
				new(-2500f, -1304f, 498f),
				//Tunnels
				new(-1655f, -2004f, 354f),
				//Connector
				new(-2343f, -1327f, 450f),
			},
			new Vector[]
			{
				new(-2146f, 440f, 498f),
				new(-1852f, 632f, 490f),
				new(-2566f, 780f, 490f),
				new(-2224f, 1035f, 498f),
				new(-2335, 368f, 500f),
			},
			new Vector[]
			{
				new(-799f, 418f, 114f),
				new(-1387f, 281f, 34f),
				new(-727f, -97f, 57f),
				new(-1206f, 67f, 118),
				new(-1152f, -106f, 118f),
			}
		);

		private static readonly AllRetakeSpots Vertigo = new(
			"de_vertigo",
			new Vector[]
			{
				new(-347f, 683f, 11772f),
				new(-1144f, -540f, 11772f),
				new(-318f, -613f, 11772f),
				new(-432f, -587f, 11772f),
			},
			new Vector[]
			{
				new(-2216f, 869f, 11742f),
				new(-2146f, 653f, 11742f),
				new(-2348f, 739f, 11742f),
				new(-2327f, 903f, 11742f),
			},
			new Vector[]
			{
				//Mid
				new(-1808f, -51f, 11790f),
				//CT Spawn
				new(-818f, 764f, 11790f),
				//Tunnels
				new(1540f, -661f, 11502f),
				//Backdoor
				new(-678f, 432f, 11790f),
				//Connector
				new(-1709f, -485f, 11566f),
			},
			new Vector[]
			{
				// A window -> CT
				new(-710f, 315f, 11790f),
				// elevator
				new(-953f, -79f, 11790f),
				// mid
				new(-1843f, -130f, 11790f),
				// mid
				new(-1937f, 333f, 11790f),
				// CT
				new(-1067f, 549f, 11790f),
				// B tunnels
				new(-1890f, -285f, 11566f),
				// B tunnels
				new(-1942f, -438f, 11566f),
			},
			new Vector[]
			{
				new(-119f, -1431f, 11790f),
				new(-1101f, -736f, 11790f),
				new(-63f, -781f, 11790f),
				new(-735f, -715f, 11790f),
				new(-328f, -844f, 11790f),
			},
			new Vector[]
			{
				new(-2160f, 415f, 11790f),
				new(-2525f, 388f, 11758f),
				new(-2594f, 472f, 11762f),
				new(-2585f, 1065f, 11763f),
				new(-2111f, 1069f, 11763f),
				new(-1915f, 746f, 11790f),
				new(-2240f, 863f, 11758f),
			}
		);

		private static readonly AllRetakeSpots Ancient = new(
			"de_ancient",
			new Vector[]
			{
				new(-1280f, 794f, 55f),
				new(-1242f, 908f, 55f),
				new(-1453f, 862f, 55f),
				new(-1241f, 730f, 55f),
			},
			new Vector[]
			{
				new(756f, 18f, 137f),
				new(820f, 152f, 137f),
				new(869f, 83f, 137f),
				new(690f, -67f, 137f),
			},
			new Vector[]
			{
				//Main Hall
				new(-1814f, -624f, 86f),
				//Outside
				new(-1682f, -1056f, -7f),
				//Mid -> Donut
				new(-660f, -251f, 70f),
				//Donut
				new(-1317f, -353f, 117f),
				//House
				new(-192f, 774f, 92f),
			},
			new Vector[]
			{
				// short
				new(-140f, -750f, 167f),
				// mid
				new(-513f, -589f, 47f),
				// ct
				new(-201f, 871f, 91f),
				// ct
				new(-288f, 833f, 91f),
				// t
				new(821f, -1408f, 12f),
			},
			new Vector[]
			{
				new(-2087f, 1116f, 130f),
				new(-1763f, 992f, 66f),
				new(-1149f, 569f, 87f),
				new(-1782f, 422f, 187f),
				new(-1398f, 1122f, 111f),
			},
			new Vector[]
			{
				new(698f, -251f, 150f),
				new(800f, -18f, 144f),
				new(340f, 275f, 170f),
				new(963f, 351f, 140f),
				new(1042f, 60f, 144f),
			}
		);

		private static readonly AllRetakeSpots Inferno = new(
			"de_inferno",
			new Vector[]
			{
				new(2103f, 181f, 162f),
				new(1819f, 490f, 164f),
				new(1986f, 360f, 162f),
				new(2040f, 640f, 164f),
			},
			new Vector[]
			{
				new(463f, 2684f, 164f),
				new(268f, 2880f, 164f),
				new(544f, 2912f, 164f),
				new(148f, 2631f, 164f),
			},
			new Vector[]
			{
				//CT Spawn
				new(1979f, 1736f, 174f),
				//Arch
				new(1792f, 1561f, 174f),
				//Mid
				new(376f, 593f, 97f),
				//Second mid
				new(476f, -90f, 83f),
				//Aps
				new(1223f, -664f, 142f),
			},
			new Vector[]
			{
				// ct
				new(2377f, 2078f, 146f),
				// ct
				new(1635f, 2080f, 190f),
				// ct
				new(1932f, 2417f, 138f),
				// banana
				new(142f, 1370f, 131f),
				// banana
				new(-12f, 1371f, 138f),
				// banana
				new(367f, 1743f, 143f),
			},
			new Vector[]
			{
				new(1848f, 680f, 174f),
				new(1830f, 451f, 224f),
				new(2120f, 317f, 174f),
				new(1990f, 685f, 174f),
				new(1841f, 206f, 234f),
			},
			new Vector[]
			{
				new(33f, 2629f, 175f),
				new(314f, 2451f, 181f),
				new(39f, 3205f, 174f),
				new(39f, 2941f, 175f),
				new(471f, 2469f, 181f),
				new(758f, 2870f, 153f),
				new(527f, 3431f, 174f),
				new(719f, 3156f, 174f),
			}
		);

		private static readonly AllRetakeSpots Nuke = new(
			"de_nuke",
			new Vector[]
			{
				new(662f, -609f, -412f),
				new(755f, -770f, -391f),
				new(646f, -781f, -412f),
				new(560f, -698f, -391f),
			},
			new Vector[]
			{
				new(633f, -1256f, -766f),
				new(652f, -1050f, -766f),
				new(419f, -805f, -766f),
				new(838f, -860f, -766f)
			},
			new Vector[]
			{
				//Garage
				new(1608f, -1859f, -402),
				//Outside
				new(139f, -2073f, -402f),
				//Control Room
				new(172f, -31f, -402f),
				//Ramp
				new(446f, 439f, -429f),
				//T spawn
				new(-717f, -1053f, -402f),
			},
			new Vector[]
			{
				// ramp
				new(436f, 41f, -401f),
				// ramp
				new(832f, 88f, -401f),
				// secret
				new(1368f, -1934f, -625f),
				// secret
				new(1550f, -1928f, -625f),
				// vent
				new(526f, -1434f, -584f),
			},
			new Vector[]
			{
				new(442f, -345f, -402f),
				new(720f, -567f, -387f),
				new(394f, -783f, -378f),
				new(910f, -1437f, -402f),
				new(375f, -1112f, -378f),
			},
			new Vector[]
			{
				new(916f, -1316f, -753f),
				new(830f, -368f, -753f),
				new(386f, -591f, -753f),
				new(1407f, -991f, -753f),
				new(648f, -1272f, -755f),
				new(643f, -1302f, -635f),
				new(1266f, -477f, -635f),
				new(638f, -166f, -635f),
			}
		);

		private static readonly AllRetakeSpots Anubis = new(
			"de_anubis",
			new Vector[]
			{
				new(1156f, 1817f, -187f),
				new(1512f, 2039f, -187f),
				new(964f, 1740f, -187f),
				new(1312f, 2150f, -187f),
			},
			new Vector[]
			{
				new(-1136f, 700f, -1f),
				new(-960f, 646f, 1f),
				new(-906f, 560f, 1f),
				new(-1173f, 566f, 1f),
			},
			new Vector[]
			{
				//Canal
				new(1145f, 370f, -138f),
				//Canal
				new(575f, 45f, -138f),
				//Mid
				new(-88f, 763f, 14f),
				//Mid Doors
				new(286f, 474f, -16f),
				//T Side Upper
				new(1394f, -163f, -18f),
			},
			new Vector[]
			{
				// temple
				new(-212f, 1564f, 41f),
				// temple
				new(-453f, 1294f, 166f),
				// ct
				new(-725f, 2060f, 30f),
				// ct tunnel
				new(-860f, 1530f, -17f),
				// a main
				new(-1308f, -423f, 110f),
				// a main
				new(-1100f, -51f, 14f),
				// canal
				new(-254f, 102f, -137f),
				// canal
				new(660f, 64f, -137f),
			},
			new Vector[]
			{
				new(953f, 2339f, -26f),
				new(887f, 1620f, -60f),
				new(1204f, 2039f, -178f),
				new(1058f, 1991f, -178f),
				new(1036f, 1768f, -178f),
				new(1163f, 1492f, -138f),
			},
			new Vector[]
			{
				new(-1126f, 284f, -9f),
				new(-1546f, 716f, 52f),
				new(-643f, 540f, 52f),
				new(-643f, 908f, 52f),
				new(-1059f, 570f, 10f),
				new(-1059f, 570f, 10f),
			}
		);

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
				{ RetakeSpots.Dust2.Map, RetakeSpots.Dust2 },
				{ RetakeSpots.Mirage.Map, RetakeSpots.Mirage },
				{ RetakeSpots.Overpass.Map, RetakeSpots.Overpass },
				{ RetakeSpots.Vertigo.Map, RetakeSpots.Vertigo },
				{ RetakeSpots.Anubis.Map, RetakeSpots.Anubis },
				{ RetakeSpots.Ancient.Map, RetakeSpots.Ancient },
				{ RetakeSpots.Nuke.Map, RetakeSpots.Nuke },
				{ RetakeSpots.Inferno.Map, RetakeSpots.Inferno }
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
