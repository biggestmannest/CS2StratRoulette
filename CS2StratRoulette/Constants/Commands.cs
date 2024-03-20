using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Constants
{
	public static class Commands
	{
		public const string CheatsEnable = "sv_cheats 1";
		public const string CheatsDisable = "sv_cheats 0";

		public const string BuyAllowGrenadesDisable = "mp_buy_allow_grenades 0";
		public const string BuyAllowGrenadesEnable = "mp_buy_allow_grenades 1";

		public const string ThirdPersonEnable = "thirdperson 1";
		public const string ThirdPersonDisable = "thirdperson 0";

		public static readonly string BuyAllowNone = $"mp_buy_allow_guns {BuyAllow.None.Str()}";
		public static readonly string BuyAllowAll = $"mp_buy_allow_guns {BuyAllow.All.Str()}";
	}
}
