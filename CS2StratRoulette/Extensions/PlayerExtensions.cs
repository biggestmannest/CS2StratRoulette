using CS2StratRoulette.Constants;
using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

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

			if (!controller.IsValid ||
				controller.IsHLTV ||
				!controller.PlayerPawn.TryGetValue(out var entity))
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

		public static bool IsAlive(this CCSPlayerController @this, CCSPlayerPawn? pawn = null)
		{
			if (pawn is not null)
			{
				return pawn.IsAlive();
			}

			return @this.TryGetPlayerPawn(out pawn) && pawn.IsAlive();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAlive(this CCSPlayerPawn @this)
		{
			return (@this.LifeState == (byte)LifeState_t.LIFE_ALIVE);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EquipKnife(this CCSPlayerController controller)
		{
			controller.ExecuteClientCommand(Commands.EquipKnife);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EquipSecondary(this CCSPlayerController controller)
		{
			controller.ExecuteClientCommand(Commands.EquipSecondary);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EquipPrimary(this CCSPlayerController controller)
		{
			controller.ExecuteClientCommand(Commands.EquipPrimary);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EquipGrenade(this CCSPlayerController controller)
		{
			controller.ExecuteClientCommand(Commands.EquipGrenade);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void EquipC4(this CCSPlayerController controller)
		{
			controller.ExecuteClientCommand(Commands.EquipC4);
		}

		public static void RemoveC4(this CCSPlayerPawn pawn)
		{
			pawn.ForEachWeapon((weapon) =>
			{
				System.Console.WriteLine($"[PlayerExtensions::RemoveC4] {weapon.DesignerName}");
				if (weapon.DesignerName != "weapon_c4")
				{
					return;
				}

				if (weapon.VData is null)
				{
					return;
				}

				var data = new CCSWeaponBaseVData(weapon.VData.Handle);

				System.Console.WriteLine($"[PlayerExtensions::RemoveC4] {data.WeaponType}");
			});
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
