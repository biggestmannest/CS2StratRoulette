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

			if (!controller.PlayerPawn.TryGetValue(out var entity))
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

		public static void RemoveWeaponsByType(this CCSPlayerPawn pawn, params CSWeaponType[] types)
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
					if (data.WeaponType == type)
					{
						pawn.RemovePlayerItem(weapon.Value);
					}
				}
			}
		}
	}
}
