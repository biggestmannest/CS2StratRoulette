using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

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

		public static string Str<T>(this T @this,
			[StringSyntax(StringSyntaxAttribute.NumericFormat)]
			string format,
			System.IFormatProvider? provider = null)
			where T : INumber<T> =>
			@this.ToString(format, provider ?? CultureInfo.InvariantCulture);
	}
}
