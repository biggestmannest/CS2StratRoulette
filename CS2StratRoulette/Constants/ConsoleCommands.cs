using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Constants
{
	public static class ConsoleCommands
	{
		public const string EnableRadar = "sv_disable_radar 0";
		public const string DisableRadar = "sv_disable_radar 1";

		public const string CheatsEnable = "sv_cheats 1";
		public const string CheatsDisable = "sv_cheats 0";

		public const string BuyAllowGrenadesDisable = "mp_buy_allow_grenades 0";
		public const string BuyAllowGrenadesEnable = "mp_buy_allow_grenades 1";

		public const string ThirdPersonEnable = "thirdperson 1";
		public const string ThirdPersonDisable = "thirdperson 0";

		public const string EquipPrimary = "slot1";
		public const string EquipSecondary = "slot2";
		public const string EquipKnife = "slot3";
		public const string EquipGrenade = "slot4";
		public const string EquipC4 = "slot5";

		public static readonly string BuyAllowNone = $"mp_buy_allow_guns {BuyAllow.None.Str()}";
		public static readonly string BuyAllowAll = $"mp_buy_allow_guns {BuyAllow.All.Str()}";
	}
}
