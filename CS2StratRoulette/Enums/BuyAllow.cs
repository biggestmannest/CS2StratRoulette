namespace CS2StratRoulette.Enums
{
	/*
	 * https://developer.valvesoftware.com/wiki/Mp_buy_allow_guns
	 *
	 * To restrict armor, use mp_max_armor. Set to 0 to disable all armor purchases, set to 1 to allow only Kevlar or set to 2 to allow both Kevlar and Kevlar + Helmet. As this ConVar affects only purchases, armor can still be equipped with give or game_player_equip for example.
	 * To restrict the grenade group, use mp_buy_allow_grenades. Set to 0 or 1 to disable or to enable the purchase of grenades.
	 * To restrict the Zeus x27, use mp_weapons_allow_zeus.
	 * To restrict the Riot Shield, use sv_shield_purchase_restricted_to.
	 * To restrict any specific item(s), use mp_items_prohibited. For example, mp_items_prohibited ItemDefinition.DEFUSE_KIT
	 */

	[System.Flags]
	public enum BuyAllow : byte
	{
		None = 0,

		/// <summary>
		/// Glock-18, P2000, USP-S, Dual Berettas, P250, Tec-9, CZ75-Auto, Five-SeveN, Desert Eagle, R8 Revolver
		/// </summary>
		Pistols = 1 << 0,

		/// <summary>
		/// MAC-10, MP9, MP7, MP5-SD, UMP-45, P90, PP-Bizon
		/// </summary>
		SubMachineGuns = 1 << 1,

		/// <summary>
		/// Galil AR, FAMAS, AK-47, M4A4, M4A1-S, SG 553, AUG
		/// </summary>
		Rifles = 1 << 2,

		/// <summary>
		/// Nova, XM1014, Sawed-Off, MAG-7
		/// </summary>
		Shotguns = 1 << 3,

		/// <summary>
		/// SSG 08, AWP, G3SG1, SCAR-20
		/// </summary>
		Snipers = 1 << 4,

		/// <summary>
		/// M249, Negev
		/// </summary>
		Heavy = 1 << 5,

		All = 0xff,
	}
}
