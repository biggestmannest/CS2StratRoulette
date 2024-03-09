using System;
using System.Globalization;

namespace CS2StratRoulette.Extensions
{
	public static class ConvertableExtensions
	{
		public static string Str<T>(this T @this, IFormatProvider? provider) where T : struct, IConvertible
		{
			return @this.ToString(provider ?? CultureInfo.InvariantCulture);
		}

		public static string Str<T>(this T? @this, IFormatProvider? provider) where T : struct, IConvertible
		{
			return @this?.ToString(provider ?? CultureInfo.InvariantCulture) ?? string.Empty;
		}
	}
}
