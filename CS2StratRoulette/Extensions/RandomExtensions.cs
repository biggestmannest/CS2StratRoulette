using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CS2StratRoulette.Extensions
{
	public static class RandomExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool FiftyFifty(this System.Random @this) =>
			((@this.Next() & 1) == 0);

		public static void Shuffle<T>(this System.Random @this, IList<T> list)
		{
			var i = list.Count;

			while (i > 1)
			{
				var j = @this.Next(--i);
				(list[i], list[j]) = (list[j], list[i]);
			}
		}
	}
}
