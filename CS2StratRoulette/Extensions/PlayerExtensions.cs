using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Extensions
{
	public static class PlayerExtensions
	{
		public static bool TryGetPlayerController(this CCSPlayerController? ccsPlayerController,
		                                          [NotNullWhen(true)] out CCSPlayerController? playerController)
		{
			playerController = null;

			if (ccsPlayerController is null || !ccsPlayerController.IsValid)
			{
				return false;
			}

			playerController = ccsPlayerController;

			return true;
		}

		public static bool TryGetPlayerPawn(this CCSPlayerController controller,
		                                    [NotNullWhen(true)] out CCSPlayerPawn? pawn)
		{
			pawn = null;

			if (!controller.IsValid || !controller.PlayerPawn.TryGetValue(out var entity))
			{
				return false;
			}

			if (!entity.IsValid)
			{
				return false;
			}

			pawn = entity;

			return true;
		}

		/// <summary>
		/// Remove weapons by type.
		/// </summary>
		/// <param name="pawn">The to remove weapons from</param>
		/// <param name="except">When <see langword="true"/> it will keep the provided types</param>
		/// <param name="types"></param>
		/// <example>
		/// <code>
		/// // this will remove all weapons except those of type pistol
		///	pawn.RemoveWeaponsByType(
		///		true,
		///		CSWeaponType.WEAPONTYPE_PISTOL
		/// );
		///
		/// // this will remove all weapons of type pistol
		///	pawn.RemoveWeaponsByType(
		///		false,
		///		CSWeaponType.WEAPONTYPE_PISTOL
		/// );
		/// </code>
		/// </example>
		public static void RemoveWeaponsByType(this CCSPlayerPawn pawn,
		                                       bool except = false,
		                                       params CSWeaponType[] types)
		{
			if (pawn.WeaponServices is null)
			{
				return;
			}

			var weapons = pawn.WeaponServices.MyWeapons;

			foreach (var weapon in weapons)
			{
				if (!weapon.TryGetValue(out var entity))
				{
					continue;
				}

				if (!entity.IsValid || weapon.Value!.VData is null)
				{
					continue;
				}

				var data = new CCSWeaponBaseVData(weapon.Value.VData.Handle);

				foreach (var type in types)
				{
					if (except ? data.WeaponType != type : data.WeaponType == type)
					{
						pawn.RemovePlayerItem(weapon.Value);
					}
				}
			}
		}
	}
}
