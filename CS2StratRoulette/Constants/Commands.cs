using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Constants
{
	public static class Commands
	{
		public static readonly string BuyAllowNone = $"mp_buy_allow_guns {BuyAllow.None.Str()}";
		public static readonly string BuyAllowAll = $"mp_buy_allow_guns {BuyAllow.All.Str()}";

		public const string BuyAllowGrenadesDisable = "mp_buy_allow_grenades 0";
		public const string BuyAllowGrenadesEnable = "mp_buy_allow_grenades 1";
	}
}
