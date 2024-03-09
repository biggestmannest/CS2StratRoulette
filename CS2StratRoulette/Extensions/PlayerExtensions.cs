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
		/// Remove all weapons from player pawn not included in types.
		/// </summary>
		/// <param name="pawn">The player pawn</param>
		/// <param name="types">The weapon types to keep</param>
		public static void KeepWeaponsByType(this CCSPlayerPawn pawn, params CSWeaponType[] types)
		{
			pawn.ForEachWeapon((weapon) =>
			{
				if (weapon.VData is null)
				{
					return;
				}

				var data = new CCSWeaponBaseVData(weapon.VData.Handle);
				var remove = true;

				// ReSharper disable once LoopCanBeConvertedToQuery
				foreach (var type in types)
				{
					// ReSharper disable once InvertIf
					if (type == data.WeaponType)
					{
						remove = false;
						break;
					}
				}

				if (remove)
				{
					pawn.RemovePlayerItem(weapon);
				}
			});
		}

		/// <summary>
		/// Remove weapons from player pawn by type.
		/// </summary>
		/// <param name="pawn">The player pawn</param>
		/// <param name="types">The weapon types to remove</param>
		public static void RemoveWeaponsByType(this CCSPlayerPawn pawn, params CSWeaponType[] types)
		{
			pawn.ForEachWeapon((weapon) =>
			{
				if (weapon.VData is null)
				{
					return;
				}

				var data = new CCSWeaponBaseVData(weapon.VData.Handle);
				var remove = false;

				// ReSharper disable once LoopCanBeConvertedToQuery
				foreach (var type in types)
				{
					// ReSharper disable once InvertIf
					if (type == data.WeaponType)
					{
						remove = true;
						break;
					}
				}

				if (remove)
				{
					pawn.RemovePlayerItem(weapon);
				}
			});
		}

		public static void ForEachWeapon(this CCSPlayerPawn pawn, System.Action<CBasePlayerWeapon> func)
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

				if (!entity.IsValid)
				{
					continue;
				}

				func(entity);
			}
		}
	}
}
