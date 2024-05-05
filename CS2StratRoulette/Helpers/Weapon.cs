using System;

namespace CS2StratRoulette.Helpers
{
	public static class Weapon
	{
		private const string WeaponPrefix = "weapon_";

		private static readonly int WeaponPrefixLength = Weapon.WeaponPrefix.Length;

		public static bool IsGrenade(string weapon) =>
			weapon.Substring(Weapon.WeaponPrefixLength) is "hegrenade" or
														   "decoy" or
														   "flashbang" or
														   "smokegrenade" or
														   "molotov" or
														   "incgrenade";

		public static bool IsWeapon(string item) =>
			item.StartsWith(Weapon.WeaponPrefix, StringComparison.OrdinalIgnoreCase);
	}
}
