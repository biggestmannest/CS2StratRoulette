using CounterStrikeSharp.API.Modules.Utils;

namespace CS2StratRoulette.Constants
{
	public static class RandomTPs
	{
		public static readonly Vector[] Mirage =
		{
			new(-516f, -448f, -103f),
			new(-1690f, 217f, -105f),
			new(-1919f, -596f, -104f),
			new(-1193f, -636f, -104f),
			new(-464f, -1893f, -116f),
			new(-1671f, -1914f, -205f),
			new(47f, -2071f, 25f),
			new(-448f, -1593f, 25f),
			new(654f, -1474f, -200f),
			new(1207f, -1194f, -143f),
			new(1226f, -114f, -104f),
			new(992f, 534f, -198f),
			new(-99f, -855f, -104f),
			new(-1006f, 32f, -304f),
			new(-372f, 613f, -17f),
			new(-2304f, 696f, 25f),
			new(-2570f, 287f, -104f),
			new(-557f, -1035f, -160f),
			new(-1138f, -1387f, -103f),
			//ladder room, a lot of room for error so X and Y have to be specific.
			new(-1032.11f, -96.35f, -104f),
			new(492f, 821f, -72f),
			new(945f, -2262f, 24f),
			new(-177.75f, -2333f, -104f),
			new(-908f, -2352f, -104f),
			new(465f, -662f, -97f),
		};

		public static readonly Vector[] Nuke =
		{
			new(1782f, -496f, -288f),
			new(1853f, -1867f, -352f),
			new(273f, -2208f, -352f),
			new(-765f, -1044f, -352f),
			new(-1026f, -1073f, -352f),
			new(-123f, -997f, -352f),
			new(182f, -948.35f, -224f),
			new(111f, -1010f, -32f),
			//z specific because they might clip into silo which we DONT want!!!!!!
			new(87f, -1649f, 45.03f),
			new(746f, -1716f, -176f),
			new(761f, -1070f, -352f),
			new(665f, -391f, -352f),
			new(1093f, -398.82f, -64f),
			new(1134f, -677f, -176f),
			new(1239f, -428f, -352f),
			new(383f, -151f, -208f),
			new(536f, 251f, -256f),
			new(614f, 371f, -493.37f),
			new(638f, -931f, -708f),
			new(1337f, -957f, -704f),
			new(1278f, -509f, -576f),
			new(755f, -1597, -576f),
			new(188f, -1385f, -712f),
			new(421f, -1015f, -192f),
			new(-2723f, -678f, -352f),
		};

		public static readonly Vector[] Overpass =
		{
			/*
			 * I FUCKING HATE THIS MAP
			 * I WALK IN A STRAIGHT LINE ON A STRAIGHT FUCKING PAVED ROAD
			 * AND MY Z GOES +-2.5 FOR NO FUCKING REASON
			 * FIX YOUR SHIT MORONS
			 */
			new(-2240f, -801f, 455f),
			new(-2625f, -1800f, 541f),
			new(-2962f, -1292f, 590f),
			new(-1664f, -491f, 164f),
			new(-970f, -561f, 164f),
			new(-462f, 96f, 74f),
			new(-301f, -1357f, 79f),
			new(-480, -2027f, 213f),
			new(-1225f, -3067f, 299f),
			new(-1390f, -2184f, 404.03f),
			new(-2932f, -2472.54f, 541f),
			new(-3671f, -1111f, 554f),
			new(-3364f, -812f, 556.03f),
			new(-2475f, -757.67f, 500.03f),
			new(-1960f, 1260f, 421f),
			new(-2359f, 1300f, 424f),
			new(-1081f, 2f, 168f),
			new(-834f, 438f, 165f),
			new(-1700f, 496f, 324.03f),
			new(-1907f, 576f, 172.03f),
			new(-1862f, -581f, 196f),
			new(-416f, 312.45f, 55.27f),
			new(-1427f, 362f, 74f),
			new(-1849f, -1753.30f, 164.03f),
			new(-3106.33f, 383.77f, 548.03f),
		};

		public static readonly Vector[] Dust2 =
		{
			new(386f, 2590f, 110f),
			new(332f, 1449f, 14f),
			new(-429f, 1140f, -82f),
			new(-448f, 39f, 13f),
			new(-1566f, -443f, 144f),
			new(-1448f, -4f, 83f),
			new(-1662f, 1151f, 45f),
			new(-1944f, 2011f, 12f),
			new(-904f, 2265f, -67f),
			new(-774f, 1446f, -98f),
			new(-2062f, 2986f, 47f),
			new(608f, 507f, 15f),
			new(669f, 1185f, 16f),
			new(1422f, 201f, 71f),
			new(1710f, 353f, 70f),
			new(145f, 360f, 13f),
			new(413f, -975f, 14f),
			new(-2131f, -962f, 142f),
			new(-1517f, -261f, 142f),
			new(1756f, 2074f, 17f),
			new(1076f, 3037f, 144f),
			new(595f, 2758f, 28f),
			new(-286f, 1758f, -95f),
			new(-1260f, 2680f, 136f),
			new(-1555f, 1803f, 14f),
		};

		public static readonly Vector[] Italy =
		{
			new(-319f, -161f, -18f),
			new(-586f, 400f, 22f),
			new(-27f, 392f, -82f),
			new(484f, 356f, -146f),
			new(-341f, 939f, -146f),
			new(-1449f, 1030f, 15f),
			new(-816f, 1644f, 14f),
			new(-97f, 2248f, -86f),
			new(362f, 2398f, 14f),
			new(821f, 1979f, 14f),
			new(772f, 1272f, -55f),
			new(936f, 366f, -138f),
			new(1048f, -672f, -138f),
			new(319f, 95f, -145f),
			new(-607f, 2117f, 23f),
			new(-489f, 1211f, -42f),
			new(-328f, 1767f, -138f),
			new(-181f, 627f, -137f),
			new(-645f, 721f, 22f),
			new(-1292f, -11f, -145f),
			new(-1290f, 1300f, -139),
			new(968f, 2457f, 142f),
			new(968f, 2436f, 14f),
			new(662f, 2192f, 14f),
			new(194f, 1452f, -137f),
		};

		public static readonly Vector[] Vertigo =
		{
			new(-1417f, -110f, 11502f),
			new(-293f, -240f, 11790f),
			new(-952f, -264f, 11790f),
			new(-1928f, 263f, 11790f),
			new(-1388f, -759f, 11790f),
			new(-1840.16f, -745.15f, 11533f),
			new(-1268f, -768f, 11502f),
			new(-664f, 177f, 11790f),
			new(-326f, -9f, 11887f),
			new(-1768f, 1063f, 11762f),
			new(-2166f, 296f, 11790f),
			new(-2504f, -140f, 11566f),
			new(-1757f, -1452f, 11790f),
			new(-880f, 571f, 11887f),
			new(-1700f, 94f, 11921f),
			new(-1196f, -110f, 11502f),
			new(-1103f, -1104f, 11790f),
			new(-1711f, -557f, 11566f),
			new(-1471f, 583f, 11888f),
			new(-58f, -646f, 11918f),
			new(-422f, -108f, 11886f),
			new(-2472f, -741f, 11557f),
			new(-2613f, 195f, 11765f),
			new(-2162f, 178f, 11630f),
			new(-804f, -435f, 11833f),
		};
	}
}
