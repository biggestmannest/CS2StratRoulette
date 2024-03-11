using System.Runtime.CompilerServices;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Extensions
{
	public static class StrategyFlagsExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this StrategyFlags @this, StrategyFlags flag) =>
			((@this & flag) != StrategyFlags.None);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static StrategyFlags Remove(this StrategyFlags @this, StrategyFlags flag) =>
			(@this & ~flag);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static StrategyFlags Add(this StrategyFlags @this, StrategyFlags flag) =>
			(@this | flag);
	}
}
