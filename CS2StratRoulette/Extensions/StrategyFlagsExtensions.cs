using System.Runtime.CompilerServices;
using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Extensions
{
	public static class StrategyFlagsExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static StrategyFlags Remove(this ref StrategyFlags @this, StrategyFlags flag) =>
			@this &= ~flag;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static StrategyFlags Add(this ref StrategyFlags @this, StrategyFlags flag) =>
			@this |= flag;
	}
}
