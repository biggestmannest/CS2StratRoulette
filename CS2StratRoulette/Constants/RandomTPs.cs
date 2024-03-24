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
	}
}
