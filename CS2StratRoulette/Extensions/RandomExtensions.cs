using System.Runtime.CompilerServices;

namespace CS2StratRoulette.Extensions
{
	public static class RandomExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool FiftyFifty(this System.Random @this) =>
			((@this.Next() & 1) == 0);
	}
}
