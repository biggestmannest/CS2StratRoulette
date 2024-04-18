using System.Globalization;

namespace CS2StratRoulette.Extensions
{
	public static class ConvertableExtensions
	{
		public static string Str<T>(this T @this, System.IFormatProvider? provider = null)
			where T : struct, System.IConvertible =>
			@this.ToString(provider ?? CultureInfo.InvariantCulture);

		public static string Str<T>(this T? @this, System.IFormatProvider? provider = null)
			where T : struct, System.IConvertible =>
			@this?.ToString(provider ?? CultureInfo.InvariantCulture) ?? string.Empty;
	}
}
