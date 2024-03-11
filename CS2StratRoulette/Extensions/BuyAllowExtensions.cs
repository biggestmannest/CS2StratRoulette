using CS2StratRoulette.Enums;

namespace CS2StratRoulette.Extensions
{
	public static class BuyAllowExtensions
	{
		public static string Str(this BuyAllow @this) =>
            ((byte)@this).ToString(System.Globalization.CultureInfo.InvariantCulture);
	}
}
