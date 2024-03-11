using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Core;

namespace CS2StratRoulette.Extensions
{
	public static class WeaponExtensions
	{
		public static bool TryGetData(this CBasePlayerWeapon weapon,
									  [NotNullWhen(true)] out CBasePlayerWeaponVData? data)
		{
			data = null;

			if (!weapon.IsValid)
			{
				return false;
			}

			data = weapon.VData;

			return (data is not null);
		}
	}
}
