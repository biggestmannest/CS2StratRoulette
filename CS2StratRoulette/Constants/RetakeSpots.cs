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
				//Kitchen
				new(-1654f, -693f, -154f),
				//Kitchen
				new(-2149f, -570f, -154f),
				//Palace
				new(1341f, -1312f, -154f),
				//Palace
				new(1316f, -1068f, -154f),
				//T Spawn
				new(1221f, -425f, -152f),
				//Short
				new(204f, -134f, -153f),
				//Top mid
				new(420f, -838f, -151f)
			},
			new Vector[]
			{
				// house
				new Vector(20f, 805f, -121f),
				// back alley
				new Vector(-254f, 815f, -123f),
				// tunnel
				new Vector(-978f, 420f, -353f),
				// tunnel
				new Vector(-1000f, 148f, -353f),
				// short
				new Vector(-780f, -400f, -154f),
				// short
				new Vector(-464f, -410f, -153f),
				// window
				new Vector(-1180f, -1008f, -153f),
				// window -> market
				new Vector(-1594f, -808f, -153f),
				// ladder room
				new Vector(-1170f, -294f, -42f),
				// B apps
				new Vector(-1076f, 418f, -65f),
			},
			new Vector[]
			{
				new(-235f, -2355f, -154f),
				new(-325f, -1546f, -154f),
				new(125f, -1928f, -154f),
				new(-5f, -2157f, -154f),
				new(-406f, -2370f, -154f),
				new(-129f, -1433f, -58f),
				new(-222f, -1552f, -40f),
				new(-437f, -1550f, -26f),
			},
			new Vector[]
			{
				new(-1920f, 400f, -75f),
				new(-2180f, 100f, -145f),
				new(-2528f, 290f, -153f),
				new(-2620f, 116f, -150f),
				new(-2300f, 788f, -110f),
				new(-1958f, 833f, -10f),
				new(-1730f, 705f, -34f),
				new(-1285f, 488f, -153f),
				new(-1064f, 196f, -158f),
				new(-1597f, -196f, -151f),
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
				//connector
				new(-2080f, -891f, 146f),
				//connector
				new(-1711f, -1055f, 114f),
				//long
				new(-3592f, -2002f, 495f),
				//long
				new(-3837f, -1805f, 505f),
				//toilet
				new(-3325f, -924f, 506f),
				//under A
				new(-2173f, 256f, 146f),
				//under A
				new(-1871f, 572f, 122f),
				//heaven
				new(-1832f, 546f, 274f),
				//heaven
				new(-2159f, 923f, 274f),
			},
			new Vector[]
			{
				// connector
				new(-1780f, -1077f, 114f),
				// connector
				new(-2012f, -975f, 146f),
				// ct
				new(-2012f, 858f, 498f),
				// bank
				new(-2656f, 1360f, 492f),
				// "storage room"
				new(-2418f, 1130f, 370f),
				// heaven
				new(-1880f, 420f, 274f),
				// "under A"
				new(-1883f, 578f, 122f),
				// "canal"
				new(-254f, -1270f, 34f),
				// "pipe"
				new(-898f, -954f, 39f),
				// water
				new(-1510f, -386f, 39f),
			},
			new Vector[]
			{
				new(-2226f, 1018f, 498f),
				new(-2568f, 1138f, 498f),
				new(-2353f, 1249f, 498f),
				new(-2211f, 470f, 498f),
				new(-2576f, 785f, 490f),
				new(-2444f, 807f, 490f),
				new(-1969f, 589f, 490f),
				new(-2373f, 1092f, 498f),
			},
			new Vector[]
			{
				new(-1130f, -80f, 117f),
				new(-800f, 430f, 115f),
				new(-1726f, 75f, 130f),
				new(-2015f, -422f, 146f),
				new(-230f, 104f, 35f),
				new(-900f, 204f, 114f),
				new(-1440f, 516f, 43f),
				new(-1890f, 250f, 183f),
				new(-1582f, -108f, 146f),
				new(-982f, -590f, 114f),
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
