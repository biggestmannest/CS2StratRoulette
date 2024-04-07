using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Knockback : Strategy
	{
		private const float MaxWeight = 30f;

		private static readonly Vector Velocity = new(200f, 200f, 200f);

		public override string Name =>
			"Knockback II";

		public override string Description =>
			"Your guns now have a noticeable knock back.";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			plugin.RegisterEventHandler<EventWeaponFire>(this.OnWeaponFire);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			const string weaponFire = "weapon_fire";
			plugin.DeregisterEventHandler(weaponFire, this.OnWeaponFire, true);

			return true;
		}

		private HookResult OnWeaponFire(EventWeaponFire @event, GameEventInfo _)
		{
			if (!this.Running)
			{
				return HookResult.Continue;
			}

			var controller = @event.Userid;

			if (!controller.TryGetPlayerPawn(out var pawn))
			{
				return HookResult.Continue;
			}

			var designerName = @event.Weapon;
			CBasePlayerWeaponVData? wData = null;

			System.Console.WriteLine($"[Knockback::OnWeaponFire] event_weapon {designerName}");

			pawn.ForEachWeapon((w) =>
			{
				System.Console.WriteLine($"[Knockback::OnWeaponFire] pawn_weapon {w.DesignerName}");
				if (string.Equals(w.DesignerName, designerName, System.StringComparison.OrdinalIgnoreCase) &&
					w.TryGetData(out var data))
				{
					System.Console.WriteLine($"[Knockback::OnWeaponFire] found {w.DesignerName}");
					wData = data;
				}
			});

			if (wData is null)
			{
				System.Console.WriteLine($"[Knockback::OnWeaponFire] wData is null");
				return HookResult.Continue;
			}

			var vAngle = pawn.V_angle;
			var velocity = pawn.AbsVelocity;
			var knockback = (wData.Weight / Knockback.MaxWeight);

			var forward = new Vector(0f, 0f, 0f);

			NativeAPI.AngleVectors(vAngle.Handle, forward.Handle, System.IntPtr.Zero, System.IntPtr.Zero);

			velocity += (Knockback.Velocity * knockback).Multi(forward * -1f);

			System.Console.WriteLine($"[Knockback::OnWeaponFire] {velocity} / {knockback.Str()}");

			pawn.Teleport(
				pawn.AbsOrigin ?? VectorExtensions.Zero,
				vAngle,
				velocity
			);

			return HookResult.Continue;
		}
	}
}
