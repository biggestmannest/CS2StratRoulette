using CounterStrikeSharp.API.Modules.Utils;

namespace CS2StratRoulette.Constants
{
	public static class Points
	{
		public static readonly Vector[] Mirage =
		{
			// (770 - 130) / 4 = 160

			// facing +00.00 -90.00 +00.00
			new(-130f, -1805f, -154f),
			new(-130f, -1945f, -154f),
			// facing +00.00 +00.00 +00.00
			new(-290f, -1805f, -154f),
			new(-150f, -1805f, -154f),
			// facing +00.00 +90.00 +00.00
			new(-450f, -1805f, -154f),
			new(-450f, -1665f, -154f),
			// facing +00.00 +-180.00 +00.00
			new(-610f, -1805f, -154f),
			new(-750f, -1805f, -154f),

			// mirage mid
			// facing +00.00 +90.00 +00.00
			new(1337f, -1337f, -154f),
			new(1337f, 400f, -154f),
		};
	}
}
